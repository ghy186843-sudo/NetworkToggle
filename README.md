# 🌐 以太网开关 (Network Toggle)

> 一键禁用 / 启用 Windows 以太网适配器 — 快捷、轻量、免打扰

<p align="center">
  <img src="NetworkToggle.ico" width="96" height="96" alt="icon">
</p>

<p align="center">
  <a href="https://github.com/ghy186843-sudo/NetworkToggle/releases/latest">
    <img src="https://img.shields.io/badge/⬇️_下载安装包-v1.0-blue?style=for-the-badge" alt="下载">
  </a>
  <br>
  <sub>点击上方按钮 → 下载 <b>以太网开关_Setup.exe</b> → 双击安装</sub>
</p>

---

## 📖 简介

**以太网开关** 是一个 Windows 小工具，让你可以**一键切换**有线以太网适配器的启用/禁用状态。

适用于需要频繁切换有线网络的场景：内网/外网切换、网络调试、断网测试、家长控制等。

### ✨ 功能特性

| 特性 | 说明 |
|---|---|
| 🔌 **一键切换** | 绿色按钮 = 已启用，红色按钮 = 已禁用，直观明了 |
| 🔍 **自动检测** | 启动时自动识别系统中的以太网适配器 |
| ⚡ **管理员提权** | 内建自提权逻辑，无需手动右键「以管理员身份运行」 |
| 🎨 **桌面图标** | 自带多尺寸图标（16~256px），任务栏/桌面清晰显示 |
| 🔄 **状态刷新** | 每 3 秒自动检测网卡状态，防止外部修改 |
| 📦 **标准安装** | Inno Setup 打包，向导式安装，支持自定义路径 |

### 🖥️ 界面预览

```
┌──────────────────────────────┐
│     🌐 以太网适配器开关       │
│                              │
│  🟢 适配器: Realtek ...      │
│         已启用               │
│                              │
│  ┌──────────────────┐        │
│  │      禁  用      │        │
│  └──────────────────┘        │
│  ┌──────────┐               │
│  │  退  出  │               │
│  └──────────┘               │
└──────────────────────────────┘
```

---

## 🚀 快速开始

### 📥 安装

1. 下载最新版 `以太网开关_Setup.exe` → [Releases](../../releases)
2. 双击运行安装向导（简体中文界面）
3. 自定义安装路径，勾选「创建桌面快捷方式」
4. 点击安装 → 完成

### 🔧 卸载

- 开始菜单 → 以太网开关 → 卸载
- 或通过 Windows 设置 → 应用 → 卸载

---

## 🛠️ 技术栈

| 技术 | 用途 |
|---|---|
| **C# / .NET Framework 4.8** | 主程序（WinForms GUI） |
| **WMI / netsh** | 网络适配器控制 |
| **Inno Setup 6** | 安装程序打包 |
| **PowerShell** | 图标生成 & 构建脚本 |

### 📁 项目结构

```
网络切换脚本/
├── NetworkToggle.cs          # 主程序源码
├── NetworkToggle.ico         # 应用图标（多尺寸）
├── setup.iss                 # Inno Setup 安装脚本
├── README.md                 # 本文件
└── .gitignore
```

### 🔨 自行编译

```powershell
# 编译主程序（需要 .NET Framework 4.8 SDK）
csc /target:winexe /win32icon:NetworkToggle.ico `
    /out:NetworkToggle.exe `
    /reference:System.Windows.Forms.dll `
    /reference:System.Management.dll `
    NetworkToggle.cs

# 打包安装程序（需要 Inno Setup 6）
iscc setup.iss
```

---

## 📄 系统要求

- **操作系统**: Windows 10 / Windows 11
- **权限**: 管理员权限（程序自动提权）
- **框架**: .NET Framework 4.8（Windows 10+ 自带）

---

## 📝 License

MIT License — 随意使用、修改、分发。

---

<p align="center">
  <sub>Made with ❤️ by AI & Human</sub>
</p>
