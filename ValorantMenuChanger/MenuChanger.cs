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

            string ep4hc = valorantPath + "/live/ShooterGame/Content/Movies/Menu/Riot_Mirrorverse_Ep4_Act2_HD_220209-H264.mp4";

            if(File.Exists(ep4hc))
            {
                Console.WriteLine("ep4hc exists!");
                // Patch ep4 main menu file
                try
                {
                    File.Copy(videoPath, ep4hc, true);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

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
