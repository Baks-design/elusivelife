using System;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Status
{
    public interface IPlayerStatus
    {
        PlayerStatusData CurrentStatus { get; }

        void UpdateStatus(Func<PlayerStatusData, PlayerStatusData> statusUpdate);
    }
}