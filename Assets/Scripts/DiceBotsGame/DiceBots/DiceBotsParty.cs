using System.Collections.Generic;
using System.Linq;
using DiceBotsGame.WorldLevels;
using UnityEngine;

namespace DiceBotsGame.DiceBots {
   public class DiceBotsParty : MonoBehaviour {
      [SerializeField] private Transform[] diceBotSlots;

      public WorldCubeTile CurrentTile { get; private set; }
      private readonly List<DiceBot> diceBotsInParty = new List<DiceBot>();

      public bool IsFull => diceBotsInParty.Count == diceBotSlots.Length;
      public bool AllBotsAtWorldTarget => diceBotsInParty.All(t => t.AtWorldTarget);

      public void AddToParty(DiceBot diceBot) {
         diceBotsInParty.Add(diceBot);
         diceBot.SetWorldTargetPosition(diceBotSlots[diceBotsInParty.Count - 1]);
      }

      public void UpdateBotsWorldPosition() {
         foreach (var bot in diceBotsInParty) {
            bot.UpdateWorldPosition();
         }
      }

      public void SnapBotsToWorldPosition() {
         foreach (var bot in diceBotsInParty) {
            bot.SnapToWorldTarget();
         }
      }

      public void SetWorldPosition(WorldCubeTile newTile) {
         if (CurrentTile == newTile) return;

         transform.rotation = newTile.ApplyAnchorForwardToPlayer ? newTile.PlayerAnchorTransform.rotation :
            CurrentTile ? Quaternion.LookRotation(newTile.PlayerAnchorTransform.position - transform.position, Vector3.up) : Quaternion.identity;
         transform.position = newTile.PlayerAnchorTransform.position;
         CurrentTile = newTile;
      }
   }
}