using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace Unmoddable;

public static class PosterMaker1000Patches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PosterMaker1000), nameof(PosterMaker1000.SetupStickerButtons))]
    public static void SetupStickerButtonsPrefix(PosterMaker1000 __instance)
    {
        int total = 0;
        Debug.Log("Loading sprites...");
        File.AppendAllText("boot.log", $"Loading sprites...\r\n");
        try
        {
            string location = Path.Combine(ModsAPI.CommonContentDirectory, "Stickers");
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
            Debug.LogWarning("Failed to load stickers!\r\n" + ex.GetType().Name + ": " + ex.Message + "\r\n" + ex.StackTrace);
        }
    }
}