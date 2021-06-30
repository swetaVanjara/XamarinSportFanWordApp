using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.CustomNotification
{
    class Notifications
    {
        public static Notifications Instance = new Notifications();

        public NotificationHubClient Hub { get; set; }

        //bool enableTestSend = true;

        private Notifications()
        {
            //Hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://fanwordbeta.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=Z030s1TggFodcapvdeWN6P1MzZqigTYz70FgcvjXXj0=",
            //                                                           "fanwordBeta", enableTestSend);
            Hub = NotificationHubClient.CreateClientFromConnectionString(ConfigurationManager.AppSettings["AzureHubConnection"],
                                                                         ConfigurationManager.AppSettings["HubName"]);
        }
    }
}
