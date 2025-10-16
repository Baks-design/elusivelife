using Alchemy.Inspector;
using Unity.Cinemachine;
using UnityEngine;
using ElusiveLife.Runtime.Game.Player.Components.Collision;
using ElusiveLife.Runtime.Game.Player.Configs;
using ElusiveLife.Runtime.Game.Player.Data;
using ElusiveLife.Runtime.Game.Player.Interfaces;

namespace ElusiveLife.Runtime.Game.Player.Views
{
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        [field: SerializeField, Required] public CharacterController Controller { get; set; }
        [field: SerializeField, Required] public CinemachineCamera Cam { get; set; }
        [field: SerializeField, Required] public Transform Yaw { get; set; }
        [field: SerializeField, Required] public Transform Pitch { get; set; }
        [field: SerializeField, Required] public Animator Animator { get; set; }
        [field: SerializeField, InlineEditor] public PlayerMovementConfig MovementConfig { get; set; }
        [field: SerializeField, InlineEditor] public PlayerCollisionConfig CollisionConfig { get; set; }
        [field: SerializeField, InlineEditor] public PlayerCameraConfig CameraConfig { get; set; }
        [field: SerializeField, InlineEditor] public HeadBobConfig HeadBobConfig { get; set; }
        [field: SerializeField, InlineEditor] public PerlinNoiseConfig PerlinNoiseConfig { get; set; }
        [field: SerializeField, InlineEditor] public PlayerSoundConfig SoundConfig { get; set; }
       // [field: SerializeField, InlineEditor] public SoundLibraryObject SoundLibrary { get; set; }
        [field: SerializeField, ReadOnly] public PlayerMovementData MovementData { get; set; }
        [field: SerializeField, ReadOnly] public PlayerCollisionData CollisionData { get; set; }
        [field: SerializeField, ReadOnly] public PlayerCameraData CameraData { get; set; }
        private CharacterPush _characterPush;

        private void Start()
        {
            SetupComponents();
            SetupData();
            SetupClasses();
        }

        private void SetupComponents()
        {
            Cam.OutputChannel = OutputChannels.Default;
            Cam.StandbyUpdate = CinemachineVirtualCameraBase.StandbyUpdateMode.RoundRobin;
            Cam.Lens.FieldOfView = 60f;
            Cam.Lens.NearClipPlane = 0.15f;
            Cam.Lens.FarClipPlane = 100f;
            Cam.Lens.Dutch = 0f;

            Controller.slopeLimit = 45f;
            Controller.stepOffset = 0.3f;
            Controller.skinWidth = 0.01f;
            Controller.minMoveDistance = 0.001f;
            Controller.center = new Vector3(0f, Controller.height / 2f + Controller.skinWidth, 0f);
            Controller.radius = 0.5f;
            Controller.height = 1.8f;

            Animator.applyRootMotion = false;
            Animator.animatePhysics = false;
            Animator.updateMode = AnimatorUpdateMode.Normal;
            Animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        }

        private void SetupData()
        {
            MovementData.InitCamHeight = Yaw.localPosition.y;
            MovementData.CurrentStateHeight = MovementData.InitCamHeight;

            CollisionData = new PlayerCollisionData
            {
                InitCenter = Controller != null ? Controller.center : Vector3.zero,
                InitHeight = Controller != null ? Controller.height : 0f,
                OnGrounded = true,
                OnAirborne = false,
                PreviouslyGrounded = true,
                FinalRayLength = CollisionConfig.RayLength + Controller.center.y
            };
        }

        private void SetupClasses() => _characterPush = new CharacterPush(this);

        private void OnControllerColliderHit(ControllerColliderHit hit) => _characterPush?.PushBody(hit);
    }
}
