# BOOTSTRAP
Loads-in all the mods.

References only built-in game DLLs.

0Harmony and other integrations are provided by UNMODDABLE.

Loads-in all the Assemblies, respecting their dependency order.

# TODOs
- Load DLLs from BOOTSTRAP directly instead of relying on UNMODDABLE providing DLLs.
- Use Camelcase for names and assemblies (C#-compliance), but SCREAMING-case for visible mod names.
- Merge UNMODDABLE and BOOTSTRAP. Move functions to OUTBREAK instead.