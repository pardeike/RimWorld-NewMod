using HarmonyLib;
using UnityEngine;
using Verse;

namespace {modCompactName}
{
	public class {modCompactName}Main : Mod
	{
		public static {modCompactName}Settings Settings;

		public {modCompactName}Main(ModContentPack content) : base(content)
		{
			Settings = GetSettings<{modCompactName}Settings>();

			var harmony = new Harmony("{modId}");
			harmony.PatchAll();
		}

		public override void DoSettingsWindowContents(Rect inRect) => Settings.DoWindowContents(inRect);
		public override string SettingsCategory() => "{modReadableName}";
	}
}