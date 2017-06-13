#include GetEnv("LANDIS_SDK") + '\packaging\initialize.iss'

#define ExtInfoFile1 "Drought Disturbance.txt"
#include LandisSDK + '\packaging\read-ext-info1.iss'
#include LandisSDK + '\packaging\Landis-vars1.iss'

[Setup]
#include LandisSDK + '\packaging\Setup-directives1.iss'
LicenseFile={#LandisSDK}\licenses\LANDIS-II_Binary_license.rtf

[Files]
Source: {#LandisExtDir}\{#ExtensionAssembly1}.dll; DestDir: {app}\bin\extensions; Flags: replacesameversion
Source: {#LandisExtDir}\Landis.Extension.BaseHarvest.dll; DestDir: {app}\bin\extensions; Flags: replacesameversion
#define UserGuideSrc1 "LANDIS-II " + ExtensionName1 + " vX.Y User Guide.pdf"
#define UserGuide1    StringChange(UserGuideSrc1, "X.Y", MajorMinor1)
Source: docs\{#UserGuide1}; DestDir: {app}\docs; DestName: {#UserGuide1}; Flags: replacesameversion
Source: examples\*; DestDir: {app}\examples\{#ExtensionName1}; Flags: recursesubdirs replacesameversion
#define ExtensionInfo1  ExtensionName1 + " " + MajorMinor1 + ".txt"
Source: {#ExtInfoFile1}; DestDir: {#LandisExtInfoDir}; DestName: {#ExtensionInfo1}; Flags: replacesameversion

[Run]
Filename: {#ExtAdminTool}; Parameters: "remove ""{#ExtensionName1}"" "; WorkingDir: {#LandisExtInfoDir}
Filename: {#ExtAdminTool}; Parameters: "add ""{#ExtensionInfo1}"" "; WorkingDir: {#LandisExtInfoDir}

[UninstallRun]
Filename: {#ExtAdminTool}; Parameters: "remove ""{#ExtensionName1}"" "; WorkingDir: {#LandisExtInfoDir}

[Code]
#include LandisSDK + '\packaging\Pascal-code1.iss'

//-----------------------------------------------------------------------------

function InitializeSetup_FirstPhase(): Boolean;
begin
  Result := True
end;

