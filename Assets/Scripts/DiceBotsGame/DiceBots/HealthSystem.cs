using System;
using UnityEngine;
using UnityEngine.Events;

namespace DiceBotsGame.DiceBots {
   [Serializable]
   public class HealthSystem {
      [SerializeField] protected int maxHealth = 5;
      [SerializeField] protected int currentHealth;

      public UnityEvent OnHealthChanged { get; } = new UnityEvent();

      public int MaxHealth => maxHealth;
      public int CurrentHealth => currentHealth;
      public float HealthRatio => (float)currentHealth / maxHealth;
      public bool IsAlive => currentHealth > 0;
      public bool IsDead => !IsAlive;

      public UnityEvent<int> OnDamaged { get; } = new UnityEvent<int>();
      public UnityEvent<int> OnHealed { get; } = new UnityEvent<int>();
      public UnityEvent<int> OnChanged { get; } = new UnityEvent<int>();
      public UnityEvent OnDied { get; } = new UnityEvent();
      public UnityEvent OnResurrected { get; } = new UnityEvent();

      public HealthSystem(int maxHealth) {
         this.maxHealth = maxHealth;
         currentHealth = this.maxHealth;
      }

      public void Damage(int damage) {
         if (IsDead) return;

         var actualDamage = Mathf.Min(currentHealth, Mathf.Max(0, damage));
         if (actualDamage <= 0) return;

         currentHealth -= actualDamage;

         OnDamaged.Invoke(actualDamage);
         OnChanged.Invoke(-actualDamage);
         if (IsDead) OnDied.Invoke();
      }

      public void Heal(int amount, bool resurrect) {
         if (IsDead && !resurrect) return;

         var actualHeal = Mathf.Min(MaxHealth - currentHealth, Mathf.Max(0, amount));

         if (actualHeal <= 0) return;

         currentHealth += actualHeal;

         OnHealed.Invoke(actualHeal);
         OnChanged.Invoke(actualHeal);
         if (currentHealth == actualHeal) OnResurrected.Invoke();
      }
   }
}