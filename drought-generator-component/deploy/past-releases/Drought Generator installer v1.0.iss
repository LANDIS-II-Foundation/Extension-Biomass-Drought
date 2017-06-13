#define PackageName      "Drought Generator"
#define PackageNameLong  "Drought Generator Extension"
#define Version          "1.0"

#define ReleaseType      "official"
#define ReleaseNumber    "1"

#define CoreVersion      "6.0"
#define CoreReleaseAbbr  ""

#include AddBackslash(GetEnv("LANDIS_DEPLOY")) + "package (Setup section) v6.0.iss"


[Files]
#define BuildDir "C:\Program Files\LANDIS-II\6.0\bin"

; The extension's assembly
Source: {#BuildDir}\Landis.Extension.DroughtGenerator.dll; DestDir: {app}\bin; Flags: replacesameversion

; The user guide
Source: docs\LANDIS-II Drought Generator v1.0 User Guide.pdf; DestDir: {app}\docs; DestName: LANDIS-II {#PackageName} v{#Version}{#ReleaseAbbr} User Guide.pdf

; Sample input file
Source: examples\*; DestDir: {app}\examples\drought-generator

; The extension's info file
#define ExtensionInfoFile PackageName + " " + Version + ReleaseAbbr+ ".txt"
Source: Drought Generator v1.0.txt; DestDir: {#LandisPlugInDir}; DestName: {#ExtensionInfoFile}

[Run]
;; Run plug-in admin tool to add an entry for the plug-in
#define PlugInAdminTool  CoreBinDir + "\Landis.PlugIns.Admin.exe"
Filename: {#PlugInAdminTool}; Parameters: "remove ""Drought Generator"" "; WorkingDir: {#LandisPlugInDir}
Filename: {#PlugInAdminTool}; Parameters: "add ""{#ExtensionInfoFile}"" "; WorkingDir: {#LandisPlugInDir}

[UninstallRun]

[Code]
#include AddBackslash(GetEnv("LANDIS_DEPLOY")) + "package (Code section) v3.iss"


//-----------------------------------------------------------------------------

function CurrentVersion_PostUninstall(currentVersion: TInstalledVersion): Integer;
begin
    Result := 0;
end;
//-----------------------------------------------------------------------------

function InitializeSetup_FirstPhase(): Boolean;
begin
  Result := True
end;
