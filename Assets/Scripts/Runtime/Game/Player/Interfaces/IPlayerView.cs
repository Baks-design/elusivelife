using GameToolkit.Runtime.Game.Behaviours.Player;
using Unity.Cinemachine;
using UnityEngine;

namespace ElusiveLife.Game.Player
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