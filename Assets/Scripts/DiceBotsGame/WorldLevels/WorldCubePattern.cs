using System;
using System.Collections.Generic;
using DiceBotsGame.DiceBots;
using DiceBotsGame.Utils;
using UnityEngine;

namespace DiceBotsGame.WorldLevels {
   [CreateAssetMenu]
   public class WorldCubePattern : ScriptableObject {
      [SerializeField] private int cubeSize = 3;
      [SerializeField] protected WorldCubeTile spawnTilePrefab;
      [SerializeField] protected WorldCubeTile faceExitTilePrefab;
      [SerializeField] protected WorldCubeTile faceEntryTilePrefab;
      [SerializeField] protected WorldCubeTile defaultTilePrefab;
      [SerializeField] protected FacePreset[] facePresets = new FacePreset[Cubes.FaceCount];
      [SerializeField] protected float tileOffset = 3.1f;

      public int CubeSize => cubeSize;

      public WorldCubeTile SpawnTilePrefab => spawnTilePrefab;
      public WorldCubeTile FaceExitTilePrefab => faceExitTilePrefab;
      public WorldCubeTile FaceEntryTilePrefab => faceEntryTilePrefab;
      public IReadOnlyList<FacePreset> FacePresets => facePresets;
      public WorldCubeTile DefaultTilePrefab => defaultTilePrefab;
      public float TileOffset => tileOffset;

      [Serializable] public class FacePreset {
         [SerializeField] protected WorldCubeTile[] mandatoryFaces;
         [SerializeField] protected EncounterPreset[] possibleEncounters;

         public IReadOnlyList<WorldCubeTile> MandatoryFaces => mandatoryFaces;
         public IReadOnlyCollection<EncounterPreset> PossibleEncounters => possibleEncounters;
      }
   }
}