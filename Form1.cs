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
using System.Windows.Forms;
using VoiceMacroConsole.Models;

namespace Ultra_Command
{
    public partial class Form1 : Form
    {
        private SpeechSynthesizer _speechSynthesizer;
        private SpeechRecognitionEngine _recognizer;
        private Profile currentProfile;

        public Form1()
        {
            InitializeComponent();
            _speechSynthesizer = new SpeechSynthesizer();
            LoadJson();
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
            VerifyText(e.Result.Text);
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            _speechSynthesizer.Pause();
            _recognizer.Dispose();
        }

        private void profileSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var profiles = new DataTable();
            //DataColumn column;
            //DataRow row;

            //// Create first column and add to the DataTable.
            //column = new DataColumn();
            //column.DataType = System.Type.GetType("System.Int32");
            //column.ColumnName = "ID";
            //column.AutoIncrement = true;
            //column.Caption = "ID";
            //column.ReadOnly = true;
            //column.Unique = true;
            //profiles.Columns.Add(column);

            //// Create second column.
            //column = new DataColumn();
            //column.DataType = System.Type.GetType("System.String");
            //column.ColumnName = "Name";
            //column.AutoIncrement = false;
            //column.Caption = "Name";
            //column.ReadOnly = false;
            //column.Unique = false;
            //profiles.Columns.Add(column);

            //for (int i = 0; i <= 4; i++)
            //{
            //    row = profiles.NewRow();
            //    row["childID"] = i;
            //    row["ChildItem"] = "Item " + i;
            //    row["ParentID"] = 0;
            //    profiles.Rows.Add(row);
            //}
        }

        public void LoadJson()
        {
            var filePath = System.IO.Directory.GetCurrentDirectory() + "\\Profiles\\3_ed.json";
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                currentProfile = JsonConvert.DeserializeObject<Profile>(json);
            }
        }

        private void VerifyText(string voiceCommandReceived)
        {
            // var testt = VirtualKeyCode.LCONTROL;
            foreach (var voiceCommand in currentProfile.VoiceCommands)
            {
                foreach (var triggerKeywords in voiceCommand.TriggerKeywords)
                {
                    if (string.Equals(triggerKeywords.ToLower(), voiceCommandReceived.ToLower()))
                    {
                        var allKeyPresses = voiceCommand.Commands.Select(x => x.Execute);
                        foreach (var keyPress in allKeyPresses)
                        {
                            if (keyPress.ToList().Count == 1)
                            {
                                // Keyboard.KeyDown(Keys.D1);
                                Keyboard.KeyDown(keyPress.ElementAt(0));
                                Thread.Sleep(100);
                                Keyboard.KeyUp(keyPress.ElementAt(0));
                            }
                            else if (keyPress.ToList().Count >= 2)
                            {
                                Keyboard.ShortcutKeys(keyPress.ToArray());
                            }
                        }

                        Speak(voiceCommand.Say.ToList());
                    }
                }
            }
        }

        private void Speak(List<string> sayText)
        {
            var random = new Random();
            int index = random.Next(sayText.Count);
            var synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
            synthesizer.Rate = 0;
            synthesizer.Speak(sayText.ElementAt(index));
        }
    }
}
