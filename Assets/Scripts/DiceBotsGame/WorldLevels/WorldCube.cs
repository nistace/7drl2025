using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiceBotsGame.DiceBots;
using DiceBotsGame.Utils;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace DiceBotsGame.WorldLevels {
   public class WorldCube : MonoBehaviour {
      [SerializeField] protected Transform center;
      [SerializeField] protected float rotationSpeed = 30;
      [SerializeField] protected float lerpMinScale = .1f;
      [SerializeField] protected float lerpMaxScale = 1;

      public int CurrentFaceIndex { get; private set; }
      private int Size { get; set; }

      private List<WorldCubeTile> Tiles { get; } = new List<WorldCubeTile>();
      private WorldCubeTile[] EntryTiles { get; } = new WorldCubeTile[Cubes.FaceCount];
      private WorldCubeTile[] ExitTiles { get; } = new WorldCubeTile[Cubes.FaceCount];
      public WorldCubeTile SpawnTile => EntryTiles[0];
      private Coroutine RotateRoutine { get; set; }
      private bool Built => Tiles.Count > 0;
      public float InnerSphereRadius { get; private set; }

      public WorldCubeTile this[int face, int x, int y] => Tiles[face * Size * Size + x * Size + y];

      public bool Exists(int x, int y) => x >= 0 && y >= 0 && x < Size && y < Size;

      private void Start() {
         RotateToFace(0);
      }

      public void Build(WorldCubePattern pattern) {
         if (Built) {
            Debug.LogError("WorldCube is already built. Cannot built twice.", this);
            return;
         }

         Size = pattern.CubeSize;
         InnerSphereRadius = pattern.CubeSize * .5f * pattern.TileOffset;
         center.localScale = Vector3.one * Size * pattern.TileOffset * .99f;

         for (var faceIndex = 0; faceIndex < Cubes.FaceCount; ++faceIndex) {
            var faceRotator = new GameObject($"FaceRotator {faceIndex}").transform;
            faceRotator.SetParent(transform);
            faceRotator.localPosition = Vector3.zero;
            faceRotator.localRotation = Cubes.faceRotations[faceIndex];
            faceRotator.localScale = Vector3.one;

            var face = new GameObject($"Face {faceIndex}").transform;
            face.SetParent(faceRotator);
            face.localPosition = new Vector3(0, InnerSphereRadius, 0);
            face.localRotation = Quaternion.identity;
            face.localScale = Vector3.one;

            var tiles = new List<WorldCubeTile>();

            if (faceIndex == 0) {
               var spawnTile = Instantiate(pattern.SpawnTilePrefab, face.transform);
               EntryTiles[faceIndex] = spawnTile;
               tiles.Add(SpawnTile);
            }
            if (faceIndex < Cubes.FaceCount - 1) {
               var exitTile = Instantiate(pattern.FaceExitTilePrefab, face.transform);
               ExitTiles[faceIndex] = exitTile;
               tiles.Add(exitTile);
            }
            if (faceIndex > 0) {
               var entryTile = Instantiate(pattern.FaceEntryTilePrefab, face.transform);
               EntryTiles[faceIndex] = entryTile;
               tiles.Add(entryTile);
            }
            tiles.AddRange(pattern.FacePresets[faceIndex].MandatoryFaces.Take(Size * Size - tiles.Count).Select(t => Instantiate(t, face.transform)));
            tiles.AddRange(Enumerable.Repeat(pattern.DefaultTilePrefab, Size * Size - tiles.Count).Select(t => Instantiate(t, face.transform)));

            var tilesWithEncounter = tiles.Select(t => t.GetComponent<WorldCubeTileEncounter>()).Where(t => t).ToArray();
            var encountersQueue = new Queue<EncounterPreset>();
            while (encountersQueue.Count < tilesWithEncounter.Length) {
               foreach (var randomEncounter in pattern.FacePresets[faceIndex].PossibleEncounters.OrderBy(_ => Random.value)) {
                  encountersQueue.Enqueue(randomEncounter);
               }
            }
            foreach (var tileWithEncounter in tilesWithEncounter) {
               var encounterPreset = encountersQueue.Dequeue();
               tileWithEncounter.SetUp(encounterPreset.DisplayName, encounterPreset.DiceBotPatterns.Select(DiceBotFactory.Instantiate).ToArray());
            }

            var tileQueue = new Queue<WorldCubeTile>(tiles.OrderBy(_ => Random.value));

            for (var x = 0; x < pattern.CubeSize; ++x)
            for (var y = 0; y < pattern.CubeSize; ++y) {
               var tile = tileQueue.Dequeue();
               tile.SetUp(faceIndex, x, y);
               tile.transform.localPosition = new Vector3((x - (pattern.CubeSize - 1) * .5f) * pattern.TileOffset, 0, (y - (pattern.CubeSize - 1) * .5f) * pattern.TileOffset);
               tile.transform.localRotation = Quaternion.identity;
               tile.transform.localScale = Vector3.one;
               Tiles.Add(tile);
            }
         }
      }

      public void RotateToFace(int faceIndex, UnityAction callback = null) {
         if (RotateRoutine != null) StopCoroutine(RotateRoutine);
         RotateRoutine = StartCoroutine(DoRotateToFace(faceIndex, callback));
      }

      private IEnumerator DoRotateToFace(int faceIndex, UnityAction callback) {
         CurrentFaceIndex = faceIndex;

         var targetRotation = Quaternion.Inverse(Cubes.faceRotations[CurrentFaceIndex]);
         while (!Mathf.Approximately(Quaternion.Angle(transform.rotation, targetRotation), 0)) {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
         }

         transform.rotation = targetRotation;
         RotateRoutine = null;
         callback?.Invoke();
      }

      public WorldCubeTile GetFaceEntryInCurrentFace() => EntryTiles[CurrentFaceIndex];

      public void Lerp(float lerp) {
         transform.localScale = Mathf.Lerp(lerpMinScale, lerpMaxScale, lerp) * Vector3.one;
      }
   }
}