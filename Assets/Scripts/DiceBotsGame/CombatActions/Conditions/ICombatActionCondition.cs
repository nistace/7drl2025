using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;

namespace DiceBotsGame.CombatActions.Conditions {
   public interface ICombatActionCondition {
      bool Check(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile, int value);
   }
}