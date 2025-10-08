using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Configs;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Data;
using Unity.Cinemachine;
using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces
{
    public interface IPlayerView
    {
        CharacterController Controller { get; }
        CinemachineCamera Cam { get; }
        Transform Yaw { get; }
        Transform Pitch { get; }
        Animator Animator { get; }

        PlayerMovementConfig MovementConfig { get; }
        PlayerCollisionConfig CollisionConfig { get; }
        PlayerCameraConfig CameraConfig { get; }
        HeadBobConfig HeadBobConfig { get; }
        PerlinNoiseConfig PerlinNoiseConfig { get; }

        PlayerMovementData MovementData { get; }
        PlayerCollisionData CollisionData { get; set; }
        PlayerCameraData CameraData { get; }
    }
}