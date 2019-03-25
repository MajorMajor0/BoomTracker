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

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace BoomTracker
{
	public static class Extensions
	{
		public static BitmapImage ToBitmapImage(this Bitmap bitmap)
		{
			BitmapImage bi = new BitmapImage();
			bi.BeginInit();
			MemoryStream ms = new MemoryStream();
			bitmap.Save(ms, ImageFormat.Bmp);
			ms.Seek(0, SeekOrigin.Begin);
			bi.StreamSource = ms;
			bi.EndInit();
			return bi;
		}
	}
}
