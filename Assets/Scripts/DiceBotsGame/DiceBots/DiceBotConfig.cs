using UnityEngine;

namespace DiceBotsGame.DiceBots {
   [CreateAssetMenu]
   public class DiceBotConfig : ScriptableObject {
      [SerializeField] protected float worldMovementSpeed = 3;
      [SerializeField] protected float worldMovementMaxAngle = 10;
      [SerializeField] protected float worldRotationSpeed = 30;

      public float WorldMovementSpeed => worldMovementSpeed;
      public float WorldMovementMaxAngle => worldMovementMaxAngle;
      public float WorldRotationSpeed => worldRotationSpeed;
   }
}