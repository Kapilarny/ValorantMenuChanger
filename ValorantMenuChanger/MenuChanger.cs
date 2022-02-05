using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ValorantMenuChanger
{
    internal class MenuChanger
    {
        public static void ReplaceMenu(string videoPath, string valorantPath)
        {
            string destPath = valorantPath + "/live/ShooterGame/Content/Movies/Menu/HomeScreen.mp4";

            try
            {
                File.Copy(videoPath, destPath, true);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static bool IsValorantRunning()
        {
            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName == "VALORANT")
                {
                    return true;
                }
            }

            return false;
        }
    }
}
