using System;
using System.Collections;
using System.Collections.Generic;

namespace DiceBotsGame.Utils {
   public class Map<T1, T2> : IEnumerable<KeyValuePair<T1, T2>> {
      private readonly Dictionary<T1, T2> forward = new Dictionary<T1, T2>();
      private readonly Dictionary<T2, T1> reverse = new Dictionary<T2, T1>();

      public T2 this[T1 t1] => forward[t1];
      public T1 this[T2 t2] => reverse[t2];

      public void Add(T1 t1, T2 t2) {
         if (forward.ContainsKey(t1) || reverse.ContainsKey(t2)) throw new ArgumentException();

         forward.Add(t1, t2);
         reverse.Add(t2, t1);
      }

      public void Remove(T1 key) {
         if (!TryGetValue(key, out var value)) throw new ArgumentException();

         reverse.Remove(value);
         forward.Remove(key);
      }

      public void Remove(T2 key) {
         if (!TryGetValue(key, out var value)) throw new ArgumentException();

         forward.Remove(value);
         reverse.Remove(key);
      }

      public bool TryGetValue(T1 key, out T2 value) => forward.TryGetValue(key, out value);
      public bool TryGetValue(T2 key, out T1 value) => reverse.TryGetValue(key, out value);

      public bool ContainsKey(T1 key) => forward.ContainsKey(key);
      public bool ContainsKey(T2 key) => reverse.ContainsKey(key);

      public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator() => forward.GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

      public void Clear() {
         forward.Clear();
         reverse.Clear();
      }
   }
}