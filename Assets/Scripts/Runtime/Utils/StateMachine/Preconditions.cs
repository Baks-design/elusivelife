using System;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Extensions;

namespace ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.StateMachine
{
    public class Preconditions
    {
        private Preconditions()
        {
        }

        public static T CheckNotNull<T>(T reference) => CheckNotNull(reference, null);

        public static T CheckNotNull<T>(T reference, string message) =>
            reference is UnityEngine.Object obj && obj.OrNull() == null || reference == null
                ? throw new ArgumentNullException(message)
                : reference;

        public static void CheckState(bool expression) => CheckState(expression, null);

        public static void CheckState(bool expression, string messageTemplate, params object[] messageArgs) =>
            CheckState(expression, string.Format(messageTemplate, messageArgs));

        public static void CheckState(bool expression, string message)
        {
            if (expression)
                return;

            throw message == null ? new InvalidOperationException() : new InvalidOperationException(message);
        }
    }
}