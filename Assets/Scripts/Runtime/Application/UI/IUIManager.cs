using Cysharp.Threading.Tasks;

namespace ElusiveLife.Runtime.Application.UI
{
    public interface IUIManager
    {
        UniTask ShowMainMenu();
        UniTask HideMainMenu();
        UniTask ShowGameHUD();
        UniTask HideGameHUD();
    }
}