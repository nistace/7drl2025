using UnityEngine;

namespace DiceBotsGame.Utils {
   public static class NumberUtils {
      public static bool Approximately(this float a, float b, float tolerance = .001f) => Mathf.Abs(a - b) <= tolerance;
   }
}