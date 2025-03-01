using DiceBotsGame.Visuals3D.TileHighlights;
using UnityEngine;

namespace DiceBotsGame.WorldLevels {
   public class WorldCubeTile : MonoBehaviour {
      [SerializeField] protected TileHighlight highlight;
      [SerializeField] protected WorldCubeTileConfig config;
      [SerializeField] protected Transform playerAnchorTransform;
      [SerializeField] protected bool applyAnchorForwardToPlayer;

      private Vector3Int Coordinates { get; set; }
      public WorldCubeTileActivity Activity { get; private set; }

      public int FaceIndex => Coordinates.z;
      public int XInFace => Coordinates.x;
      public int YInFace => Coordinates.y;
      public bool IsTraversable => Activity == null || Activity.Solved || Activity.IsOptional(out _);
      public Transform PlayerAnchorTransform => playerAnchorTransform;
      public bool ApplyAnchorForwardToPlayer => applyAnchorForwardToPlayer;

      private void Start() {
         Activity = GetComponent<WorldCubeTileActivity>();
         SetHighlight(WorldCubeTileConfig.HighlightType.None, true);
      }

      public void SetHighlight(WorldCubeTileConfig.HighlightType highlightType, bool snap = false) {
         highlight.ChangeHighlight(config.GetHighlight(highlightType), snap);
      }

      public void SetUp(int faceIndex, int xInFace, int yInFace) {
         Coordinates = new Vector3Int(xInFace, yInFace, faceIndex);
         name = $"Tile {Coordinates.z}.{Coordinates.x}.{Coordinates.y}";
      }
   }
}