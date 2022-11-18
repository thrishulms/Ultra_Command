using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultra_Command.Models
{
    public class Profile
    {
        public string Name { get; set; }
        public IEnumerable<VoiceCommand> VoiceCommands { get; set; }
    }
}
