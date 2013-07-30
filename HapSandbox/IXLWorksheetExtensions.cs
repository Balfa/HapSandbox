using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using ClosedXML.Excel;
using ExCSS;
using ExCSS.Model;
using HtmlAgilityPack;
using System.Drawing;

namespace Mo.HapSandbox
{
	public static class IXLWorksheetExtensions
	{
		public static int AddTd(this IXLWorksheet worksheet, int rowId, int colId, HtmlNode cell)
		{
			// add text
			worksheet.Cell(rowId, colId).Value = cell.InnerText;

			// add data type
			worksheet.Cell(rowId, colId).DataType = GetDataType(cell.InnerText);

			// colspan merge
			int colspan = cell.GetAttributeValue("colspan", 1);
			if (colspan > 1)
			{
				var mergeUs = worksheet.Range(rowId, colId, rowId, colId + (colspan - 1));
				mergeUs.Merge();
			}

			// Try style with CssStyleCollection
			var stylernater = CssStyleTools.Create();
			stylernater.Value = cell.GetAttributeValue("style", "");
			string color = stylernater["color"];
			if (!string.IsNullOrWhiteSpace(color))
			{
				// rgb(
				//var snot = "rgb(255, 0, 0)";
				var snot = color;
				var poo = snot.Substring(4, snot.Length-4);
				var foo = poo.Substring(0, poo.Length - 1);
				string[] noo = foo.Split(',');
				var goo = noo.Select(x => x.Trim());
				List<int> hoo = goo.Select(x => int.Parse(x)).ToList();
				worksheet.Cell(rowId, colId).Style.Font.FontColor = XLColor.FromArgb(hoo[0], hoo[1], hoo[2]); //.FromName(color);
			}
			// YAAHGHGhghghgh. This is just as bad as ExCss!

			// add style?
			//worksheet.AddExCssStyle(rowId, colId, cell);

			return colspan;
		}
		private static void AddExCssStyle(this IXLWorksheet worksheet, int rowId, int colId, HtmlNode cell)
		{
			var css = cell.GetAttributeValue("style", "");
			var parser = new StylesheetParser();
			var stylesheet = parser.Parse(css);
			SimpleSelector color =
				stylesheet.RuleSets.SelectMany(x => x.Selectors)
				          .SelectMany(x => x.SimpleSelectors)
				          .FirstOrDefault(x => x.ElementName == "color");
			string thing = (color == null) ? "" : color.Child.Pseudo;
			worksheet.Cell(rowId, colId).Style.Font.FontColor = XLColor.FromName(thing);
		}
		private static XLCellValues GetDataType(string innerText)
		{
			int testValue;
			if (int.TryParse(innerText, out testValue))
			{
				return XLCellValues.Number;
			}
			return XLCellValues.Text;
		}
	}
}
