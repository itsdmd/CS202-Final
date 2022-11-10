using Contract;
using System.Windows;
using System.Windows.Controls;

namespace FinalProject
{
	public partial class EditRuleWindow : Window
	{
		public IRule ReturnData = null;
		bool validConfig = true;
		
		public EditRuleWindow(IRule passedInData)
		{
			InitializeComponent();

			ReturnData = passedInData;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			ruleNameTextBlock.Text = ReturnData.Name;
			ruleConfigTextBox.Text = ReturnData.Config;
		}

		private void okButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = validConfig;
		}

		private void ruleConfigTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				string newConfig = ReturnData.Name + " " + ruleConfigTextBox.Text;

				RuleFactory f = new RuleFactory();
				ReturnData = f.StringToIRuleConverter(newConfig);
			}
			catch
			{
				validConfig = false;
				MessageBox.Show("Invalid config!");
			}
		}
	}
}
