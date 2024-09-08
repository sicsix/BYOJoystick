@echo off
for /f "tokens=2*" %%A in ('reg query "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 667970" /v InstallLocation 2^>nul') do set "installDir=%%B"

mklink "Assembly-CSharp.dll" "%installDir%\VTOLVR_Data\Managed\Assembly-CSharp.dll"
mklink "ModLoader.Framework.dll" "%installDir%\@Mod Loader\Managed\ModLoader.Framework.dll"
mklink "Unity.InputSystem.dll" "%installDir%\VTOLVR_Data\Managed\Unity.InputSystem.dll"
mklink "Unity.TextMeshPro.dll" "%installDir%\VTOLVR_Data\Managed\Unity.TextMeshPro.dll"
mklink "UnityEngine.AssetBundleModule.dll" "%installDir%\VTOLVR_Data\Managed\UnityEngine.AssetBundleModule.dll"
mklink "UnityEngine.AudioModule.dll" "%installDir%\VTOLVR_Data\Managed\UnityEngine.AudioModule.dll"
mklink "UnityEngine.CoreModule.dll" "%installDir%\VTOLVR_Data\Managed\UnityEngine.CoreModule.dll"
mklink "UnityEngine.dll" "%installDir%\VTOLVR_Data\Managed\UnityEngine.dll"
mklink "UnityEngine.InputLegacyModule.dll" "%installDir%\VTOLVR_Data\Managed\UnityEngine.InputLegacyModule.dll"
mklink "UnityEngine.UI.dll" "%installDir%\VTOLVR_Data\Managed\UnityEngine.UI.dll"