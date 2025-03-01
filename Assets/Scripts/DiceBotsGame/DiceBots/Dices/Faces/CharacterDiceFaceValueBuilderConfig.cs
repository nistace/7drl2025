using System;
using UnityEngine;

namespace DiceBotsGame.DiceBots.Dices.Faces {
   [CreateAssetMenu]
   public class CharacterDiceFaceValueBuilderConfig : ScriptableObject {
      [SerializeField] protected BarInfo[] decreasingValueBars;

      [Serializable] public class BarInfo {
         [SerializeField] protected MeshRenderer prefab;
         [SerializeField] protected Vector3 offset;
         [SerializeField] protected int value = 1;

         public MeshRenderer Prefab => prefab;
         public Vector3 Offset => offset;
         public int Value => value;
      }

      public void Build(Transform targetContainer, int value, Material emissiveMaterial) {
         var remainingValue = value;
         var offset = Vector3.zero;
         foreach (var barInfo in decreasingValueBars) {
            while (remainingValue >= barInfo.Value) {
               var bar = Instantiate(barInfo.Prefab, targetContainer);
               bar.material = emissiveMaterial;
               bar.transform.localRotation = Quaternion.identity;
               bar.transform.localPosition = offset;
               bar.transform.localScale = Vector3.one;
               offset += barInfo.Offset;
               remainingValue -= barInfo.Value;
            }
         }
      }
   }
}