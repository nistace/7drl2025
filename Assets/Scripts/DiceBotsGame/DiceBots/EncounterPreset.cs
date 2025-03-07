using System.Collections.Generic;
using UnityEngine;

namespace DiceBotsGame.DiceBots {
   [CreateAssetMenu]
   public class EncounterPreset : ScriptableObject {
      [SerializeField] protected string displayName;
      [SerializeField] protected DiceBotPattern[] diceBotPatterns;
      [SerializeField] protected int encounterLevel = 1;
      [SerializeField] private bool boss;

      public IReadOnlyList<DiceBotPattern> DiceBotPatterns => diceBotPatterns;
      public string DisplayName => displayName;
      public int EncounterLevel => encounterLevel;
      public bool Boss => boss;
   }
}