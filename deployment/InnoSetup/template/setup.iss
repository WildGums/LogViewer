#define AppName "Log Viewer"
#define SourceDirectory ""
; #define AppVersion "3.1"
#define AppVersion "[VERSION]"
; #define AppDisplayVersion "3.1 beta 1"
#define AppDisplayVersion "[VERSION_DISPLAY]"
#define AppNameWithDisplayVersion AppName + " " + AppDisplayVersion
#define AppNameWithVersion AppName + " " + AppVersion
#define OutputPrefix "LogViewer"
#define Website "http://www.wildgums.com/"
#define Company "WildGums"
#define IconName "logo"
#define ExecutableName "LogViewer.exe"
#define AppMutex Company + "_" + AppName

#define OutputFileWithSpaces OutputPrefix + "_" + AppDisplayVersion
#define OutputFile StringChange(OutputFileWithSpaces, " ", "_")

[_ISTool]
EnableISX=false
Use7zip=false

[Setup]
AppName={#AppNameWithVersion}
AppVerName={#AppNameWithDisplayVersion}
AppID={#AppMutex}
AppPublisher={#Company}
AppCopyright={#Company}
DefaultDirName={pf32}\{#Company}\{#AppName}
DefaultGroupName={#AppName}
UsePreviousSetupType=true
OutputDir=..\output
OutputBaseFilename={#OutputFile}
UninstallDisplayName={#AppName}
Compression=lzma2/Ultra64
UseSetupLdr=true
SolidCompression=true
ShowLanguageDialog=yes
VersionInfoVersion={#AppVersion}
AppVersion={#AppVersion}
InternalCompressLevel=Ultra64
AppPublisherURL={#Website}
AppSupportURL={#Website}
AppUpdatesURL={#Website}
AppContact={#Website}
VersionInfoCompany={#Company}
AppMutex={#AppMutex}
LanguageDetectionMethod=none
DisableStartupPrompt=True
WizardImageFile=resources\[WIZARDIMAGEFILE].bmp
; WizardImageFile=resources\logo_large_nightly.bmp
WizardSmallImageFile=resources\logo_small.bmp
SetupIconFile=resources\{#IconName}.ico
UninstallDisplayIcon={app}\resources\{#IconName}.ico
SetupLogging=true
; For signing, the following sign tool must be configured
; Name: Signtool
; Command: "C:\Source\SMS_Certificates\Tools\signtool.exe" sign /t "http://timestamp.comodoca.com/authenticode" /f "C:\Source\SMS_Certificates\CodeSigning\current.pfx" "$f"
;SignTool=Signtool

[InnoIDE_Settings]
UseRelativePaths=true

[Dirs]
Name: {app}\doc; 
Name: {app}\resources;

[Files]
Source: readme.txt; DestDir: {app};
Source: resources\*; DestDir: {app}\resources; Flags: createallsubdirs recursesubdirs;
Source: resources\{#IconName}.ico; DestDir: {app}\resources;

;--------------------
; Application content
;--------------------

; Copy all files
Source: *; DestDir: {app}\; Excludes: "*.iss"; Flags: createallsubdirs recursesubdirs;

;-----------------
; Application data
;-----------------

; Copy all files
;Source: data\*; DestDir: {app}\data;

[CustomMessages]
DotNetMissing=This setup requires the .NET Framework. Please download and install the .NET Framework and run this setup again. Do you want to download the framework now?

[ThirdPartySettings]
CompileLogMethod=append

[UninstallDelete]
Name: {app}; Type: filesandordirs

[Icons]
Name: "{group}\{#AppName}"; Filename: "{app}\{#ExecutableName}"; WorkingDir: "{app}"
Name: "{group}\Go to website"; Filename: "{#Website}"
Name: "{group}\Uninstall {#AppName}"; Filename: "{app}\unins000.exe"; WorkingDir: "{app}"; IconFilename: "{app}\resources\{#IconName}.ico"

[Types]
Name: Full; Description: "Full installation";
;Name: Custom; Description: Custom; Flags: IsCustom; 

[Components]
Name: core; Description: Libraries;
;Name: snippets; Description: "Code snippets"; Types: Full Custom; 
;Name: templates; Description: "Project and Item templates"; Types: Full Custom;  

[Languages]
Name: "English"; MessagesFile: "compiler:Default.isl"
Name: "Czech"; MessagesFile: "compiler:Languages\Czech.isl"
Name: "Danish"; MessagesFile: "compiler:Languages\Danish.isl"
Name: "Dutch"; MessagesFile: "compiler:Languages\Dutch.isl"
Name: "Finnish"; MessagesFile: "compiler:Languages\Finnish.isl"
Name: "French"; MessagesFile: "compiler:Languages\French.isl"
Name: "German"; MessagesFile: "compiler:Languages\German.isl"
Name: "Hungarian"; MessagesFile: "compiler:Languages\Hungarian.isl"
Name: "Italian"; MessagesFile: "compiler:Languages\Italian.isl"
Name: "Japanese"; MessagesFile: "compiler:Languages\Japanese.isl"
Name: "Norwegian"; MessagesFile: "compiler:Languages\Norwegian.isl"
Name: "Polish"; MessagesFile: "compiler:Languages\Polish.isl"
Name: "Portuguese"; MessagesFile: "compiler:Languages\Portuguese.isl"
Name: "Russian"; MessagesFile: "compiler:Languages\Russian.isl"
Name: "Spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Run]
Filename: "{app}\{#ExecutableName}"; WorkingDir: "{app}"; Flags: nowait postinstall runasoriginaluser skipifsilent; Description: "Start {#AppName}"

[ThirdParty]
CompileLogMethod=append

[Code]
//=========================================================================
// GetUninstallString
//=========================================================================

function GetUninstallString(): String;
var
  sUnInstPath: String;
  sUnInstallString: String;
begin
  sUnInstPath := ExpandConstant('Software\Microsoft\Windows\CurrentVersion\Uninstall\{#emit SetupSetting("AppId")}_is1');
  sUnInstallString := '';
  if not RegQueryStringValue(HKLM, sUnInstPath, 'UninstallString', sUnInstallString) then
    RegQueryStringValue(HKCU, sUnInstPath, 'UninstallString', sUnInstallString);
  Result := sUnInstallString;
end;

//=========================================================================
// IsDotNetFrameworkInstalled
//=========================================================================

function IsUpgrade(): Boolean;
begin
  Result := (GetUninstallString() <> '');
end;

//=========================================================================
// UnInstallOldVersion
//=========================================================================

function UnInstallOldVersion(): Integer;
var
  sUnInstallString: String;
  iResultCode: Integer;
begin
// Return Values:
// 1 - uninstall string is empty
// 2 - error executing the UnInstallString
// 3 - successfully executed the UnInstallString

  // default return value
  Result := 0;

  // get the uninstall string of the old app
  sUnInstallString := GetUninstallString();
  if sUnInstallString <> '' then begin
    sUnInstallString := RemoveQuotes(sUnInstallString);
    if Exec(sUnInstallString, '/SILENT /NORESTART /SUPPRESSMSGBOXES','', SW_HIDE, ewWaitUntilTerminated, iResultCode) then
      Result := 3
    else
      Result := 2;
  end else
    Result := 1;
end;

//=========================================================================
// CurStepChanged
//=========================================================================

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if (CurStep=ssInstall) then
  begin
    if (IsUpgrade()) then
    begin
      UnInstallOldVersion();
    end;
  end;
end;

//=========================================================================
// IsDotNetFrameworkInstalled
//=========================================================================
{
	Checks whether the right version of the .NET framework is installed
}

function IsDotNetFrameworkInstalled : Boolean;
var
    ErrorCode: Integer;
    NetFrameWorkInstalled : Boolean;
begin
	// Check if the .NET framework is installed
	//NetFrameWorkInstalled := RegKeyExists(HKLM,'SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727'); 	// 2.0
	//NetFrameWorkInstalled := RegKeyExists(HKLM,'SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.0'); 		// 3.0
	//NetFrameWorkInstalled := RegKeyExists(HKLM,'SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5'); 			// 3.5
	NetFrameWorkInstalled := RegKeyExists(HKLM,'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4'); 			// 4.0
    //NetFrameWorkInstalled := RegKeyExists(HKLM,'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4'); 			// 4.5

	// If the .NET framework is not installed, show message to user to download the framework
	if NetFrameWorkInstalled = false then
	begin
		if MsgBox(ExpandConstant('{cm:DotNetMissing}'), mbConfirmation, MB_YESNO) = idYes then
		begin
			ShellExec('open',
				//'http://www.microsoft.com/downloads/details.aspx?FamilyID=0856eacb-4362-4b0d-8edd-aab15c5e04f5&displaylang=en', // 2.0
				//'http://www.microsoft.com/downloads/details.aspx?FamilyID=10cc340b-f857-4a14-83f5-25634c3bf043&displaylang=en', // 3.0
				//'http://www.microsoft.com/downloads/details.aspx?FamilyId=333325fd-ae52-4e35-b531-508d977d32a6&displaylang=en', // 3.5
				'http://www.microsoft.com/downloads/details.aspx?familyid=9CFB2D51-5FF4-4491-B0E5-B386F32C0992&displaylang=en', // 4.0
				//'http://www.microsoft.com/en-us/download/details.aspx?id=30653', // 4.5
				'','',SW_SHOWNORMAL,ewNoWait,ErrorCode);
		end;
	end;

	// Return result
	Result := NetFrameWorkInstalled;
end;

//=========================================================================
// INITIALIZESETUP
//=========================================================================
{
	This function initializes the setup.
}

function InitializeSetup(): Boolean;
var
  sPrevPath: String;
begin
  // Check .NET framework
  if (IsDotNetFrameworkInstalled() = false) then
  begin
	Result := false;
	exit;
  end;

  Result := true;
end;