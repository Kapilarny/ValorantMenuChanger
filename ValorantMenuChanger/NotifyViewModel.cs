using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace ValorantMenuChanger
{
    public class NotifyViewModel : ViewModelBase
    {
        public ICommand NotifyCommand { get; }

        public NotifyViewModel(NotifyIcon notifyIcon)
        {
            NotifyCommand = new NotifyCommand(notifyIcon);
        }
    }
}
