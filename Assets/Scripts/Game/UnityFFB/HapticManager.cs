using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DLLsCPP;
using Game.UnityFFB.Structures;
using UnityEngine;

namespace Game.UnityFFB
{
    public static class HapticManager
    {
        public static void Initialize()
        {
            NativeSDL3.InitSDL();
        }

        public static void Dispose()
        {
            NativeSDL3.DisposeSDL();
        }

        public static HapticDevice[] EnumerateHapticDevices()
        {
            var count = 0;
            var infosPtr = NativeSDL3.GetHapticDevicesInfo(ref count);
            var list = new HapticDevice[count];

            for (int i = 0; i < count; i++)
            {
                var info = Marshal.PtrToStructure<FFBDeviceInfo>(infosPtr);
                list[i] = info;
                
                Debug.Log($"ID-{info.ID}. {info.Name} - MAX EFFECTS: {info.MaxEffects}, MAX PLAYING: {info.MaxPlayingEffects}");
            }
            
            return list;
        }

        public static void CloseAllHaptics()
        {
            NativeSDL3.CloseAllHaptics();
        }

        // public static HapticDevice OpenHaptic()
        // {
        //     // NativeSDL3.
        //     
        //     // return new HapticDevice();
        // }
    }
}