using System;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;

namespace DiceBotsGame.CombatActions.Effects {
   public static class CombatEffectHelper {
      public static DiceBot GetTarget(CombatGrid combatGrid, DiceBot self, CombatGridTile targetedTile, ETarget target) => target switch {
         ETarget.Self => self,
         ETarget.BotOnTile => combatGrid.GetDiceBotAtPosition(targetedTile),
         _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
      };
   }
}