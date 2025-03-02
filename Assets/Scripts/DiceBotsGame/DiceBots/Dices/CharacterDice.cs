using System.Collections.Generic;
using System.Linq;
using DiceBotsGame.CombatActions;
using DiceBotsGame.DiceBots.Dices.Faces;
using DiceBotsGame.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace DiceBotsGame.DiceBots.Dices {
   public class CharacterDice : MonoBehaviour {
      [SerializeField] protected Rigidbody diceBody;
      [SerializeField] protected CharacterDiceData data;
      [SerializeField] protected CharacterDiceFace[] faces = new CharacterDiceFace[Cubes.FaceCount];
      [SerializeField] protected Transform facesContainer;
      public CharacterDiceFace LastRolledFace { get; private set; }
      public IReadOnlyList<CombatActionDefinition> CoreActions => data.CoreCombatActions;
      public bool IsAttached => diceBody.isKinematic;
      public bool IsRolling => !IsAttached && (!diceBody.linearVelocity.sqrMagnitude.Approximately(0) || !diceBody.angularVelocity.sqrMagnitude.Approximately(0));
      public bool IsStuckWhileRolling => !IsAttached && !IsRolling && !HasValidRolledFace();
      public CharacterDiceData Data => data;

      public UnityEvent OnStartedRolling = new UnityEvent();
      public UnityEvent<CharacterDiceFace> OnSavedRolledFace { get; } = new UnityEvent<CharacterDiceFace>();

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

      public void Roll(Vector3 force, Vector3 torque) {
         diceBody.isKinematic = false;
         diceBody.AddForce(force, ForceMode.Impulse);
         diceBody.AddTorque(torque, ForceMode.Impulse);
         OnStartedRolling.Invoke();
      }

      public void AttachToBody() {
         diceBody.isKinematic = true;
      }

      public bool HasValidRolledFace() => faces.Min(t => Vector3.Angle(t.transform.up, Vector3.up)) < 5f;

      public void SaveRolledFace() {
         LastRolledFace = faces.OrderBy(t => Vector3.Angle(t.transform.up, Vector3.up)).First();
         OnSavedRolledFace.Invoke(LastRolledFace);
      }
   }
}