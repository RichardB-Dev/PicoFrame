using DeskFrame.Classes;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace DeskFrame
{
    public partial class form_Frame : Form
    {

        #region Public Members

        Frame frameData = new Frame();
        Config configData = new Config();
        public int displayOffset = 0;

        #endregion

        #region Initialise Form
        //***** Initialise Form *****

        public form_Frame(ref Config pConfigData, Frame pFrame, int offset = 0)
        {
            InitializeComponent();
            frameData = pFrame;
            configData = pConfigData;

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

            displayOffset = offset;
        }

        internal void Resize()
        {
            throw new NotImplementedException();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (frameData.Index == 0)
            {
                MenuItemsEnable(false);
                DockWindow();
            }
            else
            {
                LoadFrame(frameData);
                btn_Add.Visible = false;
                menuStrip.Enabled = true;
                MenuItemsEnable(true);
            }
        }


        private void form_Frame_Activated(object sender, EventArgs e)
        {
            this.SendToBack();
        }

        private void pnl_Frame_Click(object sender, EventArgs e)
        {
            this.SendToBack();
        }

        #endregion

        #region Controllers
        //***** Controllers *****

        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (SelectImage())
            {
                btn_Add.Visible = false;
                menuStrip.Enabled = true;
                MenuItemsEnable(true);
                EditFrameMessage();
            }
        }

        private void tb_DisplayMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                tb_DisplayMessage.BorderStyle = BorderStyle.None;
                tb_DisplayMessage.ReadOnly = true;
                pnl_Frame.Focus();
                frameData.Message = tb_DisplayMessage.Text;
                SaveFrame();
            }
        }

        private void tb_DisplayMessage_Enter(object sender, EventArgs e)
        {
            if (tb_DisplayMessage.BorderStyle == BorderStyle.None)
            {
                pnl_Frame.Focus();
            }
        }

        #region Menu
        // Menu - Change Image
        private void changeImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectImage();
        }

        // Menu - Change Size
        private void smallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSize(300);
        }
        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSize(400);
        }
        private void largeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSize(600);
        }

        // Menu - Edit Frame Message
        private void editMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditFrameMessage();
        }

        //Menu - New Frame
        public void newFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Load new frame
            btn_NewFrame.PerformClick();
        }

        //Menu - remove Frame 
        private void removeFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveFrame();
            this.Close();
        }

        #endregion


        #endregion

        #region Functions
        //***** Functions *****

        public void LoadFrame(Frame pFrame)
        {
            pnl_Image.BackgroundImage = Image.FromFile(pFrame.Image);
            tb_DisplayMessage.Text = pFrame.Message;
            ScaleForm(pnl_Image.BackgroundImage.Size, frameData.Size);
        }

        public void SaveFrame()
        {
            frameData = configData.SaveFrame(frameData);            
        }

        private void RemoveFrame()
        {
            configData.RemoveFrame(frameData);
        }

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

            this.SetBounds(bound_X, bound_Y + displayOffset, this.Width, this.Height);
            this.SendToBack();
        }

        private bool SelectImage()
        {
            OpenFileDialog SelectedFile = new OpenFileDialog();
            SelectedFile.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            DialogResult result = SelectedFile.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = SelectedFile.FileName;
                try
                {
                    pnl_Image.BackgroundImage = Image.FromFile(file);
                    frameData.Image = file;
                    ScaleForm(pnl_Image.BackgroundImage.Size, frameData.Size);
                    SaveFrame();
                    return true;
                }
                catch (IOException)
                {
                    MessageBox.Show("Please select an image file type.");
                }
            }
            return false;
        }

        private void EditFrameMessage()
        {
            tb_DisplayMessage.ReadOnly = false;
            tb_DisplayMessage.BorderStyle = BorderStyle.Fixed3D;
            tb_DisplayMessage.Focus();
        }

        private void SetSize(int Size = 400)
        {
            ScaleForm(pnl_Image.BackgroundImage.Size, Size);
            frameData.Size = Size;
            SaveFrame();
        }

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

            this.Focus();
            DockWindow();

        }

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
