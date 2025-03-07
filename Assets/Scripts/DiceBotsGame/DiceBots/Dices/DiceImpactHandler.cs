using DiceBotsGame.Audio;
using DiceBotsGame.Utils;
using UnityEngine;

namespace DiceBotsGame.DiceBots.Dices {
   [RequireComponent(typeof(Rigidbody))]
   public class DiceImpactHandler : MonoBehaviour {
      [SerializeField] protected Rigidbody diceBody;

      [SerializeField] protected AudioClip[] clips;
      [SerializeField] protected SfxSource sfxSource;
      [SerializeField] protected float maxImpulse = 40;
      [SerializeField] protected float volumeScale = 2;

      private void OnCollisionEnter(Collision other) {
         if (diceBody.isKinematic) return;
         sfxSource.Play(clips.Roll(), Mathf.Clamp01(volumeScale * other.impulse.magnitude / maxImpulse));
      }
   }
}