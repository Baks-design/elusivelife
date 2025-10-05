using UnityEngine;

namespace ElusiveLife.Game.Settings
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Config/Game Settings")]
    public class GameSettings : ScriptableObject, IGameSettings
    {
        [field: SerializeField] public int PointsPerHit { get; set; } = 10;
        [field: SerializeField] public int PointsPerMiss { get; set; } = -5;
        [field: SerializeField] public float AimRotationDegreesPerSecond { get; set; } = 60f;
        [field: SerializeField] public float DespawnTargetAfterSeconds { get; set; } = 5f;
        [field: SerializeField] public float TargetSpawnCooldownInSeconds { get; set; } = 4f;
    }
}