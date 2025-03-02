using System;
using System.Collections;
using UnityEngine;

namespace DiceBotsGame.Visuals3D.TileHighlights {
   [RequireComponent(typeof(MeshRenderer))]
   public class TileHighlight : MonoBehaviour {
      private static readonly int emissionColorShaderId = Shader.PropertyToID("_EmissionColor");
      [SerializeField] protected MeshRenderer[] meshRenderers;
      [SerializeField] protected float changeHighlightSpeed = 2;

      [NonSerialized]
      private Material material;
      [NonSerialized]
      private Color originColor;
      [NonSerialized]
      private Color targetColor;
      [NonSerialized]
      private float changeProgress;
      [NonSerialized]
      private Coroutine changeColorRoutine;

      private void Reset() {
         meshRenderers = GetComponentsInChildren<MeshRenderer>();
      }

      public void ChangeHighlight(TileHighlightInfo info, bool snap = false) => ChangeHighlight(info.Color, info.Intensity, snap);

      public void ChangeHighlight(Color color, float intensity, bool snap = false) {
         if (!material) {
            material = new Material(meshRenderers[0].material);
            foreach (var meshRenderer in meshRenderers) {
               meshRenderer.material = material;
            }
         }

         targetColor = color * intensity;
         originColor = material.GetColor(emissionColorShaderId);
         if (snap) {
            material.SetColor(emissionColorShaderId, targetColor);
            changeProgress = 1;
         }
         else if (targetColor != originColor) {
            changeProgress = 0;
            changeColorRoutine ??= StartCoroutine(DoChangeHighlight());
         }
      }

      private IEnumerator DoChangeHighlight() {
         while (changeProgress < 1) {
            changeProgress += Time.deltaTime * changeHighlightSpeed;
            material.SetColor(emissionColorShaderId, Color.Lerp(originColor, targetColor, changeProgress));
            yield return null;
         }
         changeColorRoutine = null;
      }
   }
}