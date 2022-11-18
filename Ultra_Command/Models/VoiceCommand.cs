using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultra_Command.Models
{
    public class VoiceCommand
    {
        public string Name { get; set; }
        public IEnumerable<string> TriggerKeywords { get; set; }
        public IEnumerable<Command> Commands { get; set; }
        public IEnumerable<string> Say { get; set; }
    }
}
