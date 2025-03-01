using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DiceBotsGame.Visuals3D.TileHighlights;
using UnityEngine;

namespace DiceBotsGame.WorldLevels {
   [CreateAssetMenu]
   public class WorldCubeTileConfig : ScriptableObject {
      public enum HighlightType {
         None = 0,
         Hover = 1,
         Path = 2
      }

      [SerializeField] protected SerializedDictionary<HighlightType, TileHighlightInfo> highlightInfos = new SerializedDictionary<HighlightType, TileHighlightInfo>();
      [SerializeField] protected TileHighlightInfo defaultHighlight;

      public TileHighlightInfo GetHighlight(HighlightType highlightType) => highlightInfos.GetValueOrDefault(highlightType, defaultHighlight);
   }
}