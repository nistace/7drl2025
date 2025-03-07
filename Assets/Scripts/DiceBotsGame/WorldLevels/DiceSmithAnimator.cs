using DiceBotsGame.Audio;
using DiceBotsGame.DiceBots.Dices;
using DiceBotsGame.GameSates;
using UnityEngine;

namespace DiceBotsGame.WorldLevels {
   public class DiceSmithAnimator : MonoBehaviour {
      [SerializeField] protected WorldCubeTileActivity activity;
      [SerializeField] protected SfxSource sfxSource;
      [SerializeField] protected AudioClip hitAnvilAudioClip;
      [SerializeField] protected CharacterDice dice;

      private void Start() {
         activity.OnSolvedChanged.AddListener(HandleOnSolvedChanged);
      }

      private void HandleOnSolvedChanged() {
         if (activity.Solved) {
            dice.DetachFromBody();
         }
         else {
            dice.AttachToBody();
         }
      }

      private void EventHitAnvil() {
         if (GameInfo.WorldCube.CurrentFaceIndex == activity.Face) {
            sfxSource.Play(hitAnvilAudioClip);
         }
      }
   }
}