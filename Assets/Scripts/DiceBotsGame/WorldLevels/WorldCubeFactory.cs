using UnityEngine;

namespace DiceBotsGame.WorldLevels {
   public class WorldCubeFactory : MonoBehaviour {
      private static WorldCubeFactory instance { get; set; }

      [SerializeField] protected WorldCube worldCubePrefab;

      private void Awake() {
         instance = this;
      }

      public static WorldCube InstantiateWorldCube(WorldCubePattern pattern) {
         var worldCube = Instantiate(instance.worldCubePrefab);
         worldCube.Build(pattern);
         return worldCube;
      }
   }
}