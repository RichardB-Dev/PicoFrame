using DeskFrame.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace DeskFrame
{
    public partial class form_Frame : Form
    {
        //***** Public Members *****
        #region Public Members

        Config configData = new Config(); // Refrence Config Data
        Frame frameData = new Frame(); // Local Frame Data
        Image frameImage;
        public int frameVerticalOffset = 0; // Vertical Frame Position Offset

        #endregion

        //***** Initialise Form *****
        #region Initialise Form

        /// <summary>
        /// Form Frame Constructor
        /// </summary>
        /// <param name="_ConfigData"></param>
        /// <param name="_FrameData"></param>
        /// <param name="_FrameVerticalOffset"></param>
        public form_Frame(ref Config _ConfigData, Frame _FrameData, int _FrameVerticalOffset = 0)
        {
            InitializeComponent();

            //Load public members
            frameData = _FrameData;
            configData = _ConfigData;
            frameVerticalOffset = _FrameVerticalOffset;

            //Setup Initial Size
            this.SendToBack();
            this.Width = 220;
            this.Height = 250;
            pnl_Frame.Width = 220;
            pnl_Frame.Height = 250;
            pnl_Image.Left = 10;
            pnl_Image.Top = 20;
            pnl_Image.Width = 200;
            pnl_Image.Height = 200;            
        }

        /// <summary>
        /// Load frame data(image, message, size)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void form_Frame_Load(object sender, EventArgs e)
        {
            // If index is unset then create new frame
            if (frameData.Index == 0)
            {
                SaveFrame(); //Create frame data
            }

            // Load frame data if image is set
            if (!String.IsNullOrEmpty(frameData.Image))
            {
                LoadFrame(frameData);
                btn_Add.Visible = false;
                menuStrip.Enabled = true;
                MenuItemsEnable(true);
            }
            else
            {
                MenuItemsEnable(false);
                DockWindow();
            }
        }

        /// <summary>
        /// Frame focused
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void form_Frame_Activated(object sender, EventArgs e)
        {
            this.SendToBack();
        }
        
        /// <summary>
        /// Frame unfocus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void form_Frame_Deactivate(object sender, EventArgs e)
        {
            //If message is in edit mode, save and disable textbox
            if (tb_DisplayMessage.BorderStyle == BorderStyle.Fixed3D)
            {
                SaveFrameMessage();
            }
        }

        #endregion

        //***** Controller Handlers *****
        #region Controller Handlers
            
        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (SelectImage()) //Select frame image
            {
                btn_Add.Visible = false;
                menuStrip.Enabled = true;
                MenuItemsEnable(true);
                EditFrameMessage(); //Set message to edit mode
            }
        }

        private void tb_DisplayMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) // If Enter key is pressed, save and disable textbox
            {
                SaveFrameMessage();
            }
        }

        private void tb_DisplayMessage_Enter(object sender, EventArgs e)
        {
            // If Display Message textbox is focused but not in edit mode, then change focus
            // Controller not disabled because of Grey Disable styling
            if (tb_DisplayMessage.BorderStyle == BorderStyle.None)
            {
                pnl_Frame.Focus();
            }
        }
        
        #region Menu

        /// <summary>
        /// Open file dialog to select new frame image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectImage();
        }
        
        /// <summary>
        /// Update frame form size - Small
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void smallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSize(300);
        }
        /// <summary>
        /// Update frame form size - Medium
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSize(400);
        }
        /// <summary>
        /// Update frame form size - Large
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void largeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSize(600);
        }

        /// <summary>
        /// Set display message to edit mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditFrameMessage();
        }

        /// <summary>
        /// Open new frame 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void newFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btn_NewFrame.PerformClick(); // Parent Event : Control Form - Create New Frame
        }

        /// <summary>
        /// Delete and close this frame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void removeFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveFrame(); // Delete saved data
            this.Close();
        }

        #endregion

        #endregion

        //***** Functions *****
        #region Functions        

        #region Save, Load and Delete data

        /// <summary>
        /// Load Frame Data
        /// </summary>
        /// <param name="pFrame"></param>
        public void LoadFrame(Frame pFrame)
        {
            if (!String.IsNullOrEmpty(pFrame.Image))
            {
              //  frameImage = Image.FromFile(pFrame.Image);
              //  pnl_Image.BackgroundImage.Dispose();
                pnl_Image.BackgroundImage = Image.FromFile(pFrame.Image);
                tb_DisplayMessage.Text = pFrame.Message;
                ScaleForm(pnl_Image.BackgroundImage.Size, frameData.Size);
                           
            
            }
        }

        /// <summary>
        /// Save frame data to Json file
        /// </summary>
        public void SaveFrame()
        {
            frameData = configData.SaveFrame(frameData);            
        }

        /// <summary>
        /// Copy selected image to temp folder
        /// </summary>
        public void SaveImage(string imageFile)
        {
            frameData.Image = configData.CloneImage(imageFile, frameData.Index);
        }

        /// <summary>
        /// Delete frame from saved data
        /// </summary>
        private void RemoveFrame()
        {
            configData.RemoveFrame(frameData);
        }

        #endregion

        #region Display Settings

        /// <summary>
        /// Open dialog and select frame image
        /// </summary>
        /// <returns></returns>
        private bool SelectImage()
        {
            OpenFileDialog SelectedFile = new OpenFileDialog();
            SelectedFile.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"; //Allowed image filetypes
            DialogResult result = SelectedFile.ShowDialog(); // Show the dialog.

            if (result == DialogResult.OK) // Test result.
            {
                string imageFile = SelectedFile.FileName;
                try
                {
                    if (pnl_Image.BackgroundImage != null) pnl_Image.BackgroundImage.Dispose();
                    SaveImage(imageFile);
                    SaveFrame();       
                    pnl_Image.BackgroundImage = Image.FromFile(frameData.Image);
                    ScaleForm(pnl_Image.BackgroundImage.Size, frameData.Size);
                     
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please select an image file type.");
                }
            }
            return false;
        }

        /// <summary>
        /// Update frame size (Small:300, Medium:400, Large:600)
        /// </summary>
        /// <param name="Size"></param>
        private void SetSize(int Size = 400)
        {
            ScaleForm(pnl_Image.BackgroundImage.Size, Size);
            frameData.Size = Size;
            SaveFrame();
        }

        /// <summary>
        /// Scale form based on image size ratio
        /// </summary>
        /// <param name="OriginalImageSize"></param>
        /// <param name="FrameSize"></param>
        private void ScaleForm(Size OriginalImageSize, double FrameSize = 400)
        {
            double PanelWidth = OriginalImageSize.Width;
            double PanelHeight = OriginalImageSize.Height;
            double FullLength = PanelWidth + PanelHeight;
            pnl_Image.Width = Convert.ToInt32((FrameSize / FullLength) * PanelWidth);
            pnl_Image.Height = Convert.ToInt32((FrameSize / FullLength) * PanelHeight);

            pnl_Frame.Width = pnl_Image.Width + 20;
            pnl_Frame.Height = pnl_Image.Height + 50;

            this.Width = pnl_Image.Width + 20;
            this.Height = pnl_Image.Height + 50;
            tb_DisplayMessage.Width = this.Width - 50;

            this.Focus();
            DockWindow();
        }

        /// <summary>
        /// Position frame to right of screen
        /// </summary>
        public void DockWindow()
        {
            int bound_X = 0;
            int bound_Y = 0;
            Rectangle bounds = Screen.PrimaryScreen.Bounds;

            //Get Screen Top Right Corner Location
            if (bounds.X >= 0)
            {
                bound_X = bounds.X + bounds.Width - this.Width - 20;
                bound_Y = bounds.Y + 20;
            }
            else
            {
                bound_X = bounds.X + this.Width - 40;
                bound_Y = bounds.Y;
            }

            this.SetBounds(bound_X, bound_Y + frameVerticalOffset, this.Width, this.Height);
            this.SendToBack();
        }

        #endregion

        /// <summary>
        /// Enable message textbox for editing 
        /// </summary>
        private void SaveFrameMessage()
        {
            tb_DisplayMessage.BorderStyle = BorderStyle.None;
            tb_DisplayMessage.ReadOnly = true;
            frameData.Message = tb_DisplayMessage.Text;
            pnl_Frame.Focus(); // Remove focus from textbox
            SaveFrame();
        }

        /// <summary>
        /// Enable message textbox for editing 
        /// </summary>
        private void EditFrameMessage()
        {
            tb_DisplayMessage.ReadOnly = false;
            tb_DisplayMessage.BorderStyle = BorderStyle.Fixed3D;
            tb_DisplayMessage.Focus();
        }

        /// <summary>
        /// Enable/Disable Image Menu Items
        /// </summary>
        /// <param name="pEnable"></param>
        private void MenuItemsEnable(bool pEnable = true)
        {
            changeImageToolStripMenuItem.Enabled = pEnable;
            sizeToolStripMenuItem.Enabled = pEnable;
            smallToolStripMenuItem.Enabled = pEnable;
            mediumToolStripMenuItem.Enabled = pEnable;
            largeToolStripMenuItem.Enabled = pEnable;
            editMessageToolStripMenuItem.Enabled = pEnable;
        }

        #endregion

    }
}
