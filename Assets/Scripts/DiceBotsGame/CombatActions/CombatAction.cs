using UnityEngine;

namespace DiceBotsGame.CombatActions {
   public class CombatAction : MonoBehaviour {
      [SerializeField] protected string actionName = "Acting";
      [SerializeField] protected Sprite sprite;
      [SerializeField] protected Mesh mesh;

      public Mesh Mesh => mesh;
      public Sprite Sprite => sprite;

      public string ActionName => actionName;
   }
}