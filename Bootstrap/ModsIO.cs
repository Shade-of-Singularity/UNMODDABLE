using System.IO;
using UnityEngine;

namespace Unmoddable;

/// <summary>
/// Stores common IO information.
/// </summary>
public static class ModsIO
{
    /// <summary>
    /// Directory where all possible modded content is stored.
    /// </summary>
    /// <remarks>
    /// Do not use directly unless you certain you need this!
    /// Use <see cref="ModsDirectory"/> and <see cref="CommonContentDirectory"/> for all your needs.
    /// </remarks>
    public static string UnmoddableDirectory = Path.Combine(Application.persistentDataPath, "UNMODDABLE");
    /// <summary>
    /// Directory for mods!
    /// </summary>
    /// <remarks>
    /// Mod directoryies should have unique identifiers:
    /// </remarks>
    public static string ModsDirectory = Path.Combine(UnmoddableDirectory, "MODS");
    /// <summary>
    /// Directory for common mod data, shared between mods.
    /// </summary>
    /// <remarks>
    /// Fair warning!
    /// As there is no standards for COMMON folder - please join UNBEATABLE modding community to agree on what you can use -^-
    /// If uncertain - provide a config file in your mod's directory instead.
    /// Link: <see href="https://discord.gg/dcellgames"/>
    /// </remarks>
    public static string CommonContentDirectory = Path.Combine(UnmoddableDirectory, "COMMON");
    /// <summary>
    /// Directory where mod's mainly read-only configurations are stored.
    /// (Exception - when exposed to user via a config screen)
    /// </summary>
    /// <remarks>
    /// Note: please don't put dates in your config files - it will be hard to sync them via GIT system!
    /// Use versioning instead.
    /// <para>Configuration system is WIP</para>
    /// </remarks>
    public static string ConfigurationsDirectory = Path.Combine(UnmoddableDirectory, "CONFIGS");
    /// <summary>
    /// Directory for mod's dynamic data, independent from profile system.
    /// This data is not syncronized via GIT.
    /// </summary>
    /// <remarks>
    /// Might be removed in the future, if mods will integrate with profiles themselves.
    /// </remarks>
    public static string DynamicDataDirectory = Path.Combine(UnmoddableDirectory, "DYNAMIC");
}
