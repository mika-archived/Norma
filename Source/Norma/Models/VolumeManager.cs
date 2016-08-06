using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Vannatech.CoreAudio.Enumerations;
using Vannatech.CoreAudio.Interfaces;

// ReSharper disable SuspiciousTypeConversion.Global

namespace Norma.Models
{
    // https://gist.github.com/sverrirs/d099b34b7f72bb4fb386
    // http://stackoverflow.com/questions/14306048/controling-volume-mixer
    internal class VolumeManager
    {
        public static void SetVolume(float volumeLevel)
        {
            var volume = GetVolumeObject();
            if (volume == null)
                return;
            var guid = Guid.Empty;
            volume.SetMasterVolume(volumeLevel / 100, guid);
            Marshal.ReleaseComObject(volume);
        }

        public static void SetMute(bool isMute)
        {
            var volume = GetVolumeObject();
            if (volume == null)
                return;
            var guid = Guid.Empty;
            volume.SetMute(isMute, guid);
            Marshal.ReleaseComObject(volume);
        }

        private static ISimpleAudioVolume GetVolumeObject()
        {
            IMMDeviceEnumerator deviceEnumerator = null;
            IAudioSessionEnumerator sessionEnumerator = null;
            IAudioSessionManager2 sessionManager = null;
            IMMDevice device = null;

            try
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                deviceEnumerator = (IMMDeviceEnumerator) new MMDeviceEnumerator();
                deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out device);

                // ReSharper disable once InconsistentNaming
                var IID_IAudioSessionManager2 = typeof(IAudioSessionManager2).GUID;
                object obj;
                device.Activate(IID_IAudioSessionManager2, 0, IntPtr.Zero, out obj);
                sessionManager = (IAudioSessionManager2) obj;

                sessionManager.GetSessionEnumerator(out sessionEnumerator);
                int count;
                sessionEnumerator.GetCount(out count);

                ISimpleAudioVolume volumeControl = null;
                for (var i = 0; i < count; i++)
                {
                    IAudioSessionControl sessionControl = null;
                    IAudioSessionControl2 sessionControl2 = null;
                    try
                    {
                        sessionEnumerator.GetSession(i, out sessionControl);
                        sessionControl2 = sessionControl as IAudioSessionControl2;

                        if (sessionControl2 == null)
                        {
                            // 多分通らない
                            string displayName;
                            sessionControl.GetDisplayName(out displayName);
                            if (displayName == "Norma")
                            {
                                volumeControl = sessionControl as ISimpleAudioVolume;
                                break;
                            }
                        }
                        else
                        {
                            uint processId;
                            sessionControl2.GetProcessId(out processId);
                            if (processId == Process.GetCurrentProcess().Id)
                            {
                                volumeControl = sessionControl2 as ISimpleAudioVolume;
                                break;
                            }
                        }
                    }
                    finally
                    {
                        // うーん
                        /*
                        if (sessionControl != null)
                            Marshal.ReleaseComObject(sessionControl);
                        if (sessionControl2 != null)
                            Marshal.ReleaseComObject(sessionControl2);
                        */
                    }
                }
                return volumeControl;
            }
            finally
            {
                if (sessionEnumerator != null)
                    Marshal.ReleaseComObject(sessionEnumerator);
                if (sessionManager != null)
                    Marshal.ReleaseComObject(sessionManager);
                if (device != null)
                    Marshal.ReleaseComObject(device);
                if (deviceEnumerator != null)
                    Marshal.ReleaseComObject(deviceEnumerator);
            }
        }
    }

    [ComImport]
    [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    internal class MMDeviceEnumerator {}
}