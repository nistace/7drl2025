using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DiceBotsGame.Visuals3D.TileHighlights;
using UnityEngine;

namespace DiceBotsGame.CombatGrids {
   [CreateAssetMenu]
   public class CombatGridTileConfig : ScriptableObject {
      public enum HighlightType {
         None = 0,
         HoveredNone = 7,
         DefaultSelectable = 1,
         HoveredDefaultSelectable = 2,
         AllySelectable = 3,
         HoveredAllySelectable = 4,
         OpponentSelectable = 5,
         HoveredOpponentSelectable = 6
      }

      [SerializeField] protected SerializedDictionary<HighlightType, TileHighlightInfo> highlightInfos = new SerializedDictionary<HighlightType, TileHighlightInfo>();
      [SerializeField] protected TileHighlightInfo defaultHighlight;

      public TileHighlightInfo GetHighlight(HighlightType highlightType) => highlightInfos.GetValueOrDefault(highlightType, defaultHighlight);
   }
}