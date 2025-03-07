using System;
using UnityEngine;
using UnityEngine.Events;

namespace DiceBotsGame.DiceBots {
   [Serializable]
   public class HealthSystem {
      [SerializeField] protected int maxHealth;
      [SerializeField] protected int currentHealth;

      public UnityEvent OnHealthChanged { get; } = new UnityEvent();

      public int MaxHealth => maxHealth;
      public int CurrentHealth => currentHealth;
      public int MissingHealth => MaxHealth - currentHealth;
      public float HealthRatio => (float)currentHealth / maxHealth;
      public bool IsAlive => currentHealth > 0;
      public bool IsDead => !IsAlive;

      public UnityEvent<int> OnDamaged { get; }
      public UnityEvent<int> OnHealed { get; }
      public UnityEvent<int> OnChanged { get; }
      public UnityEvent OnDied { get; }
      public UnityEvent OnResurrected { get; }
      public UnityEvent<int> OnMaxHealthChanged { get; }

      public HealthSystem() : this(5) { }

      public HealthSystem(int maxHealth) {
         this.maxHealth = maxHealth;
         currentHealth = this.maxHealth;
         OnDamaged = new UnityEvent<int>();
         OnHealed = new UnityEvent<int>();
         OnChanged = new UnityEvent<int>();
         OnDied = new UnityEvent();
         OnResurrected = new UnityEvent();
         OnMaxHealthChanged = new UnityEvent<int>();
      }

      public void ChangeMaxHealth(int maxHealth) {
         this.maxHealth = maxHealth;
         if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
            OnChanged.Invoke(currentHealth);
         }
         OnMaxHealthChanged.Invoke(maxHealth);
      }

      public int Damage(int damage) {
         if (IsDead) return 0;

         var actualDamage = Mathf.Min(currentHealth, Mathf.Max(0, damage));
         if (actualDamage <= 0) return 0;

         currentHealth -= actualDamage;

         OnDamaged.Invoke(actualDamage);
         OnChanged.Invoke(-actualDamage);
         if (IsDead) OnDied.Invoke();

         return actualDamage;
      }

      public int Heal(int amount, bool resurrect) {
         if (IsDead && !resurrect) return 0;

         var actualHeal = Mathf.Min(MaxHealth - currentHealth, Mathf.Max(0, amount));

         if (actualHeal <= 0) return 0;

         currentHealth += actualHeal;

         OnHealed.Invoke(actualHeal);
         OnChanged.Invoke(actualHeal);
         if (currentHealth == actualHeal) OnResurrected.Invoke();

         return actualHeal;
      }
   }
}