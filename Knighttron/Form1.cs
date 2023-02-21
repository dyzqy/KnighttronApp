using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Knighttron.DiscordRPC;
using System.Security.Cryptography;
using System.Threading;

namespace Knighttron
{
    public partial class Form1 : Form
    {
        public float scale = 1f;
        int swf_Width = 720;
        int swf_Height = 480;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) { }

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        public static Process p;

        private DiscordRpc.EventHandlers handlers;
        private DiscordRpc.RichPresence presence;

        void DiscordRPC()
        {
            string appid = "1077210514708516864";
            this.handlers = default(DiscordRpc.EventHandlers);
            DiscordRpc.Initialize(appid, ref this.handlers, true, null);
            this.handlers = default(DiscordRpc.EventHandlers);
            DiscordRpc.Initialize(appid, ref this.handlers, true, null);
            setuprpc();
            this.presence.startTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            DiscordRpc.UpdatePresence(ref this.presence);
        }

        void setuprpc()
        {
            this.presence.details = "Playing Knighttron v1.2.1";
            this.presence.largeImageKey = "icon";
        }

        void dfi()
        {
            foreach (var flashprocess in Process.GetProcessesByName("rgf"))
            {
                try
                {
                    flashprocess.Kill();
                }
                catch (Exception ex)
                {
                    ShowPopup("Something is preventing Knighttron from making necessary actions, try restarting your pc. \n Detailed Error Info:\n" + ex, "Knighttron - Error", MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                dfi();
                DiscordRPC();
                VerifyFile("rgf.exe", "0e9e4c81b7037b50acef88b9285d983bc096faf02bea5165187ba5c056cbcce0");
                VerifyFile("discord-rpc-w32.dll", "517240600b04d454cc5ab7b03e43c4af5a0b831fd2515f25c015a83652ad4cac");
                this.MaximizeBox = false;
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                Log.Text = "";
                Log.Text += "Starting game...";
                Log.Visible = true;
                this.Cursor = Cursors.Hand;
                string serverurl = "https://github.com/dyzqy/KnighttronApp/blob/main/Games/knighttron-fix.swf?raw=true";
                string gd = serverurl + "";
                string pd = "rgf.exe";
                Log.Visible = false;
                ProcessStartInfo si = new ProcessStartInfo(pd);
                si.WindowStyle = ProcessWindowStyle.Minimized;
                si.Arguments = gd;
                p = Process.Start(si);
                p.WaitForInputIdle();
                SetParent(p.MainWindowHandle, panel1.Handle);
                MoveWindow(p.MainWindowHandle, -8, -50, swf_Width + 15, swf_Height + 40, true);
                p.ToString();
            }
            catch (Exception chkex)
            {
                ShowPopup("Something is preventing Knighttron from making necessary actions, try restarting your pc. If the issue persists, contact me on discord: dyzqy#8466", "Knighttron - Error", MessageBoxIcon.Error);
                dfi();
                Environment.Exit(0);
            }
            while (true)
            {
                if (p.HasExited)
                {
                    Environment.Exit(0);
                }
                try
                {
                    MoveWindow(p.MainWindowHandle, -8, -50, swf_Width + 15, swf_Height + 40, true);
                }
                catch (Exception me)
                {
                    dfi();
                    Environment.Exit(0);
                }
                await Wait(1);
            }
        }

        public static void ShowPopup(string message, string title, MessageBoxIcon icon)
        {
            MessageBox.Show(
                message,
                title,
                MessageBoxButtons.OK,
                icon
            );
        }

        public async Task Wait(int milsec)
        {
            await Task.Delay(milsec);
            this.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        private void Log_Click(object sender, EventArgs e)
        {

        }

        void VerifyFile(string path, string hash)
        {
            if (File.Exists(path))
            {
                if (ComputeHash(path) != hash)
                {
                    ShowPopup("Vaffanculo skid11!!!!1!!111!!!!1!11!!!\nYour parents don't love you btw! :)", "Knighttron - No Skids Allowed!!!!", MessageBoxIcon.Error);
                    dfi();
                    Environment.Exit(0);
                }
            }
            else
            {
                ShowPopup($"Failed to locate \"{path}\", please ensure it's next to this exe!", "Knighttron - Error", MessageBoxIcon.Error);
                dfi();
                Environment.Exit(0);
            }
        }

        string ComputeHash(string path)
        {
            return BitConverter.ToString(SHA256.Create().ComputeHash(File.OpenRead(path))).Replace("-", "").ToLowerInvariant();
        }

        void SpawnSkidMsg()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
