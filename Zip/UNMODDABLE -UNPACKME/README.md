# UNMODDABLE
UNMODDABLE - is UNBEATALBE modding system.

This entry contains actual mods and mod content.

# How to install mods & mod content
1. Make sure you've completed the `BOOTSTRAP` guide.
2. Find where UNBEATABLE's AppData is.
    - On Windows: `%UserProfile%/AppData/LocalLow/D-CELL GAMES/UNBEATABLE`
    - On Linux: `$XDG_CONFIG_HOME/unity3d/D-CELL GAMES/UNBEATABLE`
    - On macOS, somewhere around: `~/Library/Application Support/unity.dcellgames.unbeatable`
    - Otherwise, google: `Unity {Your OS}: Application.persistentDataPath`. 
3. Put entire `UNMODDABLE` folder in there (i.e. next to `PROFILES`).

**Done!**

**Mods will now launch together with your game!**

If you suspect mods might have had issues during loading - check `unmoddable.log` for `Exception`s.

*Notice: `unmoddable.log` is where `UNBEATABLE.exe` is!*

# Adding custom stickers
Simply put them in `UNMODDABLE/COMMON/Stickers`. PNG-only.

Windows full path:
```
%UserProfile%/AppData/LocalLow/D-CELL GAMES/UNBEATABLE/UNMODDABLE/COMMON/Stickers
```

Sub-folders are scanned as well.