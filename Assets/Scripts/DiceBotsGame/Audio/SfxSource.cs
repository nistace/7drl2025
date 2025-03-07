using UnityEngine;

namespace DiceBotsGame.Audio {
   [RequireComponent(typeof(AudioSource))]
   public class SfxSource : MonoBehaviour {
      public static float Volume { get; set; } = 1;
      [SerializeField] protected AudioSource source;

      public void Play(AudioClip clip, float volumeScale) => source.PlayOneShot(clip, Volume * volumeScale);
   }
}