using System;

using MyoSharp.Device;

namespace MyoSharp.GabyRM.Internal
{
    internal static class ConsoleHelper
    {
        #region Methods
        internal static void UserInputLoop(IHub hub)
        {
            string userInput;
            while (!string.IsNullOrEmpty((userInput = Console.ReadLine())))
            {
                if (userInput.Equals("pose", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var myo in hub.Myos)
                    {
                        Console.WriteLine("Myo {0} está en pose {1}.", myo.Handle, myo.Pose);
                    }
                }
                else if (userInput.Equals("arm", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var myo in hub.Myos)
                    {
                        Console.WriteLine("Myo {0} está en el brazo {1}.", myo.Handle, myo.Arm.ToString().ToLower());
                    }
                }
                else if (userInput.Equals("count", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Hay {0} Myo(s) conectados.", hub.Myos.Count);
                }
            }
        }
        #endregion
    }
}