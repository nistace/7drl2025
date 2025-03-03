using System;
using System.Linq;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine;

namespace DiceBotsGame.CombatActions.AI {
   [Serializable]
   public class CombatAiScoring {

      [SerializeField] private int scorePerValue = 1;
      [SerializeField] private int scorePerSelfHealthPoints = 0;
      [SerializeField] private int scorePerSelfMissingHealthPoints = 0;
      [SerializeField] private int scorePerTargetHealthPoints = 0;
      [SerializeField] private int scorePerTargetMissingHealthPoints = 0;
      [SerializeField] private int scorePerDistanceWithSelf = 0;
      [SerializeField] private int scorePerDistanceWithAlly = 0;
      [SerializeField] private int scorePerDistanceWithOpponent = 0;

      public int EvaluateScore(CombatGrid combatGrid, DiceBot actor, CombatGridTile tile, int value) {
         var result = 0;

         if (scorePerValue != 0) {
            result += value * scorePerValue;
         }

         if (scorePerSelfHealthPoints != 0) {
            result += actor.HealthSystem.CurrentHealth * scorePerSelfHealthPoints;
         }

         if (scorePerSelfMissingHealthPoints != 0) {
            result += actor.HealthSystem.MissingHealth * scorePerSelfMissingHealthPoints;
         }

         if (scorePerTargetHealthPoints != 0) {
            var other = combatGrid.GetDiceBotAtPosition(tile);
            result += other ? other.HealthSystem.CurrentHealth * scorePerTargetHealthPoints : 0;
         }

         if (scorePerTargetMissingHealthPoints != 0) {
            var other = combatGrid.GetDiceBotAtPosition(tile);
            result += other ? other.HealthSystem.MissingHealth * scorePerTargetMissingHealthPoints : 0;
         }

         if (scorePerDistanceWithSelf != 0) {
            result += CombatGrid.Distance(tile.Coordinates, combatGrid.GetDiceBotPosition(actor)) * scorePerDistanceWithSelf;
         }

         if (scorePerDistanceWithAlly != 0) {
            var bots = combatGrid.GetBotTeam(actor);
            if (bots.Count > 1) {
               var distanceWithClosest = bots.Where(t => t != actor).Min(t => CombatGrid.Distance(tile.Coordinates, combatGrid.GetDiceBotPosition(t)));
               result += distanceWithClosest * scorePerDistanceWithAlly;
            }
         }

         if (scorePerDistanceWithOpponent != 0) {
            var bots = combatGrid.GetBotOpponents(actor);
            if (bots.Count > 0) {
               var distanceWithClosest = bots.Min(t => CombatGrid.Distance(tile.Coordinates, combatGrid.GetDiceBotPosition(t)));
               result += distanceWithClosest * scorePerDistanceWithOpponent;
            }
         }

         return result;
      }

   }
}
