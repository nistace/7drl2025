using UnityEngine;

namespace DiceBotsGame.WorldLevels.Activities {
   public class WorldCubeTileActivity : MonoBehaviour {
      public enum EType {
         None = 0,
         ExitFace = 1,
         EnterFace = 2,
      }

      [SerializeField] protected EType type;
      [SerializeField] protected OptionalActivityInfo optionalActivityInfo;

      public bool Solved { get; set; }
      public EType Type => type;
      public bool IsOptional(out OptionalActivityInfo optional) => optional = optionalActivityInfo;
   }
}