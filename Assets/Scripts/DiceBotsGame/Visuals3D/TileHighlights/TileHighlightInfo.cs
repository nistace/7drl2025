using UnityEngine;

namespace DiceBotsGame.Visuals3D.TileHighlights {
   [CreateAssetMenu]
   public class TileHighlightInfo : ScriptableObject {
      [SerializeField] protected Color color = Color.white;
      [SerializeField] protected float intensity = 1;

      public Color Color {
         get => color;
         set => color = value;
      }

      public float Intensity {
         get => intensity;
         set => intensity = value;
      }
   }
}