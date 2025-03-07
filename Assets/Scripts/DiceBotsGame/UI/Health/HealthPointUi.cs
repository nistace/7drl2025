using UnityEngine;
using UnityEngine.UI;

namespace DiceBotsGame.UI.Health {
   public class HealthPointUi : MonoBehaviour {
      [SerializeField] protected Image image;

      public Color Color {
         set => image.color = value;
      }
   }
}