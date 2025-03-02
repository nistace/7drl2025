using UnityEngine;

namespace DiceBotsGame.CombatGrids {
   public class CombatGridFactory : MonoBehaviour {
      private static CombatGridFactory instance { get; set; }

      [SerializeField] protected CombatGrid combatGridPrefab;

      private void Awake() {
         instance = this;
      }

      public static CombatGrid InstantiateCombatGrid(CombatGridPattern pattern) {
         var combatGrid = Instantiate(instance.combatGridPrefab);
         combatGrid.Build(pattern);
         return combatGrid;
      }
   }
}