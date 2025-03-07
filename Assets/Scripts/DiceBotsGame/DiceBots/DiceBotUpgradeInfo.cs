using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using DiceBotsGame.CombatActions;
using UnityEngine;

namespace DiceBotsGame.DiceBots {
   [Serializable]
   public class DiceBotUpgradeInfo {
      [SerializeField] private int levelUpAdditionalHealth = 1;
      [SerializeField] private SerializedDictionary<UpgradableAction, CombatAction> upgradableActions;
      [SerializeField] private CombatActionDefinition[] initialActions;

      public int LevelUpAdditionalHealth => levelUpAdditionalHealth;

      public List<CombatActionDefinition> GetActionUpgrades(CombatActionDefinition dataCombatAction) {
         var results = new List<CombatActionDefinition>();

         if (dataCombatAction.IsValidAction) {
            if (dataCombatAction.ConstantStrength < dataCombatAction.Action.MaxValue) {
               results.Add(new CombatActionDefinition(dataCombatAction.Action, dataCombatAction.ConstantStrength + 1));
            }

            results.AddRange(upgradableActions.Where(t => t.Key.Check(dataCombatAction))
               .Select(upgradedAction => new CombatActionDefinition(upgradedAction.Value, upgradedAction.Key.TransformValue(dataCombatAction.ConstantStrength))));
         }
         else {
            results.AddRange(initialActions);
         }

         return results;
      }

      [Serializable]
      private class UpgradableAction {
         public enum EEffectOnLevel {
            ResetToOne = 1,
            KeepMinus1 = 2,
            KeepExact = 3
         }

         [SerializeField] protected CombatAction action;
         [SerializeField] protected int minLevel = 2;
         [SerializeField] protected EEffectOnLevel effectOnLevel = EEffectOnLevel.ResetToOne;

         public bool Check(CombatActionDefinition actionDefinition) => action == actionDefinition.Action && minLevel <= actionDefinition.ConstantStrength;

         public int TransformValue(int input) => effectOnLevel switch {
            EEffectOnLevel.ResetToOne => 1,
            EEffectOnLevel.KeepMinus1 => input - 1,
            EEffectOnLevel.KeepExact => input,
            _ => throw new ArgumentOutOfRangeException()
         };
      }
   }
}