using ElusiveLife.Game.Player;
using UnityEngine;

namespace GameToolkit.Runtime.Game.Behaviours.Player
{
    public class CameraBreathing
    {
        readonly IPlayerView playerView;
        readonly CameraScroller cameraScroller;

        public CameraBreathing(
            IPlayerView playerView,
            CameraScroller cameraScroller)
        {
            this.playerView = playerView;
            this.cameraScroller = cameraScroller;
        }

        public void UpdateBreathing()
        {
            if (playerView.MovementData.IsMoving)
                return;

            cameraScroller.UpdateNoise();

            var noise = playerView.CameraData.Noise;
            var useX = playerView.CameraConfig.X;
            var useY = playerView.CameraConfig.Y;
            var useZ = playerView.CameraConfig.Z;

            switch (playerView.PerlinNoiseConfig.TransformTarget)
            {
                case TransformTarget.Position:
                    UpdatePosition(noise, useX, useY, useZ);
                    break;
                case TransformTarget.Rotation:
                    UpdateRotation(noise, useX, useY, useZ);
                    break;
                case TransformTarget.Both:
                    UpdateBoth(noise, useX, useY, useZ);
                    break;
            }
        }

        void UpdatePosition(Vector3 noise, bool useX, bool useY, bool useZ)
        {
            var currentPos = playerView.Cam.transform.localPosition;

            var newPos = new Vector3(
                useX ? noise.x : currentPos.x,
                useY ? noise.y : currentPos.y,
                useZ ? noise.z : currentPos.z
            );

            playerView.Cam.transform.localPosition = newPos;
        }

        void UpdateRotation(Vector3 noise, bool useX, bool useY, bool useZ)
        {
            var currentRot = playerView.Cam.transform.localEulerAngles;

            var newRot = new Vector3(
                useX ? noise.x : currentRot.x,
                useY ? noise.y : currentRot.y,
                useZ ? noise.z : currentRot.z
            );

            playerView.Cam.transform.localEulerAngles = newRot;
        }

        void UpdateBoth(Vector3 noise, bool useX, bool useY, bool useZ)
        {
            var currentPos = playerView.Cam.transform.localPosition;
            var currentRot = playerView.Cam.transform.localEulerAngles;

            var newPos = new Vector3(
                useX ? noise.x : currentPos.x,
                useY ? noise.y : currentPos.y,
                useZ ? noise.z : currentPos.z
            );
            var newRot = new Vector3(
                useX ? noise.x : currentRot.x,
                useY ? noise.y : currentRot.y,
                useZ ? noise.z : currentRot.z
            );

            playerView.Cam.transform.localPosition = newPos;
            playerView.Cam.transform.localEulerAngles = newRot;
        }
    }
}
