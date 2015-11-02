using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyoSharp.Communication;
using System.Runtime.InteropServices;

namespace MyoSharp.GabyRM
{
    public static class Player
    {
        private static IWavePlayer waveOutDevice;
        private static AudioFileReader audioFileReader;
        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);
        public static void Init()
        {
            waveOutDevice = new WaveOut();
            audioFileReader = new AudioFileReader("test.mp3");
            waveOutDevice.Init(audioFileReader);
        }
        public static bool TogglePlay()
        {
            if (waveOutDevice.PlaybackState.Equals(PlaybackState.Playing))
            {
                waveOutDevice.Pause();
                return false;
            }
            else
            {
                waveOutDevice.Play();
                return true;
            }
        }
        public static void Stop()
        {
            waveOutDevice.Stop();
        }
        public static void Skip(int roll)
        {
            if (roll >= 7 && waveOutDevice.PlaybackState.Equals(PlaybackState.Playing))
            {
                audioFileReader.Skip(-1);
            }
            if (roll <= 1 && waveOutDevice.PlaybackState.Equals(PlaybackState.Playing))
            {
                audioFileReader.Skip(1);
            }

        }
        public static void Volume(int pitch)
        {
            int NewVolume = ((ushort.MaxValue / 10) * pitch + 1);
            uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
            waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);
        }
    }
}
