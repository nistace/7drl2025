using UnityEngine;

namespace DiceBotsGame.DiceBots.Dices.Faces {
   public class CharacterDiceFace : MonoBehaviour {
      [SerializeField] protected MeshRenderer[] emissiveRenderers;
      [SerializeField] protected CharacterDiceFaceData data;
      [SerializeField] protected Transform valueContainer;
      [SerializeField] protected Transform combatActionModelAnchor;

      public CharacterDiceFaceData Data => data;
      public bool Playable => data.HasCombatAction;
      public Transform ValueContainer => valueContainer;

      public void SetUp(CharacterDiceFaceData data, Material emissiveMaterial) {
         this.data = new CharacterDiceFaceData(data);
         for (var childIndex = 0; childIndex < combatActionModelAnchor.childCount; ++childIndex) {
            Destroy(combatActionModelAnchor.GetChild(childIndex).gameObject);
         }

         foreach (var emissiveRenderer in emissiveRenderers) {
            emissiveRenderer.material = emissiveMaterial;
         }

         if (data.HasCombatAction) {
            var combatModel = Instantiate(data.CombatActionModel, combatActionModelAnchor);
            combatModel.material = emissiveMaterial;
         }
      }
   }
}