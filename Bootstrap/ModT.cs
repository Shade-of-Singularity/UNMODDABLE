using System;
using System.Runtime.CompilerServices;

namespace Unmoddable;

/// <summary>
/// Base class for all mods.
/// </summary>
/// <remarks>
/// <typeparamref name="T"/> should reference the mod which implements this class. For example:
/// <para><![CDATA[public sealed class MyMod : Mod<MyMod> { ... }]]></para>
/// </remarks>
/// <typeparam name="T">Type of your mod to globalize!</typeparam>
public abstract class Mod<T> : Mod where T : Mod<T>, new()
{
    /// <summary>
    /// Static instance of the mod which can be accessed from everywhere.
    /// </summary>
    public static T Instance { get; protected internal set; } = null!;

    /// <inheritdoc/>
    public Mod() : base()
    {
        if (!typeof(T).IsAssignableFrom(GetType()))
            throw new Exception($"{Prefix} {GetType().Name} should provide itself as {nameof(Mod)}<T>'s T generic argument!");
        if (Instance is not null)
            throw new InvalidOperationException($"{Prefix} Cannot create a second ({GetType().Name}) instance!");
        Instance = (T)this;
    }

    /// <inheritdoc/>
    public override void Load() => base.Load();

    /// <inheritdoc/>
    public override void Unload() => base.Unload();

    /// <inheritdoc/>
    public static void Initialize() => Instance ??= new();

    /// <inheritdoc cref="Mod.Out(object?, object?)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static new void Out(object? obj, object? prefix = null) => ((Mod)Instance).Out(obj, prefix);

    /// <inheritdoc cref="Mod.Out(Exception?, object?)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static new void Out(Exception? ex, object? prefix = null) => ((Mod)Instance).Out(ex, prefix);

    /// <inheritdoc cref="Mod.OutW(object?, object?)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static new void OutW(object? obj, object? prefix = null) => ((Mod)Instance).OutW(obj, prefix);

    /// <inheritdoc cref="Mod.OutE(object?, object?)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static new void OutE(object? obj, object? prefix = null) => ((Mod)Instance).OutE(obj, prefix);
}