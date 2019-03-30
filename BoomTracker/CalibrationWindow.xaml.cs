using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BoomTracker
{
	/// <summary>
	/// Interaction logic for CalibrationWindow.xaml
	/// </summary>
	public partial class CalibrationWindow : Window
	{

		private CalibrationWindowViewModel CWVM;
		public CalibrationWindow()
		{
			InitializeComponent();
			CWVM = DataContext as CalibrationWindowViewModel;
#if DEBUG
			CalibrationWindowDebugStuff();
#endif
		}

#if DEBUG
		void CalibrationWindowDebugStuff()
		{
			PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Critical;

			Brush debugBrush = new SolidColorBrush(Colors.Blue);

			Button bonusButton = new Button
			{
				Content = "BONUS!",
				Width = 70,
				Height = 20,
				Margin = new Thickness(5),
				Foreground = debugBrush
			};

			Button pauseButton = new Button
			{
				Content = "Pause",
				Width = 70,
				Height = 20,
				Margin = new Thickness(5),
				Foreground = debugBrush
			};

			bonusButton.Click += BonusButton_Click;
			pauseButton.Click += PauseButton_Click;
			ButtonsStackpanel.Children.Add(bonusButton);
			ButtonsStackpanel.Children.Add(pauseButton);

			void BonusButton_Click(object sender, RoutedEventArgs e)
			{
				DebugStuff.MainWindowBonusAsync();
				DigitsListBox.ItemsSource = DebugStuff.CalibrateOcr();
			}

			void PauseButton_Click(object sender, RoutedEventArgs e)
			{
			}

		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			CWVM.Stop();
		}
#endif

	}
}
