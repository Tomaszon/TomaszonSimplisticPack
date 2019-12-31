using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PackageCopier
{
	public partial class PackageCopier : Form
	{
		public Data Model { get; set; } = new Data();

		public PackageCopier()
		{
			InitializeComponent();

			DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
			textBoxSourceLocation.DataBindings.Add(new Binding("Text", Model, nameof(Model.SourcePath)));
			textBoxTargetLocation.DataBindings.Add(new Binding("Text", Model, nameof(Model.TargetPath)));
			checkBoxDeletePreviousVersion.DataBindings.Add(new Binding("Checked", Model, nameof(Model.DeletePreviousVersion)));
			buttonCopy.DataBindings.Add(new Binding("Enabled", Model, nameof(Model.CanProceed)));
			labelStatus.DataBindings.Add(new Binding("Text", Model, nameof(Model.Status)));
		}

		private void ButtonCopy_Click(object sender, EventArgs e)
		{
			Copy().Start();
		}

		public Task Copy()
		{
			return new Task(() =>
			{
				try
				{
					UpdateUI(Status.InProgress);

					if (Model.DeletePreviousVersion && Directory.Exists(Model.TargetPath))
					{
						Directory.Delete(Model.TargetPath, true);
					}

					foreach (string dirPath in Directory.GetDirectories(Model.SourcePath, "*", SearchOption.AllDirectories))
					{
						Directory.CreateDirectory(dirPath.Replace(Model.SourcePath, Model.TargetPath));
					}

					foreach (string newPath in Directory.GetFiles(Model.SourcePath, "*.*", SearchOption.AllDirectories))
					{
						File.Copy(newPath, newPath.Replace(Model.SourcePath, Model.TargetPath), true);
					}

					UpdateUI(Status.Done);
				}
				catch (Exception ex)
				{
					UpdateUI(Status.Error);

					MessageBox.Show(ex.Message + "\n\n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			});
		}

		public void UpdateUI(Status status)
		{
			if (labelStatus.InvokeRequired)
			{
				labelStatus.BeginInvoke(new Action<Status>(UpdateUI), status);
			}
			else
			{
				labelStatus.Focus();
				Model.Status = status;
			}

			if (status != Status.InProgress)
			{
				if (buttonCopy.InvokeRequired)
				{
					buttonCopy.BeginInvoke(new Action<Status>(UpdateUI), status);
				}
				else
				{
					buttonCopy.Focus();
				}
			}
		}

		private void LinkLabelSource_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				Process.Start(Model.SourcePath);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n\n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void LinkLabelTartget_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				Process.Start(Model.TargetPath);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n\n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
