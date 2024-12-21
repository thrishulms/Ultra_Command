using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Ultra_Command.Models;

namespace Ultra_Command.Windows
{
    /// <summary>
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        private Profile _currentProfile;

        public ProfileWindow(Profile currentProfile)
        {
            _currentProfile = currentProfile;
            InitializeComponent();
            ProfileName.Content = _currentProfile.Name;
        }
    }
}
