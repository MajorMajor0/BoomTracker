using System.Windows;
using System.Windows.Input;

namespace BoomTracker
{
	/// <summary>
	/// Interaction logic for NewPlayerDialog.xaml
	/// </summary>
	public partial class NewPlayerDialog : Window
	{
		public string PlayerName { get; set; } = "";

		public NewPlayerDialog()
		{
			EnterCommand = new Command(Enter, EnterCanExecute);
			InitializeComponent();
			DataContext = this;
			FocusManager.SetFocusedElement(this, NewPlayerNameBox);
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}

		public Command EnterCommand { get; private set; }

		private bool EnterCanExecute()
		{
			return PlayerName.Length > 0;
		}

		private void Enter()
		{
			DialogResult = true;
			Close();
		}
	}
}
