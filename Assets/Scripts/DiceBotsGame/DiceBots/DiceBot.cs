using DiceBotsGame.CombatActions.AI;
using DiceBotsGame.DiceBots.Dices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DiceBotsGame.DiceBots {
   public class DiceBot : MonoBehaviour {
      [SerializeField] protected Transform diceAnchor;
      [SerializeField] protected DiceBotConfig config;
      [SerializeField] protected CharacterDice dice;
      [SerializeField] protected DiceBotEmissiveMaterial emissive;
      [SerializeField] protected HealthSystem healthSystem;

      public CharacterDice Dice => dice;
      private Transform WorldTarget { get; set; }
      public bool AtWorldTarget => !WorldTarget || transform.position == WorldTarget.position;
      public HealthSystem HealthSystem => healthSystem;
      public DiceBotConfig Config => config;
      public Color Color => emissive.Color;
      public string DisplayName { get; private set; }
      public CombatAi CombatAi { get; private set; }
      public DiceBotUpgradeInfo UpgradeInfo { get; private set; }

      public void SetUp(string name, CombatAi combatAi, DiceBotUpgradeInfo upgradeInfo, CharacterDice newDice, DiceBotEmissiveMaterial emissive) {
         if (dice) {
            Destroy(dice.gameObject);
         }

         DisplayName = name;
         CombatAi = combatAi;
         UpgradeInfo = upgradeInfo;
         dice = newDice;
         newDice.transform.SetParent(diceAnchor);
         newDice.transform.localPosition = Vector3.zero;
         newDice.transform.localRotation = Quaternion.identity;
         newDice.transform.localScale = Vector3.one;

         this.emissive = emissive;
         healthSystem = new HealthSystem(newDice.Data.CoreHealth);
         this.emissive.SetObservedHealthSystem(healthSystem);
      }

      public void SetWorldTargetPosition(Transform worldTarget) => WorldTarget = worldTarget;

      public void SnapToWorldTarget() {
         transform.position = WorldTarget.transform.position;
         transform.rotation = WorldTarget.transform.rotation;
      }

      public void UpdateWorldPosition() {
         if (WorldTarget) {
            MoveTowards(WorldTarget);
         }
      }

      public void MoveTowards(Transform target) {
         if (transform.position == target.position) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, config.WorldRotationSpeed * Time.deltaTime);
         }
         else {
            var rotationTowardsTarget = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
            if (Quaternion.Angle(transform.rotation, rotationTowardsTarget) < config.WorldMovementMaxAngle) {
               transform.position = Vector3.MoveTowards(transform.position, target.position, config.WorldMovementSpeed * Time.deltaTime);
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationTowardsTarget, config.WorldRotationSpeed * Time.deltaTime);
         }
      }

      public void SnapTo(Transform target) {
         transform.position = target.position;
         transform.rotation = target.rotation;
      }

      public void Roll(float coefficient = 1) {
         var randomForce = Quaternion.Euler(0, Random.Range(0, 360f), 0)
                           * Vector3.Slerp(Vector3.up, Vector3.forward, Random.Range(config.RollMinAngle, config.RollMaxAngle) / 90)
                           * (Random.Range(config.RollMinForce, config.RollMaxForce) * coefficient);
         var randomTorque = new Vector3((Random.Range(0, 2) - 1) * Random.Range(config.RollMinTorque, config.RollMaxTorque),
                               (Random.Range(0, 2) - 1) * Random.Range(config.RollMinTorque, config.RollMaxTorque),
                               (Random.Range(0, 2) - 1) * Random.Range(config.RollMinTorque, config.RollMaxTorque))
                            * coefficient;
         dice.Roll(randomForce, randomTorque);
      }

      public void Reassemble() => dice.AttachToBody();

      public bool IsRolling() => dice.IsRolling;
      public bool IsStuckWhileRolling() => dice.IsStuckWhileRolling;
      public void SaveRolledFace() => dice.SaveRolledFace();
   }
}