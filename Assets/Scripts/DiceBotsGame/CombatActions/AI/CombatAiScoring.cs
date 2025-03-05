using System;
using DiceBotsGame.CombatGrids;
using UnityEngine;

namespace DiceBotsGame.CombatActions.AI {
   [ Serializable ]
   public class CombatAiScoring {

      [ SerializeField ] private int scoreInitial = 0;
      [ SerializeField ] private int scorePerValue = 1;
      [ SerializeField ] private int scorePerSelfHealthPoints = 0;
      [ SerializeField ] private int scorePerSelfMissingHealthPoints = 0;
      [ SerializeField ] private int scorePerTargetHealthPoints = 0;
      [ SerializeField ] private int scorePerTargetMissingHealthPoints = 0;
      [ SerializeField ] private int scorePerDistanceBetweenActorAndAlly = 0;
      [ SerializeField ] private int scorePerDistanceBetweenActorAndOpponent = 0;
      [ SerializeField ] private int scorePerDistanceBetweenTileAndSelf = 0;
      [ SerializeField ] private int scorePerDistanceBetweenTileAndAlly = 0;
      [ SerializeField ] private int scorePerDistanceBetweenTileAndOpponent = 0;

      public int EvaluateScore(CombatAiScoringData data, CombatGridTile tile, int actionValue) {
         var tileBot = data.CombatGrid.GetDiceBotAtPosition(tile);

         var result = scoreInitial;
         result += actionValue * scorePerValue;
         result += data.SelfHealth * scorePerSelfHealthPoints;
         result += data.SelfMissingHealth * scorePerSelfMissingHealthPoints;
         result += tileBot ? tileBot.HealthSystem.CurrentHealth * scorePerTargetHealthPoints : 0;
         result += tileBot ? tileBot.HealthSystem.MissingHealth * scorePerTargetMissingHealthPoints : 0;
         result += scorePerDistanceBetweenActorAndAlly * data.DistanceBetweenSelfAndClosestAlly;
         result += scorePerDistanceBetweenActorAndOpponent * data.DistanceBetweenSelfAndClosestOpponent;
         result += scorePerDistanceBetweenTileAndSelf * data.GetPathDistanceWithSelf(tile.Coordinates);

         if (scorePerDistanceBetweenTileAndAlly != 0) {
            result += scorePerDistanceBetweenTileAndAlly * data.GetPathDistanceWithClosestAlly(tile.Coordinates);
         }

         if (scorePerDistanceBetweenTileAndOpponent != 0) {
            result += scorePerDistanceBetweenTileAndOpponent * data.GetPathDistanceWithClosestOpponent(tile.Coordinates);
         }

         return result;
      }

   }
}
