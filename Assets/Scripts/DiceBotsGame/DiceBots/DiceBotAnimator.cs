using System;
using System.Collections.Generic;
using DiceBotsGame.Audio;
using UnityEngine;

namespace DiceBotsGame.DiceBots {
   public class DiceBotAnimator : MonoBehaviour {
      private static readonly int normalizedSpeedAnimParam = Animator.StringToHash("NormalizedSpeed");
      private static readonly int attachedAnimParam = Animator.StringToHash("Attached");
      private static readonly int takeDamageAnimParam = Animator.StringToHash("TakeDamage");
      private static readonly int deadAnimParam = Animator.StringToHash("Dead");
      [SerializeField] protected DiceBot diceBot;
      [SerializeField] protected SfxSource sfxSource;
      [SerializeField] protected Animator animator;
      [SerializeField] protected float attachDiceSpeed = 2f;

      private float smoothSpeed = 0;
      private bool hasMovedThisFrame = false;

      private Dictionary<string, HashSet<Action>> Callbacks { get; } = new Dictionary<string, HashSet<Action>>();

      private void Start() {
         diceBot.OnSetupComplete.AddListener(HandleSetupComplete);
         if (diceBot.IsSetUp) HandleSetupComplete();
      }

      private void HandleSetupComplete() {
         diceBot.OnSetupComplete.RemoveListener(HandleSetupComplete);

         diceBot.OnMovementDone.AddListener(HandleMovementDone);
         diceBot.HealthSystem.OnDamaged.AddListener(HandleDamaged);
         diceBot.HealthSystem.OnHealed.AddListener(HandleHealed);
      }

      private void OnDestroy() {
         diceBot.OnSetupComplete.RemoveListener(HandleSetupComplete);
         diceBot.OnMovementDone.RemoveListener(HandleMovementDone);
         diceBot.HealthSystem?.OnDamaged.RemoveListener(HandleDamaged);
         diceBot.HealthSystem?.OnHealed.RemoveListener(HandleHealed);
      }

      private void HandleDamaged(int damage) {
         animator.SetBool(deadAnimParam, diceBot.HealthSystem.IsDead);
         if (diceBot.HealthSystem.IsAlive) animator.SetTrigger(takeDamageAnimParam);
      }

      private void HandleHealed(int heal) {
         animator.SetBool(deadAnimParam, diceBot.HealthSystem.IsDead);
      }

      private void HandleMovementDone(float arg0) => hasMovedThisFrame = true;

      private void Update() {
         animator.SetBool(attachedAnimParam, diceBot.Dice.IsAttached);
         animator.SetFloat(normalizedSpeedAnimParam, smoothSpeed);
      }

      private void LateUpdate() {
         smoothSpeed = Mathf.Lerp(smoothSpeed, hasMovedThisFrame ? diceBot.Config.WorldMovementSpeed : 0, .1f);
         hasMovedThisFrame = false;
         if (diceBot.Dice.IsAttached) {
            diceBot.Dice.transform.localPosition = Vector3.MoveTowards(diceBot.Dice.transform.localPosition, Vector3.zero, attachDiceSpeed * Time.deltaTime);
         }
      }

      private void EventFootstep(AnimationEvent animationEvent) {
         if (smoothSpeed > .2f) sfxSource.Play(diceBot.Config.RandomFootstepClip, smoothSpeed * diceBot.Config.FootstepVolumeScale);
      }

      private void EventAnimCallback(string anim) {
         if (Callbacks.TryGetValue(anim, out var animCallbacks)) {
            foreach (var callback in animCallbacks) {
               callback?.Invoke();
            }
            Callbacks[anim].Clear();
         }
      }

      public void PlayAnimation(string animParam, Action callback = null) {
         animator.SetTrigger(animParam);
         if (callback != null) {
            if (!Callbacks.ContainsKey(animParam)) Callbacks.Add(animParam, new HashSet<Action>());
            Callbacks[animParam].Add(callback);
         }
      }
   }
}