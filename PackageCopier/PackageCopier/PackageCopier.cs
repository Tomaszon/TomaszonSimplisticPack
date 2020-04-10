using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
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
			textBoxSourceLocation.DataBindings.Add(new Binding("Text", Model, nameof(Model.SourcePath), false, DataSourceUpdateMode.OnPropertyChanged));
			textBoxTargetLocation.DataBindings.Add(new Binding("Text", Model, nameof(Model.TargetPath), false, DataSourceUpdateMode.OnPropertyChanged));
			checkBoxDeletePreviousVersion.DataBindings.Add(new Binding("Checked", Model, nameof(Model.DeletePreviousVersion), false, DataSourceUpdateMode.OnPropertyChanged));
			checkBoxAutoCopy.DataBindings.Add(new Binding("Checked", Model, nameof(Model.AutoCopy), false, DataSourceUpdateMode.OnPropertyChanged));
			buttonCopy.DataBindings.Add(new Binding("Enabled", Model, nameof(Model.CanProceed), false, DataSourceUpdateMode.OnPropertyChanged));
			labelStatus.DataBindings.Add(new Binding("Text", Model, nameof(Model.Status), false, DataSourceUpdateMode.OnPropertyChanged));
			fileSystemWatcher1.Path = Model.SourcePath;
			fileSystemWatcher1.IncludeSubdirectories = true;
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

		private void OnRenamed(object sender, RenamedEventArgs e)
		{
			if (Model.AutoCopy && !File.GetAttributes(e.FullPath).HasFlag(FileAttributes.Directory))
			{
				string oldName = GetOutputName(e.OldFullPath);

				OnDeleted(sender, new FileSystemEventArgs(WatcherChangeTypes.Deleted, Path.GetDirectoryName(oldName), Path.GetFileName(oldName)));

				OnChanged(sender, new FileSystemEventArgs(WatcherChangeTypes.Created, Path.GetDirectoryName(e.FullPath), Path.GetFileName(e.FullPath)));
			}
		}

		private void OnDeleted(object sender, FileSystemEventArgs e)
		{
			if (Model.AutoCopy)
			{
				string outputName = GetOutputName(e.FullPath);

				File.Delete(outputName);
			}
		}

		private void OnChanged(object sender, FileSystemEventArgs e)
		{
			if (Model.AutoCopy && !File.GetAttributes(e.FullPath).HasFlag(FileAttributes.Directory))
			{
				string outputName = GetOutputName(e.FullPath);

				while (true)
				{
					try
					{
						using (FileStream input = new FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
						using (FileStream output = new FileStream(outputName, FileMode.OpenOrCreate, FileAccess.Write))
						{
							int bufferSize = 1024 * 1024;
							output.SetLength(input.Length);
							int bytesRead = -1;
							byte[] bytes = new byte[bufferSize];

							while ((bytesRead = input.Read(bytes, 0, bufferSize)) > 0)
							{
								output.Write(bytes, 0, bytesRead);
							}
						}

						break;
					}
					catch (IOException)
					{
						Thread.SpinWait(100);
					}
				}
			}
		}

		private string GetOutputName(string eventArgsFullPath)
		{
			return eventArgsFullPath.Replace(Model.SourcePath, Model.TargetPath);
		}
	}
}
