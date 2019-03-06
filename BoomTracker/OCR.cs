using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace BoomTracker
{
	public class OCR
	{
		private static TesseractEngine[] engines;

		static OCR()
		{
			engines = new TesseractEngine[3];

			for (int i = 0; i < engines.Length; i++)
			{
				engines[i] = new TesseractEngine(FileLocation.TesseractData, "eng");
				engines[i].SetVariable("tessedit_char_whitelist", "0123456789");
			}
		}

		public int ReadNumber(Bitmap bitmap, int engineNumber)
		{
			int returner = 0;

				using (var scorePage = engines[engineNumber].Process(bitmap, PageSegMode.SingleWord))
				{
					if (!int.TryParse(scorePage.GetText(), out returner))
					{
						returner = 0;
					}
				}

			return returner;
		}
	}
}
