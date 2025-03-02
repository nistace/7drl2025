using System.Collections.Generic;
using UnityEngine;

namespace DiceBotsGame.UI {
   public class HealthBarManager : MonoBehaviour {
      private static HealthBarManager Instance { get; set; }

      [SerializeField] protected HealthPointUi healthPointPrefab;
      [SerializeField] protected Color fullHealthPointColor = new Color(.8f, 0, 0);
      [SerializeField] protected Color emptyHealthPointColor = new Color(.3f, .3f, .3f);

      private Queue<HealthPointUi> Pool { get; } = new Queue<HealthPointUi>();

      private void Awake() {
         Instance = this;
         gameObject.SetActive(false);
      }

      private void OnDestroy() {
         if (Instance == this) Instance = null;
      }

      public static HealthPointUi Get(Transform parent) {
         var image = (Instance.Pool.Count == 0) ? Instantiate(Instance.healthPointPrefab, parent) : Instance.Pool.Dequeue();

         image.transform.SetParent(parent);
         return image;
      }

      public static void Release(HealthPointUi point) {
         if (Instance.Pool.Contains(point)) return;
         point.transform.SetParent(Instance.transform);
         Instance.Pool.Enqueue(point);
      }

      public static void SetFull(HealthPointUi healthPoint, bool full) => healthPoint.Color = full ? Instance.fullHealthPointColor : Instance.emptyHealthPointColor;
   }
}