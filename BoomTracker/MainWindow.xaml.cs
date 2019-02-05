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
