using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ultra_Command
{
    public partial class Form1 : Form
    {
        private SpeechSynthesizer _speechSynthesizer;
        private SpeechRecognitionEngine _recognizer;
        public Form1()
        {
            InitializeComponent();
            _speechSynthesizer = new SpeechSynthesizer();
        }

        private void ListenBtn_Click(object sender, EventArgs e)
        {
            _speechSynthesizer.Pause();
            _recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));

            // Create and load a dictation grammar.  
            _recognizer.LoadGrammar(new DictationGrammar());

            // Configure input to the speech recognizer.  
            _recognizer.SetInputToDefaultAudioDevice();

            // Start asynchronous, continuous speech recognition.  
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);

            // Add a handler for the speech recognized event.  
            _recognizer.SpeechRecognized +=
              new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
        }

        // Handle the SpeechRecognized event.  
        public void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            SpeechRecordedTextBox.AppendText($"\r\n{e.Result.Text}");
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            _speechSynthesizer.Pause();
            _recognizer.Dispose();
        }
    }
}
