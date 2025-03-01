using System.Collections.Generic;
using System.Linq;
using DiceBotsGame.WorldLevels;
using UnityEngine;

namespace DiceBotsGame.DiceBots {
   public class DiceBotsParty : MonoBehaviour {
      [SerializeField] private Transform[] diceBotSlots;

      public WorldCubeTile CurrentTile { get; set; }
      private readonly List<DiceBot> diceBotsInParty = new List<DiceBot>();

      public bool IsFull => diceBotsInParty.Count == diceBotSlots.Length;
      public bool AllBotsAtWorldTarget => diceBotsInParty.All(t=> t.AtWorldTarget);

      public void AddToParty(DiceBot diceBot) {
         diceBotsInParty.Add(diceBot);
         diceBot.SetWorldTargetPosition(diceBotSlots[diceBotsInParty.Count - 1]);
      }

      public void UpdateBotsWorldPosition() {
         foreach (var bot in diceBotsInParty) {
            bot.UpdateWorldPosition();
         }
      }
   }
}