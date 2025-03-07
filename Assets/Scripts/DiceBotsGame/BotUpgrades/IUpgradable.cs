using UnityEngine.Events;

namespace DiceBotsGame.BotUpgrades {
   public interface IUpgrade {
      bool Selected { get; protected set; }

      void Apply();

      void SetSelected(bool selected) {
         if (Selected == selected) return;
         Selected = selected;
         OnSelectedChanged.Invoke();
      }

      public UnityEvent OnSelectedChanged { get; }
   }
}