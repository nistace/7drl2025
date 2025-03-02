using System.Collections;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;

namespace DiceBotsGame.CombatActions {
   public interface ICombatActionEffect {
      IEnumerator Execute(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile, int value);
   }
}