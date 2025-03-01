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

      private Vector3Int Coordinates { get; set; }
      public IWorldCubeTileActivity Activity { get; private set; }

      public int FaceIndex => Coordinates.z;
      public int XInFace => Coordinates.x;
      public int YInFace => Coordinates.y;
      public bool IsTraversable => Activity == null || Activity.IsTraversable;

      private void Start() {
         Activity = GetComponent<IWorldCubeTileActivity>();
         SetHighlight(HighlightType.None);
      }

      public void SetHighlight(HighlightType highlightType) => highlightRenderer.gameObject.SetActive(highlightType != HighlightType.None);

      public void SetUp(int faceIndex, int xInFace, int yInFace) {
         Coordinates = new Vector3Int(xInFace, yInFace, faceIndex);
         name = $"Tile {Coordinates.z}.{Coordinates.x}.{Coordinates.y}";
      }
   }
}