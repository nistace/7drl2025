using Unity.Cinemachine;
using UnityEngine;

namespace DiceBotsGame.Cameras {
   public class MainCameraController : MonoBehaviour {
      private static MainCameraController instance { get; set; }
      public static Camera MainCamera => instance ? instance.mainCamera : default;
      public static bool AtAnchorPosition => !instance.cinemachineBrain.IsBlending;

      [SerializeField] private CinemachineBrain cinemachineBrain;
      [SerializeField] protected CinemachineCamera worldCamera;
      [SerializeField] protected Transform worldCameraTargetTransform;
      [SerializeField, Range(0, 1)] protected float worldDistanceRatioBetweenCenterAndTarget;
      [SerializeField] protected Camera mainCamera;

      private CinemachineCamera currentCamera;
      public static Transform worldTarget { get; set; }
      public static Vector3 worldCenterPosition { get; set; }

      private void Awake() {
         instance = this;
      }

      public static void ActivateCamera(CinemachineCamera camera) {
         if (instance.currentCamera) instance.currentCamera.enabled = false;
         instance.currentCamera = camera ? camera : instance.worldCamera;
         camera.enabled = true;
      }

      public static void ActivateWorldCamera() {
         ActivateCamera(instance.worldCamera);
      }

      private void LateUpdate() {
         if (worldTarget) {
            worldCameraTargetTransform.position = Vector3.Lerp(worldCenterPosition, worldTarget.position, worldDistanceRatioBetweenCenterAndTarget);
         }
      }
   }
}