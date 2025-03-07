using UnityEngine;

namespace DiceBotsGame.Audio {
   [RequireComponent(typeof(AudioSource))]
   public class SfxSource : MonoBehaviour {
      public static float Volume { get; set; } = 1;
      [SerializeField] protected AudioSource source;

      private void Reset() {
         source = GetComponent<AudioSource>();
      }

      public void Play(AudioClip clip, float volumeScale) => source.PlayOneShot(clip, Volume * volumeScale);
      public void Play(AudioClip clip) => Play(clip, 1);
   }
}