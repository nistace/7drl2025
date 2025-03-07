using System;
using DiceBotsGame.CombatActions;
using DiceBotsGame.DiceBots.Dices.Faces;
using UnityEngine;
using UnityEngine.Events;

namespace DiceBotsGame.BotUpgrades {
   [Serializable]
   public class FaceUpgrade : IUpgrade {
      [SerializeField] private CharacterDiceFace face;
      [SerializeField] private CombatActionDefinition upgrade;
      
      bool IUpgrade.Selected { get; set; }
      public CharacterDiceFace Face => face;
      public CombatActionDefinition Upgrade => upgrade;
      public UnityEvent OnSelectedChanged { get; } = new UnityEvent();

      public FaceUpgrade() { }

      public FaceUpgrade(CharacterDiceFace face, CombatActionDefinition upgrade) {
         this.face = face;
         this.upgrade = upgrade;
      }

      public void Apply() => face.ChangeFace(upgrade);
   }
}