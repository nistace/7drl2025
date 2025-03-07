using System.Collections.Generic;
using DiceBotsGame.DiceBots;
using UnityEngine;

namespace DiceBotsGame.WorldLevels {
   [RequireComponent(typeof(WorldCubeTileActivity))]
   public class WorldCubeTileEncounter : MonoBehaviour {
      [SerializeField] protected WorldCubeTileActivity activity;
      [SerializeField] protected Transform[] worldSlots;

      private EncounterPreset Preset { get; set; }
      public string DisplayName => Preset.DisplayName;
      public int Level => Preset.EncounterLevel;
      public bool Boss => Preset.Boss;

      private DiceBot[] diceBots;

      public WorldCubeTileActivity Activity => activity;
      public IReadOnlyList<DiceBot> DiceBots => diceBots;

      private void Reset() {
         activity = GetComponent<WorldCubeTileActivity>();
      }

      public void SetUp(EncounterPreset encounterPreset, DiceBot[] encounterBots) {
         Preset = encounterPreset;
         diceBots = encounterBots;

         SnapAllBotsToWorldSlots();

         activity.DisplayName = encounterPreset.DisplayName;
      }

      public void SnapAllBotsToWorldSlots() {
         for (var i = 0; i < diceBots.Length; i++) {
            diceBots[i].transform.SetParent(worldSlots[i]);
            diceBots[i].transform.localPosition = Vector3.zero;
            diceBots[i].transform.localRotation = Quaternion.identity;
         }
      }
   }
}