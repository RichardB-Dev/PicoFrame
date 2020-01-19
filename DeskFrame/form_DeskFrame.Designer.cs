namespace DeskFrame
{
    partial class form_DeskFrame
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(form_DeskFrame));
            this.ni_DeskFrame = new System.Windows.Forms.NotifyIcon(this.components);
            this.cms_NotifyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeDeskFrameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_NotifyMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ni_DeskFrame
            // 
            this.ni_DeskFrame.Icon = ((System.Drawing.Icon)(resources.GetObject("ni_DeskFrame.Icon")));
            this.ni_DeskFrame.Text = "Desk Frame";
            this.ni_DeskFrame.Visible = true;
            // 
            // cms_NotifyMenu
            // 
            this.cms_NotifyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeDeskFrameToolStripMenuItem});
            this.cms_NotifyMenu.Name = "cms_NotifyMenu";
            this.cms_NotifyMenu.Size = new System.Drawing.Size(168, 26);
            // 
            // closeDeskFrameToolStripMenuItem
            // 
            this.closeDeskFrameToolStripMenuItem.Name = "closeDeskFrameToolStripMenuItem";
            this.closeDeskFrameToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.closeDeskFrameToolStripMenuItem.Text = "Close Desk Frame";
            this.closeDeskFrameToolStripMenuItem.Click += new System.EventHandler(this.closeDeskFrameToolStripMenuItem_Click);
            // 
            // form_DeskFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 207);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "form_DeskFrame";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "DeskFrame";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.DeskFrame_Load);
            this.Resize += new System.EventHandler(this.DeskFrame_Resize);
            this.cms_NotifyMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon ni_DeskFrame;
        private System.Windows.Forms.ContextMenuStrip cms_NotifyMenu;
        private System.Windows.Forms.ToolStripMenuItem closeDeskFrameToolStripMenuItem;
    }
}