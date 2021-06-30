using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fanword.Api.Models;
using System.Threading.Tasks;
using System.Web;

namespace Fanword.Api.Controllers
{
    [RoutePrefix("api/Notify")]
    public class NotifyController : ApiController
    {
        [HttpPost, Route("sendpushMessage")]
        public async Task<HttpResponseMessage> Post()
        {
            // var user = HttpContext.Current.User.Identity.Name;
            //string[] userTag = new string[2];
            //userTag[0] = "username:" + "440868453679";
            //userTag[1] = "from:" + "TestUser";
            // var msg = "hello";
            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;
            HttpStatusCode ret = HttpStatusCode.InternalServerError;

            // Android
            var notif = "{ \"data\" : {\"message\":\"" + "From backend api test notification" + "\"}}";
            outcome = await Models.Notifications.Instance.Hub.SendGcmNativeNotificationAsync(notif);
            //    outcome = await Models.Notifications.Instance.Hub.SendGcmNativeNotificationAsync("{ \"data\" : {\"msg\":\"" + msg + "\"}}");
            //   var notificationDetails = await Models.Notifications.Instance.Hub.GetNotificationOutcomeDetailsAsync(outcome.NotificationId);
            if (outcome != null)
            {
                if (!((outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Abandoned) ||
                    (outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Unknown)))
                {
                    ret = HttpStatusCode.OK;
                }
            }

            return Request.CreateResponse(ret);
        }

        [HttpPost, Route("sendpushMessage")]
        public async Task<HttpResponseMessage> PostUserNotification([FromUri]string pns, [FromUri] string userTag = null)
        {
            // var user = HttpContext.Current.User.Identity.Name;
            //string[] userTag = new string[2];
            //userTag[0] = "username:" + "440868453679";
            //userTag[1] = "from:" + "TestUser";
            // var msg = "hello";
            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;
            HttpStatusCode ret = HttpStatusCode.InternalServerError;

            // Android
            switch (pns.ToLower())
            {
                case "gcm":
                    var notif = "{ \"data\" : {\"message\":\"" + "From backend api test notification" + "\"}}";
                    if (!string.IsNullOrEmpty(userTag))
                    {
                        outcome = await Models.Notifications.Instance.Hub.SendGcmNativeNotificationAsync(notif, userTag);
                    }
                    else
                    {
                        outcome = await Models.Notifications.Instance.Hub.SendGcmNativeNotificationAsync(notif);
                    }
                    break;
                case "apns":
                    var alert = "{\"aps\":{\"alert\":\"" + "From backend api test notification" + "\"}}";
                    if (!string.IsNullOrEmpty(userTag))
                    {
                        outcome = await Models.Notifications.Instance.Hub.SendAppleNativeNotificationAsync(alert, userTag);
                    }
                    else
                    {
                        outcome = await Models.Notifications.Instance.Hub.SendAppleNativeNotificationAsync(alert);
                    }

                    break;
            }
            //    outcome = await Models.Notifications.Instance.Hub.SendGcmNativeNotificationAsync("{ \"data\" : {\"msg\":\"" + msg + "\"}}");
            //   var notificationDetails = await Models.Notifications.Instance.Hub.GetNotificationOutcomeDetailsAsync(outcome.NotificationId);
            if (outcome != null)
            {
                if (!((outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Abandoned) ||
                    (outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Unknown)))
                {
                    ret = HttpStatusCode.OK;
                }
            }

            return Request.CreateResponse(ret);
        }

    }
}
