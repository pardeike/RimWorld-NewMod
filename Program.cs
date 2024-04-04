using System.Runtime.Versioning;

namespace NewMod
{
	// publish with
	// dotnet publish -c Release --self-contained true

	internal class Program
	{
		[SupportedOSPlatform("windows10.0.18362")]
		static void Main(string[] args)
		{
			if (args.Length < 2 || args.Length > 6)
			{
				Console.WriteLine("Usage: NewMod.exe <git-name> <mod-compact-name> [<mod-readable-name>] [<mod-author>] [<mod-owner>] [<description>]");
				return;
			}
			var gitName = args[0];
			var modCompactName = args[01].Replace(" ", "");
			var modReadableName = args.Length >= 3 ? args[2] : args[0];
			var modAuthor = args.Length >= 4 ? args[3] : "Anonymous";
			var modOwner = args.Length >= 5 ? args[4] : null;
			var description = args.Length == 6 ? args[5] : null;

			try
			{
				Console.WriteLine($"Creating new mod: {modReadableName}");
				if (Tools.CreateRoot(modCompactName))
					return;
				Console.WriteLine("Initializing git repository");
				Tools.RunGitCommand("init");
				Tools.CreateFiles(modCompactName, modReadableName, modAuthor, gitName, modOwner, description);
				Tools.RunGitCommand("add .");
				Tools.RunGitCommand("commit -m \"initial commit\"");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
		}
	}
}
