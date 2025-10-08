using UnityEngine;

namespace ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers
{
    public static class GameSystem
    {
        public static void SetCursor(bool isLocked) =>
            Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
