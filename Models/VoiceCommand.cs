using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceMacroConsole.Models
{
    class VoiceCommand
    {
        public string Name { get; set; }
        public IEnumerable<string> TriggerKeywords { get; set; }
        public IEnumerable<Command> Commands { get; set; }
        public IEnumerable<string> Say { get; set; }
}
}
