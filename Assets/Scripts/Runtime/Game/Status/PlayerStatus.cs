using System;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Status
{
    public class PlayerStatus : IPlayerStatus
    {
        public PlayerStatusData CurrentStatus { get; private set; }

        public event Action<PlayerStatusData, PlayerStatusData> OnStatusChanged;

        public PlayerStatus(PlayerStatusData initialStatus) => CurrentStatus = initialStatus;

        public void UpdateStatus(Func<PlayerStatusData, PlayerStatusData> statusUpdate)
        {
            var previousStatus = CurrentStatus;
            CurrentStatus = statusUpdate(CurrentStatus);
            if (CurrentStatus.Equals(previousStatus)) return;

            OnStatusChanged?.Invoke(CurrentStatus, previousStatus);
        }
    }
}