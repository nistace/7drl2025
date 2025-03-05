using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DiceBotsGame.CombatActions.AI {
   [ Serializable ]
   public class CombatAi {
      [ SerializeField ] private SerializedDictionary<CombatAction, CombatAiScoring> scoringPerAction = new SerializedDictionary<CombatAction, CombatAiScoring>();

      public bool TryChooseAction(CombatGrid grid,
         DiceBot actor,
         Dictionary<CombatActionDefinition, HashSet<CombatGridTile>> options,
         out (CombatActionDefinition action, CombatGridTile tile) choice) {
         choice = default;
         if (options.Count == 0) return false;

         var data = new CombatAiScoringData(grid, actor);

         choice = default;
         var choiceScore = 0;

         foreach (var action in options) {
            if (scoringPerAction.TryGetValue(action.Key.Action, out var scoring)) {
               foreach (var actionTile in action.Value) {
                  var actionTileScore = scoring.EvaluateScore(data, actionTile, action.Key.ConstantStrength);
                  if (actionTileScore > choiceScore || actionTileScore > 0 && Random.value > .5f) {
                     choice = (action.Key, actionTile);
                     choiceScore = actionTileScore;
                  }
                  Debug.Log($"{action.Key.DisplayName} {actionTile.Coordinates} : {actionTileScore}");
               }

            }
         }

         Debug.Log(choiceScore);
         return choiceScore > 0;
      }

   }
}
