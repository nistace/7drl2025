using System.Linq;
using DiceBotsGame.DiceBots.Dices.Faces;
using DiceBotsGame.Utils;
using UnityEngine;

namespace DiceBotsGame.DiceBots.Dices {
   public class CharacterDice : MonoBehaviour {
      [SerializeField] protected Rigidbody diceBody;
      [SerializeField] protected CharacterDiceData data;
      [SerializeField] protected CharacterDiceFace[] faces = new CharacterDiceFace[Cubes.FaceCount];
      [SerializeField] protected Transform facesContainer;
      public bool IsAttached => diceBody.isKinematic;
      public bool IsRolling => !IsAttached && !diceBody.linearVelocity.sqrMagnitude.Approximately(0);

      public void SetUp(CharacterDiceData data, CharacterDiceFace[] faces) {
         this.data = new CharacterDiceData(data);

         for (var i = 0; i < faces.Length; ++i) {
            if (this.faces[i]) {
               Destroy(this.faces[i].gameObject);
            }
         }

         this.faces = faces;

         for (var i = 0; i < faces.Length; ++i) {
            var faceTransform = faces[i].transform;
            faceTransform.SetParent(facesContainer);
            faceTransform.localPosition = Vector3.zero;
            faceTransform.localRotation = Cubes.faceRotations[i];
            faceTransform.localScale = Vector3.one;
         }
      }

      public int EvaluateMaxHealth() {
         return data.CoreHealth + faces.Sum(t => t.Data.HealthPoints);
      }

      public void Roll(Vector3 force, Vector3 torque) {
         diceBody.isKinematic = false;
         diceBody.AddForce(force, ForceMode.Impulse);
         diceBody.AddTorque(torque, ForceMode.Impulse);
      }

      public void AttachToBody() {
         diceBody.isKinematic = true;
      }
   }
}