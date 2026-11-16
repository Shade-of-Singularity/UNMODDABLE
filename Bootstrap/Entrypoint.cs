using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
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

        File.Delete("boot.log");
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
            File.AppendAllText("boot.log", $"All native libraries located, initializing mods.\r\n");
            Initialize();
            var total = stopwatch.Elapsed;
            File.AppendAllText("boot.log", $"Mods initialized successfully ({total:c})\r\n");
            return;
        }
    }
    public sealed class CustomUnityLogHandler : ILogHandler
    {
        public static readonly CustomUnityLogHandler Instance = new();
        private CustomUnityLogHandler() { }

        public void LogException(Exception exception, UnityEngine.Object context)
        {
            File.AppendAllText("run.log", $"{context}: [{exception.GetType().Name}] {exception.Message}\n{exception.StackTrace}");
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            if (args is null || args.Length == 0)
            {
                File.AppendAllText("run.log", $"{context}: {format}");
            }
            else
            {
                File.AppendAllText("run.log", $"{context}: {format} << {string.Join(", ", args)}");
            }
        }
    }

    public static void Initialize()
    {
        Directory.CreateDirectory(ModsAPI.ModsDirectory);
        Directory.CreateDirectory(ModsAPI.CommonContentDirectory);

        UnityEngine.Debug.unityLogger.logHandler = CustomUnityLogHandler.Instance;

        //Debug.developerConsoleEnabled = true;
        //Debug.developerConsoleVisible = true;
        DependencyMap<string, Assembly> map = new(
            static a => a.GetReferencedAssemblies().Select(static a => a.FullName).ToHashSet());

        var native = AppDomain.CurrentDomain.GetAssemblies().Select(static a => a.GetName().Name).ToHashSet();
        foreach (var file in Directory.GetFiles(ModsAPI.ModsDirectory, "*.dll", SearchOption.AllDirectories))
        {
            try
            {
                string extn = Path.GetFileNameWithoutExtension(file);
                if (native.Contains(extn))
                {
                    File.AppendAllText("boot.log", $"Skipping native assembly: {extn}\r\n");
                    continue;
                }

                Assembly assembly = Assembly.LoadFrom(file);
                string name = assembly.FullName;
                map.TryAdd(name, assembly);
                File.AppendAllText("boot.log", $"Located assembly: {name}\r\n");
            }
            catch (Exception ex)
            {
                File.AppendAllText("boot.log", $"Failed to load DLL!\n{ex.GetType()}: {ex.Message}\n{ex.StackTrace}\r\n");
            }
        }

        Harmony.DEBUG = true;
        if (map.TryResolve(out var assemblies))
        {
            File.AppendAllText("boot.log", $"Resolved total of ({assemblies.Count}) assemblies.\r\n");
            foreach (var assembly in map)
            {
                try
                {
                    foreach (var type in assembly.GetTypes().Where(
                        static t => !t.IsAbstract && !t.ContainsGenericParameters && typeof(Mod).IsAssignableFrom(t)))
                    {
                        File.AppendAllText("boot.log", $"Mod type found: {type.FullName}\r\n");
                        Mods.Add((Mod)Activator.CreateInstance(type));
                    }

                    foreach (var module in assembly.GetModules())
                        RuntimeHelpers.RunModuleConstructor(module.ModuleHandle);
                    File.AppendAllText("boot.log", $"Initialized '{assembly.GetName().Name}'\r\n");
                }
                catch (Exception ex)
                {
                    File.AppendAllText("boot.log", $"Failed to initialize assembly! {assembly.FullName}" +
                        $"\r\n{ex.GetType()}: {ex.Message}\r\n{ex.StackTrace}");
                }
            }
        }
        else File.AppendAllText("boot.log", $"Failed to resolve assemblies.\r\n");

        foreach (var mod in Mods)
        {
            try
            {
                mod.Load();
                File.AppendAllText("boot.log", $"Loaded mod '{mod.Name}'\r\n");
            }
            catch (Exception ex)
            {
                File.AppendAllText("boot.log", $"Failed to load mod! {mod.Name}\r\n{ex.GetType()}: {ex.Message}\r\n{ex.StackTrace}");
            }
        }
    }
}