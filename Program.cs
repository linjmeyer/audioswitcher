using AudioSwitcher.AudioApi.CoreAudio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var ids = GetDeviceTxtIds();
            if (ids == null || ids.Length == 0) return;

            // Find the current Id, toggle to next one in devices.txt (order is important)
            var currentId = _controller.DefaultPlaybackDevice.Id;
            var toggleToNext = false;
            var wasToggled = false;
            foreach(var id in ids)
            {
                if (toggleToNext)
                {
                    SetDevice(id);
                    wasToggled = true;
                    break;
                }

                if (id == currentId)
                {
                    toggleToNext = true;
                    continue;
                }
            }

            // If we never toggled, set to first device (current device is last in list)
            if (toggleToNext && !wasToggled) SetDevice(ids.First());
        }

        private static void SetDevice(Guid id)
        {
            _controller.DefaultPlaybackDevice = _controller.GetDevice(id);
            Console.WriteLine($"Set playback device: {_controller.DefaultPlaybackDevice.Name}");
        }

        private static Guid[] GetDeviceTxtIds()
        {
            var guids = new List<Guid>();
            var rawIds = File.ReadAllLines("devices.txt");
            foreach(var rawId in rawIds)
            {
                if (string.IsNullOrWhiteSpace(rawId)) continue;
                guids.Add(Guid.Parse(rawId));  
            }
            return guids.ToArray();
        }
    }
}
