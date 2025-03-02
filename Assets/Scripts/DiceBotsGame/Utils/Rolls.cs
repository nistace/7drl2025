using System.Collections.Generic;
using UnityEngine;

namespace DiceBotsGame.Utils {
   public static class Rolls {
      public static T Roll<T>(this IReadOnlyList<T> list) => list[Random.Range(0, list.Count)];
   }
}