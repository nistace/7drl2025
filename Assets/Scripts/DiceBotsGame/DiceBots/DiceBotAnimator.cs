using DiceBotsGame.Audio;
using UnityEngine;

namespace DiceBotsGame.DiceBots {
   public class DiceBotAnimator : MonoBehaviour {
      private static readonly int normalizedSpeedAnimParam = Animator.StringToHash("NormalizedSpeed");
      private static readonly int attachedAnimParam = Animator.StringToHash("Attached");
      [SerializeField] protected DiceBot diceBot;
      [SerializeField] protected SfxSource sfxSource;
      [SerializeField] protected Animator animator;
      [SerializeField] protected float attachDiceSpeed = 2f;

      private float smoothSpeed = 0;
      private bool hasMovedThisFrame = false;

      private void Start() {
         diceBot.OnMovementDone.AddListener(HandleMovementDone);
      }

      private void HandleMovementDone(float arg0) => hasMovedThisFrame = true;

      private void Update() {
         animator.SetBool(attachedAnimParam, diceBot.Dice.IsAttached);
         animator.SetFloat(normalizedSpeedAnimParam, smoothSpeed);
      }

      private void LateUpdate() {
         smoothSpeed = Mathf.Lerp(smoothSpeed, hasMovedThisFrame ? diceBot.Config.WorldMovementSpeed : 0, .3f);
         hasMovedThisFrame = false;
         if (diceBot.Dice.IsAttached) {
            diceBot.Dice.transform.localPosition = Vector3.MoveTowards(diceBot.Dice.transform.localPosition, Vector3.zero, attachDiceSpeed * Time.deltaTime);
         }
      }

      private void EventFootstep(AnimationEvent animationEvent) {
         if (smoothSpeed > .2f) sfxSource.Play(diceBot.Config.RandomFootstepClip, smoothSpeed * diceBot.Config.FootstepVolumeScale);
      }
   }
}