using System;
using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.GabyRM.Internal;
using MyoSharp.Exceptions;
using MyoSharp.Poses;

namespace MyoSharp.GabyRM
{
    internal class MyoMusic
    {
        #region Methods
        private static void Main()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("--- Estado ---");
            Player.Init();
            Console.SetCursorPosition(0, 2);
            Console.WriteLine("El reproductor ha sido inicializado");
            Console.SetCursorPosition(0, 3);
            Console.WriteLine("--- Controles ---");
            Console.SetCursorPosition(0, 6);
            Console.WriteLine("--- Posicionamiento ---");
            Console.SetCursorPosition(0, 10);
            Console.WriteLine("--- Gestos ---");
            Console.SetCursorPosition(0, 12);
            Console.WriteLine("Musica parada");
            Console.SetCursorPosition(0, 13);
            Console.WriteLine("--- Bloqueo ---");
            Console.SetCursorPosition(0, 1);
            using (var channel = Channel.Create(
                ChannelDriver.Create(ChannelBridge.Create(),
                MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create()))))
            using (var hub = Hub.Create(channel))
            {
                hub.MyoConnected += (sender, e) =>
                {
                    Console.WriteLine("Myo {0} está conectado!", e.Myo.Handle);
                    e.Myo.Vibrate(VibrationType.Short);
                    e.Myo.OrientationDataAcquired += Myo_OrientationDataAcquired;
                    e.Myo.PoseChanged += Myo_PoseChanged;
                    e.Myo.Locked += Myo_Locked;
                    e.Myo.Unlocked += Myo_Unlocked;
                };
                hub.MyoDisconnected += (sender, e) =>
                {
                    Console.WriteLine("Oh no!, parece que el Myo del brazo {0} se ha desconectado!", e.Myo.Arm);
                    e.Myo.OrientationDataAcquired += Myo_OrientationDataAcquired;
                    e.Myo.PoseChanged += Myo_PoseChanged;
                    e.Myo.Locked -= Myo_Locked;
                    e.Myo.Unlocked -= Myo_Unlocked;
                };
                channel.StartListening();
                ConsoleHelper.UserInputLoop(hub);
            }
        }
        #endregion

        #region Event Handlers
        private static void Myo_OrientationDataAcquired(object sender, OrientationDataEventArgs e)
        {
            const float PI = (float)System.Math.PI;
            var roll = (int)((e.Roll + PI) / (PI * 2.0f) * 10);
            var pitch = (int)((e.Pitch + PI) / (PI * 2.0f) * 10);
            var yaw = (int)((e.Yaw + PI) / (PI * 2.0f) * 10);
            Player.Volume(pitch);
            Player.Skip(roll);
            Console.SetCursorPosition(0, 4);
            Console.WriteLine("Volumen: " + pitch);
            Console.SetCursorPosition(0, 5);
            Console.WriteLine("Avanzar: " + roll);
            Console.SetCursorPosition(0, 7);
            Console.WriteLine(@"Roll: {0}", roll);
            Console.SetCursorPosition(0, 8);
            Console.WriteLine(@"Pitch: {0}", pitch);
            Console.SetCursorPosition(0, 9);
            Console.WriteLine(@"Yaw: {0}", yaw);
        }
        private static void Myo_PoseChanged(object sender, PoseEventArgs e)
        {
            Console.SetCursorPosition(0, 11);
            ClearCurrentConsoleLine();
            Console.Write("Myo del brazo {0} detecto la pose {1}!", e.Myo.Arm, e.Myo.Pose);
            if (e.Myo.Pose.Equals(Pose.DoubleTap) || e.Myo.Pose.Equals(Pose.FingersSpread))
            {           
                Console.SetCursorPosition(0, 12);
                ClearCurrentConsoleLine();
                bool togglePlay = Player.TogglePlay();
                Console.WriteLine("Musica on/off: " + togglePlay);
                e.Myo.Vibrate(VibrationType.Long);
            }
            if (e.Myo.Pose.Equals(Pose.WaveOut))
            {
                Player.Stop();
                Console.SetCursorPosition(0, 12);
                ClearCurrentConsoleLine();
                Console.WriteLine("Musica parada");
            }
            if(e.Myo.Pose.Equals(Pose.WaveIn))
            {
                Player.Init();
                Console.SetCursorPosition(0, 2);
                ClearCurrentConsoleLine();
                Console.WriteLine("El repoductor ha sido reinicializado");
            }
        }
        private static void Myo_Unlocked(object sender, MyoEventArgs e)
        {
            Console.SetCursorPosition(0, 14);
            ClearCurrentConsoleLine();
            Console.WriteLine("Myo del brazo {0} se ha desbloqueado!", e.Myo.Arm);
        }

        private static void Myo_Locked(object sender, MyoEventArgs e)
        {
            Console.SetCursorPosition(0, 14);
            ClearCurrentConsoleLine();
            Console.WriteLine("Myo del brazo {0} se ha bloqueado!", e.Myo.Arm);
        }
        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write(" ");
            Console.SetCursorPosition(0, currentLineCursor);
        }
        #endregion
    }
}