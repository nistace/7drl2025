using UnityEngine;

namespace DiceBotsGame.CombatActions {
   public class CombatAction : MonoBehaviour {
      [SerializeField] protected MeshRenderer model;
      [SerializeField] protected CombatActionTarget target;

      public CombatActionTarget Target => target;
      public MeshRenderer Model => model;

      public void Execute() { }
   }
}