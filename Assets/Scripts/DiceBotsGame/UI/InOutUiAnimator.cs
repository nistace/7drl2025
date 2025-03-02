using System;
using System.Collections;
using DiceBotsGame.Utils;
using UnityEngine;

namespace DiceBotsGame.UI {
   [RequireComponent(typeof(RectTransform))]
   [RequireComponent(typeof(CanvasGroup))]
   public class InOutUiAnimator : MonoBehaviour {
      [SerializeField] protected RectTransform rectTransform;
      [SerializeField] protected CanvasGroup canvasGroup;
      [SerializeField] protected PositionInfo inPosition;
      [SerializeField] protected PositionInfo outPosition;
      [SerializeField] protected float smoothMovement = .5f;

      private PositionInfo targetPosition;
      private Coroutine animationRoutine;

      private void Reset() {
         rectTransform = GetComponent<RectTransform>();
         canvasGroup = GetComponent<CanvasGroup>();
         if (rectTransform) {
            inPosition = new PositionInfo(rectTransform.anchorMin, rectTransform.anchorMax, rectTransform.offsetMin, rectTransform.offsetMax, 1);
            outPosition = new PositionInfo(rectTransform.anchorMin + Vector2.down, rectTransform.anchorMax + Vector2.down, rectTransform.offsetMin, rectTransform.offsetMax, 0);
         }
      }

      public void ChangeTargetPosition(bool moveIn) {
         targetPosition = moveIn ? inPosition : outPosition;
         animationRoutine ??= StartCoroutine(DoAnimate());
      }

      public void Enter() => ChangeTargetPosition(true);
      public void Exit() => ChangeTargetPosition(false);

      public void SnapIn() {
         targetPosition = inPosition;
         targetPosition.Snap(rectTransform, canvasGroup);
      }

      public void SnapOut() {
         targetPosition = outPosition;
         targetPosition.Snap(rectTransform, canvasGroup);
      }

      private IEnumerator DoAnimate() {
         yield return null;
         var anchorMinMovement = Vector2.zero;
         var anchorMaxMovement = Vector2.zero;
         var offsetMinMovement = Vector2.zero;
         var offsetMaxMovement = Vector2.zero;
         var opacityMovement = 0f;
         while (!targetPosition.IsApproximatelyThere(rectTransform, canvasGroup)) {
            rectTransform.anchorMin = Vector2.SmoothDamp(rectTransform.anchorMin, targetPosition.AnchorMin, ref anchorMinMovement, smoothMovement);
            rectTransform.anchorMax = Vector2.SmoothDamp(rectTransform.anchorMax, targetPosition.AnchorMax, ref anchorMaxMovement, smoothMovement);
            rectTransform.offsetMin = Vector2.SmoothDamp(rectTransform.offsetMin, targetPosition.OffsetMin, ref offsetMinMovement, smoothMovement);
            rectTransform.offsetMax = Vector2.SmoothDamp(rectTransform.offsetMax, targetPosition.OffsetMax, ref offsetMaxMovement, smoothMovement);
            canvasGroup.alpha = Mathf.SmoothDamp(canvasGroup.alpha, targetPosition.CanvasGroupAlpha, ref opacityMovement, smoothMovement);
            canvasGroup.interactable = canvasGroup.alpha >= targetPosition.MinAlphaToBeInteractable;
            canvasGroup.blocksRaycasts = canvasGroup.alpha >= targetPosition.MinAlphaToBlockRaycast;
            yield return null;
         }

         targetPosition.Snap(rectTransform, canvasGroup);
         animationRoutine = null;
      }

      [Serializable] public class PositionInfo {
         [SerializeField] protected Vector2 anchorMin;
         [SerializeField] protected Vector2 anchorMax = Vector2.one;
         [SerializeField] protected Vector2 offsetMin;
         [SerializeField] protected Vector2 offsetMax;
         [SerializeField] protected float canvasGroupAlpha = 1;
         [SerializeField] protected float minAlphaToBlockRaycast = .5f;
         [SerializeField] protected float minAlphaToBeInteractable = 1;

         public Vector2 AnchorMin => anchorMin;
         public Vector2 AnchorMax => anchorMax;
         public Vector2 OffsetMin => offsetMin;
         public Vector2 OffsetMax => offsetMax;
         public float CanvasGroupAlpha => canvasGroupAlpha;
         public float MinAlphaToBlockRaycast => minAlphaToBlockRaycast;
         public float MinAlphaToBeInteractable => minAlphaToBeInteractable;

         public PositionInfo() { }

         public PositionInfo(Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax, float alpha) {
            this.anchorMin = anchorMin;
            this.anchorMax = anchorMax;
            this.offsetMin = offsetMin;
            this.offsetMax = offsetMax;
            canvasGroupAlpha = alpha;
         }

         public void Snap(RectTransform rectTransform, CanvasGroup canvasGroup) {
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.offsetMin = offsetMin;
            rectTransform.offsetMax = offsetMax;
            canvasGroup.alpha = canvasGroupAlpha;
            canvasGroup.interactable = canvasGroupAlpha >= minAlphaToBeInteractable;
            canvasGroup.blocksRaycasts = canvasGroupAlpha >= minAlphaToBlockRaycast;
         }

         public bool IsApproximatelyThere(RectTransform rectTransform, CanvasGroup canvasGroup) {
            return rectTransform.anchorMin.x.Approximately(anchorMin.x)
                   && rectTransform.anchorMin.y.Approximately(anchorMin.y)
                   && rectTransform.anchorMax.x.Approximately(anchorMax.x)
                   && rectTransform.anchorMax.y.Approximately(anchorMax.y)
                   && rectTransform.offsetMin.x.Approximately(offsetMin.x)
                   && rectTransform.offsetMin.y.Approximately(offsetMin.y)
                   && rectTransform.offsetMax.x.Approximately(offsetMax.x)
                   && rectTransform.offsetMax.y.Approximately(offsetMax.y)
                   && canvasGroup.alpha.Approximately(canvasGroupAlpha);
         }
      }
   }
}