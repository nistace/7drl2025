using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using DiceBotsGame.WorldLevels;

namespace DiceBotsGame.GameSates {
   public static class GameInfo {
      public static WorldCube WorldCube { get; private set; }
      public static CombatGrid CombatGrid { get; private set; }
      public static DiceBotsParty PlayerParty { get; private set; }

      public static void SetupGameData(WorldCube worldCube, DiceBotsParty playerParty, CombatGrid combatGrid) {
         WorldCube = worldCube;
         PlayerParty = playerParty;
         CombatGrid = combatGrid;
      }
   }
}