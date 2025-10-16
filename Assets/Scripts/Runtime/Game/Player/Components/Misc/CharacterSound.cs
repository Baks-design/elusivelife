using UnityEngine;
using ElusiveLife.Runtime.Game.Player.Interfaces;

namespace ElusiveLife.Runtime.Game.Player.Components.Misc
{
    public class CharacterSound
    {
    //     private readonly IPlayerView _playerView;
    //     //private readonly ISoundServices _soundServices;
    //     //private SoundBuilder _soundBuilder;
    //     private float _footstepTimer;
    //     private float _swimmingTimer;
    //     private float _climbingTimer;
    //     private float _fallStartHeight;
    //     private bool _wasSwimming;
    //     private bool _wasClimbing;

    //     public CharacterSound(IPlayerView playerView)
    //     {
    //         _playerView = playerView;
    //         //_soundServices = soundServices;
    //         Init();
    //     }

    //     private void Init()
    //     {
    //         _soundBuilder = _soundServices.CreateSoundBuilder();
    //         _fallStartHeight = _playerView.Controller.transform.position.y;
    //     }

    //     public void UpdateFootsteps()
    //     {
    //         if (IsFootstepConditionInvalid())
    //         {
    //             _footstepTimer = 0f;
    //             return;
    //         }

    //         PlayFootsteps();
    //     }

    //     private void PlayFootsteps()
    //     {
    //         _footstepTimer += Time.deltaTime;
    //         var currentInterval = GetCurrentFootstepInterval();
    //         if (!(_footstepTimer >= currentInterval))
    //             return;

    //         PlayFootstepSound();
    //         _footstepTimer = 0f;
    //     }

    //     private bool IsFootstepConditionInvalid() =>
    //         !_playerView.CollisionData.OnGrounded
    //         || _playerView.MovementData.IsSwimming
    //         || _playerView.MovementData.IsClimbing
    //         || !_playerView.MovementData.IsMoving;

    //     private float GetCurrentFootstepInterval() =>
    //         _playerView.MovementData.IsRunning
    //             ? _playerView.SoundConfig.FootstepIntervalRun
    //             : _playerView.SoundConfig.FootstepIntervalWalk;

    //     private void PlayFootstepSound() =>
    //         _soundBuilder
    //             .WithRandomPitch(0.9f, 1.1f)
    //             .WithSetVolume(_playerView.SoundConfig.FootstepVolume)
    //             .WithPosition(_playerView.Controller.transform.position)
    //             .Play(_playerView.SoundLibrary.FootstepClip);

    //     public void UpdateLanding()
    //     {
    //         var fallDistance = _fallStartHeight - _playerView.Controller.transform.position.y;
    //         var impactVolume = Mathf.Clamp(
    //             fallDistance * _playerView.SoundConfig.ScaleFactor,
    //             _playerView.SoundConfig.ImpactLandingMin,
    //             _playerView.SoundConfig.InmpactLandingMax) * _playerView.SoundConfig.LandingVolume;
    //         PlayLandingSound(impactVolume);
    //     }

    //     private void PlayLandingSound(float impactVolume) =>
    //         _soundBuilder
    //             .WithRandomPitch(_playerView.SoundConfig.LandingMinPitch, _playerView.SoundConfig.LandingMaxPitch)
    //             .WithSetVolume(impactVolume)
    //             .WithPosition(_playerView.Controller.transform.position)
    //             .Play(_playerView.SoundLibrary.LandingClip);

    //     public void UpdateSwimming()
    //     {
    //         var isSwimming = _playerView.MovementData.IsSwimming;

    //         if (isSwimming)
    //         {
    //             if (_playerView.MovementData.IsMoving)
    //             {
    //                 _swimmingTimer += Time.deltaTime;
    //                 if (_swimmingTimer >= _playerView.SoundConfig.SwimmingInterval)
    //                 {
    //                     PlaySwimmingSound();
    //                     _swimmingTimer = 0f;
    //                 }
    //             }
    //             else
    //                 _swimmingTimer = 0f;

    //             if (!_wasSwimming)
    //                 PlaySwimmingSound();
    //         }
    //         else
    //             _swimmingTimer = 0f;

    //         _wasSwimming = isSwimming;
    //     }

    //     private void PlaySwimmingSound() =>
    //         _soundBuilder
    //             .WithRandomPitch(_playerView.SoundConfig.SwimmingMinPitch, _playerView.SoundConfig.SwimmingMaxPitch)
    //             .WithSetVolume(_playerView.SoundConfig.SwimmingVolume)
    //             .WithPosition(_playerView.Controller.transform.position)
    //             .Play(_playerView.SoundLibrary.SwimmingClip);

    //     public void UpdateClimbing()
    //     {
    //         var isClimbing = _playerView.MovementData.IsClimbing;

    //         if (isClimbing)
    //         {
    //             if (_playerView.MovementData.IsMoving)
    //             {
    //                 _climbingTimer += Time.deltaTime;
    //                 if (_climbingTimer >= _playerView.SoundConfig.ClimbingInterval)
    //                 {
    //                     PlayClimbingSound();
    //                     _climbingTimer = 0f;
    //                 }
    //             }
    //             else
    //                 _climbingTimer = 0f;

    //             if (!_wasClimbing)
    //                 PlayClimbingSound();
    //         }
    //         else
    //             _climbingTimer = 0f;

    //         _wasClimbing = isClimbing;
    //     }

    //     private void PlayClimbingSound() =>
    //         _soundBuilder
    //             .WithRandomPitch(_playerView.SoundConfig.ClimbingMinPitch, _playerView.SoundConfig.ClimbingMaxPitch)
    //             .WithSetVolume(_playerView.SoundConfig.ClimbingVolume)
    //             .WithPosition(_playerView.Controller.transform.position)
    //             .Play(_playerView.SoundLibrary.ClimbingClip);

    //     public void UpdateJumping()
    //     {
    //         if (!_playerView.MovementData.IsJumping)
    //             return;

    //         PlayJumpSound();
    //     }

    //     private void PlayJumpSound() =>
    //         _soundBuilder
    //             .WithRandomPitch()
    //             .WithPosition(_playerView.Controller.transform.position)
    //             .Play(_playerView.SoundLibrary.JumpingClip);

    //     public void UpdateDamaging() => PlayDamageSound();

    //     private void PlayDamageSound() =>
    //         _soundBuilder
    //             .WithRandomPitch()
    //             .WithPosition(_playerView.Controller.transform.position)
    //             .Play(_playerView.SoundLibrary.DamagingClip);
    }
}