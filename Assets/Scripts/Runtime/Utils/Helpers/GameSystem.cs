using UnityEngine;

namespace ElusiveLife.Runtime.Utils.Helpers
{
    public static class GameSystem
    {
        public static void IsShowCursor(bool isLocked) =>
            Cursor.lockState = isLocked ? CursorLockMode.None : CursorLockMode.Locked;

        public static void IsTimeActive(bool active) => Time.timeScale = active ? 1f : 0f;
    }
}
