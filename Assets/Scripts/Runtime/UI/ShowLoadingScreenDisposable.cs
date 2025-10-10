using System;

namespace ElusiveLife.UI.Assets.Scripts.Runtime.UI
{
    public class ShowLoadingScreenDisposable : IDisposable
    {
        private readonly LoadingScreenView _loadingScreen;

        public ShowLoadingScreenDisposable(LoadingScreenView loadingScreen)
        {
            _loadingScreen = loadingScreen;
            loadingScreen.Show();
        }

        public void SetLoadingBarPercent(float percent) 
            => _loadingScreen.SetBarPercent(percent);

        public void Dispose() => _loadingScreen.Hide();
    }
}