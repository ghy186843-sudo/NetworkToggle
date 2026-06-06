using System;
using System.Diagnostics;
using System.Drawing;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;

namespace NetworkToggle
{
    public class MainForm : Form
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        private Label lblTitle, lblAdapterName, lblStatus;
        private Button btnToggle, btnClose;
        private Panel statusIndicator;
        private Timer statusTimer;
        private string adapterName = "";
        private bool isEnabled = false;

        [STAThread]
        static void Main()
        {
            if (!IsAdministrator())
            {
                var psi = new ProcessStartInfo
                {
                    FileName = Application.ExecutablePath,
                    UseShellExecute = true,
                    Verb = "runas"
                };
                try { Process.Start(psi); } catch { }
                return;
            }

            SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static bool IsAdministrator()
        {
            using (var identity = WindowsIdentity.GetCurrent())
            {
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public MainForm()
        {
            InitializeComponent();
            LoadAdapters();
        }

        private void InitializeComponent()
        {
            this.Text = "以太网开关";
            this.Size = new Size(380, 290);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.White;

            lblTitle = new Label() {
                Text = "以太网适配器开关",
                Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Bold),
                ForeColor = Color.Cyan, AutoSize = true,
                Location = new Point(60, 20)
            };

            statusIndicator = new Panel() {
                Size = new Size(16, 16), Location = new Point(35, 27),
                BackColor = Color.Gray
            };
            statusIndicator.Paint += (s, e) => {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.FillEllipse(new SolidBrush(statusIndicator.BackColor), 0, 0, 15, 15);
            };

            lblAdapterName = new Label() {
                Text = "正在检测适配器...",
                Font = new Font("Microsoft YaHei UI", 10F),
                ForeColor = Color.LightGray, AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(320, 25), Location = new Point(30, 55)
            };

            lblStatus = new Label() {
                Text = "状态: --",
                Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Bold),
                ForeColor = Color.Gray, AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(320, 40), Location = new Point(30, 85)
            };

            btnToggle = new Button() {
                Text = "禁  用",
                Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold),
                Size = new Size(150, 45), Location = new Point(115, 140),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(220, 60, 60),
                ForeColor = Color.White, Cursor = Cursors.Hand, Enabled = false
            };
            btnToggle.FlatAppearance.BorderSize = 0;
            btnToggle.Click += (s, e) => { if (isEnabled) DisableAdapter(); else EnableAdapter(); };
            btnToggle.MouseEnter += (s, e) => {
                if (!btnToggle.Enabled) return;
                btnToggle.BackColor = isEnabled ? Color.FromArgb(200, 40, 40) : Color.FromArgb(40, 160, 40);
            };
            btnToggle.MouseLeave += (s, e) => {
                btnToggle.BackColor = isEnabled ? Color.FromArgb(220, 60, 60) : Color.FromArgb(60, 180, 60);
            };

            btnClose = new Button() {
                Text = "退  出", Font = new Font("Microsoft YaHei UI", 10F),
                Size = new Size(90, 32), Location = new Point(145, 195),
                FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(80, 80, 80),
                ForeColor = Color.White, Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => Application.Exit();
            btnClose.MouseEnter += (s, e) => btnClose.BackColor = Color.FromArgb(100, 100, 100);
            btnClose.MouseLeave += (s, e) => btnClose.BackColor = Color.FromArgb(80, 80, 80);

            statusTimer = new Timer() { Interval = 3000 };
            statusTimer.Tick += (s, e) => RefreshStatus();
            statusTimer.Start();

            this.Controls.AddRange(new Control[] { lblTitle, statusIndicator, lblAdapterName, lblStatus, btnToggle, btnClose });
        }

        private void LoadAdapters()
        {
            try
            {
                string ethernetName = null;
                foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        if (nic.OperationalStatus == OperationalStatus.Up) { ethernetName = nic.Name; break; }
                        if (ethernetName == null) ethernetName = nic.Name;
                    }
                }
                if (string.IsNullOrEmpty(ethernetName)) ethernetName = FindEthernetViaWMI();
                if (!string.IsNullOrEmpty(ethernetName))
                {
                    adapterName = ethernetName;
                    lblAdapterName.Text = "适配器: " + adapterName;
                    btnToggle.Enabled = true;
                    RefreshStatus();
                }
                else
                {
                    lblAdapterName.Text = "未检测到以太网适配器";
                    lblStatus.Text = "状态: 无适配器";
                    lblStatus.ForeColor = Color.OrangeRed;
                }
            }
            catch (Exception ex) { lblAdapterName.Text = "检测失败: " + ex.Message; }
        }

        private string FindEthernetViaWMI()
        {
            try
            {
                using (var s = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_NetworkAdapter WHERE AdapterTypeID = 0 AND PhysicalAdapter = True"))
                {
                    foreach (ManagementObject obj in s.Get())
                    {
                        var n = obj["Name"];
                        if (n != null && !string.IsNullOrEmpty(n.ToString())) return n.ToString();
                    }
                }
            }
            catch { }
            return null;
        }

        private void RefreshStatus()
        {
            if (string.IsNullOrEmpty(adapterName)) return;
            try
            {
                bool found = false;
                foreach (var a in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (a.Name == adapterName) { isEnabled = a.OperationalStatus == OperationalStatus.Up; found = true; break; }
                }
                if (!found) isEnabled = IsAdapterEnabled(adapterName);
                UpdateUI();
            }
            catch { }
        }

        private bool IsAdapterEnabled(string name)
        {
            try
            {
                using (var s = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_NetworkAdapter WHERE Name = '" + name.Replace("'", "''") + "'"))
                {
                    foreach (ManagementObject obj in s.Get())
                    {
                        var ne = obj["NetEnabled"];
                        if (ne != null) return (bool)ne;
                    }
                }
            }
            catch { }
            return false;
        }

        private void UpdateUI()
        {
            if (isEnabled)
            {
                lblStatus.Text = "已启用";
                lblStatus.ForeColor = Color.LimeGreen;
                statusIndicator.BackColor = Color.LimeGreen;
                btnToggle.Text = "禁  用";
                btnToggle.BackColor = Color.FromArgb(220, 60, 60);
            }
            else
            {
                lblStatus.Text = "已禁用";
                lblStatus.ForeColor = Color.OrangeRed;
                statusIndicator.BackColor = Color.OrangeRed;
                btnToggle.Text = "启  用";
                btnToggle.BackColor = Color.FromArgb(60, 180, 60);
            }
        }

        private void ToggleAdapter(bool enable)
        {
            try
            {
                using (var s = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_NetworkAdapter WHERE Name = '" + adapterName.Replace("'", "''") + "'"))
                {
                    foreach (ManagementObject obj in s.Get())
                    {
                        var result = obj.InvokeMethod(enable ? "Enable" : "Disable", null);
                        if (result != null && (uint)result == 0) { isEnabled = enable; UpdateUI(); return; }
                    }
                }
                var p = new Process();
                p.StartInfo = new ProcessStartInfo
                {
                    FileName = "netsh",
                    Arguments = string.Format("interface set interface \"{0}\" admin={1}", adapterName, enable ? "enable" : "disable"),
                    Verb = "runas", UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                p.Start(); p.WaitForExit(2000);
                RefreshStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0}失败: {1}\n\n请以管理员身份运行。", enable ? "启用" : "禁用", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisableAdapter() { ToggleAdapter(false); }
        private void EnableAdapter() { ToggleAdapter(true); }
    }
}
