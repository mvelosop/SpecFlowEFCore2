namespace Domion.Lib
{
	public static class Percent
	{
		public static string Display(decimal value, decimal reference, string format = "{0:n1}%")
		{
			decimal? percent = Value(value, reference);

			if (reference == 0)
				percent = null;

			return string.Format(format, percent);
		}

		public static decimal Value(decimal value, decimal reference)
		{
			if (reference == 0) return 0;

			return 100 * value / reference;
		}
	}
}
