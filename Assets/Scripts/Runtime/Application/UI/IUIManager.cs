using Cysharp.Threading.Tasks;

namespace ElusiveLife.Runtime.Application.UI
{
    public interface IUIManager
    {
        UniTask ShowMainMenu();
        UniTask HideMainMenu();
        UniTask ShowGameHUD();
        UniTask HideGameHUD();
        UniTask ShowSlotSelection();
        UniTask HideSlotSelection();
        void UpdateScore(int score);
        void OnSlotSelectionShown();
        void OnSlotSelectionHidden();
    }
}