using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace Unmoddable;

// Note: net472 doesn't allow placing patch attribute near the method itself.
// (no matter how must I hate this)
[HarmonyPatch(typeof(PosterMaker1000))]
public static class PosterMaker1000Patches
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PosterMaker1000.Awake))]
    public static void AwakePrefix(PosterMaker1000 __instance)
    {
        int total = 0;
        $"Loading sprites...".Out();
        try
        {
            string location = Path.Combine(ModsIO.CommonContentDirectory, "Stickers");
            Directory.CreateDirectory(location);

            foreach (string file in Directory.EnumerateFiles(location, "*.png", SearchOption.AllDirectories))
            {
                Texture2D texture = new(0, 0);
                texture.LoadImage(File.ReadAllBytes(file));
                Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                sprite.name = Path.GetFileNameWithoutExtension(file);
                __instance.stickersToLoad.Add(sprite);
                total++;
            }
        }
        catch (Exception ex)
        {
            ex.Out($"Failed to load stickers!");
        }
    }
}