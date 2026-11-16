using System;

namespace Unmoddable;

public abstract class Mod<T> : Mod where T : Mod<T>
{
    public static T Instance { get; protected set; } = null!;
    public Mod()
    {
        if (!typeof(T).IsAssignableFrom(GetType()))
            throw new Exception($"{GetType().Name} should provide itself as Mod<T>'s T generic argument!");
    }

    public override void Load()
    {
        Instance = (T)this;
        base.Load();
    }
    public override void Unload()
    {
        base.Unload();
        Instance = null!;
    }
}