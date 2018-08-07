#define PackageName      "Extension Biomass Drought"
#define PackageNameLong  "Biomass Drought Disturbance System"
#define Version          "1.2.1"
#define ReleaseType      "official"
#define CoreVersion      "6.0"
#define CoreReleaseAbbr  ""

#define ExtDir "C:\Program Files\LANDIS-II\v6\bin\extensions"
#define AppDir "C:\Program Files\LANDIS-II\v6\"
#define LandisPlugInDir "C:\Program Files\LANDIS-II\plug-ins"

#include "package (Setup section) v6.0.iss"


[Files]
; Unique ==> These two (2) .dlls together constitute the Drought Biomass extension.
; NB. The Landis.Extension.DroughtGenerator.dll must be built separately and added to ..\..\src\bin\debug
; NB: Do not put an additional version number in the file name of these .dlls
; (The names of these .dlls are defined in the \src\*.csproj file for each extension)
Source: ..\..\src\bin\debug\Landis.Extension.DroughtDisturbance.dll; DestDir: {#ExtDir}; Flags: replacesameversion
Source: ..\..\src\bin\debug\Landis.Extension.DroughtGenerator.dll; DestDir: {#ExtDir}; Flags: replacesameversion


; Requisite auxiliary libraries
; NB. These libraries are used by other extensions and thus are never uninstalled.
Source: ..\..\src\bin\Debug\Landis.Library.AgeOnlyCohorts.dll; DestDir: {#ExtDir}; Flags: replacesameversion
Source: ..\..\src\bin\Debug\Landis.Library.Biomass-v1.dll; DestDir: {#ExtDir}; Flags: replacesameversion
Source: ..\..\src\bin\Debug\Landis.Library.BiomassCohorts-v2.dll; DestDir: {#ExtDir}; Flags: replacesameversion
Source: ..\..\src\bin\Debug\Landis.Library.Cohorts.dll; DestDir: {#ExtDir}; Flags: replacesameversion
Source: ..\..\src\bin\Debug\Landis.Library.Metadata.dll; DestDir: {#ExtDir}; Flags:replacesameversion


; Complete example for testing the extension
Source: ..\examples\*.txt; DestDir: {#AppDir}\examples\Biomass Drought; Flags: replacesameversion
Source: ..\examples\*.gis; DestDir: {#AppDir}\examples\Biomass Drought; Flags: replacesameversion
Source: ..\examples\*.bat; DestDir: {#AppDir}\examples\Biomass Drought; Flags: replacesameversion
Source: ..\examples\*.csv; DestDir: {#AppDir}\examples\Biomass Drought; Flags: replacesameversion


#define InfoTxta "Drought Disturbance Component 1.2.txt"
Source: {#InfoTxta}; DestDir: {#LandisPlugInDir}

#define InfoTxtb "Drought Generator Component 1.2.txt"
Source: {#InfoTxtb}; DestDir: {#LandisPlugInDir}



[Run]
;; Run plug-in admin tool to add entries for each plug-in
#define PlugInAdminTool  CoreBinDir + "\Landis.PlugIns.Admin.exe"

Filename: {#PlugInAdminTool}; Parameters: "remove ""Drought Disturbance Component"" "; WorkingDir: {#LandisPlugInDir}
Filename: {#PlugInAdminTool}; Parameters: "add ""{#InfoTxta}"" "; WorkingDir: {#LandisPlugInDir}

Filename: {#PlugInAdminTool}; Parameters: "remove ""Drought Generator Component"" "; WorkingDir: {#LandisPlugInDir}
Filename: {#PlugInAdminTool}; Parameters: "add ""{#InfoTxtb}"" "; WorkingDir: {#LandisPlugInDir}



[Code]
{ Check for other prerequisites during the setup initialization }
#include "package (Code section) v3.iss"

//-----------------------------------------------------------------------------

function InitializeSetup_FirstPhase(): Boolean;
begin
  Result := True
end;

