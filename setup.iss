; ============================================================
;  以太网开关 - Inno Setup 安装脚本
; ============================================================

[Setup]
AppId={{8F3D9A1B-2C4E-5D6F-7A8B-9C0D1E2F3A4B}
AppName=以太网开关
AppVersion=1.0
AppPublisher=NetworkToggle
AppPublisherURL=https://example.com
AppSupportURL=https://example.com
AppUpdatesURL=https://example.com
DefaultDirName={autopf}\以太网开关
DefaultGroupName=以太网开关
AllowNoIcons=yes
OutputDir=C:\Users\ghy12\Desktop\网络切换脚本
OutputBaseFilename=以太网开关_Setup
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=modern
WizardSizePercent=120,120
PrivilegesRequired=admin
ArchitecturesInstallIn64BitMode=x64compatible
DisableWelcomePage=no
DisableProgramGroupPage=no
UninstallDisplayIcon={app}\NetworkToggle.exe
UninstallDisplayName=以太网开关

[Languages]
Name: "chinesesimp"; MessagesFile: "compiler:Languages\ChineseSimplified.isl"
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "创建桌面快捷方式"; GroupDescription: "附加任务:"; Flags: checkedonce

[Files]
Source: "C:\Users\ghy12\Desktop\网络切换脚本\NetworkToggle.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\ghy12\Desktop\网络切换脚本\NetworkToggle.ico"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{autoprograms}\{groupname}\以太网开关"; Filename: "{app}\NetworkToggle.exe"; IconFilename: "{app}\NetworkToggle.ico"
Name: "{autoprograms}\{groupname}\卸载 以太网开关"; Filename: "{uninstallexe}"; IconFilename: "{app}\NetworkToggle.ico"
Name: "{autodesktop}\以太网开关"; Filename: "{app}\NetworkToggle.exe"; IconFilename: "{app}\NetworkToggle.ico"; Tasks: desktopicon

[Registry]
; 注册表记录安装路径
Root: HKLM; Subkey: "Software\NetworkToggle"; ValueType: string; ValueName: "InstallPath"; ValueData: "{app}"; Flags: uninsdeletekey
