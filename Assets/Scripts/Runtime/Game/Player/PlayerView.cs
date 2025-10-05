using UnityEngine;

namespace ElusiveLife.Game.Player
{
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        [SerializeField] Transform cannon;

        public float CannonRotation
        {
            get => cannon.eulerAngles.y;
            set => cannon.rotation = Quaternion.Euler(0f, value, 0f);
        }
    }
}
