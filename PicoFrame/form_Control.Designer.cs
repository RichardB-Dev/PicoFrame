namespace PicoFrame
{
    partial class form_Control
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(form_Control));
            this.ni_PicoFrame = new System.Windows.Forms.NotifyIcon(this.components);
            this.cms_NotifyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeDeskFrameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_NotifyMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ni_PicoFrame
            // 
            this.ni_PicoFrame.Icon = ((System.Drawing.Icon)(resources.GetObject("ni_PicoFrame.Icon")));
            this.ni_PicoFrame.Text = "Pico Frame";
            this.ni_PicoFrame.Visible = true;
            // 
            // cms_NotifyMenu
            // 
            this.cms_NotifyMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cms_NotifyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeDeskFrameToolStripMenuItem});
            this.cms_NotifyMenu.Name = "cms_NotifyMenu";
            this.cms_NotifyMenu.Size = new System.Drawing.Size(196, 28);
            // 
            // closeDeskFrameToolStripMenuItem
            // 
            this.closeDeskFrameToolStripMenuItem.Name = "closeDeskFrameToolStripMenuItem";
            this.closeDeskFrameToolStripMenuItem.Size = new System.Drawing.Size(195, 24);
            this.closeDeskFrameToolStripMenuItem.Text = "Close Desk Frame";
            this.closeDeskFrameToolStripMenuItem.Click += new System.EventHandler(this.closeDeskFrameToolStripMenuItem_Click);
            // 
            // form_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 58);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "form_Control";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "PicoFrame";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.form_Control_Load);
            this.cms_NotifyMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon ni_PicoFrame;
        private System.Windows.Forms.ContextMenuStrip cms_NotifyMenu;
        private System.Windows.Forms.ToolStripMenuItem closeDeskFrameToolStripMenuItem;
    }
}