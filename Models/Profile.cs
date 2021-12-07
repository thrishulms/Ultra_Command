using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceMacroConsole.Models
{
    class Profile
    {
        public string Name { get; set; }
        public IEnumerable<VoiceCommand> VoiceCommands { get; set; }
    }
}
