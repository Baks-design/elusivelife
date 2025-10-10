using Alchemy.Inspector;
using UnityEngine;

namespace ElusiveLife.UI.Assets.Scripts.Runtime.UI
{
    public class PauseScreenView : MonoBehaviour
    {
        [SerializeField, Required] private GameObject _pauseScreen;

        public void Show() => _pauseScreen.SetActive(true);

        public void Hide() => _pauseScreen.SetActive(false);
    }
}