using UnityEngine;

namespace DiceBotsGame.Audio {
   [RequireComponent(typeof(AudioSource))]
   public class AudioManager : MonoBehaviour {
      private static AudioManager instance { get; set; }

      [SerializeField] protected AudioSource musicSource;

      private void Awake() {
         if (instance) Destroy(gameObject);
         else {
            instance = this;
            DontDestroyOnLoad(gameObject);
         }
      }

      public static float MusicVolume {
         get => instance.musicSource.volume;
         set => instance.musicSource.volume = value;
      }

      public static float SfxVolume {
         get => SfxSource.Volume;
         set => SfxSource.Volume = value;
      }
   }
}