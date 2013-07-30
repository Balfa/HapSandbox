using CsQuery;

namespace Mo.HapSandbox
{

	/// <summary>
	/// CsQuery looks more promising than HAP! Let's use it instead!
	/// </summary>
	public class CsQueryFuns
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
		public static void Funs()
		{
			CQ dom = HtmlThatContainsTable;
			CQ colspanned = dom["td[colspan=3]"];
			IDomObject newCell = colspanned.FirstElement().Clone();
			newCell.RemoveAttribute("colspan");
			newCell.InnerText = "Chaotic";

			colspanned.AttrSet("{\"colspan\" : 2}");
			colspanned.After(newCell);
			var foo = colspanned.Render();
		}
	}
}
