using AudioSwitcher.AudioApi.CoreAudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioSwitcher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var controller = new CoreAudioController();
            var headSetGuid = Guid.Parse("{2fcd7156-328a-41ba-acf3-af2fc42544f3}");
            var speakerGuid = Guid.Parse("{9e87ac79-80a6-4f81-9db8-cf619002cefc}");
            var headSetDevice = controller.GetDevice(headSetGuid);
            var speakerDevice = controller.GetDevice(speakerGuid);

            controller.DefaultPlaybackDevice = controller.DefaultPlaybackDevice == headSetDevice ? speakerDevice : headSetDevice;
        }
    }
}
