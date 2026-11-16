using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Unmoddable;

public abstract class Mod
{
    /// <remarks>
    /// Format: {creator-name}.unbeatable.{your-project-name}
    /// Example: sog.unbeatable.unmoddable
    /// </remarks>
    public abstract string Identifier { get; }
    protected readonly Harmony Harmony;
    public Mod() => Harmony = new(Identifier);
    public virtual string Name => GetType().Name;
    public virtual string Description => "My first mod!";
    public virtual void Load()
    {
        var e = GetPatchTargetAssemblies();
        if (e is null)
        {
            var assembly = GetType().Assembly;
            Harmony.PatchAll(assembly);
            File.AppendAllText("boot.log", $"Patched: {Name} ({assembly.FullName})\r\n");
        }
        else
        {
            int counter = 0;
            while (e.MoveNext())
            {
                Harmony.PatchAll(e.Current);
                counter++;
            }
            File.AppendAllText("boot.log", $"Patched: {Name} (total assemblies: {counter})\r\n");
        }
    }

    public virtual void Unload()
    {
        Harmony.UnpatchAll();
        File.AppendAllText("boot.log", $"Unpached: {Name}\r\n");
    }

    protected virtual IEnumerator<Assembly>? GetPatchTargetAssemblies() => null;
}
