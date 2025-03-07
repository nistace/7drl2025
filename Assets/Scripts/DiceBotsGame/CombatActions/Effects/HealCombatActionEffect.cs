using System;
using System.Collections;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine;
using UnityEngine.Events;

namespace DiceBotsGame.CombatActions.Effects {
   [RequireComponent(typeof(CombatAction))]
   public class HealCombatActionEffect : MonoBehaviour, ICombatActionEffect {
      private enum EOutputValue {
         Unchanged = 0,
         HealedDamage = 1
      }

      [SerializeField] private ETarget target = ETarget.BotOnTile;
      [SerializeField] private bool resurrect;
      [SerializeField] private EOutputValue outputValue = EOutputValue.Unchanged;

      public IEnumerator Execute(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile, int value, UnityAction<int> outputValueCallback) {
         var bot = CombatEffectHelper.GetTarget(combatGrid, actor, targetTile, target);
         var healedDamage = bot.HealthSystem.Heal(value, resurrect);
         yield return null;

         outputValueCallback?.Invoke(outputValue switch {
            EOutputValue.Unchanged => value,
            EOutputValue.HealedDamage => healedDamage,
            _ => throw new ArgumentOutOfRangeException()
         });
      }
   }
}