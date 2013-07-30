using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ClosedXML.Excel;
using ExCSS;
using ExCSS.Model;
using HtmlAgilityPack;

namespace Mo.HapSandbox
{
	public class EarlyResearch
	{
		private const string HtmlTable = "<table>"
			+ "<tr><td>Name</td><td>Country</td><td>Century</td></tr>"
			+ "<tr><td>Alexander</td><td>Greece</td><td>-1</td></tr>"
			+ "<tr><td>Bismarck</td><td>Germany</td><td>19</td></tr>"
			+ "<tr><td>Ghandi</td><td>India</td><td>20</td></tr>"
			+ "</table>";
		private const string HtmlThatContainsTable = "<div><table>"
			+ "<tr><td>Name</td><td>Country</td><td>Century</td></tr>"
			+ "<tr><td colspan='3'>Militaristic</td></tr>"
			+ "<tr><td>Alexander</td><td>Greece</td><td style='color:rgb(255, 0, 0)'>-1</td></tr>"
			+ "<tr><td>Bismarck</td><td>Germany</td><td>19</td></tr>"
			+ "<tr><td colspan='3'>Pacifistic</td></tr>"
			+ "<tr><td>Ghandi</td><td>India</td><td>20</td></tr>"
			+ "</table></div>";

		private const string JoeysTable =
			@"<table id='tempTable' style='border: 4px solid rgb(0, 0, 0); color: rgb(0, 0, 0); background-color: rgb(204, 255, 0);'> <tbody style='color: rgb(0, 0, 0); background-color: rgb(204, 0, 0); border: 0px none rgb(0, 0, 0);'><tr style='color: rgb(255, 0, 0); background-color: rgba(0, 0, 0, 0); font-style: normal; font-variant: normal; font-weight: normal; font-size: 16px; line-height: normal; font-family: Times;'> <td style='font-weight: bold; color: rgb(255, 0, 0); background-color: rgb(0, 204, 0); font-style: normal; font-variant: normal; font-size: 16px; line-height: normal; font-family: Times;'>name</td> <td style='color: rgb(255, 0, 0); background-color: rgb(0, 204, 0); font-style: normal; font-variant: normal; font-weight: normal; font-size: 16px; line-height: normal; font-family: Times;'>sex</td> <td style='color: rgb(255, 165, 0); background-color: rgb(0, 204, 0); font-style: normal; font-variant: normal; font-weight: normal; font-size: 16px; line-height: normal; font-family: Times;'>age</td> </tr> <tr style='color: rgb(0, 0, 0); background-color: rgba(0, 0, 0, 0); font-style: normal; font-variant: normal; font-weight: normal; font-size: 16px; line-height: normal; font-family: Times;'> <td colspan='2' style='color: rgb(0, 0, 0); background-color: rgb(0, 204, 0); font-style: normal; font-variant: normal; font-weight: normal; font-size: 16px; line-height: normal; font-family: Times;'>The Doctor</td> <td style='color: rgb(0, 0, 0); background-color: rgb(0, 204, 0); font-style: normal; font-variant: normal; font-weight: normal; font-size: 16px; line-height: normal; font-family: Times;'>1103</td> </tr> <tr style='height: 50px; color: rgb(0, 0, 0); background-color: rgba(0, 0, 0, 0); font-style: normal; font-variant: normal; font-weight: normal; font-size: 16px; line-height: normal; font-family: Times;'> <td style='color: rgb(0, 0, 0); background-color: rgb(0, 204, 0); font-style: normal; font-variant: normal; font-weight: normal; font-size: 16px; line-height: normal; font-family: Times;'>Amy Pond</td> <td style='color: rgb(0, 0, 0); background-color: rgb(0, 204, 0); font-style: normal; font-variant: normal; font-weight: normal; font-size: 16px; line-height: normal; font-family: Times;'>Female</td> <td style='color: rgb(0, 0, 0); background-color: rgb(0, 204, 0); font-style: normal; font-variant: normal; font-weight: normal; font-size: 16px; line-height: normal; font-family: Times;'>25</td> </tr> </tbody></table>";
		public static void Basic()
		{
			HtmlNode table = HtmlNode.CreateNode(HtmlTable);
			IEnumerable<IEnumerable<string>> parsedTable =
				table.SelectNodes("tr").Select(row => row.SelectNodes("th|td").Select(cell => cell.InnerText));
		}
		public static void RootNodeOrLower()
		{
			HtmlNode table = HtmlNode.CreateNode(HtmlTable);
			if (table.Name != "table")
			{
				table = table.SelectSingleNode("table");
			}
			IEnumerable<IEnumerable<string>> parsedTable =
				table.SelectNodes("tr").Select(row => row.SelectNodes("th|td").Select(cell => cell.InnerText));
		}
		public static void Query()
		{
			HtmlNode table = HtmlNode.CreateNode(HtmlTable);

			var query = from row in table.SelectNodes("tr")
									from cell in row.SelectNodes("th|td")
									select new { Table = table.Id, CellText = cell.InnerText };

			string fancypants = query.Aggregate("", (current, cell) => current + (cell.CellText + Environment.NewLine));
		}
		public static void Css()
		{
			var parser = new StylesheetParser();
			var stylesheet = parser.Parse(".someClass{color: red; background-image: url('/images/logo.png')");
			//var noo = stylesheet.RuleSets.First().Selectors.First().SimpleSelectors.First().
			//var imageUrl = stylesheet.RuleSets
			//									 .SelectMany(r => r.Declarations).First().
			//									 .SelectMany(d => d.Expression.Terms).First(t => t.Type == TermType.Url); // Finds the '/images/logo.png' image
		}
		public static void StepByStep()
		{
			var workbook = new XLWorkbook();
			var worksheet = workbook.AddWorksheet("Table1");
			HtmlNode table = HtmlNode.CreateNode(HtmlThatContainsTable).SelectSingleNode("table");
			var rows = table.SelectNodes("tr");
			for (int i = 0; i < rows.Count; i++)
			{
				var colOffset = 1;
				var rowId = i + 1;
				var cells = rows[i].SelectNodes("th|td");
				for (int j = 0; j < cells.Count; j++)
				{
					var cell = cells[j];
					var colId = colOffset;
					colOffset += worksheet.AddTd(rowId, colId, cell);
				}
			}
			workbook.SaveAs(MethodBase.GetCurrentMethod().Name + ".xlsx");
		}
		public static void Joeys()
		{
			var workbook = new XLWorkbook();
			var worksheet = workbook.AddWorksheet("Table1");
			HtmlNode table = HtmlNode.CreateNode(JoeysTable).SelectSingleNode("tbody");
			var rows = table.SelectNodes("tr");
			for (int i = 0; i < rows.Count; i++)
			{
				var colOffset = 1;
				var rowId = i + 1;
				var cells = rows[i].SelectNodes("th|td");
				for (int j = 0; j < cells.Count; j++)
				{
					var cell = cells[j];
					var colId = colOffset;
					colOffset += worksheet.AddTd(rowId, colId, cell);
				}
			}
			workbook.SaveAs(MethodBase.GetCurrentMethod().Name + ".xlsx");
		}
	}
}
