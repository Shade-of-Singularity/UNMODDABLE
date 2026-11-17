using System.IO;
using UnityEngine;

namespace Unmoddable;

/// <summary>
/// Stores common IO information.
/// </summary>
public static class ModsIO
{
    /// <summary>
    /// Directory for mods!
    /// </summary>
    /// <remarks>
    /// Mod directoryies should have unique identifiers:
    /// </remarks>
    public static string ModsDirectory = Path.Combine(Application.persistentDataPath, "UNMODDABLE", "MODS");
    /// <summary>
    /// Directory for common mod data, shared between mods.
    /// </summary>
    /// <remarks>
    /// Fair warning!
    /// As there is no standards for COMMON folder - please join UNBEATABLE modding community to agree on what you can use -^-
    /// If uncertain - provide a config file in your mod's directory instead.
    /// Link: <see href="https://discord.gg/dcellgames"/>
    /// </remarks>
    public static string CommonContentDirectory = Path.Combine(Application.persistentDataPath, "UNMODDABLE", "COMMON");
}
