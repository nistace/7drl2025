using System.Collections.Generic;
using System.Linq;
using DiceBotsGame.Cameras;
using DiceBotsGame.GameSates.InputUtils;
using DiceBotsGame.WorldLevels;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DiceBotsGame.GameSates.WorldStates {
   public class WorldState : GameState {
      public static WorldState Instance { get; } = new WorldState();

      private WorldCubeTile hoveredTile;
      private List<WorldCubeTile> pathToHoveredTile;
      private readonly Dictionary<WorldCubeTile, List<WorldCubeTile>> paths = new Dictionary<WorldCubeTile, List<WorldCubeTile>>();

      private WorldState() { }

      protected override void Enable() {
         MainCameraController.ActivateWorldCamera();

         WorldInputUtils.ActionMap.Enable();
         WorldInputUtils.Interact.performed += HandleInteractPerformed;

         UpdatePaths();
      }

      protected override void Disable() {
         WorldInputUtils.ActionMap.Disable();
         WorldInputUtils.Interact.performed -= HandleInteractPerformed;

         UnHoverCurrentTile();
      }

      protected override void Update() {
         UpdateHoveredTile();
         GameInfo.PlayerParty.UpdateBotsWorldPosition();
      }

      private void HandleInteractPerformed(InputAction.CallbackContext obj) {
         if (hoveredTile) {
            if (pathToHoveredTile != null) {
               ChangeState(new MoveInWorldState(pathToHoveredTile));
            }
            else if (hoveredTile == GameInfo.PlayerParty.CurrentTile && hoveredTile.Activity is { Solved: false }) {
               ChangeState(new PromptWorldActivityState(hoveredTile.Activity));
            }
         }
      }

      private void UpdateHoveredTile() {
         if (Physics.Raycast(MainCameraController.MainCamera.ScreenPointToRay(WorldInputUtils.Aim.ReadValue<Vector2>()), out var hit, Mathf.Infinity, LayerMask.GetMask("WorldTile"))) {
            if (!hoveredTile || hit.collider.gameObject != hoveredTile.gameObject) {
               UnHoverCurrentTile();

               hoveredTile = hit.collider.gameObject.GetComponentInParent<WorldCubeTile>();
               if (hoveredTile.FaceIndex == GameInfo.WorldCube.CurrentFaceIndex) {
                  if (paths.TryGetValue(hoveredTile, out pathToHoveredTile)) {
                     foreach (var pathTile in pathToHoveredTile) {
                        pathTile.SetHighlight(WorldCubeTileConfig.HighlightType.Path);
                     }
                  }

                  hoveredTile.SetHighlight(WorldCubeTileConfig.HighlightType.Hover);
               }
            }
         }
         else if (hoveredTile) {
            UnHoverCurrentTile();
         }
      }

      private void UnHoverCurrentTile() {
         if (hoveredTile) {
            hoveredTile.SetHighlight(WorldCubeTileConfig.HighlightType.None);
         }
         if (pathToHoveredTile != null) {
            foreach (var pathTile in pathToHoveredTile) {
               pathTile.SetHighlight(WorldCubeTileConfig.HighlightType.None);
            }
         }
         hoveredTile = null;
         pathToHoveredTile = null;
      }

      private void UpdatePaths() {
         paths.Clear();

         var openNodes = new Queue<WorldCubeTile>();
         openNodes.Enqueue(GameInfo.PlayerParty.CurrentTile);
         var origins = new Dictionary<WorldCubeTile, WorldCubeTile> { { GameInfo.PlayerParty.CurrentTile, GameInfo.PlayerParty.CurrentTile } };

         while (openNodes.Count > 0) {
            var node = openNodes.Dequeue();
            if (node.IsTraversable) {
               foreach (var neighbour in new[] {
                              (faceIndex: node.FaceIndex, x: node.XInFace - 1, y: node.YInFace),
                              (faceIndex: node.FaceIndex, x: node.XInFace + 1, y: node.YInFace),
                              (faceIndex: node.FaceIndex, x: node.XInFace, y: node.YInFace - 1),
                              (faceIndex: node.FaceIndex, x: node.XInFace, y: node.YInFace + 1),
                           }.Where(t => GameInfo.WorldCube.Exists(t.x, t.y))
                           .Select(t => GameInfo.WorldCube[t.faceIndex, t.x, t.y])
                           .Where(t => !origins.ContainsKey(t))) {
                  origins.Add(neighbour, node);
                  openNodes.Enqueue(neighbour);
               }
            }

            if (node != GameInfo.PlayerParty.CurrentTile) {
               var path = new List<WorldCubeTile> { node };
               while (origins[path[0]] != GameInfo.PlayerParty.CurrentTile) {
                  path.Insert(0, origins[path[0]]);
               }

               paths.Add(node, path);
            }
         }
      }
   }
}