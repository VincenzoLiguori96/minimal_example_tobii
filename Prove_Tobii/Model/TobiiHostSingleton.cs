using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tobii.Interaction;
using Tobii.Interaction.Wpf;
namespace Prove_Tobii.Model
{
    public static class TobiiHost
    {
        private static Host _host = new Host();
        private static WpfInteractorAgent _wpfInteractorAgent;

        public static Host Host { get => _host; }
        public static WpfInteractorAgent WpfInteractorAgent { get => _wpfInteractorAgent; }

        public static void onExit()
        {
            Host.Dispose();           
        }

        private static void Context_ConnectionStateChanged(object sender, Tobii.Interaction.Client.ConnectionStateChangedEventArgs e)
        {
            Debug.WriteLine(e.State); 
            System.Windows.MessageBox.Show("CambioStato! Ora: "+e.State, "End",
                        MessageBoxButton.OK, MessageBoxImage.Information);

        }

        public static void onStart()
        {
            _wpfInteractorAgent = Host.InitializeWpfAgent();
        }
    }
}

