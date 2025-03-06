using UnityEngine;

namespace DiceBotsGame.DiceBots.Dices.Faces {
   public class CharacterDiceFace : MonoBehaviour {
      [SerializeField] protected MeshRenderer[] emissiveRenderers;
      [SerializeField] protected MeshRenderer faceActionRenderer;
      [SerializeField] protected MeshRenderer faceValueRenderer;
      [SerializeField] protected CharacterDiceFaceData data;
      [SerializeField] protected CharacterDiceFaceMeshConfig meshConfig;

      public CharacterDiceFaceData Data => data;
      public bool Playable => data.HasCombatAction;

      public void SetUp(CharacterDiceFaceData data, Material emissiveMaterial) {
         this.data = new CharacterDiceFaceData(data);

         foreach (var emissiveRenderer in emissiveRenderers) {
            emissiveRenderer.material = emissiveMaterial;
         }

         faceActionRenderer.enabled = data.HasCombatAction;
         faceValueRenderer.enabled = data.HasCombatAction;
         if (faceActionRenderer.enabled) {
            faceActionRenderer.GetComponent<MeshFilter>().mesh = data.CombatAction.Action.Mesh;
         }
         if (faceValueRenderer.enabled) {
            var mesh = meshConfig.Digit(data.CombatAction.ConstantStrength);

            faceValueRenderer.GetComponent<MeshFilter>().mesh = mesh;
            faceValueRenderer.enabled = mesh;
         }
      }
   }
}