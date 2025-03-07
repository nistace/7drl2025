using System.Collections.Generic;
using System.Linq;
using DiceBotsGame.DiceBots.Dices;
using DiceBotsGame.DiceBots.Dices.Faces;
using DiceBotsGame.WorldLevels;
using UnityEngine;

namespace DiceBotsGame.DiceBots {
   public class DiceBotFactory : MonoBehaviour {
      [SerializeField] protected DiceBotsParty partyPrefab;
      [SerializeField] protected DiceBot botPrefab;
      [SerializeField] protected CharacterDice dicePrefab;
      [SerializeField] protected CharacterDiceFace facePrefab;
      [SerializeField] protected Material emissiveMaterial;
      [SerializeField] protected Material metallicMaterial;

      private static DiceBotFactory instance { get; set; }
      public bool IsReady => instance;

      private void Awake() {
         instance = this;
      }

      public static DiceBot Instantiate(DiceBotPattern botPattern) {
         var dicePattern = botPattern.DicePattern;
         var color = botPattern.Color;

         var diceBot = Instantiate(instance.botPrefab);
         var dice = Instantiate(instance.dicePrefab);
         var diceBotEmissiveMaterial = DiceBotEmissiveMaterial.Instantiate(instance.emissiveMaterial, color, 1);
         var metallicMaterials = botPattern.MetallicColors.Select(t => new Material(instance.metallicMaterial) { color = t }).ToArray();

         var faces = new List<CharacterDiceFace>();
         foreach (var faceAction in dicePattern.FaceActions) {
            var face = Instantiate(instance.facePrefab);
            face.SetUp(faceAction, diceBotEmissiveMaterial.Material);

            faces.Add(face);
         }

         diceBot.Dice.SetUp(dicePattern.Data, faces.ToArray());
         diceBot.SetUp(botPattern, diceBotEmissiveMaterial, metallicMaterials);
         return diceBot;
      }

      public static DiceBotsParty InstantiateParty(WorldCubeTile spawnTile, bool snapBots, params DiceBot[] bots) {
         var party = Instantiate(instance.partyPrefab, spawnTile.transform.position, Quaternion.identity);
         party.SetWorldPosition(spawnTile);
         foreach (var bot in bots) {
            party.AddToParty(bot);
            if (snapBots) {
               bot.SnapToWorldTarget();
            }
         }

         return party;
      }
   }
}