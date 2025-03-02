using UnityEngine;

namespace DiceBotsGame.DiceBots.Dices.Faces {
   public class CharacterDiceFace : MonoBehaviour {
      [SerializeField] protected MeshRenderer[] emissiveRenderers;
      [SerializeField] protected MeshRenderer faceActionRenderer;
      [SerializeField] protected CharacterDiceFaceData data;
      [SerializeField] protected Transform valueContainer;

      public CharacterDiceFaceData Data => data;
      public bool Playable => data.HasCombatAction;
      public Transform ValueContainer => valueContainer;

      public void SetUp(CharacterDiceFaceData data, Material emissiveMaterial) {
         this.data = new CharacterDiceFaceData(data);

         foreach (var emissiveRenderer in emissiveRenderers) {
            emissiveRenderer.material = emissiveMaterial;
         }

         faceActionRenderer.enabled = data.HasCombatAction;
         if (faceActionRenderer.enabled) {
            faceActionRenderer.GetComponent<MeshFilter>().mesh = data.CombatActionMesh;
         }
      }
   }
}