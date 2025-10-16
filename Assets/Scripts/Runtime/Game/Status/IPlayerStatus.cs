using System;

namespace ElusiveLife.Runtime.Game.Status
{
    public interface IPlayerStatus
    {
        PlayerStatusData CurrentStatus { get; }

        void UpdateStatus(Func<PlayerStatusData, PlayerStatusData> statusUpdate);
    }
}