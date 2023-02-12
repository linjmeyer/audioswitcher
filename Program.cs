using AudioSwitcher.AudioApi.CoreAudio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AudioSwitcher
{
    internal class Program
    {
        private static CoreAudioController _controller = new CoreAudioController();
        static void Main(string[] args)
        {
            // Get all devices and print to console so they can be put in devices.txt
            var allDevices = _controller.GetPlaybackDevices();
            foreach (var device in allDevices)
            {
                Console.WriteLine($"{device.FullName}: {device.Id}");
            }
            // Get devices we toggle between 
            var names = GetDeviceNames();
            if (names == null || names.Length == 0) return;

            // Find the current Id, toggle to next one in devices.txt (order is important)
            var currentName = _controller.DefaultPlaybackDevice.FullName;
            var toggleToNext = false;
            var wasToggled = false;
            foreach(var targetName in names)
            {
                if (toggleToNext)
                {
                    SetDevice(targetName);
                    wasToggled = true;
                    break;
                }

                if (currentName == targetName)
                {
                    toggleToNext = true;
                    continue;
                }
            }

            // If we never toggled, set to first device (current device is last in list)
            if (!wasToggled) SetDevice(names.First());
        }

        private static void SetDevice(string name)
        {
            var toggleToDevice = _controller.GetPlaybackDevices().FirstOrDefault(x => x.FullName == name);
            if (toggleToDevice == null) throw new Exception($"Could not find device named {name}");
            _controller.DefaultPlaybackDevice = _controller.GetDevice(toggleToDevice.Id);
            Console.WriteLine($"Set playback device: {_controller.DefaultPlaybackDevice.Name}");
        }

        private static string[] GetDeviceNames()
        { 
            return File.ReadAllLines("devices.txt");
        }
    }
}
