
namespace NewMod
{
	public class Replacements(params (string placeholder, string value)[] replacements)
	{
		readonly (string placeholder, string value)[] replacements = replacements;
		public string Handle(string input)
		{
			foreach (var (placeholder, value) in replacements)
				input = input.Replace("{" + placeholder + "}", value);
			return input;
		}
	}
}