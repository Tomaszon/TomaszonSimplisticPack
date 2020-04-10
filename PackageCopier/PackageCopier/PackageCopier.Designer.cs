using System;
using System.IO;

namespace PackageCopier
{
	partial class PackageCopier
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
			this.textBoxSourceLocation = new System.Windows.Forms.TextBox();
			this.textBoxTargetLocation = new System.Windows.Forms.TextBox();
			this.buttonCopy = new System.Windows.Forms.Button();
			this.checkBoxDeletePreviousVersion = new System.Windows.Forms.CheckBox();
			this.linkLabelSource = new System.Windows.Forms.LinkLabel();
			this.linkLabelTartget = new System.Windows.Forms.LinkLabel();
			this.labelStatus = new System.Windows.Forms.Label();
			this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
			this.checkBoxAutoCopy = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
			this.SuspendLayout();
			// 
			// textBoxSourceLocation
			// 
			this.textBoxSourceLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxSourceLocation.Location = new System.Drawing.Point(67, 12);
			this.textBoxSourceLocation.Multiline = true;
			this.textBoxSourceLocation.Name = "textBoxSourceLocation";
			this.textBoxSourceLocation.Size = new System.Drawing.Size(205, 50);
			this.textBoxSourceLocation.TabIndex = 2;
			this.textBoxSourceLocation.TabStop = false;
			// 
			// textBoxTargetLocation
			// 
			this.textBoxTargetLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxTargetLocation.Location = new System.Drawing.Point(67, 68);
			this.textBoxTargetLocation.Multiline = true;
			this.textBoxTargetLocation.Name = "textBoxTargetLocation";
			this.textBoxTargetLocation.Size = new System.Drawing.Size(205, 50);
			this.textBoxTargetLocation.TabIndex = 1;
			this.textBoxTargetLocation.TabStop = false;
			// 
			// buttonCopy
			// 
			this.buttonCopy.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.buttonCopy.Location = new System.Drawing.Point(79, 146);
			this.buttonCopy.Name = "buttonCopy";
			this.buttonCopy.Size = new System.Drawing.Size(126, 23);
			this.buttonCopy.TabIndex = 0;
			this.buttonCopy.Text = "Copy";
			this.buttonCopy.UseVisualStyleBackColor = true;
			this.buttonCopy.Click += new System.EventHandler(this.ButtonCopy_Click);
			// 
			// checkBoxDeletePreviousVersion
			// 
			this.checkBoxDeletePreviousVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkBoxDeletePreviousVersion.AutoSize = true;
			this.checkBoxDeletePreviousVersion.Location = new System.Drawing.Point(12, 123);
			this.checkBoxDeletePreviousVersion.Name = "checkBoxDeletePreviousVersion";
			this.checkBoxDeletePreviousVersion.Size = new System.Drawing.Size(137, 17);
			this.checkBoxDeletePreviousVersion.TabIndex = 3;
			this.checkBoxDeletePreviousVersion.TabStop = false;
			this.checkBoxDeletePreviousVersion.Text = "Delete previous version";
			this.checkBoxDeletePreviousVersion.UseVisualStyleBackColor = true;
			// 
			// linkLabelSource
			// 
			this.linkLabelSource.LinkColor = System.Drawing.Color.Black;
			this.linkLabelSource.Location = new System.Drawing.Point(12, 12);
			this.linkLabelSource.Name = "linkLabelSource";
			this.linkLabelSource.Size = new System.Drawing.Size(49, 20);
			this.linkLabelSource.TabIndex = 4;
			this.linkLabelSource.TabStop = true;
			this.linkLabelSource.Text = "Source";
			this.linkLabelSource.VisitedLinkColor = System.Drawing.Color.Black;
			this.linkLabelSource.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelSource_LinkClicked);
			// 
			// linkLabelTartget
			// 
			this.linkLabelTartget.LinkColor = System.Drawing.Color.Black;
			this.linkLabelTartget.Location = new System.Drawing.Point(12, 68);
			this.linkLabelTartget.Name = "linkLabelTartget";
			this.linkLabelTartget.Size = new System.Drawing.Size(49, 20);
			this.linkLabelTartget.TabIndex = 5;
			this.linkLabelTartget.TabStop = true;
			this.linkLabelTartget.Text = "Target";
			this.linkLabelTartget.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelTartget_LinkClicked);
			// 
			// labelStatus
			// 
			this.labelStatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.labelStatus.AutoSize = true;
			this.labelStatus.Location = new System.Drawing.Point(211, 151);
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.Size = new System.Drawing.Size(35, 13);
			this.labelStatus.TabIndex = 8;
			this.labelStatus.Text = "status";
			// 
			// fileSystemWatcher1
			// 
			this.fileSystemWatcher1.EnableRaisingEvents = true;
			this.fileSystemWatcher1.SynchronizingObject = this;
			this.fileSystemWatcher1.Changed += new System.IO.FileSystemEventHandler(this.OnChanged);
			this.fileSystemWatcher1.Deleted += new System.IO.FileSystemEventHandler(this.OnDeleted);
			this.fileSystemWatcher1.Renamed += new System.IO.RenamedEventHandler(this.OnRenamed);
			// 
			// checkBoxAutoCopy
			// 
			this.checkBoxAutoCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkBoxAutoCopy.AutoSize = true;
			this.checkBoxAutoCopy.Location = new System.Drawing.Point(163, 124);
			this.checkBoxAutoCopy.Name = "checkBoxAutoCopy";
			this.checkBoxAutoCopy.Size = new System.Drawing.Size(109, 17);
			this.checkBoxAutoCopy.TabIndex = 9;
			this.checkBoxAutoCopy.Text = "Enable auto copy";
			this.checkBoxAutoCopy.UseVisualStyleBackColor = true;
			// 
			// PackageCopier
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 181);
			this.Controls.Add(this.checkBoxAutoCopy);
			this.Controls.Add(this.labelStatus);
			this.Controls.Add(this.linkLabelTartget);
			this.Controls.Add(this.linkLabelSource);
			this.Controls.Add(this.checkBoxDeletePreviousVersion);
			this.Controls.Add(this.buttonCopy);
			this.Controls.Add(this.textBoxTargetLocation);
			this.Controls.Add(this.textBoxSourceLocation);
			this.MaximumSize = new System.Drawing.Size(1920, 220);
			this.MinimumSize = new System.Drawing.Size(300, 220);
			this.Name = "PackageCopier";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "PackageCopier";
			((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBoxSourceLocation;
		private System.Windows.Forms.TextBox textBoxTargetLocation;
		private System.Windows.Forms.Button buttonCopy;
		private System.Windows.Forms.CheckBox checkBoxDeletePreviousVersion;
		private System.Windows.Forms.LinkLabel linkLabelSource;
		private System.Windows.Forms.LinkLabel linkLabelTartget;
		private System.Windows.Forms.Label labelStatus;
		private System.IO.FileSystemWatcher fileSystemWatcher1;
		private System.Windows.Forms.CheckBox checkBoxAutoCopy;
	}
}

