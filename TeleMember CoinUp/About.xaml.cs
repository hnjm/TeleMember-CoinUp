#region Using

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

#endregion

namespace TeleMember_CoinUp
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        private void image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Process.Start("tg://resolve?domain=CyberSoldiersST");
            }
            catch (Exception)
            {
                Process.Start("http://telegram.me/CyberSoldiersST");
            }
        }

        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Process.Start("tg://resolve?domain=RexProg");
            }
            catch (Exception)
            {
                Process.Start("http://telegram.me/RexProg");
            }
        }
    }
}