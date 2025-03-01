using System.Collections.Generic;
using System.Linq;
using DiceBotsGame.Cameras;
using DiceBotsGame.WorldLevels;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DiceBotsGame.GameSates {
   public class WorldState : GameState {
      public static WorldState Instance { get; } = new WorldState();

      private readonly InputAction aimAction;
      private readonly InputAction interactAction;

      private WorldCubeTile hoveredTile;
      private List<WorldCubeTile> pathToHoveredTile;
      private readonly Dictionary<WorldCubeTile, List<WorldCubeTile>> paths = new Dictionary<WorldCubeTile, List<WorldCubeTile>>();

      private WorldState() {
         aimAction = InputSystem.actions.FindAction("Aim");
         interactAction = InputSystem.actions.FindAction("Interact");
      }

      protected override void Enable() {
         aimAction.Enable();
         interactAction.performed += HandleInteractPerformed;
         interactAction.Enable();

         UpdatePaths();
      }

      protected override void Disable() {
         aimAction.Disable();
         interactAction.performed -= HandleInteractPerformed;
         interactAction.Disable();

         UnHoverCurrentTile();
      }

      protected override void Update() {
         UpdateHoveredTile();
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
         if (Physics.Raycast(MainCameraController.MainCamera.ScreenPointToRay(aimAction.ReadValue<Vector2>()), out var hit, Mathf.Infinity, LayerMask.GetMask("WorldTile"))) {
            if (!hoveredTile || hit.collider.gameObject != hoveredTile.gameObject) {
               UnHoverCurrentTile();

               hoveredTile = hit.collider.gameObject.GetComponentInParent<WorldCubeTile>();
               if (hoveredTile.FaceIndex == GameInfo.WorldCube.CurrentFaceIndex) {
                  if (paths.TryGetValue(hoveredTile, out pathToHoveredTile)) {
                     foreach (var pathTile in pathToHoveredTile) {
                        pathTile.SetHighlight(WorldCubeTile.HighlightType.Path);
                     }
                  }

                  hoveredTile.SetHighlight(WorldCubeTile.HighlightType.Hover);
               }
            }
         }
         else if (hoveredTile) {
            UnHoverCurrentTile();
         }
      }

      private void UnHoverCurrentTile() {
         if (hoveredTile) {
            hoveredTile.SetHighlight(WorldCubeTile.HighlightType.None);
         }
         if (pathToHoveredTile != null) {
            foreach (var pathTile in pathToHoveredTile) {
               pathTile.SetHighlight(WorldCubeTile.HighlightType.None);
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