using System.IO;
using UnityEngine;

namespace Unmoddable;

public static class ModsAPI
{
    /// <summary>
    /// Directory for mods!
    /// </summary>
    /// <remarks>
    /// Mod directoryies should have unique identifiers:
    /// </remarks>
    public static string ModsDirectory = Path.Combine(Application.persistentDataPath, "UNMODDABLE", "MODS");
    /// <summary>
    /// Directory for common data.
    /// </summary>
    /// <remarks>
    /// Fair warning!
    /// As there is no standards for COMMON folder - please join UNBEATABLE modding community to agree on what you can use -^-
    /// If uncertain - provide a config file in your mod's directory instead.
    /// </remarks>
    public static string CommonContentDirectory = Path.Combine(Application.persistentDataPath, "UNMODDABLE", "COMMON");
}
