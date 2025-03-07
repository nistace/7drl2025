using DiceBotsGame.CombatActions;
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

      public void SetUp(CombatActionDefinition action, Material emissiveMaterial) {
         foreach (var emissiveRenderer in emissiveRenderers) {
            emissiveRenderer.material = emissiveMaterial;
         }

         ChangeFace(action);
      }

      public void ChangeFace(CombatActionDefinition combatAction) {
         data = new CharacterDiceFaceData(combatAction);

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