using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiceBotsGame.CombatActions;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using DiceBotsGame.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace DiceBotsGame.GameSates.CombatStates.CombatSubStates {
   public class PlayerCombatSubState : ICombatSubState {
      private static HashSet<CombatGridTile> NoTile { get; } = new HashSet<CombatGridTile>();

      private enum Phase {
         SelectAction = 0,
         SelectTile = 1,
         Resolve = 2
      }

      private CombatGridTile hoveredTile;
      private bool overUi;

      public bool IsOver { get; private set; }
      private DiceBot playingBot { get; set; }
      private CombatActionDefinition playingAction { get; set; }
      private CombatGridTile playingTile { get; set; }
      private Phase CurrentPhase { get; set; }
      private HashSet<CombatGridTile> playingTileOptions { get; set; } = NoTile;
      private HashSet<DiceBot> PlayedBots { get; } = new HashSet<DiceBot>();

      private Dictionary<(DiceBot, CombatActionDefinition), HashSet<CombatGridTile>> optionsPerBotAttack { get; } = new Dictionary<(DiceBot, CombatActionDefinition), HashSet<CombatGridTile>>();

      private void RefreshOptionsPerBotAttack() {
         optionsPerBotAttack.Clear();
         foreach (var bot in GameInfo.PlayerParty.DiceBotsInParty.Except(PlayedBots).Where(t => t.HealthSystem.IsAlive)) {
            foreach (var action in bot.Dice.CoreActions.Union(new[] { bot.Dice.LastRolledFace.Data.CombatAction }).Where(t => t.IsValidAction)) {
               var tileCandidates = GameInfo.CombatGrid.AllTiles.Where(t => CombatActionHelper.CheckConditions(action.Action, GameInfo.CombatGrid, bot, t, action.ConstantStrength)).ToHashSet();
               if (tileCandidates.Count > 0) {
                  optionsPerBotAttack.Add((bot, action), tileCandidates);
               }
            }
         }
      }

      private void RefreshAllHighlights() {
         GameInfo.CombatGrid.UnHoverAllTiles();
         if (playingBot) {
            foreach (var tile in playingTileOptions) {
               RefreshHighlight(tile, hoveredTile == tile);
            }
         }
      }

      public void StartState() {
         MainUi.Log.SetTexts(ICombatSubState.BattleTitle, "Opportunity to act!");

         PlayedBots.Clear();
         RefreshOptionsPerBotAttack();

         IsOver = false;

         CombatInputUtils.Interact.performed += HandleInteractPerformed;
         CombatInputUtils.Cancel.performed += HandleCancelPerformed;

         CombatInfoHelper.EnableLog();
         MainUi.Combat.SetEndTurnButtonVisible(true);
         MainUi.Combat.OnEndTurnClicked.AddListener(HandleEndTurnClicked);
         MainUi.DiceBots.SetPlayerActionsInteractable(true);
         MainUi.DiceBots.OnPlayerBotActionClicked.AddListener(HandlePlayerBotActionClicked);
         MainUi.DiceBots.OnPlayerBotActionHoverStarted.AddListener(HandlePlayerBotActionHoverStarted);
         MainUi.DiceBots.OnPlayerBotActionHoverStopped.AddListener(HandlePlayerBotActionHoverStopped);
      }

      private void HandleEndTurnClicked() => IsOver = true;

      private void HandlePlayerBotActionHoverStopped(DiceBot bot, CombatActionDefinition action) {
         if (CurrentPhase != Phase.SelectAction) return;

         if (playingBot == bot && playingAction == action) {
            playingBot = default;
            playingAction = default;
            playingTileOptions = NoTile;
            RefreshAllHighlights();
         }
      }

      private void HandlePlayerBotActionHoverStarted(DiceBot bot, CombatActionDefinition action) {
         if (CurrentPhase != Phase.SelectAction) return;

         playingBot = bot;
         playingAction = action;
         playingTileOptions = optionsPerBotAttack.GetValueOrDefault((playingBot, playingAction), NoTile);
         RefreshAllHighlights();
      }

      private void HandlePlayerBotActionClicked(DiceBot bot, CombatActionDefinition action) {
         if (CurrentPhase != Phase.SelectAction) return;

         playingBot = bot;
         playingAction = action;
         playingTileOptions = optionsPerBotAttack.GetValueOrDefault((playingBot, playingAction), NoTile);

         if (playingTileOptions.Count == 0) {
            CombatInfoHelper.ShowLog($"{bot.DisplayName} cannot start {action.DisplayName} from that position");
            return;
         }

         CurrentPhase = Phase.SelectTile;
         CombatInfoHelper.DisableLog();
         RefreshAllHighlights();
         CombatInfoHelper.ShowLog($"{bot.DisplayName} initiates {action.DisplayName}");
      }

      private void HandleInteractPerformed(InputAction.CallbackContext obj) {
         if (overUi) return;
         if (CurrentPhase != Phase.SelectTile) return;
         if (!playingBot) return;
         if (playingAction is not { IsValidAction: true }) return;
         if (!playingTileOptions.Contains(hoveredTile)) {
            CombatInfoHelper.ShowLog($"This is not a valid target for {playingAction.DisplayName}");
            return;
         }

         playingTile = hoveredTile;
         playingTileOptions = NoTile;
         RefreshAllHighlights();

         playingBot.StartCoroutine(DoResolve());
      }

      private void HandleCancelPerformed(InputAction.CallbackContext obj) {
         if (CurrentPhase != Phase.SelectTile) return;
         CurrentPhase = Phase.SelectAction;
         RefreshAllHighlights();
      }

      private IEnumerator DoResolve() {
         CurrentPhase = Phase.Resolve;
         playingBot.Reassemble();

         MainUi.DiceBots.SetPlayerActionsInteractable(false);

         MainUi.Log.SetTexts(ICombatSubState.BattleTitle, $"{playingBot.DisplayName} is {playingAction.DisplayName}");

         var value = playingAction.ConstantStrength;
         foreach (var actionEffect in CombatActionHelper.GetEffects(playingAction.Action)) {
            yield return playingBot.StartCoroutine(actionEffect.Execute(GameInfo.CombatGrid, playingBot, playingTile, value, t => value = t));
         }

         CurrentPhase = Phase.SelectAction;
         CombatInfoHelper.HideLog();
         CombatInfoHelper.EnableLog();
         PlayedBots.Add(playingBot);
         RefreshOptionsPerBotAttack();
         IsOver = GameInfo.PlayerParty.DiceBotsInParty.All(t => t.HealthSystem.IsDead || PlayedBots.Contains(t));
      }

      public void Update() {
         overUi = EventSystem.current.IsPointerOverGameObject();
         CombatInputUtils.TryGetHitCombatTile(out var newHoveredTile);

         if (newHoveredTile != hoveredTile) {
            if (hoveredTile) RefreshHighlight(hoveredTile, false);
            hoveredTile = newHoveredTile;
            if (hoveredTile) RefreshHighlight(hoveredTile, true);
         }
      }

      private void RefreshHighlight(CombatGridTile tile, bool hovered) {
         var highlightType = CombatGridTileConfig.HighlightType.None;
         if (CurrentPhase != Phase.Resolve && playingTileOptions.Contains(tile) || CurrentPhase == Phase.Resolve && playingTile == tile) {
            highlightType = CombatGridTileConfig.HighlightType.DefaultSelectable;
            var diceAtPosition = GameInfo.CombatGrid.GetDiceBotAtPosition(tile);
            if (diceAtPosition) {
               highlightType = GameInfo.CombatGrid.AreInSameTeam(playingBot, diceAtPosition)
                  ? CombatGridTileConfig.HighlightType.AllySelectable
                  : CombatGridTileConfig.HighlightType.OpponentSelectable;
            }
         }

         if (hovered) {
            highlightType = highlightType switch {
               CombatGridTileConfig.HighlightType.None => CombatGridTileConfig.HighlightType.HoveredNone,
               CombatGridTileConfig.HighlightType.DefaultSelectable => CombatGridTileConfig.HighlightType.HoveredDefaultSelectable,
               CombatGridTileConfig.HighlightType.HoveredDefaultSelectable => CombatGridTileConfig.HighlightType.HoveredDefaultSelectable,
               CombatGridTileConfig.HighlightType.AllySelectable => CombatGridTileConfig.HighlightType.HoveredAllySelectable,
               CombatGridTileConfig.HighlightType.HoveredAllySelectable => CombatGridTileConfig.HighlightType.HoveredAllySelectable,
               CombatGridTileConfig.HighlightType.OpponentSelectable => CombatGridTileConfig.HighlightType.HoveredOpponentSelectable,
               CombatGridTileConfig.HighlightType.HoveredOpponentSelectable => CombatGridTileConfig.HighlightType.HoveredOpponentSelectable,
               CombatGridTileConfig.HighlightType.HoveredNone => CombatGridTileConfig.HighlightType.HoveredNone,
               _ => throw new ArgumentOutOfRangeException()
            };
         }

         tile.SetHighlight(highlightType);
      }

      public void EndState() {
         CombatInfoHelper.DisableLog();
         MainUi.DiceBots.SetPlayerActionsInteractable(false);
         MainUi.Combat.SetEndTurnButtonVisible(false);
         MainUi.Combat.OnEndTurnClicked.RemoveListener(HandleEndTurnClicked);
         CombatInputUtils.Interact.performed -= HandleInteractPerformed;
         CombatInputUtils.Cancel.performed -= HandleCancelPerformed;
         IsOver = false;
         if (hoveredTile) {
            hoveredTile.SetHighlight(CombatGridTileConfig.HighlightType.None);
            hoveredTile = null;
         }
      }
   }
}