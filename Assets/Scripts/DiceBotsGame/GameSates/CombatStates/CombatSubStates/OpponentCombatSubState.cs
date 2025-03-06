using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiceBotsGame.CombatActions;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using DiceBotsGame.UI;
using DiceBotsGame.Utils;

namespace DiceBotsGame.GameSates.CombatStates.CombatSubStates {
   public class OpponentCombatSubState : ICombatSubState {
      public bool IsOver => Phase == EPhase.ConsideringNextAction && optionsPerBotAttack.Count == 0;

      private enum EPhase {
         RefreshingActions = 0,
         ConsideringNextAction = 1,
         PlayingBot = 2
      }

      private EPhase Phase { get; set; }
      private readonly HashSet<DiceBot> playedBots = new HashSet<DiceBot>();

      private Dictionary<DiceBot, Dictionary<CombatActionDefinition, HashSet<CombatGridTile>>> optionsPerBotAttack { get; } =
         new Dictionary<DiceBot, Dictionary<CombatActionDefinition, HashSet<CombatGridTile>>>();

      public void StartState() {
         MainUi.Log.SetTexts(ICombatSubState.BattleTitle, "Opponents counter-attack!");
         playedBots.Clear();
         Phase = EPhase.RefreshingActions;
      }

      private void RefreshOptionsPerBotAttack() {
         optionsPerBotAttack.Clear();
         foreach (var bot in GameInfo.CombatGrid.OpponentBots.Where(t => !playedBots.Contains(t) && t.HealthSystem.IsAlive)) {
            foreach (var action in bot.Dice.CoreActions.Union(new[] { bot.Dice.LastRolledFace.Data.CombatAction }).Where(t => t.IsValidAction)) {
               var tileCandidates = GameInfo.CombatGrid.AllTiles.Where(t => CombatActionHelper.CheckConditions(action.Action, GameInfo.CombatGrid, bot, t, action.ConstantStrength)).ToHashSet();
               if (tileCandidates.Count > 0) {
                  if (!optionsPerBotAttack.ContainsKey(bot)) optionsPerBotAttack.Add(bot, new Dictionary<CombatActionDefinition, HashSet<CombatGridTile>>());
                  optionsPerBotAttack[bot].Add(action, tileCandidates);
               }
            }
         }
         Phase = EPhase.ConsideringNextAction;
      }

      public void Update() {
         if (Phase == EPhase.RefreshingActions) {
            RefreshOptionsPerBotAttack();
         }
         else if (Phase == EPhase.ConsideringNextAction) {
            if (optionsPerBotAttack.Any()) {
               var playingBot = optionsPerBotAttack.Keys.Roll();
               if (playingBot.CombatAi.TryChooseAction(GameInfo.CombatGrid, playingBot, optionsPerBotAttack[playingBot], out var choice)) {
                  playingBot.StartCoroutine(PlayBotTurn(playingBot, choice.action, choice.tile));
               }
               else {
                  playedBots.Add(playingBot);
                  Phase = EPhase.RefreshingActions;
               }
            }
         }
      }

      private IEnumerator PlayBotTurn(DiceBot playingBot, CombatActionDefinition action, CombatGridTile tile) {
         Phase = EPhase.PlayingBot;
         playingBot.Reassemble();

         MainUi.Log.SetTexts(ICombatSubState.BattleTitle, $"{playingBot.DisplayName} is {action.DisplayName}...");

         foreach (var actionEffect in CombatActionHelper.GetEffects(action.Action)) {
            yield return playingBot.StartCoroutine(actionEffect.Execute(GameInfo.CombatGrid, playingBot, tile, action.ConstantStrength));
         }

         playedBots.Add(playingBot);

         Phase = EPhase.RefreshingActions;
      }

      public void EndState() { }
   }
}