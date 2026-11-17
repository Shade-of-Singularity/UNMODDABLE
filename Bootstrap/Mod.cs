using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Unmoddable;

/// <summary>
/// Abstract base class for mods.
/// </summary>
/// <remarks>
/// Should not be inherited!
/// Inherit from <see cref="Mod{T}"/> instead!
/// </remarks>
/// Note: consider adding a way to specify Workshop page settings: tags, description, ect.
///  Potenitally via .toml file? Sounds very reasonable.
///  This can be done via an extensible type/string-based metadata dictionaty.
///  String will be more independent, but more prone to name clashes.
///  Same with types if modders don't use namespaces though...
///  Default guide should encourage namespace usage - same for default project.
/// TODO: Add advanced logging methods, such as TRACE, DEBUG, etc. To reduce Release mode logger strain.
/// TODO: Provide logging levels configurable by the user, to reduce disk strain on systems with HDD disk.
/// TODO: Add conditional attribute to avoid compiling those log methods in debug builds.
public abstract class Mod
{
    /// <summary>
    /// Unique mod identifier.
    /// </summary>
    /// <remarks>
    /// Format: {creator-name}.unbeatable.{your-project-name}
    /// Example: sog.unbeatable.unmoddable
    /// </remarks>
    public abstract string Identifier { get; }
    /// <summary>
    /// Dependencies on other mod's <see cref="Identifier"/>s.
    /// </summary>
    /// <remarks>
    /// This value has nothing to do with Assembly dependencies.
    /// Instead, <see cref="Dependencies"/> are used for workshop, online mod browsers and launchers.
    /// </remarks>
    public abstract string[] Dependencies { get; }
    /// <summary>
    /// Name of the mod.
    /// </summary>
    public virtual string Name { get; init; }
    /// <summary>
    /// Prefix in logs of the mod.
    /// </summary>
    /// <remarks>
    /// Usually 3 symbols long + 2 symbols for the brackets.
    /// Examples: [MOD], [BOT], [OUT]
    /// </remarks>
    public virtual string Prefix { get; init; }
    /// <summary>
    /// Mod description.
    /// </summary>
    public virtual string Description { get; init; }
    /// <summary>
    /// Whether this mod is always active.
    /// </summary>
    /// <remarks>
    /// Always active mods are not displayed in the mod-list, so this option is useful for utility mods.
    /// </remarks>
    public virtual bool AlwaysActive => false;
    /// <summary>
    /// Harmony instance wihch you can use for patching codebase.
    /// </summary>
    protected readonly Harmony Harmony;

    /// <summary>
    /// Default .ctor.
    /// </summary>
    public Mod()
    {
        Harmony = new(Identifier);

        // Name:
        string name = GetType().Name;
        Name = name;

        // Prefix:
        Span<char> chars = stackalloc char[5];
        int length = Math.Min(3, name.Length);
        var span = name.AsSpan(0, length);

        chars[0] = '[';
        span.CopyTo(chars[1..]);
        chars[length + 1] = ']';

        Prefix = new string(chars[..(length + 2)]);

        // Description:
        Description = "My first mod!";
    }

    /// <summary>
    /// Loads mod content.
    /// </summary>
    /// <remarks>
    /// Happens automatically on game start.
    /// Later we will allow user to load/unload mods dynamically.
    /// </remarks>
    public virtual void Load()
    {
        var e = GetPatchTargetAssemblies();
        if (e is null)
        {
            Out($"{Name} witheld from patching.");
        }
        else
        {
            int counter = 0;
            while (e.MoveNext())
            {
                Harmony.PatchAll(e.Current);
                counter++;
            }
            Out($"Patched: {Name} (total assemblies: {counter})");
        }
    }

    /// <summary>
    /// Unloads mod content.
    /// </summary>
    /// <remarks>
    /// Right now is never used.
    /// Later we will allow user to load/unload mods dynamically.
    /// </remarks>
    public virtual void Unload()
    {
        Harmony.UnpatchAll();
        Out($"Unpached: {Name}");
    }

    /// <summary>
    /// Enumerator over all the assemblies which has to be patched on <see cref="Load"/>.
    /// </summary>
    /// <returns>
    /// <see langword="null"/> if no assembly should be patched,
    /// and <see cref="IEnumerator{T}"/> with target <see cref="Assembly"/> items otherwise.
    /// </returns>
    protected virtual IEnumerator<Assembly>? GetPatchTargetAssemblies()
    {
        yield return GetType().Assembly;
    }

    /// <summary>
    /// Logs to Normal log.
    /// </summary>
    public void Out(object? obj, object? prefix = null)
    {
        if (prefix is not null) prefix = $"{Prefix} {prefix}";
        else prefix = Prefix;
        LoggingHelpers.Out(obj, prefix);
    }

    /// <summary>
    /// Logs to Error log.
    /// </summary>
    /// <remarks>
    /// For now logs to regular log, but it will be changed in the future.
    /// </remarks>
    public void Out(Exception? ex, object? prefix = null)
    {
        if (prefix is not null) prefix = $"{Prefix} {prefix}";
        else prefix = Prefix;
        LoggingHelpers.Out(ex, prefix);
    }

    /// <summary>
    /// Logs to Warning log.
    /// </summary>
    /// <remarks>
    /// For now logs to regular log, but it will be changed in the future.
    /// </remarks>
    public void OutW(object? obj, object? prefix = null)
    {
        if (prefix is not null) prefix = $"{Prefix} {prefix}";
        else prefix = Prefix;
        LoggingHelpers.Out(obj, prefix);
    }

    /// <summary>
    /// Logs to Error log.
    /// </summary>
    /// <remarks>
    /// For now logs to regular log, but it will be changed in the future.
    /// </remarks>
    public void OutE(object? obj, object? prefix = null)
    {
        if (prefix is not null) prefix = $"{Prefix} {prefix}";
        else prefix = Prefix;
        LoggingHelpers.Out(obj, prefix);
    }
}
