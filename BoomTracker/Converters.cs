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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace BoomTracker
{

	public class BackGroundColorConverter : IValueConverter
	{
		private static readonly SolidColorBrush white = new SolidColorBrush(Colors.White);
		private static readonly SolidColorBrush black = new SolidColorBrush(Colors.Black);
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is null)
			{
				return black;
			}

			return white;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class DroughtColorConverter : IValueConverter
	{
		private static readonly SolidColorBrush white = new SolidColorBrush(Colors.White);
		private static readonly SolidColorBrush red = new SolidColorBrush(Colors.Red);

		public DroughtColorConverter()
		{
			//if (white.CanFreeze)
			//{
				white.Freeze();
			//}
			//if (red.CanFreeze)
			//{
				red.Freeze();
			//}
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is int && (int)value > 12)
			{
				return red;
			}
			return white;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class CharToColorConverter : IMultiValueConverter
	{
		private static readonly SolidColorBrush white = new SolidColorBrush(Colors.White);
		private static readonly SolidColorBrush black = new SolidColorBrush(Colors.Black);

		/// <summary>
		/// Dictionary to match a char value to a brush for display in the playing field
		/// </summary>
		public static List<Dictionary<char, SolidColorBrush>> BrushDictionary { get; private set; }

		public CharToColorConverter()
		{
			CreateBrushDictionaries();
			//if(white.CanFreeze)
			//{
				white.Freeze();
			//}
			//if (black.CanFreeze)
			//{
				black.Freeze();
			//}
		}

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values[1] is null || values[0] is null || values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
			{
				return black;
			}

			int level = (int)values[0];
			char character = (char)values[1];

			if (BrushDictionary[level].TryGetValue(character, out var brush))
			{
				return brush;
			}

			return black;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		private static void CreateBrushDictionaries()
		{
			BrushDictionary = new List<Dictionary<char, SolidColorBrush>>();

			// Create a dictionary for each level
			for (int level = 0; level < Palette.NLevels; level++)
			{
				var B = Palette.Mode.B[level].Select(x => (byte)x).ToArray();
				var C = Palette.Mode.C[level].Select(x => (byte)x).ToArray();

				var cBrush = new SolidColorBrush(Color.FromRgb(C[0], C[1], C[2]));
				var bBrush = new SolidColorBrush(Color.FromRgb(B[0], B[1], B[2]));

				bBrush.Freeze();
				cBrush.Freeze();

				var dict = new Dictionary<char, SolidColorBrush>
					{
						{ 'A', cBrush },
						{ 'B', bBrush },
						{ 'C', cBrush },
						{ 'D', cBrush }
					};

				BrushDictionary.Add(dict);
			}
		}

	}
}
