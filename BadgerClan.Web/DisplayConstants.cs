

public static class DisplayConstants
{
	public static readonly double Dimension = 8.0;
	public static readonly int MapWidth = 997;
	public static readonly int MapHeight = 860;
	public static readonly double Root3 = Math.Sqrt(3.0);
}

public static class PlayerHelpers
{
	private static Queue<string> playerColorsQueue = new Queue<string>([]);
	public static string GetRandomColor()
	{
		if (playerColorsQueue.Count < 1)
		{
			playerColorsQueue = new Queue<string>(new List<string>([
				"red", "blue", "green", "yellow", "orange", "purple", "cyan", "magenta", "lime", "teal",
				"black", "white", "gray", "pink", "navy", "gold", "maroon", "aqua", "olive", "indigo",
				"crimson", "turquoise", "beige", "lavender", "salmon", "violet", "chartreuse", "coral", "khaki", "plum",
				"orchid", "sienna", "azure", "ivory", "mintcream", "slategray", "goldenrod", "lightseagreen", "peachpuff", "darkorchid",
				"peru", "darkcyan", "firebrick", "rosybrown", "seagreen", "mediumvioletred", "deepskyblue", "darkolivegreen", "dodgerblue", "mediumaquamarine"
			]).OrderBy(i => Random.Shared.Next()));
		}
		return playerColorsQueue.Dequeue();
	}

	public static string GetRandomPlayerName()
	{
		return playerNamesQueue.Dequeue();
	}

