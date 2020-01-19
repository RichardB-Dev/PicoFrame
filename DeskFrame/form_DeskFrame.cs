using DeskFrame.Classes;
using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DeskFrame
{
    public partial class form_DeskFrame : Form
    {

        #region Public Members

        Config configData = new Config();
        public List<form_Frame> openFrames = new List<form_Frame>();
        int displayOffset = 0;

        #endregion


        public form_DeskFrame()
        {
            InitializeComponent();
            ni_DeskFrame.ContextMenuStrip = cms_NotifyMenu;

               }

        private void DeskFrame_Load(object sender, EventArgs e)
        {
            LoadSavedData();
        }
        
        private void LoadSavedData()
        {
            if (openFrames != null)
            {
                foreach (form_Frame frame in openFrames)
                {
                    frame.Dispose();
                }
            }
            openFrames = new List<form_Frame>();
            
            if (configData.appConfig.Frames != null && configData.appConfig.Frames.Count != 0)  //Check for saved frames
            {
                displayOffset = 0;  //Reset Offset                
                foreach (Frame savedFrame in configData.appConfig.Frames) //Load Saved Frames
                {
                    createFrame(savedFrame);
                }
            }
            else
            {
                createFrame(new Frame());
            }
        }


        private void DeskFrame_Resize(object sender, EventArgs e)
        {
            ni_DeskFrame.Visible = true;
            this.Hide();
        }
         
        private void FrameFormResize(object sender, EventArgs e)
        {
            RepositionFrames();
        }

        private void FrameFormClosed(object sender, FormClosedEventArgs e)
        {
            LoadSavedData();
            RepositionFrames();
        }

        private void RepositionFrames()
        {
            displayOffset = 0;
            foreach (form_Frame formFrame in openFrames)
            {
                formFrame.displayOffset = displayOffset;
                formFrame.DockWindow();
                displayOffset = displayOffset + 10 + formFrame.Height;
            }
        }
        
        private void newFrame(object sender, EventArgs e)
        {
            createFrame(new Frame());
        }

        private void createFrame(Frame savedFrame = null)
        {
            form_Frame Frame = new form_Frame(ref configData, savedFrame, displayOffset);
            Frame.Show();
            Frame.SendToBack();
            Frame.SizeChanged += new EventHandler(FrameFormResize);
            Frame.FormClosed += new FormClosedEventHandler(FrameFormClosed);
            ((Button)Frame.Controls.Find("btn_NewFrame", true)[0]).Click += new EventHandler(newFrame);
            openFrames.Add(Frame);
            displayOffset = displayOffset + 10 + Frame.Height;
        }

        private void closeDeskFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    

    }
}
