/*This file is part of BoomTracker.
 * 
 * Robin is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published 
 * version 3 of the License, or (at your option) any later version.
 * 
 * Robin is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU 
 * General Public License for more details. 
 * 
 * You should have received a copy of the GNU General Public License
 *  along with Robin.  If not, see<http://www.gnu.org/licenses/>.*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace BoomTracker
{
	internal class Converters
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

		public class CharToColorConverter : IMultiValueConverter
		{
			private static readonly Brush blackBrush = new SolidColorBrush(Colors.Black);

			/// <summary>
			/// Dictionary to match a char value to a brush for display in the playing field
			/// </summary>
			public static List<Dictionary<char, SolidColorBrush>> BrushDictionary { get; private set; }

			public CharToColorConverter()
			{
				CreateBrushDictionaries();
			}

			public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
			{
				if (values[1] is null || values[0] is null)
				{
					return blackBrush;
				}

				int level = (int)values[0];
				char character = (char)values[1];

				if (character == 'B')
				{

				}

				if (BrushDictionary[level].TryGetValue(character, out var brush))
				{
					return brush;
				}

				return blackBrush;
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

					var dict = new Dictionary<char, SolidColorBrush>
					{
						{ 'A', new SolidColorBrush(Color.FromRgb(C[0], C[1], C[2])) },
						{ 'B', new SolidColorBrush(Color.FromRgb(B[0], B[1], B[2])) },
						{ 'C', new SolidColorBrush(Color.FromRgb(C[0], C[1], C[2])) }
					};

					BrushDictionary.Add(dict);
				}
			}
		}
	}
}
