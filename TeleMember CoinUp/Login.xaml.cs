#region Using

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows;

#endregion

namespace TeleMember_CoinUp
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("tg://resolve?domain=CyberSoldiersST_bot");
            }
            catch (Exception)
            {
                Process.Start("http://telegram.me/CyberSoldiersST_bot");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtSystemCode.Text = Activation.GetID();

            if (IsConnectedToInternet())
            {
                if (Activation.ReadRegeditKey() == Activation.MyCustomHash(Activation.GetID(), Activation.GetHash()))
                {
                    try
                    {
                        var request = new StreamReader(
                            WebRequest.Create("http://rexprog-app.xzn.ir/APP/TeleMemberCoinUpCode")
                                .GetResponse()
                                .GetResponseStream()).ReadToEnd();
                        if (!request.Contains(Activation.ReadRegeditKey()) | request == "error:Hash Is Invalid")
                        {
                            MessageBox.Show("You Have Fake Activation Code");
                            Application.Current.Shutdown();
                            return;
                        }
                        else
                        {
                            new MainWindow().Show();
                            Hide();
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("مشکل در اتصال لطفا اینترنت خود را چک کنید");
                        Application.Current.Shutdown();
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("مشکل در اتصال لطفا اینترنت خود را چک کنید");
                Application.Current.Shutdown();
                return;
            }
        }
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int description, int reservedValue);

        private bool IsConnectedToInternet()
        {
            bool a;
            int desc;
            a = InternetGetConnectedState(out desc, 0);
            return a;
        }
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (IsConnectedToInternet())
            {
                if (txtActivationCode.Text == Activation.MyCustomHash(Activation.GetID(), Activation.GetHash()))
                {
                    try
                    {
                        var request = new StreamReader(
                            WebRequest.Create("http://rexprog-app.xzn.ir/APP/TeleMemberCoinUpCode")
                                .GetResponse()
                                .GetResponseStream()).ReadToEnd();
                        if (request.Contains(txtActivationCode.Text) & request != "error:Hash Is Invalid")
                        {
                            Activation.SetRegeditKeyValue(Activation.MyCustomHash(Activation.GetID(), Activation.GetHash()));
                            MessageBox.Show("Application Is Active");
                            new MainWindow().Show();
                            Hide();
                        }
                        else
                        {
                            MessageBox.Show("You Have Fake Activation Code");
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("مشکل در ارتباط با سرور");
                    }
                }
                else
                {
                    MessageBox.Show("Activation Code Is Invalid");
                }
            }
            else
            {
                MessageBox.Show("مشکل در اتصال لطفا اینترنت خود را چک کنید");
            }
}
    }
}