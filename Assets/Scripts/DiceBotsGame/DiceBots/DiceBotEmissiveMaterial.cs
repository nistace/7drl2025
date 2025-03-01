using System;
using UnityEngine;

namespace DiceBotsGame.DiceBots {
   [Serializable]
   public class DiceBotEmissiveMaterial {
      private static readonly int emissionColorShaderId = Shader.PropertyToID("_EmissionColor");
      private const int IntensityCoefficient = 2;

      [SerializeField] protected Material material;
      [SerializeField] protected Color color;

      public Material Material => material;
      public HealthSystem ObservedHealthSystem { get; private set; }

      public static DiceBotEmissiveMaterial Instantiate(Material sourceMaterial, Color emissiveColor, float intensity) {
         var instance = new DiceBotEmissiveMaterial { material = new Material(sourceMaterial), color = emissiveColor };
         instance.SetIntensity(intensity);
         return instance;
      }

      public void SetIntensity(float intensity) => material.SetColor(emissionColorShaderId, color * intensity * IntensityCoefficient);

      public void SetObservedHealthSystem(HealthSystem healthSystem) {
         ObservedHealthSystem?.OnChanged.RemoveListener(HandleObservedHealthChanged);
         ObservedHealthSystem = healthSystem;
         if (ObservedHealthSystem != null) {
            ObservedHealthSystem?.OnChanged.AddListener(HandleObservedHealthChanged);
            SetIntensity(ObservedHealthSystem.HealthRatio);
         }
      }

      private void HandleObservedHealthChanged(int delta) => SetIntensity(ObservedHealthSystem.HealthRatio);
   }
}