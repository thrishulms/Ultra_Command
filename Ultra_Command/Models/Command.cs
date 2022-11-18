using System.Collections.Generic;
using System.Windows.Forms;
using Ultra_Command.Enums;

namespace Ultra_Command.Models
{
    public class Command
    {
        public CommandType Type { get; set; }
        public IEnumerable<Keys> Execute { get; set; }
    }
}
