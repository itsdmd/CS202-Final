using Contract;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FinalProject
{
	public partial class MainWindow : Window
	{
		public ObservableCollection<object> _originals = new ObservableCollection<object>();
		public ObservableCollection<object> _previews = new ObservableCollection<object>();
		public ObservableCollection<object> _availableRules = new ObservableCollection<object>();
		public ObservableCollection<object> _selectedRules = new ObservableCollection<object>();

		RuleFactory f = new RuleFactory();
		dynamic selectedRuleFromAvailable = null;
		dynamic selectedRuleFromPreview = null;

		string anotherFolderFullPath = "";

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			filesListView.ItemsSource = _originals;
			previewListView.ItemsSource = _previews;

			// Load .dll file to show available rules in menu
			DLLReader dllReader = new DLLReader();

			foreach (var rule in dllReader.GetRuleDict())
			{
				var item = new
				{
					RuleName = rule.Value.Name,
					RuleConfig = rule.Value.Config
				};

				_availableRules.Add(item);
			}

			ruleSelectListView.ItemsSource = _availableRules;
			rulePreviewListView.ItemsSource = _selectedRules;
		}
		
		// Read rule file
		private List<string> RuleFileReader(string ruleFilePath)
		{
			try
			{
				return new List<string>(File.ReadAllLines(ruleFilePath));
			}
			catch (Exception e)
			{
				MessageBox.Show("Rule file not found", e.ToString());
				return null;
			}
		}
		
		// Update factory's RuleString with elements in _selectedRules
		private void UpdateFactory()
		{
			// Update converter
			var converter = FindResource("converter")
				as RawToRenamedConverter;
			converter!.Factory = f;

			List<string> newRuleAsStringList = new List<string>();
			foreach (dynamic item in _selectedRules)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append(item.RuleName);
				sb.Append(" ");
				sb.Append(item.RuleConfig);

				newRuleAsStringList.Add(sb.ToString());
			}

			f.RulesAsStringList = newRuleAsStringList;

			UpdateFilePreviewList();
		}
		
		// Update file preview list
		private void UpdateFilePreviewList()
		{
			if (_originals.Count > 0)
			{
				_previews.Clear();

				foreach (dynamic item in _originals)
				{
					_previews.Add(item);
				}
			}
		}

		private void selectRulePresetButton_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog();
			
			if (dialog.ShowDialog() == true)
			{
				var info = new FileInfo(dialog.FileName);

				// Setup factory
				var fileContent = RuleFileReader(dialog.FileName);
				f.RulesAsStringList = fileContent;

				// Display the rule file name to TextBox
				ruleFileName.Text = info.Name;

				//Update rule preview list
				_selectedRules.Clear();

				foreach (var r in f.RuleList)
				{
					var item = new
					{
						RuleName = r.Name,
						RuleConfig = r.Config
					};
					_selectedRules.Add(item);
				}
				rulePreviewListView.ItemsSource = _selectedRules;

				UpdateFilePreviewList();
			}
		}

		private void addFileButton_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog();

			if (dialog.ShowDialog() == true)
			{
				// Skip if already added
				bool duplicated = false;
				foreach (dynamic item in _originals)
				{
					if (item.FullPath == dialog.FileName) { duplicated = true; break; }
				}
				if (duplicated) return;

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

		private void removeFileButton_Click(object sender, RoutedEventArgs e)
		{
			if (filesListView.SelectedIndex != -1)
			{
				_originals.RemoveAt(filesListView.SelectedIndex);

				_previews.Clear();
				foreach (var item in _originals) { _previews.Add(item); }
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
					bool duplicated = false;
					foreach (dynamic item in _originals) { if (item.FullPath == file) { duplicated = true; break; } }
					if (duplicated) continue;

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

		private void ruleSelectListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			// Update ruleConfigTextBox's text
			selectedRuleFromAvailable = ruleSelectListView.SelectedItem;
			ruleConfigTextBox.Text = selectedRuleFromAvailable.RuleConfig;
		}

		private void addRuleButton_Click(object sender, RoutedEventArgs e)
		{
			if (ruleSelectListView.SelectedItem == null) { return; }
			
			_selectedRules.Add( new { RuleName = selectedRuleFromAvailable.RuleName,
									  RuleConfig = ruleConfigTextBox.Text } );
			
			UpdateFactory();
		}

		private void ruleConfigTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			return;
		}

		private void rulePreviewListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			return;
		}

		private void moveRuleUpButton_Click(object sender, RoutedEventArgs e)
		{
			if (rulePreviewListView.SelectedItem == null) { return; }

			int index = rulePreviewListView.SelectedIndex;
			if (index == 0) { return; }
			else
			{
				var temp = _selectedRules[index - 1];
				_selectedRules[index - 1] = _selectedRules[index];
				_selectedRules[index] = temp;

				UpdateFactory();
				rulePreviewListView.ItemsSource = _selectedRules;
			}
		}

		private void moveRuleDownButton_Click(object sender, RoutedEventArgs e)
		{
			if (rulePreviewListView.SelectedItem == null) { return; }

			int index = rulePreviewListView.SelectedIndex;
			if (index == _selectedRules.Count - 1) { return; }
			else
			{
				var temp = _selectedRules[index + 1];
				_selectedRules[index + 1] = _selectedRules[index];
				_selectedRules[index] = temp;

				UpdateFactory();
				rulePreviewListView.ItemsSource = _selectedRules;
			}
		}

		private void editRuleButton_Click(object sender, RoutedEventArgs e)
		{
			return;
		}

		private void deleteRuleButton_Click(object sender, RoutedEventArgs e)
		{
			if (rulePreviewListView.SelectedItem == null) { return; }
			
			int index = rulePreviewListView.SelectedIndex;
			_selectedRules.RemoveAt(index);
			rulePreviewListView.ItemsSource = _selectedRules;
			
			UpdateFactory();
		}

		private void selectDestinationFolderButton_Click(object sender, RoutedEventArgs e)
		{
			CommonOpenFileDialog dialog = new CommonOpenFileDialog();

			dialog.InitialDirectory = "C:";
			dialog.IsFolderPicker = true;

			if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
			{
				anotherFolderFullPath = dialog.FileName;
				anotherFolderPathTextBlock.Text = anotherFolderFullPath;
			}
		}
		
		private void anotherFolderPathTextBlock_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			anotherFolderFullPath = anotherFolderPathTextBlock.Text;
		}
		
		private void renameButton_Click(object sender, RoutedEventArgs e)
		{
			foreach (dynamic item in _originals)
			{
				f.FileName = item.FileName;

				var info = new FileInfo(item.FullPath);
				var folder = info.Directory;

				// If moveToAnotherFolderCheckBox is checked, create a subdirectory called "renamed" and copy the files to it
				if (moveToAnotherFolderCheckBox.IsChecked == true)
				{
					if (anotherFolderFullPath == "")
					{
						MessageBox.Show("Please select a folder to move the renamed files to.");
						return;
					}
					try
					{
						var newPath = $"{anotherFolderFullPath}\\{f.Parse()}";
						File.Copy(item.FullPath, newPath);
					}
					catch
					{
						folder = Directory.CreateDirectory(anotherFolderFullPath);
						
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

		private void selectSubFolderButton_Click(object sender, RoutedEventArgs e)
		{
			return;
		}

		private void ruleConfigTextBoxTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{

		}
	}
}