using System;
using System.Collections.Generic;
using System.Linq;
using DiceBotsGame.CombatActions.Conditions;
using DiceBotsGame.CombatActions.Effects;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;

namespace DiceBotsGame.CombatActions {
   public static class CombatActionHelper {
      [NonSerialized]
      private static readonly Dictionary<CombatAction, IReadOnlyList<ICombatActionCondition>> conditionCache = new Dictionary<CombatAction, IReadOnlyList<ICombatActionCondition>>();
      [NonSerialized]
      private static readonly Dictionary<CombatAction, IReadOnlyList<ICombatActionEffect>> effectCache = new Dictionary<CombatAction, IReadOnlyList<ICombatActionEffect>>();

      public static bool CheckConditions(CombatAction action, CombatGrid grid, DiceBot actor, CombatGridTile tile, int value) {
         if (!conditionCache.TryGetValue(action, out var actionConditions)) {
            actionConditions = action.GetComponents<ICombatActionCondition>();
            conditionCache.Add(action, actionConditions);
         }

         return actionConditions.All(t => t.Check(grid, actor, tile, value));
      }

      public static IReadOnlyList<ICombatActionEffect> GetEffects(CombatAction action) {
         if (!effectCache.TryGetValue(action, out var actionEffects)) {
            actionEffects = action.GetComponents<ICombatActionEffect>();
            effectCache.Add(action, actionEffects);
         }
         return actionEffects;
      }
   }
}