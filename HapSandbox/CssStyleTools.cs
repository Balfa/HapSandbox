using System.Reflection;
using System.Web.UI;

namespace Mo.HapSandbox
{
	public class CssStyleTools
	{
		/// <summary>
		/// Creates an instance of the CssStyleCollection class by reflection.
		/// </summary>
		/// <returns>A new instance of the CssStyleCollection class.</returns>
		public static CssStyleCollection Create()
		{
			return (CssStyleCollection)typeof(CssStyleCollection).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0].Invoke(null);
		}

		/// <summary>
		/// Creates an instance of the CssStyleCollection class by reflection and stores view state in the provided state bag.
		/// </summary>
		/// <param name="state">The storage object for the state.</param>
		/// <returns>A new instance of the CssStyleCollection class.</returns>
		public static CssStyleCollection Create(StateBag state)
		{
			return (CssStyleCollection)typeof(CssStyleCollection).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[1].Invoke(new object[] { state });
		}
	}
}