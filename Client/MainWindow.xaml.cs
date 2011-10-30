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
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CLIENT
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        int isclk = 0;
        private void busend_Click(object sender, RoutedEventArgs e)
        {
            if (isclk == 1)
                return;
            int ass=int.Parse(tbport.Text);
            string ipaa = txip.Text;
            


                TcpClient clz = new TcpClient();
                IPAddress ipa = IPAddress.Parse(ipaa);
                clz.Connect(ipa, ass);
                TextRange textRange = new TextRange(rtbmsg.Document.ContentStart, rtbmsg.Document.ContentEnd);
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(textRange.Text);
                NetworkStream stream = clz.GetStream();
                stream.Write(msg, 0, msg.Length);
                clz.Close();
           
           // isclk = 1;
        }
    }
}


