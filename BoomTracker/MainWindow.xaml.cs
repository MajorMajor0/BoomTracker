using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;

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
			MWVM = new MainWindowViewModel();
			DataContext = MWVM;

			System.Windows.Media.Brush whiteBrush = new SolidColorBrush(Colors.White);
			System.Windows.Media.Brush blueBrush = new SolidColorBrush(Colors.Blue);
			System.Windows.Media.Brush blackBrush = new SolidColorBrush(Colors.Black);

			var ha = HorizontalAlignment.Stretch;
			var va = VerticalAlignment.Stretch;
			var margin = new Thickness(.5);
			var padding = new Thickness(0);
			double fontSize = 7;

			var converter1 = new Converters.CharToColorConverter();
			var converter2 = new Converters.BackGroundColorConverter();

			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 20; j++)
				{
					Label label = new Label
					{
						HorizontalAlignment = ha,
						VerticalAlignment = va,
						FontFamily = new System.Windows.Media.FontFamily("Tetris"),
						FontSize = fontSize,
						Margin = margin,
						Padding = padding,

					};

					var contentBinding = new Binding($"Game.CurrentGrid[{i}][{j}]");
					contentBinding.Source = DataContext;
					contentBinding.Mode = BindingMode.OneWay;
					label.SetBinding(ContentProperty, contentBinding);

					var backgroundBinding = new Binding($"Game.CurrentGrid[{i}][{j}]");
					backgroundBinding.Source = DataContext;
					backgroundBinding.Converter = new Converters.BackGroundColorConverter();
					backgroundBinding.Mode = BindingMode.OneWay;
					label.SetBinding(BackgroundProperty, backgroundBinding);

					var foregroundBinding = new MultiBinding();
					foregroundBinding.Mode = BindingMode.OneWay;
					foregroundBinding.Converter = new Converters.CharToColorConverter();
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
