using System.Collections.Generic;
using UnityEngine;

namespace DiceBotsGame.UI.Health {
   public class HealthBarUi : MonoBehaviour {
      private readonly List<HealthPointUi> healthPoints = new List<HealthPointUi>();

      public void Refresh(int current, int max) {
         while (healthPoints.Count > max) {
            HealthBarManager.Release(healthPoints[0]);
            healthPoints.RemoveAt(0);
         }
         while (healthPoints.Count < max) {
            healthPoints.Add(HealthBarManager.Get(transform));
         }

         for (var i = 0; i < healthPoints.Count; ++i) {
            HealthBarManager.SetFull(healthPoints[i], i < current);
         }
      }
   }
}