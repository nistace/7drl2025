using DiceBotsGame.Utils;
using UnityEngine;

namespace DiceBotsGame.CombatActions {
   public class CombatAction : MonoBehaviour {
      [SerializeField] protected string actionName = "Acting";
      [SerializeField] protected string displayConditions = "Acting conditions for {0}";
      [SerializeField] protected string displayEffects = "Acting effects for{0}";
      [SerializeField] protected Sprite sprite;
      [SerializeField] protected Mesh mesh;

      public Mesh Mesh => mesh;
      public Sprite Sprite => sprite;
      public string ActionName => actionName;

      public string GetDisplayConditions(int value) => displayConditions.WithInjectedParameters(("value", value));
      public string GetDisplayEffects(int value) => displayEffects.WithInjectedParameters(("value", value));
   }
}