	private static Queue<string> playerNamesQueue = new Queue<string>(new List<string>([
	"PixelPirate","ShadowSlayer",
	"DragonKnight","CyberViking",
	"StealthMaster","ArcaneWizard",
	"NightFalcon","FrostWarden",
	"IronGladiator","ThunderStorm",
	"DoomBringer","LunarMage",
	"DarkHunter","BlazeFury",
	"PhantomRogue",    "MysticSorcerer",
	"BlitzTitan",
	"QuantumReaper",    "SkylineNinja",
	"StormRider",
	"CrimsonDemon",    "InfernoDragon",
	"SilverFalcon",
	"TitanHunter",    "VortexWarrior",
	"SoulBender",
	"NeonPhantom",    "BlazeFang",
	"GalacticAce",
	"ZenithWarrior",
	"StarShade",
	"ArcaneNinja",
	"ChaosKnight",
	"FrostbiteKing",
	"VoidSlicer",
	"StarlightSorcerer",
	"CyberAssassin",
	"RogueHunter",
	"InfernoSoul",
	"EchoPhantom",
	"CelestialStriker",
	"SilentViper",
	"PhantomAssault",
	"ElectricKnight",
	"NightmareHunter",
	"SavageTitan",
	"NebulaWitch",
	"AbyssRanger",
	"BloodReaper",
	"CinderWarrior",
	"FuryDragon",
	"SilentMarauder",
	"SkyFuryMage",
	"VoidHunter",
	"StormWarden",
	"PhantomKnight",
	"EmberStriker",
	"VengefulShadow",
	"StormSoul",
	"WraithBane",
	"ShatteredTitan",
	"MysticFalcon",
	"ElectricPhantom",
	"ShadowRider",
	"FrozenSpecter",
	"GaleStorm",
	"IronSorcerer",
	"DarkWindKnight",
	"ThunderViper",
	"VenomousWitch",
	"CelestialFalcon",
	"SoulRider",
	"BlightKing",
	"MoonShadowKnight",
	"VortexAssassin",
	"CelestialRogue",
	"ArcaneTitan",
	"FrozenWarden",
	"DuskHunter",
	"InfernalViper",
	"PhantomSorcerer",
	"CobaltPhoenix",
	"FrostVengeance",
	"NebulaMage",
	"CrystalWitch",
	"ShadowStormer",
	"NightVortex",
	"VoidReaper", "TitanSlayer", "BlazingKnight", "NightfallMage", "DarkPhoenix",
	"AbyssWanderer", "DreadnoughtRanger", "SoulWitch", "InfernoKing", "GalacticShadow",
	"ArcaneRogue", "CrimsonWarden", "StormSeeker", "BlightKnight", "FrostBlade",
	"VortexMage", "PhantomHunter", "EchoKnight", "DemonSlayer", "SteelSorcerer",
	"ShadowWarden", "CelestialKnight", "ViperKing", "SilverHunter", "RadiantReaper",
	"DawnWarrior", "SavageSorcerer", "MoonlitKnight", "LunarAssassin", "ShadowRanger",
	"BloodMage", "FrostRider", "EclipseWarrior", "SpecterWitch", "ArcaneSlayer",
	"PhantomWarden", "InfernalMage", "StormKnight", "TitanSorcerer", "NebulaSlayer",
	"DemonWitch", "NightfallRogue", "VengefulKing", "StormReaper", "CelestialWarrior",
	"PhantomMage", "VoidAssassin", "VenomKnight", "BloodRider", "ShadowAssassin",
	"DarkWitch", "SpectralKnight", "FirestormWarrior", "CrystalMage", "ChaosReaper",
	"VortexWarlock", "SilentKing", "PhantomViper", "SoulVampire", "EternalWarrior",
	"BloodRevenant", "StarfallMage", "NightReaper", "IronWitch", "FuryKnight",
	"ThunderAssassin", "FrozenSorcerer", "DoomWarlock", "SolarKnight", "DarkPhoenixKing",
	"CelestialWarlock", "CrystalSorcerer", "EchoSorcerer", "LunarSlayer", "ShatteredWitch",
	"BloodViper", "TitanMage", "StormHunter", "GalacticWitch", "FrostWarlock",
	"SpecterSlayer", "NightRevenant", "EclipseSlayer", "VoidMage", "IronPhantom",
	"VengefulKnight", "SilverMage", "NightfallWarrior", "DarkReaper", "DawnMage",
	"CelestialSlayer", "PhantomWitch", "FrozenHunter", "ArcaneViper", "StormSorcerer",
	"CrimsonSorcerer", "BlazeKnight", "DuskWitch", "CinderHunter", "SoulKnight",
	"VoidSorcerer", "EchoMage", "GalacticAssassin", "ShadowWitch", "FrostWarrior",
	"IronViper", "ThunderMage", "StormVampire", "VortexKnight", "LunarViper",
	"CrimsonMage", "TitanWitch", "InfernalSorcerer", "DawnKnight", "VoidRider",
	"StormRevenant", "ArcaneHunter", "BloodKnight", "NebulaSorcerer", "CinderMage",
	"FrostAssassin", "DemonKnight", "FrozenVampire", "DarkMage", "WraithHunter",
	"ShadowTitan", "DemonHunter", "EclipseHunter", "RadiantKnight", "VortexSorcerer",
	"VoidKnight", "AbyssWarrior", "SoulWarrior", "BlazingWarlock", "DawnAssassin",
	"SilverKnight", "CelestialAssassin", "FurySorcerer", "VengefulWarrior", "SpectralMage",
	"IronHunter", "SolarSorcerer", "VoidWarrior", "FrozenWitch", "TitanWarlock",
	"NightmareMage", "ShadowSorcerer", "BloodSorcerer", "StormRogue", "DarkAssassin",
	"LunarWarrior", "SoulMage", "CrystalKnight", "VortexWarlock", "ShadowHunter",
	"VengefulMage", "MoonHunter", "TitanWitch", "NebulaHunter", "CrimsonAssassin",
	"InfernalKnight", "SolarMage", "CinderKnight", "BlazeAssassin", "VenomSorcerer",
	"DawnWarlock", "TitanViper", "EchoKnight", "AbyssSorcerer", "LunarHunter",
	"IronMage", "ShadowRevenant", "DawnSorcerer", "SpecterAssassin", "StormMage",
	"FrostVampire", "VenomKnight", "GalacticWarrior", "EclipseAssassin", "BlightMage",
	"SolarWarlock", "TitanRogue", "NebulaKnight", "BloodWarrior", "SpecterRogue",
	"NightfallHunter", "VortexVampire", "EchoWarlock", "FrozenMage", "PhantomKnight",
	"VenomWarrior", "DarkViper", "CelestialHunter", "FuryVampire", "SoulRogue",
	"TitanHunter", "WraithWarlock", "CrimsonKnight", "SoulSorcerer", "SpectralRogue",
	"BlazeWarrior", "FrozenAssassin", "GalacticKnight", "VortexMage", "MoonSorcerer",
	"ThunderVampire", "CelestialViper", "NightmareWarrior", "ShadowSlayer", "LunarMage",
	"InfernalWitch", "VenomousKnight", "StormWitch", "SpecterVampire", "BloodAssassin",
	"SolarKnight", "ArcaneWarrior", "FrostSlayer", "WraithMage", "InfernalVampire",
	"BlazingAssassin", "DemonMage", "VortexSorcerer", "NightfallWarlock", "IronWarrior",
	"FrostHunter", "FuryRogue", "TitanWarrior", "ShadowWitch", "VoidMage",
	"CelestialWitch", "SolarVampire", "DarkRogue", "FrostViper", "BlazeSorcerer",
	"FuryHunter", "StormSlayer", "VengefulAssassin", "LunarVampire", "SpectralMage",
	"NebulaWarrior", "BlazeRogue", "NightWarlock", "ShadowTitan", "DarkVampire",
	"PhantomWarrior", "VortexRogue", "SoulHunter", "DawnWarlock", "FrozenWarrior",
	"BlazeTitan", "StormMage", "CinderKnight", "ShadowWitch", "SpecterKnight"
]).OrderBy(i => Random.Shared.Next()));

}