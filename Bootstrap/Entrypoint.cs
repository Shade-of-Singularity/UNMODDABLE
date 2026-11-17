using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unmoddable;

namespace Doorstop; // Do NOT rename! Doorstop entry-point identifier!

class Entrypoint // Do NOT rename! Doorstop entry-point identifier!
{
    private const string AwaitLibrariesFileName = "AwaitLibraries.txt";
    private const string AwaitLibrariesFilePath = "BOOTSTRAP/" + AwaitLibrariesFileName;
    static HashSet<string>? AwaitedLibraries;
    static readonly List<Mod> Mods = [];

    public static void Start()
    {
        if (!File.Exists(AwaitLibrariesFilePath))
            throw new FileNotFoundException($"BOOTSTRAP should have '{AwaitLibrariesFileName}' file in the same UNBEATALBE/BOOTSTRAP folder!");

        // # - is a commented-out line.
        AwaitedLibraries = [.. File.ReadAllLines(AwaitLibrariesFilePath).Where(
            static s => !string.IsNullOrWhiteSpace(s) && !s.StartsWith('#'))];
        foreach (var loaded in AppDomain.CurrentDomain.GetAssemblies())
            AwaitedLibraries.Remove(loaded.FullName);

        LoggingHelpers.Clear();
        BootstrapMod.Initialize(); // Makes sure mod is initialized for logging purposes.

        // Waits until all game's DLLs are loaded:
        AppDomain.CurrentDomain.AssemblyLoad += HandleLoad;
    }

    private static void HandleLoad(object sender, AssemblyLoadEventArgs args)
    {
        if (AwaitedLibraries is null) return;
        if (AwaitedLibraries.Remove(args.LoadedAssembly.FullName) && AwaitedLibraries.Count == 0)
        {
            AwaitedLibraries = null;
            AppDomain.CurrentDomain.AssemblyLoad -= HandleLoad;
            Stopwatch stopwatch = Stopwatch.StartNew();
            BootstrapMod.Out($"Loading mods...");
            Initialize(out int totalMods);
            var total = stopwatch.Elapsed;
            if (totalMods == 0)
                BootstrapMod.Out($"No mods found! Place mods at: {ModsIO.ModsDirectory} ({total:c})");
            else
                BootstrapMod.Out($"Mods loaded! (total: {totalMods}) (time: {total:c})");
            return;
        }
    }

    public static void Initialize(out int totalMods)
    {
        Directory.CreateDirectory(ModsIO.UnmoddableDirectory);
        Directory.CreateDirectory(ModsIO.ModsDirectory);
        Directory.CreateDirectory(ModsIO.CommonContentDirectory);
        Directory.CreateDirectory(ModsIO.ConfigurationsDirectory);
        Directory.CreateDirectory(ModsIO.DynamicDataDirectory);
        totalMods = 0;

        //Debug.developerConsoleEnabled = true;
        //Debug.developerConsoleVisible = true;
        DependencyMap<string, Assembly> map = new(
            static a => a.GetReferencedAssemblies().Select(static a => a.FullName).ToHashSet());

        var native = AppDomain.CurrentDomain.GetAssemblies().Select(static a => a.GetName().Name).ToHashSet();
        foreach (var file in Directory.GetFiles(ModsIO.ModsDirectory, "*.dll", SearchOption.AllDirectories))
        {
            try
            {
                string extn = Path.GetFileNameWithoutExtension(file);
                if (native.Contains(extn))
                {
                    BootstrapMod.Out($"Skipping native assembly: {extn}");
                    continue;
                }

                Assembly assembly = Assembly.LoadFrom(file);
                string name = assembly.FullName;
                map.TryAdd(name, assembly);
                BootstrapMod.Out($"Located assembly: {name}");
            }
            catch (Exception ex)
            {
                BootstrapMod.Out(ex, $"Failed to load DLL! {Path.GetFileNameWithoutExtension(file)}");
            }
        }

        Harmony.DEBUG = true;
        if (map.TryResolve(out var assemblies))
        {
            foreach (var assembly in map)
            {
                try
                {
                    BootstrapMod.Out($"Analyzing '{assembly.GetName().Name}'...");
                    foreach (var type in assembly.GetTypes().Where(
                        static t => !t.IsAbstract && !t.ContainsGenericParameters && typeof(Mod).IsAssignableFrom(t)))
                    {
                        BootstrapMod.Out($"- Mod type found: {type.FullName}");
                        Mods.Add((Mod)Activator.CreateInstance(type));
                    }

                    foreach (var module in assembly.GetModules())
                        RuntimeHelpers.RunModuleConstructor(module.ModuleHandle);
                    BootstrapMod.Out($"Initialized '{assembly.GetName().Name}'");
                }
                catch (Exception ex)
                {
                    BootstrapMod.Out(ex, $"Failed to initialize assembly!");
                }
            }
        }
        else BootstrapMod.Out($"Failed to resolve assemblies.");

        foreach (var mod in Mods)
        {
            try
            {
                BootstrapMod.Out($"Loading mod '{mod.Name}'...");
                mod.Load();
                totalMods++;
                BootstrapMod.Out($"Mod loaded!");
            }
            catch (Exception ex)
            {
                BootstrapMod.Out(ex, $"Failed to load mod!");
            }
        }
    }
}