using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Fanword.Business.Builders.NotificationHandler;
using Microsoft.Owin;
using Notifications.Data.Configuration;
using Notifications.Data.Events;
using Notifications.Extensions;
using Owin;
using PushNotifications.Azure.Configuration;
using PushNotifications.Extensions;

[assembly: OwinStartup(typeof(Fanword.Api.Startup))]

namespace Fanword.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            var notificationConfig = new NotificationConfiguration(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString).RegisterNotifications(new Notification()
            {
                NotificationTriggered = (sender, model) =>
                {
                    new NotificationHandlerBuilder().NotificationTriggered(model);

                },
                NotifySpecificUsers = notification =>
                {
                    return new NotificationHandlerBuilder().NotifySpecificUsers(notification);
                },
                AddAdditionalUserNotificationData = notification =>
                {
                    return new NotificationHandlerBuilder().AddAdditionalUserNotificationData(notification);
                },
            });
            app.UseNotificationService(notificationConfig);


            //Azure Push
            var pushConfig = new PushNotifications.Configuration.PushNotificationServiceConfiguration()
                .UseAzurePushNotifications(
                    ConfigurationManager.ConnectionStrings["AzureHubConnection"].ConnectionString,
                    ConfigurationManager.AppSettings["HubName"]);
            app.UsePushNotificationService(pushConfig);
        }
    }
}
