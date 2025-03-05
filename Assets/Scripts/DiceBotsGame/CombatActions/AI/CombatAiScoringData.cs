using System.Collections.Generic;
using System.Linq;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine;
namespace DiceBotsGame.CombatActions.AI {
   public class CombatAiScoringData {

      public CombatGrid CombatGrid { get; }
      public int SelfHealth { get; }
      public int SelfMissingHealth { get; }
      public int DistanceBetweenSelfAndClosestAlly { get; }
      public int DistanceBetweenSelfAndClosestOpponent { get; }
      public Vector2Int SelfPosition { get; }
      public IReadOnlyCollection<DiceBot> Allies { get; }
      public IReadOnlyCollection<DiceBot> Opponents { get; }

      public CombatAiScoringData(CombatGrid combatGrid, DiceBot actor) {
         CombatGrid = combatGrid;
         SelfPosition = combatGrid.GetDiceBotPosition(actor);
         Allies = combatGrid.GetBotTeam(actor).Where(t => t != actor).ToHashSet();
         Opponents = combatGrid.GetBotOpponents(actor);

         SelfHealth = actor.HealthSystem.CurrentHealth;
         SelfMissingHealth = actor.HealthSystem.MissingHealth;
         DistanceBetweenSelfAndClosestAlly = GetPathDistanceWithClosestAlly(SelfPosition);
         DistanceBetweenSelfAndClosestOpponent = GetPathDistanceWithClosestOpponent(SelfPosition);
      }

      public int GetPathDistanceWithSelf(Vector2Int fromPosition) => CombatGrid.TryGetPath(SelfPosition, fromPosition, out var path) ? path.Count : 6 * 6;
      public int GetPathDistanceWithClosestAlly(Vector2Int fromPosition) => GetPathDistanceWithClosest(fromPosition, Allies);
      public int GetPathDistanceWithClosestOpponent(Vector2Int fromPosition) => GetPathDistanceWithClosest(fromPosition, Opponents);

      private int GetPathDistanceWithClosest(Vector2Int fromPosition, IReadOnlyCollection<DiceBot> targets) {
         if (targets.Count == 0) return 6 * 6;
         return targets.Min(t => CombatGrid.TryGetPath(fromPosition, CombatGrid.GetDiceBotPosition(t), out var path) ? path.Count : 6 * 6);
      }
   }
}
