using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ultra_Command
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // MicrosoftTTSL
        private SpeechSynthesizer _speechSynthesizer;
        private SpeechRecognitionEngine _recognizer;
        private bool _isListening;

        public MainWindow()
        {
            _isListening = false;
            _speechSynthesizer = new SpeechSynthesizer();
            InitializeComponent();
            // read from file
        }


        private void Recording_Button_Click(object sender, RoutedEventArgs e)
        {
            // disable/enable edit buttons.
            // start recording

            if (_isListening)
            {
                Listening_Logs.AppendText($"\r\nSTOPPING SERVICE...");
                _isListening = false;
                _speechSynthesizer.Pause();
            }
            else
            {
                _isListening = true;
                Listening_Logs.AppendText($"\r\nSTARTING RECORDING SESSION....");
                _speechSynthesizer.Pause();
                _recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));

                // greaemmer to be set to my custom commands
                _recognizer.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"Profiles/Profile1.txt")))));
                _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Default_SpeechRecognized);
                // _recognizer.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(Recognizer_SpeechRecognized);

                // Configure input to the speech recognizer.  
                _recognizer.SetInputToDefaultAudioDevice();

                // Start asynchronous, continuous speech recognition.  
                _recognizer.RecognizeAsync(RecognizeMode.Multiple);

                // Add a handler for the speech recognized event.  
                _recognizer.SpeechRecognized +=
                  new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
            }


        }

        private void Recognizer_SpeechRecognized(object sender, SpeechDetectedEventArgs e)
        {
        }

        private void Default_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            var text = e.Result.Text;
            if(text.ToLower() == "Hello".ToLower())
            {
                Listening_Logs.AppendText($"\r\nCOMMAND : Received Hello");
            }
        }

        // Handle the SpeechRecognized event.  
        public void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Listening_Logs.AppendText($"\r\n{e.Result.Text}");
        }
    }
}
