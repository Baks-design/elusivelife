using System;
using UnityEngine;

namespace ElusiveLife.Runtime.Game.Player.Data
{
    [Serializable]
    public class PlayerMovementData
    {
        public Vector3 FinalMoveVelocity;
        public Vector3 FinalMoveDirection;
        public Vector3 FinalOffset;
        public Vector3 CurrentVelocity;
        public Vector3 SmoothFinalMoveDir;
        public Vector2 SmoothInputVector;
        public float SmoothCurrentSpeed;
        public float FinalSmoothCurrentSpeed;
        public float CoyoteTimeTimer;
        public float JumpBufferTimer;
        public float InAirTimer;
        public float CurrentStateHeight;
        public float CurrentSpeed;
        public float VerticalVelocity;
        public float InitCamHeight;
        public bool IsDuringLandingAnimation;
        public bool IsMoving;
        public bool IsJumping;
        public bool IsWalking;
        public bool IsRunning;
        public bool IsSwimming;
        public bool IsClimbing;
        public bool IsCrouching;
        public bool Resetted;
        public bool IsDuringRunAnimation;
        public bool IsDuringCrouchAnimation;
    }
}
