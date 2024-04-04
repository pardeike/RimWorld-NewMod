using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;

namespace NewMod
{
	public static class Tools
	{
		public static bool CreateRoot(string dir)
		{
			if (Directory.Exists(dir))
			{
				var existingFiles = Directory.EnumerateFiles(dir).Count();
				if (existingFiles > 0)
				{
					Console.Write("Directory is not empty, should we continue and erase everything in it? ");
					if (Console.ReadLine()?.ToLower() != "yes")
						return true;
				}
				Directory.Delete(dir, true);
			}
			Directory.CreateDirectory(dir);
			Directory.SetCurrentDirectory(dir);
			return false;
		}

		public static void RunGitCommand(string cmd)
		{
			var processStartInfo = new ProcessStartInfo("git", cmd)
			{
				UseShellExecute = false,
				RedirectStandardOutput = true
			};
			var process = Process.Start(processStartInfo);
			process?.WaitForExit();
		}

		static string EmbeddedContent(string resourceName, Replacements replacements)
		{
			var assembly = typeof(Tools).Assembly;
			using Stream? stream = assembly.GetManifestResourceStream($"NewMod.Content.{resourceName}");
			if (stream == null)
				return string.Empty;
			using StreamReader reader = new StreamReader(stream);
			return replacements.Handle(reader.ReadToEnd());
		}

		static string Initials(this string text)
		{
			var initials = text.Split(' ').Select(s => s[0]);
			return new string(initials.ToArray());
		}

		[SupportedOSPlatform("windows10.0.18362")]
		public static void CreateFiles(string modCompactName, string modReadableName, string author, string gitname, string? modowner = null, string? description = null)
		{
			if (modowner != null)
				modowner += ".";
			else
				modowner = string.Empty;

			var replacements = new Replacements(
				("author", author),
				("year", $"{DateTime.Now.Year}"),
				("modId", $"{modowner}{modCompactName.ToLower()}"),
				("gitname", gitname),
				("description", description ?? ""),
				("solutionId", Guid.NewGuid().ToString()),
				("projectId", Guid.NewGuid().ToString()),
				("modCompactName", modCompactName),
				("modReadableName", modReadableName)
			);

			File.WriteAllText(".gitattributes", EmbeddedContent(".gitattributes", replacements));
			File.WriteAllText(".gitignore", EmbeddedContent(".gitignore", replacements));
			Directory.CreateDirectory("About");
			File.WriteAllText("About/About.xml", EmbeddedContent("About.xml", replacements));
			File.WriteAllText("About/Manifest.xml", EmbeddedContent("Manifest.xml", replacements));
			"About/Preview.png".GeneratePlaceholder(modReadableName.ToUpper(), 640, 358, 80);
			"About/ModIcon.png".GeneratePlaceholder(modReadableName.Initials(), 64, 64, 4);
			File.WriteAllText("Directory.Build.props", EmbeddedContent("Directory.Build.props", replacements));
			File.WriteAllText("LICENSE", EmbeddedContent("LICENSE", replacements));
			File.WriteAllText("Readme.md", EmbeddedContent("Readme.md", replacements));

			File.WriteAllText($"{modCompactName}.sln", EmbeddedContent("Solution.xml", replacements));
			Directory.CreateDirectory("Source");
			File.WriteAllText($"Source/{modCompactName}.csproj", EmbeddedContent("Project.xml", replacements));
			File.WriteAllText($"Source/Mod.cs", EmbeddedContent("Mod.cs", replacements));
			File.WriteAllText($"Source/Settings.cs", EmbeddedContent("Settings.cs", replacements));
		}

		[SupportedOSPlatform("windows10.0.18362")]
		public static void GeneratePlaceholder(this string imagePath, string text, int width, int height, int border)
		{
			var textColor = Color.Black;
			var backColor = Color.White;

			using var img = new Bitmap(1, 1);
			using var drawing = Graphics.FromImage(img);

			Font font;
			float textWidth, textHeight;
			var fontSize = 8f;
			while (true)
			{
				var newFontSize = fontSize + 1f;
				font = new Font("Arial", newFontSize, FontStyle.Bold, GraphicsUnit.Pixel);
				var textSize = drawing.MeasureString(text, font);
				(textWidth, textHeight) = (textSize.Width, textSize.Height - newFontSize / 5f);
				if (textWidth > width - border || textHeight > height)
					break;
				fontSize = newFontSize;
			}

			using var bitmap = new Bitmap(width, height);
			using Graphics graphics = Graphics.FromImage(bitmap);
			graphics.Clear(backColor);

			using var textBrush = new SolidBrush(textColor);
			graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			graphics.DrawString(text, font, textBrush, (width - textWidth) / 2, (height - textHeight) / 2);
			graphics.Save();
			bitmap.Save(imagePath, ImageFormat.Png);
		}
	}
}
