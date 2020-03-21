using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace PackageCopier
{
	public class Data : INotifyPropertyChanged
	{
		private string _targetPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\.minecraft\resourcepacks\TomaszonSimplisticPack";
		public string TargetPath
		{
			get { return _targetPath; }
			set
			{
				_targetPath = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(CanProceed));
			}
		}

		private string _sourcePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\GitHub\TomaszonSimplisticPack\Tomaszon Simplistic Pack";
		public string SourcePath
		{
			get { return _sourcePath; }
			set
			{
				_sourcePath = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(CanProceed));
			}
		}

		private bool _deletePreviousVersion = true;
		public bool DeletePreviousVersion
		{
			get { return _deletePreviousVersion; }
			set
			{
				_deletePreviousVersion = value;
				OnPropertyChanged();
			}
		}

		private bool _autoCopy = true;
		public bool AutoCopy
		{
			get { return _autoCopy; }
			set
			{
				_autoCopy = value;
				OnPropertyChanged();
			}
		}

		private Status _status;

		public Status Status
		{
			get { return _status; }
			set
			{
				_status = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(CanProceed));
			}
		}

		public bool CanProceed
		{
			get { return Directory.Exists(Directory.GetParent(TargetPath).FullName) && Directory.Exists(SourcePath) && Status != Status.InProgress; }
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public enum Status
	{
		StandingBy,
		Done,
		Error,
		InProgress
	}
}
