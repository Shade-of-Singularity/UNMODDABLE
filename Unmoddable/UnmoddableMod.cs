using UnityEngine;
using Object = UnityEngine.Object;

namespace Unmoddable;

public sealed class UnmoddableMod : Mod<UnmoddableMod>
{
    public override string Identifier => "sog.unbeatable.unmoddable";
    public override string[] Dependencies => [];
    public override string Name => "UNMODDABLE";
    public override string Prefix => "[UMO]";
    public override string Description => "You are just a part of my life...";

    public override void Load()
    {
        base.Load();
        Object.DontDestroyOnLoad(new GameObject("Focus Handler", [typeof(FocusHandler)]));
    }
}

