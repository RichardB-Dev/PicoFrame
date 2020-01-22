using DeskFrame.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace DeskFrame
{
    public class Config
    {

        //***** Public Members *****
        #region Public Members

        public AppConfig appConfig = new AppConfig();
        public string AppPath = AppDomain.CurrentDomain.BaseDirectory;
        public bool ConfigFileCreated = false;

        #endregion
        

        /// <summary>
        /// Config constructor
        /// </summary>
        public Config()
        {
            LoadAppConfig();
        }

        /// <summary>
        /// Load/Create config save file
        /// </summary>
        public void LoadAppConfig()
        {
            if (!File.Exists(AppPath + @"AppConfig.json")) // Create Config JSON file if does not exist
            {
                WriteToSaveFile(); // Create file
                string windowsStartUpFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                CreateShortcut("DeskFrame_Shortcut", windowsStartUpFolder, AppDomain.CurrentDomain.BaseDirectory + @"\DeskFrame.exe"); // Create start up shortcut
                ConfigFileCreated = true;
            }
            else
            {
                using (StreamReader r = new StreamReader(AppPath + @"AppConfig.json")) // Read saved config file
                {
                    string json = r.ReadToEnd();
                    appConfig = JsonConvert.DeserializeObject<AppConfig>(json);
                }
            }            
        }
               
        /// <summary>
        /// Add/Update frame to save file
        /// </summary>
        /// <param name="pFrame"></param>
        /// <returns></returns>
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
                    if (storedFrame.Index == pFrame.Index) // Frame found
                    {
                        storedFrame.Image = pFrame.Image;
                        storedFrame.Message = pFrame.Message;
                        storedFrame.Size = pFrame.Size;
                    }
                }
            }
            WriteToSaveFile();
            return pFrame;
        }

        /// <summary>
        /// Delete frame from save file
        /// </summary>
        /// <param name="pFrame"></param>
        /// <returns></returns>
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
            WriteToSaveFile();
            return pFrame;
        }

        /// <summary>
        /// Write local config data to save file
        /// </summary>
        private void WriteToSaveFile()
        {
            string writeJson = JsonConvert.SerializeObject(appConfig);
            System.IO.File.WriteAllText(AppPath + @"AppConfig.json", writeJson);
        }

        /// <summary>
        /// Copy image to temp folder
        /// </summary>
        /// <param name="ImageDir"></param>
        /// <param name="frameIndex"></param>
        /// <returns></returns>
        public string CloneImage(string ImageDir, int frameIndex)
        {
            string Directory_Images = AppPath + "Images";
            if (!Directory.Exists(Directory_Images))  // Create image directory if it doesnt exist
            {
                System.IO.Directory.CreateDirectory(Directory_Images);
            }

            string newImageFile = Directory_Images + @"\image_" + frameIndex.ToString() + ".png";         
            if (File.Exists(newImageFile))
            {
                File.Delete(newImageFile);
            } 

            using(Image newImage = Image.FromFile(ImageDir)){
                newImage.Save(newImageFile, ImageFormat.Png);
            }          
            return newImageFile;
        }

        /// <summary>
        /// Create app shortcut
        /// </summary>
        /// <param name="shortcutName"></param>
        /// <param name="shortcutPath"></param>
        /// <param name="targetFileLocation"></param>
        public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "Desk Frame";   // The description of the shortcut
            shortcut.TargetPath = targetFileLocation; // The path of the file that will launch when the shortcut is run
            shortcut.Save(); // Save the shortcut
        }

    }

}
