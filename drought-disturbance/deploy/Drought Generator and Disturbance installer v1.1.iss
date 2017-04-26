#include GetEnv("LANDIS_SDK") + '\packaging\initialize.iss'

#define ExtInfoFile1 "Drought Generator.txt"
#include LandisSDK + '\packaging\read-ext-info1.iss'
#include LandisSDK + '\packaging\Landis-vars1.iss'

#define ExtInfoFile2 "Drought Disturbance.txt"    
#include LandisSDK + '\packaging\read-ext-info2.iss'
#include LandisSDK + '\packaging\Landis-vars2.iss'

#define CombinedExtensionName ExtensionName1 + " and " + ExtensionName2
#pragma message Version1
#pragma message Version2
#if Version2 > Version1
#define CombinedVersion Version2
#define CombinedMajorMinor MajorMinor2
#define CombinedReleaseAsInt ReleaseAsInt2
#define CombinedPatchLevel PatchLevel2
#define CombinedVersionRelease VersionRelease2
#define CombinedReleaseForAppName ReleaseForAppName2
#define CombinedReleaseForAppVerName ReleaseForAppVerName2
#else
#define CombinedVersion Version1
#define CombinedMajorMinor MajorMinor1
#define CombinedReleaseAsInt ReleaseAsInt1
#define CombinedPatchLevel PatchLevel1
#define CombinedVersionRelease VersionRelease1
#define CombinedReleaseForAppName ReleaseForAppName1
#define CombinedReleaseForAppVerName ReleaseForAppVerName1
#endif

[Setup]
;#include LandisSDK + '\packaging\Setup-directives1.iss'
;#include LandisSDK + '\packaging\Setup-directives2.iss'
#include LandisSDK + '\packaging\Setup-directives_comb.iss'
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

Source: {#LandisExtDir}\{#ExtensionAssembly2}.dll; DestDir: {app}\bin\extensions; Flags: replacesameversion
Source: {#LandisExtDir}\Landis.Extension.BaseHarvest.dll; DestDir: {app}\bin\extensions; Flags: replacesameversion
#define UserGuideSrc2 "LANDIS-II " + ExtensionName2 + " vX.Y User Guide.pdf"
#define UserGuide2    StringChange(UserGuideSrc2, "X.Y", MajorMinor2)
Source: docs\{#UserGuide2}; DestDir: {app}\docs; DestName: {#UserGuide2}; Flags: replacesameversion
Source: examples\*; DestDir: {app}\examples\{#ExtensionName2}; Flags: recursesubdirs replacesameversion
#define ExtensionInfo2  ExtensionName2 + " " + MajorMinor2 + ".txt"
Source: {#ExtInfoFile2}; DestDir: {#LandisExtInfoDir}; DestName: {#ExtensionInfo2}; Flags: replacesameversion


[Run]
Filename: {#ExtAdminTool}; Parameters: "remove ""{#ExtensionName1}"" "; WorkingDir: {#LandisExtInfoDir}
Filename: {#ExtAdminTool}; Parameters: "add ""{#ExtensionInfo1}"" "; WorkingDir: {#LandisExtInfoDir}

Filename: {#ExtAdminTool}; Parameters: "remove ""{#ExtensionName2}"" "; WorkingDir: {#LandisExtInfoDir}
Filename: {#ExtAdminTool}; Parameters: "add ""{#ExtensionInfo2}"" "; WorkingDir: {#LandisExtInfoDir}

[UninstallRun]
Filename: {#ExtAdminTool}; Parameters: "remove ""{#ExtensionName1}"" "; WorkingDir: {#LandisExtInfoDir}
Filename: {#ExtAdminTool}; Parameters: "remove ""{#ExtensionName2}"" "; WorkingDir: {#LandisExtInfoDir}

[Code]
#include LandisSDK + '\packaging\Pascal-code1.iss'

//-----------------------------------------------------------------------------

function InitializeSetup_FirstPhase(): Boolean;
begin
  Result := True
end;

