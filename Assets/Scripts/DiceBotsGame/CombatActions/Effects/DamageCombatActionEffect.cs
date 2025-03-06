﻿using System;
using System.Collections;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine;
using UnityEngine.Events;

namespace DiceBotsGame.CombatActions.Effects {
   [RequireComponent(typeof(CombatAction))]
   public class DamageCombatActionEffect : MonoBehaviour, ICombatActionEffect {
      private enum EOutputValue {
         Unchanged = 0,
         DealtDamage = 1
      }

      [SerializeField] private EEffectTarget target = EEffectTarget.BotOnTile;
      [SerializeField] private EOutputValue outputValue = EOutputValue.Unchanged;

      public IEnumerator Execute(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile, int value, UnityAction<int> outputValueCallback) {
         var bot = CombatEffectHelper.GetTarget(combatGrid, actor, targetTile, target);
         var dealtDamage = bot.HealthSystem.Damage(value);
         yield return null;

         outputValueCallback?.Invoke(outputValue switch {
            EOutputValue.Unchanged => value,
            EOutputValue.DealtDamage => dealtDamage,
            _ => throw new ArgumentOutOfRangeException()
         });
      }
   }
}