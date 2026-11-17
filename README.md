# UNMODDABLE
Mod loader for UNBEATABLE with some default mods.

# Modding Guide
Guide is WIP! Since no one asked me about it.

Message me directly - I will help you personally (my pleasure to help ^-^)
- Discord: darkjune
- Gmail: darkjune.unmanaged@gmail.com

# Building
1. Open Visual Studio.
	- View -> Terminal -> Developer PowerShell
	- Alternatively, just regular console in `UNMODDABLE` folder.
2. Build with a command:
	- For mods: `dotnet publish UNMODDABLE.slnx -p:PublishProfile=FolderProfile`
	- For zipping: `dotnet publish UNMODDABLE.slnx -p:PublishProfile=ZipProfile`

# Trivia (since no one asked)
## Modding
If you want to make mods - visit project's repo:

https://github.com/Shade-of-Singularity/UNMODDABLE

Root folder for all modded content:
```
%UserProfile%/AppData/LocalLow/D-CELL GAMES/UNBEATABLE/UNMODDABLE
```

## Mods folder
All mod assemblies are stored here:

Location:
```
%UserProfile%/AppData/LocalLow/D-CELL GAMES/UNBEATABLE/UNMODDABLE/MODS
```

## Common folder
This is a data folder, for information, which community agreed on how to share.

For examle - custom stickers from UNMODDABLE are stored in `.../COMMON/Stickers`.

Location:
```
%UserProfile%/AppData/LocalLow/D-CELL GAMES/UNBEATABLE/UNMODDABLE/COMMON
```

## Configs folder
This is where read-only configs are stored.

This folder is shared with end-user, and synchronized over GIT when people collab on modpacks.

For data which is personalized for each player (i.e. volume settings, etc) - use Dynamics instead.

Location:
```
%UserProfile%/AppData/LocalLow/D-CELL GAMES/UNBEATABLE/UNMODDABLE/CONFIGS
```

## Dynamics folder
This is where dynamic player-specific data is stored.

This data is usually never sent to players with modpacks.

Treat this a a temp storage.

Location:
```
%UserProfile%/AppData/LocalLow/D-CELL GAMES/UNBEATABLE/UNMODDABLE/DYNAMICS
```