using Alchemy.Inspector;
using GameToolkit.Runtime.Game.Behaviours.Player;
using Unity.Cinemachine;
using UnityEngine;

namespace ElusiveLife.Game.Player
{
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        [field: SerializeField, Required] public CharacterController Controller { get; private set; }
        [field: SerializeField, Required] public CinemachineCamera Cam { get; private set; }
        [field: SerializeField, Required] public Transform Yaw { get; private set; }
        [field: SerializeField, Required] public Transform Pitch { get; private set; }
        [field: SerializeField, Required] public Animator Animator { get; private set; }

        [field: SerializeField] public PlayerMovementConfig MovementConfig { get; private set; }
        [field: SerializeField] public PlayerCollisionConfig CollisionConfig { get; private set; }
        [field: SerializeField] public PlayerCameraConfig CameraConfig { get; private set; }
        [field: SerializeField, InlineEditor] public HeadBobConfig HeadBobConfig { get; private set; }
        [field: SerializeField, InlineEditor] public PerlinNoiseConfig PerlinNoiseConfig { get; private set; }

        [field: SerializeField, ReadOnly] public PlayerMovementData MovementData { get; private set; }
        [field: SerializeField, ReadOnly] public PlayerCollisionData CollisionData { get; set; }
        [field: SerializeField, ReadOnly] public PlayerCameraData CameraData { get; private set; }

        CharacterPush characterPush;

        void Start()
        {
            SetupComponents();
            SetupData();
            SetupClasses();
        }

        void SetupComponents()
        {
            Cam.OutputChannel = OutputChannels.Default;
            Cam.StandbyUpdate = CinemachineVirtualCameraBase.StandbyUpdateMode.RoundRobin;
            Cam.Lens.FieldOfView = 60f;
            Cam.Lens.NearClipPlane = 0.1f;
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
            Animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
        }

        void SetupData() => CollisionData = new PlayerCollisionData
        {
            InitCenter = Controller.center,
            InitHeight = Controller.height,
            OnGrounded = true,
            OnAirborne = false,
            PreviouslyGrounded = true,
            FinalRayLength = CollisionConfig.RayLength + Controller.center.y
        };

        void SetupClasses() => characterPush = new CharacterPush(Controller, CollisionData, CollisionConfig);

        void OnControllerColliderHit(ControllerColliderHit hit) => characterPush.PushBody(hit);
    }
}
