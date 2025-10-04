using EngineRoom.Examples.Interfaces;
using UnityEngine;

namespace EngineRoom.Examples.Views
{
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        [SerializeField]
         Transform cannon;

       public float CannonRotation
        {
            get => cannon.eulerAngles.y;
            set => cannon.rotation = Quaternion.Euler(0f, value, 0f);
        }
    }
}
