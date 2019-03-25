/*This file is part of BoomTracker.
 * 
 * BoomTracker is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published 
 * version 3 of the License, or (at your option) any later version.
 * 
 * BoomTracker is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU 
 * General Public License for more details. 
 * 
 * You should have received a copy of the GNU General Public License
 *  along with BoomTracker.  If not, see<http://www.gnu.org/licenses/>.*/

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
