using UnityEngine;

namespace DiceBotsGame.DiceBots {
   public class DiceBotAnimator : MonoBehaviour {
      private static readonly int normalizedSpeedAnimParam = Animator.StringToHash("NormalizedSpeed");
      private static readonly int attachedAnimParam = Animator.StringToHash("Attached");
      [SerializeField] protected DiceBot diceBot;
      [SerializeField] protected Animator animator;
      [SerializeField] protected float attachDiceSpeed = 2f;

      private Vector3 previousPosition;

      private void Update() {
         var diceBotTransform = diceBot.transform;
         animator.SetBool(attachedAnimParam, diceBot.Dice.IsAttached);

         var normalizedSpeed = (diceBotTransform.position == previousPosition) ? 0 : (diceBotTransform.position - previousPosition).magnitude / Time.deltaTime / diceBot.Config.WorldMovementSpeed;
         previousPosition = diceBotTransform.position;
         animator.SetFloat(normalizedSpeedAnimParam, normalizedSpeed);
      }

      private void LateUpdate() {
         if (diceBot.Dice.IsAttached) {
            diceBot.Dice.transform.localPosition = Vector3.MoveTowards(diceBot.Dice.transform.localPosition, Vector3.zero, attachDiceSpeed * Time.deltaTime);
         }
      }
   }
}