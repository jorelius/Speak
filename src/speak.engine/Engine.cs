using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace speak.core
{
    public class Engine
    {
        public SpeechSynthesizer Speaker = new SpeechSynthesizer();

        /// <summary>
        /// Current speaking progress 
        /// 0.0 to 1.0
        /// </summary>
        public double Progress { get; set; }

        public string Text { get; set; }

        /// <summary>
        /// Speaking voice
        /// </summary>
        public string Voice
        {
            get
            {
                return Speaker.Voice.Name;
            }
            set
            {
                Speaker.SelectVoice(value);
            }
        }

        /// <summary>
        /// Rate at which speaker reads
        /// </summary>
        public int Rate
        {
            get
            {
                return Speaker.Rate; 
            }
            set
            {
                if (value > 10)
                {
                    Speaker.Rate = 10;
                }
                else if (value < -10)
                {
                    Speaker.Rate = -10;
                }
                else
                {
                    Speaker.Rate = value; 
                }
            }
        }
        
        #region Events

        public event EventHandler<SpeakStartedEventArgs> SpeakStarted;

        public event EventHandler<SpeakProgressEventArgs> SpeakProgress;

        public event EventHandler<SpeakCompletedEventArgs> SpeakCompleted;

        void synth_SpeakStarted(object sender, SpeakStartedEventArgs e)
        {
            Progress = 0;
            SpeakStarted?.Invoke(this, e);
        }

        void synth_SpeakProgress(object sender, SpeakProgressEventArgs e)
        {
            Progress = (double)e.CharacterPosition / (double)Text.Length;
            SpeakProgress?.Invoke(this, e);
        }

        void synth_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            Progress = 1;
            SpeakCompleted?.Invoke(this, e);
        }

        #endregion Events

        public Engine()
        {
            // Add a handler for the SpeakStarted, SpeakProgress, and SpeakCompleted events.
            // This supports progress calculation. 
            Speaker.SpeakStarted +=
              new EventHandler<SpeakStartedEventArgs>(synth_SpeakStarted);

            Speaker.SpeakProgress +=
              new EventHandler<SpeakProgressEventArgs>(synth_SpeakProgress);

            Speaker.SpeakCompleted +=
                new EventHandler<SpeakCompletedEventArgs>(synth_SpeakCompleted);
        }

        /// <summary>
        /// Save to Audio file 
        /// </summary>
        /// <param name="path"></param>
        public void Save(string path)
        {
            Speaker.SetOutputToWaveFile(path);
            Speak();
        }

        /// <summary>
        /// Speak the text using the default audio device
        /// </summary>
        public void Speak()
        {
            Speaker.Speak(Text);

            // trigger completed event manually
            synth_SpeakCompleted(this, null); 
        }

        /// <summary>
        /// Voices available on machine
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VoiceInfo> InstalledVoices()
        {
            return Speaker.GetInstalledVoices().Select(v => v.VoiceInfo);
        }
    }
}
