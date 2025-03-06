using System.Collections;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine.Events;

namespace DiceBotsGame.CombatActions.Effects {
   public interface ICombatActionEffect {
      IEnumerator Execute(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile, int value, UnityAction<int> outputValueCallback);
   }
}