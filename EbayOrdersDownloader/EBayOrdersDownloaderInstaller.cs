using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace EbayOrdersDownloader
{
    [RunInstaller(true)]
    public partial class EBayOrdersDownloaderInstaller : System.Configuration.Install.Installer
    {
        public EBayOrdersDownloaderInstaller()
        {
            InitializeComponent();


            var processInstaller = new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.DisplayName = EBayOrdersDownloaderService.ServicePrettyName;
            serviceInstaller.ServiceName = EBayOrdersDownloaderService.ServicePrettyName;           
            serviceInstaller.StartType = ServiceStartMode.Manual;
            serviceInstaller.Description = "Download orders from eBay.";
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
