using DiceBotsGame.Visuals3D.TileHighlights;
using UnityEngine;

namespace DiceBotsGame.CombatGrids {
   public class CombatGridTile : MonoBehaviour {
      [SerializeField] protected CombatGridTileConfig config;
      [SerializeField] protected TileHighlight tileHighlight;

      public Vector2Int Coordinates { get; private set; }
      public int X => Coordinates.x;
      public int Y => Coordinates.y;

      private void Start() {
         tileHighlight.ChangeHighlight(config.GetHighlight(CombatGridTileConfig.HighlightType.None), true);
      }

      public void SetUp(int x, int y) {
         Coordinates = new Vector2Int(x, y);
      }

      public void SetHighlight(CombatGridTileConfig.HighlightType type) {
         tileHighlight.ChangeHighlight(config.GetHighlight(type));
      }
   }
}