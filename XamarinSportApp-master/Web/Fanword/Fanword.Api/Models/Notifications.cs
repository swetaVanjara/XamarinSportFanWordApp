using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.NotificationHubs;
using System.Configuration;

namespace Fanword.Api.Models
{
    public class Notifications
    {
        public static Notifications Instance = new Notifications();

        public NotificationHubClient Hub { get; set; }

        //bool enableTestSend = true;

        private Notifications()
        {
            //Hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://fanwordbeta.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=Z030s1TggFodcapvdeWN6P1MzZqigTYz70FgcvjXXj0=",
              //                                                           "fanwordBeta", enableTestSend);
            Hub = NotificationHubClient.CreateClientFromConnectionString(ConfigurationManager.ConnectionStrings["AzureHubConnection"].ConnectionString,
                                                                         ConfigurationManager.AppSettings["HubName"]);
        }

   
    }
}