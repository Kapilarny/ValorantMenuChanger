using System.Windows.Forms;

namespace ValorantMenuChanger
{
    internal class NotifyCommand : BaseCommand
    {
        private readonly NotifyIcon _notifyIcon;

        public NotifyCommand(NotifyIcon notifyIcon)
        {
            _notifyIcon = notifyIcon;
        }

        public override void Execute(object parameter)
        {
            _notifyIcon.ShowBalloonTip(3000, "Valorant Menu Changer", parameter.ToString(), ToolTipIcon.Info);
        }
    }
}
