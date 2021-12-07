using System.Collections.Generic;
using System.Windows.Forms;
using VoiceMacroConsole.Enums;

namespace VoiceMacroConsole.Models
{
    class Command
    {
        public CommandType Type { get; set; }
        public IEnumerable<Keys> Execute { get; set; }
    }
}
