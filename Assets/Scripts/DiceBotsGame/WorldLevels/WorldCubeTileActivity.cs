using System;
using Unity.Cinemachine;
using UnityEngine;

namespace DiceBotsGame.WorldLevels {
   public class WorldCubeTileActivity : MonoBehaviour {
      public enum EType {
         None = 0,
         ExitFace = 1,
         EnterFace = 2,
         MeetEncounter = 3
      }

      [SerializeField] protected EType type;
      [SerializeField] protected OptionalActivityInfo optionalActivityInfo;
      [SerializeField] protected CinemachineCamera activityCamera;

      public bool Solved { get; set; }
      public EType Type => type;
      public CinemachineCamera ActivityCamera => activityCamera;
      public bool IsOptional(out OptionalActivityInfo optional) => optional = optionalActivityInfo;

      private void Start() {
         activityCamera.enabled = false;
      }
   }
}