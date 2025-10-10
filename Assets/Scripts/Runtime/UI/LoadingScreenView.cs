using Alchemy.Inspector;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace ElusiveLife.UI.Assets.Scripts.Runtime.UI
{
    public class LoadingScreenView : MonoBehaviour
    {
        [SerializeField, Required] private Image _loadingBar;
        [SerializeField, Required] private Canvas _loadingCanvas;
        [SerializeField, Required] private CinemachineCamera _loadingCamera;

        [Inject]
        public void Construct()
        {
        }

        public void Show()
        {
            _loadingCanvas.gameObject.SetActive(true);
            _loadingCamera.Priority.Value = 99;
        }

        public void SetBarPercent(float percent) => _loadingBar.fillAmount = percent;

        public void Hide()
        {
            _loadingCanvas.gameObject.SetActive(false);
            _loadingCamera.Priority.Value = 0;
        }
    }
}