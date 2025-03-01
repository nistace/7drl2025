using DiceBotsGame.DiceBots;
using UnityEngine;

namespace DiceBotsGame.WorldLevels {
   [RequireComponent(typeof(WorldCubeTileActivity))]
   public class WorldCubeTileEncounter : MonoBehaviour {
      [SerializeField] protected WorldCubeTileActivity activity;
      [SerializeField] protected Transform[] worldSlots;

      private void Reset() {
         activity = GetComponent<WorldCubeTileActivity>();
      }

      public void SetUp(DiceBot[] encounterBots) {
         for (var i = 0; i < encounterBots.Length; i++) {
            encounterBots[i].transform.SetParent(worldSlots[i]);
            encounterBots[i].transform.localPosition = Vector3.zero;
            encounterBots[i].transform.localRotation = Quaternion.identity;
         }
      }
   }
}