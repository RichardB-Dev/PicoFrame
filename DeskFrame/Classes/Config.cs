using DeskFrame.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DeskFrame
{
    public class Config
    {

        //Public Members
        #region Public Members

        public AppConfig appConfig = new AppConfig();
        public string AppPath = AppDomain.CurrentDomain.BaseDirectory;
        public string TempDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\DeskFrame\";


        #endregion

        public Config()
        {
            LoadAppConfig();
        }

        public void LoadAppConfig()
        {
            if (!Directory.Exists(TempDataPath))
            {
                System.IO.Directory.CreateDirectory(TempDataPath);
            }
            if (!File.Exists(TempDataPath + @"AppConfig.json"))
            {
                string writeJson = JsonConvert.SerializeObject(appConfig);
                System.IO.File.WriteAllText(TempDataPath + @"AppConfig.json", writeJson);
                CreateShortcut("DeskFrame_Shortcut", @"C:\Users\Switch\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup", AppDomain.CurrentDomain.BaseDirectory + @"\DeskFrame.exe");

            }
            else
            {
                using (StreamReader r = new StreamReader(TempDataPath + @"AppConfig.json"))
                {
                    string json = r.ReadToEnd();
                    appConfig = JsonConvert.DeserializeObject<AppConfig>(json);
                }
            }

            
        }

        public void SaveFrames()
        {
            string writeJson = JsonConvert.SerializeObject(appConfig);
            //write string to file
            System.IO.File.WriteAllText(TempDataPath + @"AppConfig.json", writeJson);
        }

        public Frame SaveFrame(Frame pFrame)
        {
            if (pFrame.Index == 0) // Save New Frame
            {
                if (appConfig.Frames == null || appConfig.Frames.Count() == 0)
                {
                    appConfig.Frames = new List<Frame>();
                    pFrame.Index = 1; //Set starting index
                }
                else
                {
                    pFrame.Index = appConfig.Frames[appConfig.Frames.Count() - 1].Index + 1; //Increment Index
                }
                    
                appConfig.Frames.Add(pFrame);
            }
            else { // Update Existing Frame
                foreach (Frame storedFrame in appConfig.Frames)
                {
                    if (storedFrame.Index == pFrame.Index)
                    {
                        storedFrame.Image = pFrame.Image;
                        storedFrame.Message = pFrame.Message;
                        storedFrame.Size = pFrame.Size;
                    }
                }
            }

            string writeJson = JsonConvert.SerializeObject(appConfig);
            System.IO.File.WriteAllText(TempDataPath + @"AppConfig.json", writeJson);

            return pFrame;
        }

        public Frame RemoveFrame(Frame pFrame)
        {
            List<Frame> updatedFrames = new List<Frame>();
            foreach (Frame storedFrame in appConfig.Frames)
            {
                if (storedFrame.Index != pFrame.Index)
                {
                    updatedFrames.Add(storedFrame);
                }
            }
            appConfig.Frames = updatedFrames;
            string writeJson = JsonConvert.SerializeObject(appConfig);
            System.IO.File.WriteAllText(TempDataPath + @"AppConfig.json", writeJson);

            return pFrame;
        }

        public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "Desk Frame";   // The description of the shortcut
          //  shortcut.IconLocation = @"c:\myicon.ico";           // The icon of the shortcut
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
            shortcut.Save();                                    // Save the shortcut
        }
    }

}
