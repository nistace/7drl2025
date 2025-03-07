using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DiceBotsGame.DiceBots;
using DiceBotsGame.DiceBots.Dices.Faces;
using UnityEngine;

namespace DiceBotsGame.BotUpgrades {
   [Serializable]
   public class BotUpgrade {
      [SerializeField] private DiceBot bot;
      [SerializeField] private HealthUpgrade healthUpgrade;
      [SerializeField] private SerializedDictionary<CharacterDiceFace, FaceUpgrade> faceUpgrades;

      public DiceBot Bot => bot;
      public HealthUpgrade HealthUpgrade => healthUpgrade;
      public bool HasHealthUpgrade => healthUpgrade != null && healthUpgrade.NewHealth > bot.HealthSystem.MaxHealth;
      public IReadOnlyDictionary<CharacterDiceFace, FaceUpgrade> FaceUpgrades => faceUpgrades;

      public BotUpgrade(DiceBot bot, HealthUpgrade healthUpgrade, IReadOnlyDictionary<CharacterDiceFace, FaceUpgrade> faceUpgrades) {
         this.bot = bot;
         this.healthUpgrade = healthUpgrade;
         this.faceUpgrades = new SerializedDictionary<CharacterDiceFace, FaceUpgrade>(faceUpgrades);
      }
   }
}