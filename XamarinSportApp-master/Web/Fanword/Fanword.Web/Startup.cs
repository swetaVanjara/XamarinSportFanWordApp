using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using EmailService.Extensions;
using Fanword.Business.Builders.NotificationHandler;
using Fanword.Business.Hangfire.RssFeeds;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Owin;
using Notifications.Data.Configuration;
using Notifications.Data.Enum;
using Notifications.Data.Events;
using Notifications.Extensions;
using Owin;
using PushNotifications.Azure.Configuration;
using PushNotifications.Extensions;

[assembly: OwinStartupAttribute(typeof(Fanword.Web.Startup))]
namespace Fanword.Web {

    public class MyRestrictiveAuthorizationFilter : IAuthorizationFilter, IDashboardAuthorizationFilter {
        //comment
        public bool Authorize(IDictionary<string, object> owinEnvironment) {
            // In case you need an OWIN context, use the next line,
            // `OwinContext` class is the part of the `Microsoft.Owin` package.
             var context = new OwinContext(owinEnvironment);

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            return context.Authentication.User.Identity.IsAuthenticated;


        }

        public bool Authorize(DashboardContext context) {
            return true;
        }
    }

    public enum NotificatinoT
    {
        Type1,
        Type2,
    }

    public partial class Startup {
        public void Configuration(IAppBuilder app) {
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

            //email service
            var config = new EmailService.Data.Configurations.EmailServiceConfiguration(
                ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                ConfigurationManager.AppSettings["SendGridApiToken"],
                ConfigurationManager.AppSettings["FromEmail"], "https://fanword.blob.core.windows.net/appimages/emailHeader.png", "Fanword", "2815 Fletcher Avenue #38 Lincoln, NE 68504", "");

            app.UseEmailService(config);


            //Azure Push

            var pushConfig = new PushNotifications.Configuration.PushNotificationServiceConfiguration()
                .UseAzurePushNotifications(
                    ConfigurationManager.ConnectionStrings["AzureHubConnection"].ConnectionString,
                    ConfigurationManager.AppSettings["HubName"]);
            app.UsePushNotificationService(pushConfig);

            var useServer = false;

            bool.TryParse(ConfigurationManager.AppSettings["UseHangfireServer"], out useServer);
            if (useServer) {
                GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");
                
                app.UseHangfireServer(new BackgroundJobServerOptions() {
                    Queues = new string[] { "default", "rss_feed", "push_notifications" }
                });

                app.UseHangfireDashboard("/Hangfire",new DashboardOptions() {
                    Authorization =  Enumerable.Empty<IDashboardAuthorizationFilter>(),
                    AuthorizationFilters = Enumerable.Empty<IAuthorizationFilter>()
                });
                RecurringJob.AddOrUpdate("Rss Feed Sync", () => new RssFeedWorker().StartSyncAll(), Cron.MinuteInterval(5));
            }
        }
    }
}
