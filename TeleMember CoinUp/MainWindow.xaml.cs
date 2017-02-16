#region Using

using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Threading;
using RestSharp;

#endregion

namespace TeleMember_CoinUp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _closed;
        private int _error;
        private int coin;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            coin = 0;
            _closed = false;
            _error = 0;
            GiveFeedbac:
            if (!_closed & coin < int.Parse(txtCoin.Text))
            {
                DoEvents();
                var tgId = txtID.Text;
                string channelId;

                var request =
    (HttpWebRequest)WebRequest.Create("http://tm.teletops.ir/Service1.svc/GetChannels");
                request.CookieContainer = new CookieContainer();
                request.ContentType = "application/json; charset=utf-8";
                request.Method = "POST";
                request.KeepAlive = true;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.Headers.Add("sign", (int.Parse(a(tgId + ";0"))).ToString());
                request.UserAgent = "Dalvik/1.6.0 (Linux; U; Android 4.4.2; NoxW Build/KOT49H)";

                var sp1 = request.ServicePoint;
                var prop1 = sp1.GetType()
                    .GetProperty("HttpBehaviour", BindingFlags.Instance | BindingFlags.NonPublic);
                prop1.SetValue(sp1, (byte)0, null);

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = "{\"categoryId\":\"0\",\"tgId\":\"" +
                                    tgId + "\"}";
                    streamWriter.Write(json);
                }
                var response = (HttpWebResponse)request.GetResponse();

                var result1 = new StreamReader(response.GetResponseStream()).ReadToEnd();

                MatchCollection channelIds;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    try
                    {
                        channelIds = Regex.Matches(result1, "\"tgChannelId\":(.*?),");
                    }
                    catch (Exception)
                    {
                        goto GiveFeedbac;
                    }
                }
                else
                {
                    _error++;
                    goto GiveFeedbac;
                }
                foreach (Match id in channelIds)
                {
                    if (!_closed & coin < int.Parse(txtCoin.Text))
                    {
                        DoEvents();
                        channelId = id.Groups[1].Value;
                        int h = 0;
                        char[] paramString = (tgId + ";" + channelId + ";" + tgId + ";" + channelId).ToCharArray();
                        int k = paramString.Length;
                        int l = 0;
                        while (h < k)
                        {
                            l += paramString[h];
                            h += 1;
                        }

                        var request2 =
                            (HttpWebRequest)WebRequest.Create("http://tm.teletops.ir/Service1.svc/JoinChannel4");
                        request2.CookieContainer = new CookieContainer();
                        request2.ContentType = "application/json; charset=utf-8";
                        request2.Method = "POST";
                        request2.KeepAlive = true;
                        request2.Headers.Add("Accept-Encoding", "gzip");
                        request2.Headers.Add("sign", Convert.ToString(l));
                        request2.UserAgent = "Dalvik/1.6.0 (Linux; U; Android 4.4.2; NoxW Build/KOT49H)";

                        var sp = request2.ServicePoint;
                        var prop = sp.GetType()
                            .GetProperty("HttpBehaviour", BindingFlags.Instance | BindingFlags.NonPublic);
                        prop.SetValue(sp, (byte)0, null);

                        var hashed = (tgId + channelId).ToCharArray();
                        int n;
                        var j = 0;
                        int i;
                        for (i = 0; j < hashed.Length; i = n + i)
                        {
                            n = hashed[j];
                            j++;
                        }

                        using (var streamWriter = new StreamWriter(request2.GetRequestStream()))
                        {
                            string json12 = "{\"hash\":\"" + Convert.ToString(i) + "\",\"channelId\":\"" + channelId +
                                            "\",\"tgId\":\"" +
                                            tgId + "\"}";
                            streamWriter.Write(json12);
                        }

                        var response2 = (HttpWebResponse)request2.GetResponse();

                        var result = new StreamReader(response2.GetResponseStream()).ReadToEnd();
                        if (response2.StatusCode == HttpStatusCode.OK)
                        {
                            if (result.Contains("OK"))
                            {
                                coin++;
                            }
                            else
                            {
                                _error++;
                            }
                        }
                        else
                        {
                            _error++;
                        }
                        lblCoin.Content = "Coin : " + coin;
                        lblError.Content = "Error : " + _error;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            if (!_closed & coin < int.Parse(txtCoin.Text))
            {
                goto GiveFeedbac;
            } 
        }
        public static String a(String paramString)
        {
            int i = 0;
            var paramString1 = paramString.ToCharArray();
            int k = paramString1.Length;
            int j = 0;
            while (i < k)
            {
                j += paramString1[i];
                i += 1;
            }
            return Convert.ToString(j);
        }
        public void DoEvents()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }

        public object ExitFrame(object f)
        {
            ((DispatcherFrame) f).Continue = false;

            return null;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _closed = true;
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _closed = true;
            Application.Current.Shutdown();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            new About().Show();
        }

        public class RequestObject
        {
            public string tgId { get; set; }
            public string hash { get; set; }
            public string channelId { get; set; }
        }

        public class RequestObject1
        {
            public string categoryId { get; set; }
            public string tgId { get; set; }
        }
    }
}