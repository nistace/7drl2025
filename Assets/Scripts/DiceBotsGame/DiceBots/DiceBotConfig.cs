using UnityEngine;

namespace DiceBotsGame.DiceBots {
   [CreateAssetMenu]
   public class DiceBotConfig : ScriptableObject {
      [SerializeField] protected float rollMinForce = 300;
      [SerializeField] protected float rollMaxForce = 500;
      [SerializeField] protected float rollMinAngle = 5;
      [SerializeField] protected float rollMaxAngle = 30;
      [SerializeField] protected float rollMinTorque = 30;
      [SerializeField] protected float rollMaxTorque = 90;
      [SerializeField] protected float worldMovementSpeed = 3;
      [SerializeField] protected float worldMovementMaxAngle = 10;
      [SerializeField] protected float worldRotationSpeed = 30;

      public float WorldMovementSpeed => worldMovementSpeed;
      public float WorldMovementMaxAngle => worldMovementMaxAngle;
      public float WorldRotationSpeed => worldRotationSpeed;
      public float RollMinAngle => rollMinAngle;
      public float RollMaxAngle => rollMaxAngle;
      public float RollMinTorque => rollMinTorque;
      public float RollMaxTorque => rollMaxTorque;
      public float RollMinForce => rollMinForce;
      public float RollMaxForce => rollMaxForce;
   }
}