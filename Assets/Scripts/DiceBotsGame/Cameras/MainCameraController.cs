using UnityEngine;

namespace DiceBotsGame.Cameras {
   public class MainCameraController : MonoBehaviour {
      private static MainCameraController instance { get; set; }
      public static Camera MainCamera => instance ? instance.mainCamera : default;

      [SerializeField] protected Camera mainCamera;

      private void Awake() {
         instance = this;
      }
   }
}