using System.Collections.Generic;
using UnityEngine;

namespace DiceBotsGame.Utils {
   public static class Cubes {
      public const int FaceCount = 6;

      public static IReadOnlyList<Quaternion> faceRotations { get; } = new[] {
         Quaternion.Euler(0, 0, 0), Quaternion.Euler(90, 0, -90), Quaternion.Euler(90, 0, 0), Quaternion.Euler(90, -90, 0), Quaternion.Euler(270, 0, 0), Quaternion.Euler(180, 0, 0)
      };
   }
}