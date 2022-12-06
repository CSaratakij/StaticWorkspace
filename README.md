# StaticWorkspace
- Use VirtualDesktop stuff with global hotkey in Windows11 (static workspace style, up to 10 workspaces)

# Keyboard Shortcut
- Mod : Windows
- Switch to workspace : Mod + (1-0)
- Switch to next workspace : Mod + Peroid
- Switch to previous workspace : Mod + Comma
- Switch to back and forth : Mod + Grave Accent
- Move focus window to workspace : Mod + Shift + (1-0)
- Move focus window to next workspace : Mod + Shift + Period
- Move focus window to previous workspace : Mod + Shift + Comma

# Getting Started
1) [Build 'VirtualDesktop' (.dll)](https://github.com/CSaratakij/VirtualDesktop/tree/feature-win11-library)
2) [Build 'NonInvasiveKeyboardHook' (.dll)](https://github.com/CSaratakij/NonInvasiveKeyboardHook/tree/hotfix-netframework-48-library)
3) Add .dll as reference
4) Build solution
5) Run executable, use tray icon to exit

# Requirement
- .Net Framework 4.8

# Dependencies
- [VirtualDesktop](https://github.com/MScholtes/VirtualDesktop)
- [NonInvasiveKeyboardHook](https://github.com/kfirprods/NonInvasiveKeyboardHook)

# Note
- Run build as 'Administrator' to make global keyboard shortcut work with root process 'ex. TaskManager'
- Test only Windows 11 22H2
- Hidden COM interface is subject to change out of MS whim
- This software is not stable until MS decide to expose internal 'VirtualDesktop' api publicly
