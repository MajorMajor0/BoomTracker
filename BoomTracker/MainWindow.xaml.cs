using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace BoomTracker
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		private MainWindowViewModel MWVM;

		public MainWindow()
		{
			InitializeComponent();
			MWVM = (MainWindowViewModel)DataContext;

			CreatePlayingFieldGrid();

#if DEBUG
			MainWindowDebugStuff();
#endif
		}

		private void CreatePlayingFieldGrid()
		{
			var charToColorConverter = new Converters.CharToColorConverter();
			var backGroundColorConverter = new Converters.BackGroundColorConverter();

			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 20; j++)
				{
					Label label = new Label
					{
						HorizontalAlignment = HorizontalAlignment.Stretch,
						VerticalAlignment = VerticalAlignment.Stretch,
						FontFamily = new FontFamily("Tetris"),
						FontSize = 7,
						Margin = new Thickness(.5),
						Padding = new Thickness(0),
						SnapsToDevicePixels = true
					};

					var contentBinding = new Binding($"Game.CurrentGrid[{i}][{j}]")
					{
						Source = DataContext,
						Mode = BindingMode.OneWay
					};

					label.SetBinding(ContentProperty, contentBinding);

					var backgroundBinding = new Binding($"Game.CurrentGrid[{i}][{j}]")
					{
						Source = DataContext,
						Converter = backGroundColorConverter,
						Mode = BindingMode.OneWay
					};

					label.SetBinding(BackgroundProperty, backgroundBinding);

					var foregroundBinding = new MultiBinding
					{
						Mode = BindingMode.OneWay,
						Converter = charToColorConverter
					};

					foregroundBinding.Bindings.Add(new Binding($"Game.CurrentLevel"));
					foregroundBinding.Bindings.Add(new Binding($"Game.CurrentGrid[{i}][{j}]"));
					label.SetBinding(ForegroundProperty, foregroundBinding);

					Grid.SetColumn(label, i);
					Grid.SetRow(label, j);

					PlayingField.Children.Add(label);
				}
			}
		}

		private void MainWindow_Closing(object sender, CancelEventArgs e)
		{
			MWVM.Stop();
		}

		private void ImageButton_Click(object sender, RoutedEventArgs e)
		{
		}

		void Exit_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

#if DEBUG
		void MainWindowDebugStuff()
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
			}

			void PauseButton_Click(object sender, RoutedEventArgs e)
			{
			}

		}
#endif
		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				var e = new PropertyChangedEventArgs(propertyName);
				handler(this, e);
			}
		}
	}
}
