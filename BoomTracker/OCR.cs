using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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

			//SetScoreAddresses();
		}

		public int[] ReadNumber(Bitmap bitmap, int engineNumber)
		{
			int[] returner = new int[2];

			using (var page = engines[engineNumber].Process(bitmap, PageSegMode.SingleWord))
			{
				returner[1] = (int)(page.GetMeanConfidence() * 1000);

				if (!int.TryParse(page.GetText(), out returner[0]))
				{
					returner[0] = 0;
				}
			}

			return returner;
		}	
	}
}
