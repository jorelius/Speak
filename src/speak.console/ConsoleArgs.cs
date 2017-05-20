using PowerArgs;
using PowerArgs.Cli;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace speak
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    public class ConsoleArgs
    {
        static core.Engine Engine = new core.Engine();
        static CliProgressBar bar = new CliProgressBar("{%}")
        { BorderPen = new ConsoleCharacter(' ', ConsoleColor.Green, ConsoleColor.Black) };

        [HelpHook, ArgShortcut("-?"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgDescription("Show speaker voices available"), ArgShortcut("voices")]
        public bool ShowVoices { get; set; }

        [ArgDescription("Set speaking voice"), ArgShortcut("v")]
        public string Voice { get; set; }

        [ArgDescription("Set speaking rate (-10 to 10)"), ArgRange(-10, 10), ArgDefaultValue(0), ArgShortcut("r")]
        public int Rate { get; set; }

        [ArgDescription("Text to speak"), ArgShortcut("t"), ArgPosition(0)]
        public string Text { get; set; }

        [ArgDescription("Show progress bar")]
        public bool UI { get; set; }

        [ArgDescription("Save to file"), ArgShortcut("s")]
        public string SaveFile { get; set; }

        static void synth_SpeakStarted(object sender, SpeakStartedEventArgs e)
        {
            bar.Progress = 0;
            bar.Render();
        }

        static void synth_SpeakProgress(object sender, SpeakProgressEventArgs e)
        {
            bar.Progress = Engine.Progress;
            bar.Update();
        }

        static void synth_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            bar.Progress = 1;
            bar.Update();
        }

        public void InstalledVoices()
        {
            foreach (var voice in Engine.InstalledVoices())
            {
                Console.WriteLine("{0} | {1} | {2}", voice.Id, voice.Name, voice.Description);
            }
        }

        public void Main()
        {
            if (ShowVoices)
            {
                InstalledVoices();
                return;
            }

            // check for piped input
            string pipedText;
            if (Console.IsInputRedirected)
            {
                using (Stream s = Console.OpenStandardInput())
                using (var reader = new StreamReader(s))
                {
                    pipedText = reader.ReadToEnd();
                }

                Text = pipedText;
            }

            if (String.IsNullOrWhiteSpace(Text))
            {
                // nothing to do
                return; 
            }
            
            // setup engine
            Engine.Text = Text;
            Engine.Rate = Rate;

            if (!String.IsNullOrWhiteSpace(Voice))
            {
                Engine.Voice = Voice;
            }
                 
            // setup UI elements
            if (UI) 
            {
                // Add a handlers for the SpeakStarted, SpeakProgress, and SpeakCompleted events.
                Engine.SpeakStarted +=
                    new EventHandler<SpeakStartedEventArgs>(synth_SpeakStarted);

                Engine.SpeakProgress +=
                    new EventHandler<SpeakProgressEventArgs>(synth_SpeakProgress);

                Engine.SpeakCompleted +=
                    new EventHandler<SpeakCompletedEventArgs>(synth_SpeakCompleted);
            }

            if (string.IsNullOrWhiteSpace(SaveFile))
            {
                Engine.Speak();
            }
            else
            {
                Engine.Save(SaveFile);
            }
        }
    }
}
