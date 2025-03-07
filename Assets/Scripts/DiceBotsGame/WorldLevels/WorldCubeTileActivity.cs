using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace DiceBotsGame.WorldLevels {
   public class WorldCubeTileActivity : MonoBehaviour {
      private static readonly int interactingAnimParam = Animator.StringToHash("Interacting");
      private static readonly int solvedAnimParam = Animator.StringToHash("Solved");

      public enum EType {
         None = 0,
         ExitFace = 1,
         EnterFace = 2,
         MeetEncounter = 3,
         DiceSmith = 4
      }

      [SerializeField] protected EType type;
      [SerializeField] protected string displayName = "This Activity";
      [SerializeField] protected OptionalActivityInfo optionalActivityInfo;
      [SerializeField] protected CinemachineCamera activityCamera;
      [SerializeField] protected Animator[] animators;

      public int Face { get; set; }
      public bool Solved { get; private set; }
      public EType Type => type;
      public CinemachineCamera ActivityCamera => activityCamera;

      public UnityEvent OnSolvedChanged { get; } = new UnityEvent();

      public string DisplayName {
         get => displayName;
         set => displayName = value;
      }

      public bool IsOptional(out OptionalActivityInfo optional) => optional = optionalActivityInfo;

      private void Start() {
         activityCamera.enabled = false;
      }

      public void SetInteracting(bool interacting) {
         foreach (var animator in animators) animator.SetBool(interactingAnimParam, interacting);
      }

      public void SetSolved(bool solved) {
         Solved = solved;
         foreach (var animator in animators)  animator.SetBool(solvedAnimParam, solved);
      }
   }
}