using System;
using System.Collections.Generic;
using DiceBotsGame.DiceBots;
using DiceBotsGame.DiceBots.Dices.Faces;
using DiceBotsGame.Utils;
using UnityEngine;

namespace DiceBotsGame.BotUpgrades {
   [Serializable]
   public class PartyUpgrade {
      [SerializeField] protected BotUpgrade[] botUpgrades;
      [SerializeField] private int upgradeCountAllowed;

      public IReadOnlyList<BotUpgrade> BotUpgrades => botUpgrades;

      public int UpgradeCountAllowed => upgradeCountAllowed;

      public PartyUpgrade(BotUpgrade[] botUpgrades, int upgradeCountAllowed) {
         this.botUpgrades = botUpgrades;
         this.upgradeCountAllowed = upgradeCountAllowed;
      }

      public static PartyUpgrade GenerateForLevelUp(DiceBotsParty party) {
         var botUpgrades = new List<BotUpgrade>();
         foreach (var bot in party.DiceBotsInParty) {
            var faceUpgrades = new Dictionary<CharacterDiceFace, FaceUpgrade>();
            HealthUpgrade healthUpgrade = null;
            if (bot.HealthSystem.IsAlive) {
               foreach (var face in bot.Dice.Faces) {
                  var possibleUpgrades = bot.UpgradeInfo.GetActionUpgrades(face.Data.CombatAction);
                  if (possibleUpgrades.Count > 0) faceUpgrades.Add(face, new FaceUpgrade(face, possibleUpgrades.Roll()));
               }
               healthUpgrade = new HealthUpgrade(bot.HealthSystem, bot.HealthSystem.MaxHealth + bot.UpgradeInfo.LevelUpAdditionalHealth);
            }
            botUpgrades.Add(new BotUpgrade(bot, healthUpgrade, faceUpgrades));
         }
         return new PartyUpgrade(botUpgrades.ToArray(), 3);
      }
   }
}