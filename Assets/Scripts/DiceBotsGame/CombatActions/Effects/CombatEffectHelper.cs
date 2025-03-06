using System;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;

namespace DiceBotsGame.CombatActions.Effects {
   public static class CombatEffectHelper {
      public static DiceBot GetTarget(CombatGrid combatGrid, DiceBot self, CombatGridTile targetedTile, EEffectTarget target) => target switch {
         EEffectTarget.Self => self,
         EEffectTarget.BotOnTile => combatGrid.GetDiceBotAtPosition(targetedTile),
         _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
      };
   }
}