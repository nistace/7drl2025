using DiceBotsGame.CombatActions.AI;
using DiceBotsGame.DiceBots.Dices;
using UnityEngine;

namespace DiceBotsGame.DiceBots {
   [CreateAssetMenu]
   public class DiceBotPattern : ScriptableObject {
      [SerializeField] protected string displayName;
      [SerializeField] protected float size = 1;
      [SerializeField] protected CharacterDicePattern dicePattern;
      [SerializeField] protected Color[] metallicColors = new Color[] { Color.black };
      [SerializeField] protected Color color;
      [SerializeField] protected CombatAi combatAi;
      [SerializeField] protected DiceBotUpgradeInfo upgradeInfo;

      public string DisplayName => displayName;
      public CharacterDicePattern DicePattern => dicePattern;
      public Color Color => color;
      public CombatAi CombatAi => combatAi;
      public DiceBotUpgradeInfo UpgradeInfo => upgradeInfo;
      public Color[] MetallicColors => metallicColors;
      public float Size => size;
   }
}