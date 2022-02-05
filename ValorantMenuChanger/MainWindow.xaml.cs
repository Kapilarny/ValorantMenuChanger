using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Forms = System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

using Microsoft.Win32;
using System.Reflection;
using IWshRuntimeLibrary;

namespace ValorantMenuChanger
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Forms.NotifyIcon notifyIcon = new Forms.NotifyIcon();
        private readonly Forms.Timer timer = new Forms.Timer();

        private bool alreadyPatched = false;

        public MainWindow()
        {
            ValorantSettings defaultSettings = GetCurrentSettings();
            if(defaultSettings == null)
            {
                defaultSettings = new ValorantSettings();
                defaultSettings.valorantPath = "VALORANT Folder Path";
                defaultSettings.videoPath = "Video Path (mp4 only!)";
            }

            notifyIcon.Icon = ValorantMenuChanger.Properties.Resources.purpleblue;
            notifyIcon.Text = "Valorant Menu Changer";
            notifyIcon.Click += NotifyIcon_Click;
            notifyIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add("Exit", null, onExit);
            notifyIcon.Visible = true;

            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 5000;
            timer.Start();

            InitializeComponent();

            valorantPathInput.Text = defaultSettings.valorantPath;
            videoPathInput.Text = defaultSettings.videoPath;
        }

        private ValorantSettings GetCurrentSettings()
        {
            ValorantSettings settings = new ValorantSettings();

            if (!System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/ValorantMenuChanger/settings.txt")) return null;

            using (StreamReader sr = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/ValorantMenuChanger/settings.txt"))
            {
                settings.videoPath = sr.ReadLine();
                settings.valorantPath = sr.ReadLine();
            };

            return settings;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if(MenuChanger.IsValorantRunning() && !alreadyPatched)
            {
                string videoPath;
                string valorantPath;

                if (!System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/ValorantMenuChanger/settings.txt")) return;

                using (StreamReader sr = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/ValorantMenuChanger/settings.txt"))
                {
                    videoPath = sr.ReadLine();
                    valorantPath = sr.ReadLine();
                };

                if (!System.IO.File.Exists(videoPath)) return;
                if (!Directory.Exists(valorantPath + "/live")) return;

                MenuChanger.ReplaceMenu(videoPath, valorantPath);

                new NotifyViewModel(notifyIcon).NotifyCommand.Execute("Successfully patched the Menu!");

                alreadyPatched = true;
            } else if(!MenuChanger.IsValorantRunning() && alreadyPatched) 
            {
                alreadyPatched = false;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void closeBtnPress(object sender, MouseButtonEventArgs e)
        {
            this.Hide();

            ShowInTaskbar = false;
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void onExit(object sender, EventArgs e)
        {
            notifyIcon.Dispose();
            Close();
        }

        private void SaveOptionsClick(object sender, RoutedEventArgs e)
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/ValorantMenuChanger");
            System.IO.File.Create(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/ValorantMenuChanger/settings.txt").Close();


            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/ValorantMenuChanger/settings.txt"))
            {
                sw.WriteLine(videoPathInput.Text);
                sw.WriteLine(valorantPathInput.Text);
            };

            alreadyPatched = false;
            new NotifyViewModel(notifyIcon).NotifyCommand.Execute("Successfully saved the options!");

            this.Hide();
        }

        private void RunOnStartupClick(object sender, RoutedEventArgs e)
        {
            string startUpFolderPath =
              Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            if(System.IO.File.Exists(startUpFolderPath + "\\" +"ValorantMenuChanger.lnk"))
            {
                System.IO.File.Delete(startUpFolderPath + "\\" + "ValorantMenuChanger.lnk");
                new NotifyViewModel(notifyIcon).NotifyCommand.Execute("Saved! ValorantMenuChanger now doesn't run on startup!");
                return;
            }

            WshShell wshShell = new WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut;

            shortcut =
              (IWshRuntimeLibrary.IWshShortcut)wshShell.CreateShortcut(
                startUpFolderPath + "\\" +
                "ValorantMenuChanger.lnk");

            shortcut.TargetPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName);
            shortcut.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            shortcut.Description = "Launch My Application";
            shortcut.Save();

            new NotifyViewModel(notifyIcon).NotifyCommand.Execute("Saved! ValorantMenuChanger now runs on startup!");
        }
    }

    public class ValorantSettings
    {
        public string valorantPath;
        public string videoPath;
    }
}
