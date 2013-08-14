using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using DazCamUI.Controller;
using DazCamUI.Properties;

namespace DazCamUI.UI
{
    public partial class ScanForIp : Form
    {
        private readonly Machine _machine;

        public string FoundAddress = "0.0.0.0";

        private readonly IList<Thread> _threads;

        private const int MaxThreads = 25;

        public ScanForIp(Machine machine)
        {
            _machine = machine;
            _threads = new List<Thread>();
            InitializeComponent();
        }

        private void ScanForIP_Load(object sender, EventArgs e)
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    lbIP.Items.Add(TrimEndofIp(ip.ToString()) + "*");

            if (lbIP.Items.Count == 1)
                lbIP.SelectedIndex = 0;
            else if (lbIP.Items.Count == 0)
                MessageBox.Show(Resources.NoNetwork, Resources.FindMachineTitle, MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (lbIP.SelectedIndex != -1)
            {
                LaunchThreads(TrimEndofIp(lbIP.SelectedItem.ToString()));

                if (FoundAddress != "0.0.0.0")
                    this.Close();
                else
                    MessageBox.Show(Resources.NoMachine, Resources.FindMachineTitle, MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show(Resources.SelectAddress, Resources.FindMachineTitle, MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
            }

        }

        private string TrimEndofIp(string ip)
        {
            var pos = ip.LastIndexOf('.') + 1;
            if (pos >= 0)
                ip = ip.Substring(0, pos);
            return ip;
        }

        public void LaunchThreads(string ipBase)
        {
            int i = 0;

            while (FoundAddress == "0.0.0.0" && i < 256)
            {
                if (_threads.Count < MaxThreads)
                {
                    var thread = new Thread(() => ThreadEntry(i++, ipBase))
                    {
                        IsBackground = true,
                        Name = string.Format("MyThread{0}", i)
                    };
                    _threads.Add(thread);
                    thread.Start();
                }

                lock (_threads)
                {
                    var deadThreads = _threads.Where(t => !t.IsAlive).ToList();
                    foreach (var t in deadThreads)
                        _threads.Remove(t);
                }

            }

            lock (_threads)
            {
                while (_threads.Count > 0)
                {
                    _threads[0].Abort();
                    _threads.RemoveAt(0);
                }
            }
        }

        public void KillThread(int index)
        {
            var id = string.Format("MyThread{0}", index);
            var thread = _threads.FirstOrDefault(t => t.Name == id);
            if (thread != null)
                thread.Abort();
        }

        void ThreadEntry(int index, string ipBase)
        {
            var responseText = "";
            responseText = SocketConnection.SendCommand("Scan", ipBase + index, 3);
            Console.WriteLine(index);
            if (responseText.Contains("IP-"))
                FoundAddress = responseText.Substring(6);
            KillThread(index);
        }
    }
}
