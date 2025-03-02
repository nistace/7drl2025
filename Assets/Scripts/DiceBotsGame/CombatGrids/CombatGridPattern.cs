using UnityEngine;

namespace DiceBotsGame.CombatGrids {
   [CreateAssetMenu]
   public class CombatGridPattern : ScriptableObject {
      [SerializeField] protected CombatGridTile tilePrefab;
      [SerializeField] private int size = 6;
      [SerializeField] protected float cellOffset = 1.55f;

      public int Size => size;
      public CombatGridTile TilePrefab => tilePrefab;
      public float CellOffset => cellOffset;
   }
}