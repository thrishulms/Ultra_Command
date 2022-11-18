using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultra_Command.Profiles.Modules.SpeechModule.Interfaces
{
    public interface ICommandRecognizer
    {
        void CreateSpeechSystem();

        void StartListening();

        void StopListening();
    }
}
