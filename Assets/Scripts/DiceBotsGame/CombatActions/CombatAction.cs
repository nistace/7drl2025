using System.Collections.Generic;
using System.Linq;
using DiceBotsGame.CombatActions.Conditions;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine;

namespace DiceBotsGame.CombatActions {
   public class CombatAction : MonoBehaviour {
      [SerializeField] protected Sprite sprite;
      [SerializeField] protected MeshRenderer model;

      public MeshRenderer Model => model;
      public Sprite Sprite => sprite;

      public IReadOnlyList<ICombatActionEffect> Effects => GetComponents<ICombatActionEffect>();
      private IReadOnlyList<ICombatActionCondition> Conditions => GetComponents<ICombatActionCondition>();

      public bool CheckConditions(CombatGrid grid, DiceBot actor, CombatGridTile tile, int value) => Conditions.All(t => t.Check(grid, actor, tile, value));
   }
}