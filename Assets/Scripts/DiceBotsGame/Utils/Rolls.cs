using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DiceBotsGame.Utils {
   public static class Rolls {
      public static T Roll<T>(this IReadOnlyList<T> list) => list[Random.Range(0, list.Count)];
      public static T RollOrDefault<T>(this IReadOnlyList<T> list, T defaultValue = default) => list.Count == 0 ? defaultValue : list[Random.Range(0, list.Count)];
      public static T Roll<T>(this IEnumerable<T> list) => list.OrderBy(t => Random.value).First();
   }
}