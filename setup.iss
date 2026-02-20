[Setup]
AppName=Sim Gamepad
AppVersion=1.0.0
DefaultDirName={autopf}\Sim Gamepad
DefaultGroupName=Sim Gamepad
UninstallDisplayIcon={app}\Sim Gamepad.exe
Compression=lzma2
SolidCompression=yes
OutputDir=bin\Release\installer
OutputBaseFilename=SimGamepad_Installer
SetupIconFile=assets\icon.ico
WizardImageFile=assets\wizard_large.bmp
WizardSmallImageFile=assets\wizard_small.bmp

[Files]
Source: "bin\Release\net10.0-windows\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\Sim Gamepad"; Filename: "{app}\Sim Gamepad.exe"
Name: "{autodesktop}\Sim Gamepad"; Filename: "{app}\Sim Gamepad.exe"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "Create a &desktop icon"; GroupDescription: "Additional icons:"

[Run]
Filename: "{app}\Sim Gamepad.exe"; Description: "{cm:LaunchProgram,Sim Gamepad}"; Flags: nowait postinstall skipifsilent
