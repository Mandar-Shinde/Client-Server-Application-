using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO.Ports;
using Microsoft.Win32;
using System.Windows.Threading;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
//using System.Data.Objects;

namespace svr
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string myHost = System.Net.Dns.GetHostName();
            System.Net.IPHostEntry myIPs = System.Net.Dns.GetHostEntry(myHost);
            int i = 0;
            string s;
            foreach (System.Net.IPAddress myIP in myIPs.AddressList)
            {
                s = myIP.ToString();
                if (s.Length < 16)
                {
                    cobx_ip.Items.Add(s);
                    // ipsz[i] = s;
                    if (s != null)
                    {
                        cobx_ip.SelectedIndex = 0;                       
                    }
                    i++;
                }
            }
        }

        int ass;
        string ipaa ;
        object h;
        Thread gprs, pinger;


        int alter = 1;
        private void button1_Click(object sender, RoutedEventArgs e)
        {

            if (alter == 0)
                return;

            alter = 0;
            ipaa = (string)cobx_ip.SelectedItem;
            ass = int.Parse((textBox2.Text).ToString());

            gprs = new Thread(TCPwork);
            gprs.IsBackground = true;            
            gprs.Start();

            pinger = new Thread(thr_pinger);
            pinger.IsBackground = true;
            pinger.Start();
        }

        public void thr_pinger()
        {
            while (true)
            {
                Thread.Sleep(1900000);
                TcpClient clz = new TcpClient();

                IPAddress ipa = IPAddress.Parse(ipaa);
                
                clz.Connect(ipa, ass);

                byte[] msg = System.Text.Encoding.ASCII.GetBytes("`");

                NetworkStream stream = clz.GetStream();

                stream.Write(msg, 0, msg.Length);


                clz.Close();
            }

        }

        public void TCPwork(object state)
        {
           
            IPAddress ipa = IPAddress.Parse(ipaa);
            TcpListener tcpl = new TcpListener(ipa, ass);
            
            while (true)
            {
                int j = 0;
           
                tcpl.Start();               
                TcpClient clsa = tcpl.AcceptTcpClient();
                NetworkStream ns = clsa.GetStream();           

                Byte[] bbg = new Byte[200];
                char[] srt = new char[200];
              //  byte[] ping = { (byte)'M', (byte)'a', (byte)'n', (byte)'d', (byte)'a', (byte)'r' };//{77,97,110,100,97,114};

                ns.Read(bbg, 0, bbg.Length);
                //ns.Write(ping, 0, 6);

                for (int i = 0;  bbg[i] != 0; i++,j++)
                   srt[i] = (char)bbg[i];
                
                string aa = new string(srt).Substring(0, j);
               
                if(aa!="`")
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<string>(updat_monitor_ui), aa);
                
                tcpl.Stop();
            }
        }

        public void updat_monitor_ui(string s)
        {
            richTextBox1.AppendText(s.ToString());
            richTextBox1.AppendText("\n");
            richTextBox1.ScrollToEnd();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown(0);
            try
            {
                Environment.Exit(0);

            }catch(ThreadAbortException exz)
            {}
      
            Thread.Sleep(100);
        }
    }

   

}
