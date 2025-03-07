using System;
using DiceBotsGame.DiceBots;
using UnityEngine;
using UnityEngine.Events;

namespace DiceBotsGame.BotUpgrades {
   [Serializable]
   public class HealthUpgrade : IUpgrade {
      [SerializeField] protected HealthSystem healthSystem;
      [SerializeField] private int newHealth;

      public int NewHealth => newHealth;
      bool IUpgrade.Selected { get; set; }
      public UnityEvent OnSelectedChanged { get; } = new UnityEvent();

      public HealthUpgrade(HealthSystem healthSystem, int newHealth) {
         this.healthSystem = healthSystem;
         this.newHealth = newHealth;
      }

      public void Apply() {
         healthSystem.ChangeMaxHealth(newHealth);
         healthSystem.Heal(newHealth, false);
      }
   }
}