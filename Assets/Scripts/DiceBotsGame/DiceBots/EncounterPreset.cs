using System.Collections.Generic;
using UnityEngine;

namespace DiceBotsGame.DiceBots {
   [CreateAssetMenu]
   public class EncounterPreset : ScriptableObject {
      [SerializeField] protected string displayName;
      [SerializeField] protected DiceBotPattern[] diceBotPatterns;

      public IReadOnlyList<DiceBotPattern> DiceBotPatterns => diceBotPatterns;
      public string DisplayName => displayName;
   }
}