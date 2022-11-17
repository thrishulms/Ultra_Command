using System;
using System.Collections.Generic;
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

                var listOfCommands = ["Activate Commands", "Activate Commands", "Arm Weapons"];
                
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


        }

        // Handle the SpeechRecognized event.  
        public void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Listening_Logs.AppendText($"\r\n{e.Result.Text}");
        }
    }
}
