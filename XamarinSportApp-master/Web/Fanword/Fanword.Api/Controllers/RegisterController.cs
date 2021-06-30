using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;
using Fanword.Api.Models;
using System.Threading.Tasks;
using System.Web;




namespace Fanword.Api.Controllers
{
    [RoutePrefix("api/Register")]
    public class RegisterController : ApiController
    {
        private NotificationHubClient hub;

        

        public RegisterController()
        {
            hub = Models.Notifications.Instance.Hub;
        }


        public class DeviceRegistration
        {
            public string Platform { get; set; }
            public string Handle { get; set; }
            public string[] Tags { get; set; }
        }



        [HttpPost, Route("getRegistrationId")]
        public async Task<string> Post(string handle = null)
        {
            string newRegistrationId = null;

            // make sure there are no existing registrations for this push handle (used for iOS and Android)
            if (handle != null)
            {
                var registrations = await hub.GetRegistrationsByChannelAsync(handle, 100);

                foreach (RegistrationDescription registration in registrations)
                {
                    if (newRegistrationId == null)
                    {
                        newRegistrationId = registration.RegistrationId;
                    }
                    else
                    {
                        await hub.DeleteRegistrationAsync(registration);
                    }
                }
            }

            if (newRegistrationId == null)
                newRegistrationId = await hub.CreateRegistrationIdAsync();

            return newRegistrationId;
        }
    }

  
}
