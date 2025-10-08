using System;
using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Data
{
    [Serializable]
    public class PlayerCollisionData
    {
        public Vector3 InitCenter;
        public Vector3 GroundedNormal;
        public float InitHeight;
        public float FinalRayLength;
        public bool OnGrounded;
        public bool PreviouslyGrounded;
        public bool HasObstructed;
        public bool HasRoofed;
        public bool OnAirborne;
        public bool WasGrounded;
        public bool HasObjectColliding;
    }
}
