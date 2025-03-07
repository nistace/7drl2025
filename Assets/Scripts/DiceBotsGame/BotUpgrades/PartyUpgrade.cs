using System;
using System.Collections.Generic;
using System.Linq;
using DiceBotsGame.DiceBots;
using DiceBotsGame.DiceBots.Dices.Faces;
using DiceBotsGame.Utils;
using UnityEngine;

namespace DiceBotsGame.BotUpgrades {
   [Serializable]
   public class PartyUpgrade {
      [SerializeField] private string displayTitle;
      [SerializeField] protected BotUpgrade[] botUpgrades;
      [SerializeField] private int upgradeCountAllowed;

      public string DisplayTitle => displayTitle;
      public IReadOnlyList<BotUpgrade> BotUpgrades => botUpgrades;
      public int UpgradeCountAllowed => upgradeCountAllowed;

      public PartyUpgrade(string displayTitle, BotUpgrade[] botUpgrades, int upgradeCountAllowed) {
         this.displayTitle = displayTitle;
         this.botUpgrades = botUpgrades;
         this.upgradeCountAllowed = upgradeCountAllowed;
      }

      public static PartyUpgrade GenerateForLevelUp(DiceBotsParty party, int encounterLevel) {
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
         return new PartyUpgrade("Level up!", botUpgrades.ToArray(), encounterLevel);
      }

      public static PartyUpgrade GenerateForSmith(DiceBotsParty party, int strength) {
         var botUpgrades = new List<BotUpgrade>();
         var selectableUpgrades = 0;
         foreach (var bot in party.DiceBotsInParty) {
            var faceUpgrades = new Dictionary<CharacterDiceFace, FaceUpgrade>();
            if (bot.HealthSystem.IsAlive) {
               foreach (var face in bot.Dice.Faces) {
                  var possibleUpgrades = bot.UpgradeInfo.GetActionUpgrades(face.Data.CombatAction);
                  if (possibleUpgrades.Count > 0) faceUpgrades.Add(face, new FaceUpgrade(face, possibleUpgrades.Roll()));
               }
            }
            if (faceUpgrades.Count > 0) {
               selectableUpgrades++;
            }
            botUpgrades.Add(new BotUpgrade(bot, null, new Dictionary<CharacterDiceFace, FaceUpgrade>(faceUpgrades.Take(1))));
         }
         return new PartyUpgrade("The DiceSmith", botUpgrades.ToArray(), selectableUpgrades);
      }
   }
}