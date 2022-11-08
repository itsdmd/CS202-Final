using Contract;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.ComponentModel;

namespace FinalProject
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		public ObservableCollection<object> _originals = new ObservableCollection<object>();
		public ObservableCollection<object> _previews = new ObservableCollection<object>();
		RuleFactory f = new RuleFactory();

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			filesListView.ItemsSource = _originals;
			previewListView.ItemsSource = _previews;
		}

		private void selectRuleButton_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog();

			if (dialog.ShowDialog() == true)
			{
				var info = new FileInfo(dialog.FileName);

				// Display the rule file name to TextBox
				ruleFileName.Text = info.Name;

				f.RuleFilePath = info.FullName;

				var converter = FindResource("converter")
					as RawToRenamedConverter;
				converter!.Factory = f;

				if (_originals.Count > 0)
				{
					_previews.Clear();

					foreach (dynamic item in _originals)
					{
						_previews.Add(item);
					}
				}
			}
		}

		private void addFileButton_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog();
			
			if (dialog.ShowDialog() == true)
			{
				// Skip if already added
				if (_originals.Contains(dialog.FileName)) { return; }
				
				var info = new FileInfo(dialog.FileName);

				var rawItem = new
				{
					FullPath = dialog.FileName,
					FileName = info.Name
				};

				_originals.Add(rawItem);
				_previews.Add(rawItem);
			}
		}

		private void addFolderButton_Click(object sender, RoutedEventArgs e)
		{
			CommonOpenFileDialog dialog = new CommonOpenFileDialog();
			
			dialog.InitialDirectory = "C:";
			dialog.IsFolderPicker = true;
			
			if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
			{
				// Add all files in the folder
				var files = Directory.GetFiles(dialog.FileName);
				
				foreach (var file in files)
				{
					// Skip if already added
					if (_originals.Contains(dialog.FileName)) { return; }
					
					var info = new FileInfo(file);

					var rawItem = new
					{
						FullPath = file,
						FileName = info.Name
					};

					_originals.Add(rawItem);
					_previews.Add(rawItem);
				}
			}
		}

		private void renameButton_Click(object sender, RoutedEventArgs e)
		{
			foreach(dynamic item in _originals)
			{
				f.FileName = item.FileName;
				
				var info = new FileInfo(item.FullPath);
				var folder = info.Directory;
				
				// If moveToSubDirCheckBox is checked, create a subdirectory called "renamed" and copy the files to it
				if (moveToSubDirCheckBox.IsChecked == true)
				{
					try
					{
						var newPath = $"{folder}\\renamed\\{f.Parse()}";
						File.Copy(item.FullPath, newPath);
					}
					catch
					{
						folder = Directory.CreateDirectory(folder.FullName + "\\renamed");
						var newPath = $"{folder}\\{f.Parse()}";
						File.Copy(item.FullPath, newPath);
					}
				}
				else
				{
					var newPath = $"{folder}\\{f.Parse()}";
					File.Move(item.FullPath, newPath);
				}
			}
			
			MessageBox.Show($"Renamed {_originals.Count} files");
		}
	}
}