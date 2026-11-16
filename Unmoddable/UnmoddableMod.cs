using System.IO;
using UnityEngine;

namespace Unmoddable;

public sealed class UnmoddableMod : Mod<UnmoddableMod>
{
    public override string Identifier => "sog.unbeatable.unmoddable";
    public override string Name => "UNMODDABLE";
    public override string Description => "And you are just a part of my life...";

    public override void Load()
    {
        base.Load();
        Object.DontDestroyOnLoad(new GameObject("Focus Handler", [typeof(FocusHandler)]));
        File.AppendAllText("boot.log", $"Added focus handler\r\n");
        File.AppendAllText("boot.log", $"PM1000 exists: {Object.FindAnyObjectByType<PosterMaker1000>()}\r\n");
    }
}

