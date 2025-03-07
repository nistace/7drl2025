using System.Collections.Generic;
using System.Linq;
using DiceBotsGame.DiceBots;
using DiceBotsGame.Utils;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace DiceBotsGame.CombatGrids {
   public class CombatGrid : MonoBehaviour {
      [SerializeField] protected Transform offsetTransform;
      [SerializeField] protected Transform tileContainer;
      [SerializeField] protected float tileContainerLerpMinScale = .1f;
      [SerializeField] protected float tileContainerLerpMaxScale = 1;
      [SerializeField] protected CinemachineCamera cinemachineCamera;
      [SerializeField] protected CinemachineBasicMultiChannelPerlin cameraShake;
      [SerializeField] protected float upMovementOffset = 5;
      [SerializeField] protected float transitionLerpSpeed = 1;

      public CinemachineCamera CinemachineCamera => cinemachineCamera;
      private List<CombatGridTile> Tiles { get; } = new List<CombatGridTile>();
      public IReadOnlyCollection<CombatGridTile> AllTiles => Tiles;
      public CombatGridTile this[int x, int y] => Tiles[x * Size + y];
      public CombatGridTile this[Vector2Int coordinates] => this[coordinates.x, coordinates.y];
      private Map<DiceBot, CombatGridTile> PositionPerBot { get; } = new Map<DiceBot, CombatGridTile>();
      private IReadOnlyList<DiceBot> PlayerBots { get; set; }
      private List<DiceBot> OpponentBots { get; set; }
      public IReadOnlyList<DiceBot> ListOfOpponentBots => OpponentBots;
      public bool AreAllBotsAtTheirPosition => PositionPerBot.All(t => t.Key.transform.transform.position == t.Value.transform.position);
      private int Size { get; set; }
      public float TransitionLerpSpeed => transitionLerpSpeed;

      public UnityEvent<DiceBot> OnNewOpponent { get; } = new UnityEvent<DiceBot>();

      public void PrepareCombat(Transform origin, IReadOnlyList<DiceBot> playerBots, IReadOnlyList<DiceBot> opponentBots) {
         transform.position = origin.position;
         transform.rotation = origin.rotation;
         tileContainer.localScale = Vector3.zero;

         PlayerBots = playerBots;
         OpponentBots = opponentBots.ToList();

         for (var i = 0; i < playerBots.Count; ++i) {
            var tile = this[0, IndexToY(i)];
            var bot = playerBots[i];
            PositionPerBot.Add(bot, tile);
            bot.transform.SetParent(offsetTransform);
         }
         for (var i = 0; i < OpponentBots.Count; ++i) {
            var tile = this[Size - 1, Size - IndexToY(i) - 1];
            var bot = opponentBots[i];
            PositionPerBot.Add(bot, tile);
            bot.transform.SetParent(offsetTransform);
         }
      }

      public void AddOpponentToBattle(DiceBot opponent, CombatGridTile tile) {
         PositionPerBot.Add(opponent, tile);
         opponent.transform.SetParent(offsetTransform);
         OpponentBots.Add(opponent);
         OnNewOpponent.Invoke(opponent);
      }

      private static int IndexToY(int x) => x switch {
         <= 0 => 3,
         1 => 2,
         2 => 4,
         3 => 1,
         4 => 5,
         _ => 0
      };

      public void Build(CombatGridPattern pattern) {
         Size = pattern.Size;
         for (var x = 0; x < Size; ++x)
         for (var y = 0; y < Size; ++y) {
            var tile = Instantiate(pattern.TilePrefab, tileContainer);
            tile.transform.localPosition = new Vector3((x - (Size - 1) * .5f) * pattern.CellOffset, 0, (y - (pattern.Size - 1) * .5f) * pattern.CellOffset);
            tile.transform.localRotation = Quaternion.identity;
            tile.transform.localScale = Vector3.one;
            tile.SetUp(x, y);
            Tiles.Add(tile);
         }
      }

      public void UpdateAllBotsPosition() {
         foreach (var positionPerBot in PositionPerBot) {
            positionPerBot.Key.MoveTowards(positionPerBot.Value.transform);
         }
      }

      public void SnapAllBotsPosition() {
         foreach (var positionPerBot in PositionPerBot) {
            positionPerBot.Key.SnapTo(positionPerBot.Value.transform);
         }
      }

      public void StartBattle() => gameObject.SetActive(true);

      public void EndBattle() {
         foreach (var bot in PositionPerBot) {
            bot.Key.transform.SetParent(null);
         }

         PositionPerBot.Clear();
         gameObject.SetActive(false);
      }

      public void Lerp(float lerpValue) {
         offsetTransform.localPosition = new Vector3(-transform.position.x * lerpValue, upMovementOffset * lerpValue, -transform.position.z * lerpValue);
         tileContainer.localScale = Mathf.Lerp(tileContainerLerpMinScale, tileContainerLerpMaxScale, lerpValue) * Vector3.one;
      }

      public Vector2Int GetDiceBotPosition(DiceBot actor) => PositionPerBot[actor].Coordinates;

      private bool Exists(Vector2Int coordinates) => coordinates.x >= 0 && coordinates.x < Size && coordinates.y >= 0 && coordinates.y < Size;

      public bool TryGetPath(Vector2Int from, Vector2Int to, out IReadOnlyList<Vector2Int> path) {
         path = null;
         var openNodes = new Queue<Vector2Int>();
         openNodes.Enqueue(from);
         var origins = new Dictionary<Vector2Int, Vector2Int>();

         while (openNodes.Count > 0) {
            var node = openNodes.Dequeue();

            foreach (var neighbour in new[] { node + Vector2Int.left, node + Vector2Int.up, node + Vector2Int.right, node + Vector2Int.down }) {
               if (Exists(neighbour) && (neighbour == to || !PositionPerBot.ContainsKey(this[neighbour.x, neighbour.y])) && origins.TryAdd(neighbour, node)) {
                  openNodes.Enqueue(neighbour);
                  if (neighbour == to) {
                     var pathList = new List<Vector2Int> { neighbour };
                     while (pathList[0] != from && origins.TryGetValue(pathList[0], out var previous)) {
                        pathList.Insert(0, previous);
                     }
                     path = pathList;
                     return true;
                  }
               }
            }
         }
         return false;
      }

      public void SetBotPosition(DiceBot actor, CombatGridTile targetTile) {
         PositionPerBot.Remove(actor);
         PositionPerBot.Add(actor, targetTile);
      }

      public DiceBot GetDiceBotAtPosition(CombatGridTile targetTile) => PositionPerBot.GetValueOrDefault(targetTile);
      public CombatGridTile GetTileAtPosition(Vector2Int coordinates) => this[coordinates.x, coordinates.y];
      public bool AreInSameTeam(DiceBot first, DiceBot second) => PlayerBots.Contains(first) == PlayerBots.Contains(second);

      public void UnHoverAllTiles() {
         foreach (var tile in AllTiles) {
            tile.SetHighlight(CombatGridTileConfig.HighlightType.None);
         }
      }

      public IReadOnlyList<DiceBot> GetBotTeam(DiceBot bot) => PlayerBots.Contains(bot) ? PlayerBots : OpponentBots;
      public IReadOnlyList<DiceBot> GetBotOpponents(DiceBot bot) => PlayerBots.Contains(bot) ? OpponentBots : PlayerBots;

      public static int Distance(Vector2Int first, Vector2Int second) => Mathf.Abs(first.x - second.x) + Mathf.Abs(first.y - second.y);
   }
}