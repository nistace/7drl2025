using System.Collections;
using DiceBotsGame.Audio;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine;
using UnityEngine.Events;
using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;

namespace DiceBotsGame.CombatActions.Effects {
   [RequireComponent(typeof(CombatAction))]
   public class DamageCombatActionEffect : MonoBehaviour, ICombatActionEffect {
      private enum EOutputValue {
         Unchanged = 0,
         DealtDamage = 1
      }

      [SerializeField] private ETarget target = ETarget.BotOnTile;
      [SerializeField] private EOutputValue outputValue = EOutputValue.Unchanged;
      [SerializeField] private AudioClip actorClipOnHit;

      public IEnumerator Execute(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile, int value, UnityAction<int> outputValueCallback) {
         var bot = CombatEffectHelper.GetTarget(combatGrid, actor, targetTile, target);

         bot.Reassemble();
         actor.Reassemble();
         actor.transform.LookAt(bot.transform);

         var hit = false;
         actor.GetComponent<DiceBotAnimator>().PlayAnimation("Attack", () => hit = true);

         for (var ttl = 0f; ttl < 2 && !hit; ttl += Time.deltaTime) {
            yield return null;
         }

         if (actorClipOnHit && actor.TryGetComponent(out SfxSource sfxSource)) {
            sfxSource.Play(actorClipOnHit);
         }

         var dealtDamage = bot.HealthSystem.Damage(value);

         yield return null;

         outputValueCallback?.Invoke(outputValue switch {
            EOutputValue.Unchanged => value,
            EOutputValue.DealtDamage => dealtDamage,
            _ => throw new ArgumentOutOfRangeException()
         });
      }
   }
}