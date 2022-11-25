using InputManager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Ultra_Command.Models;

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
        private Profile _currentProfile;

        public MainWindow()
        {
            _isListening = false;
            _speechSynthesizer = new SpeechSynthesizer();
            InitializeComponent();
            LoadProfiles();
            LoadFile();
        }

        private void LoadProfiles()
        {
            string profilesPath = System.AppDomain.CurrentDomain.BaseDirectory + "Profiles\\";
            var myFiles = Directory.GetFiles(profilesPath, "*.json", SearchOption.AllDirectories);
            foreach(var file in myFiles)
            {
                ProfileSelector.Items.Add(file);
            }
        }

        private void LoadFile(string filePath = null)
        {
            if(filePath == null)
            {
                filePath = System.IO.Directory.GetCurrentDirectory() + "\\Profiles\\EliteDangerous.json";
            }
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                _currentProfile = JsonConvert.DeserializeObject<Profile>(json);
            }
        }

        private void Recording_Button_Click(object sender, RoutedEventArgs e)
        {
            // disable/enable edit buttons.
            // start recording

            if (_isListening)
            {
                UpdateTextBox($"\r\nSTOPPING SERVICE...");
                _isListening = false;
                _recognizer.RecognizeAsyncCancel();
            }
            else
            {
                _isListening = true;
                UpdateTextBox($"\r\nLISTENING FOR COMMANDS.....");
                _speechSynthesizer.Pause();
                _recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));

                var keywords = CreateListOfKeywords(_currentProfile);

                // greaemmer to be set to my custom commands
                _recognizer.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(keywords))));

                // Configure input to the speech recognizer.  
                _recognizer.SetInputToDefaultAudioDevice();

                // Start asynchronous, continuous speech recognition.  
                _recognizer.RecognizeAsync(RecognizeMode.Multiple);

                // Add a handler for the speech recognized event.  
                _recognizer.SpeechRecognized +=
                  new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
            }


        }

        private string[] CreateListOfKeywords(Profile currentProfile)
        {
            return currentProfile.VoiceCommands.SelectMany(x => x.TriggerKeywords).ToArray();
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechDetectedEventArgs e)
        {
        }

        // Handle the SpeechRecognized event.  
        public void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            var text = e.Result.Text;
            foreach (var voiceCommand in _currentProfile.VoiceCommands)
            {
                if (voiceCommand.TriggerKeywords.Contains(text))
                {
                    ExecuteCommands(voiceCommand);
                    UpdateTextBox(e.Result.Text);
                }
            }
        }

        private void ExecuteCommands(VoiceCommand voiceCommand)
        {
            var allKeyPresses = voiceCommand.Commands.Select(x => x.Execute);
            foreach (var keyPress in allKeyPresses)
            {
                if (keyPress.ToList().Count == 1)
                {
                    //Keyboard.KeyDown(keyPress.ElementAt(0));
                    //Thread.Sleep(50);
                    //Keyboard.KeyUp(keyPress.ElementAt(0));
                    Keyboard.ShortcutKeys(keyPress.ToArray());
                }
                else if (keyPress.ToList().Count >= 2)
                {
                    Keyboard.ShortcutKeys(keyPress.ToArray());
                }
            }

            if(voiceCommand.Say.ToList().Count > 0)
            {
                Speak(voiceCommand.Say.ToList());
            }
        }

        private void Speak(List<string> list)
        {
            var random = new Random();
            int index = random.Next(list.Count);
            var synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
            synthesizer.Rate = 0;
            synthesizer.Speak(list.ElementAt(index));
        }

        public void UpdateTextBox(string text)
        {
            Listening_Logs.AppendText($"\r\n{text}");
            Listening_Logs.ScrollToEnd();
        }

        private void ProfileSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            LoadFile(e.AddedItems[0].ToString());
        }
    }
}
