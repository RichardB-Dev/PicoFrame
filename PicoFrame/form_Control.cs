using PicoFrame.Classes;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PicoFrame
{
    public partial class form_Control : Form
    {

        //***** Public Members *****
        #region Public Members

        Config configData = new Config();
        public List<form_Frame> openFrames = new List<form_Frame>();
        int frameVerticalOffset = 0;

        #endregion

        //***** Initialise Form *****
        #region Initialise Form

        /// <summary>
        /// Control form constructor
        /// </summary>
        public form_Control()
        {      
            InitializeComponent();
            ni_PicoFrame.ContextMenuStrip = cms_NotifyMenu; // Add items to Notify Icon
            ni_PicoFrame.Visible = true;
            this.Hide();
        }

        /// <summary>
        /// Open saved frames
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void form_Control_Load(object sender, EventArgs e)
        {
            if (configData.ConfigFileCreated) // If just installed, minimize all open windows
            {
                MinimizeAll();
            }
            LoadSavedData();
        }

        #endregion
        
        //***** Controller Handlers *****
        #region Controller Handlers

        /// <summary>
        /// Close all frames and application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeDeskFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        //***** Functions *****
        #region Functions  
            
        /// <summary>
        /// Open frames
        /// </summary>
        private void LoadSavedData()
        {
            if (openFrames.Count != 0) // If frames are already loaded, close all frames
            {
                foreach (form_Frame frame in openFrames)
                {
                    frame.Dispose();
                }
            }
            openFrames = new List<form_Frame>(); // Clear open frames list
            
            if (configData.appConfig.Frames != null && configData.appConfig.Frames.Count != 0)  // Check for saved frames
            {
                frameVerticalOffset = 0;  // Reset Offset                
                foreach (Frame savedFrame in configData.appConfig.Frames) // Load Saved Frames
                {
                    createFrame(savedFrame);
                }
            }
            else
            {
                createFrame(new Frame()); // Create blank frame
            }
        }

        /// <summary>
        /// Handle child new frame click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newFrame(object sender, EventArgs e)
        {
            createFrame(new Frame());
        }

        /// <summary>
        /// Open saved/new frame
        /// </summary>
        /// <param name="savedFrame"></param>
        private void createFrame(Frame savedFrame = null)
        {
            form_Frame Frame = new form_Frame(ref configData, savedFrame, frameVerticalOffset);
            Frame.Show();
            Frame.SendToBack();
            Frame.SizeChanged += new EventHandler(FrameFormResize);
            Frame.FormClosed += new FormClosedEventHandler(FrameFormClosed);
            ((Button)Frame.Controls.Find("btn_NewFrame", true)[0]).Click += new EventHandler(newFrame);
            openFrames.Add(Frame);
            frameVerticalOffset = frameVerticalOffset + 10 + Frame.Height;
        }

        /// <summary>
        /// Handle Frame Resize(Small,Medium,Large) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrameFormResize(object sender, EventArgs e)
        {
            RepositionFrames();
        }

        /// <summary>
        /// Reposition all frames vertical spacing
        /// </summary>
        private void RepositionFrames()
        {
            frameVerticalOffset = 0; // Reset offset
            foreach (form_Frame formFrame in openFrames)
            {
                formFrame.frameVerticalOffset = frameVerticalOffset;
                formFrame.DockWindow();
                frameVerticalOffset = frameVerticalOffset + 10 + formFrame.Height;
            }
        }

        /// <summary>
        /// Reload and reposition all frames
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrameFormClosed(object sender, FormClosedEventArgs e)
        {
            LoadSavedData();
            RepositionFrames();
        }


        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Minimize all open windows
        /// </summary>
        private void MinimizeAll()
        {
            IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
            SendMessage(lHwnd, 0x111, (IntPtr)419, IntPtr.Zero);
        }

        #endregion

    }
}
