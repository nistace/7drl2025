using DiceBotsGame.WorldLevels.Activities;
using UnityEngine;

namespace DiceBotsGame.WorldLevels {
   public class WorldCubeTile : MonoBehaviour {
      public enum HighlightType {
         None = 0,
         Hover = 1,
         Path = 2
      }

      [SerializeField] protected MeshRenderer highlightRenderer;
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
         SetHighlight(HighlightType.None);
      }

      public void SetHighlight(HighlightType highlightType) => highlightRenderer.gameObject.SetActive(highlightType != HighlightType.None);

      public void SetUp(int faceIndex, int xInFace, int yInFace) {
         Coordinates = new Vector3Int(xInFace, yInFace, faceIndex);
         name = $"Tile {Coordinates.z}.{Coordinates.x}.{Coordinates.y}";
      }
   }
}