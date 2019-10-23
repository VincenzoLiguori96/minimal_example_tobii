using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Prove_Tobii.Model;
using Tobii.Interaction;
using Tobii.Interaction.Wpf;

namespace Prove_Tobii
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            TobiiHost.onStart();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            TobiiHost.onExit();
            base.OnExit(e);
        }

        /*public Host Host
        {
            get { return _host; }
        }*/
    }
}
