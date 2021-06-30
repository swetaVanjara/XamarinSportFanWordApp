using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ModernHttpClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notifications.Mobile.Configuration;
using Plugin.Connectivity;
using Plugin.Settings;
using Polly;

namespace Fanword.Shared.Service
{
    public class ServiceApiBase
    {
#if DEBUG
        //public const string HubName = "fanworddev";
        //public const string AzureConnectionString = "Endpoint=sb://agilxsbnew.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=algUxmIxlNZPUx6cd+P0jfYl1YDyAnjxdmVPcOF5BKo=";

        public const string SenderID = "440868453679"; // Google API Project Number

        // Azure app specific connection string and hub path
        public const string AzureConnectionString = "Endpoint=sb://fanwordbeta.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=Z030s1TggFodcapvdeWN6P1MzZqigTYz70FgcvjXXj0=";
        public const string HubName = "fanwordBeta";

        //public static string PortalURL = "http://192.168.5.236:12138/";
        //public static string PortalURL = "https://fanwordapidev.azurewebsites.net";
        //public static string MvcPortalURL = "https://fanword-dev.agilx.com";

        //public static string PortalURL = "http://192.168.0.126:12145";

        public static string PortalURL = "https://fanword-api-beta.azurewebsites.net"; 
        public static string MvcPortalURL = "https://fanwordportalbeta.azurewebsites.net";

        //public static string PortalURL = "https://fanwordapibeta.azurewebsites.net";
        //public static string MvcPortalURL = "https://fanword-beta.agilx.com";

#elif HOCKEYAPP
        public const string HubName = "fanwordbeta";
        public const string AzureConnectionString = "Endpoint=sb://agilxsbnew.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=2vTEyPwl21cHGFFYlaz/RpV8Ux+s7WL8U4mSf4UQgSc=";

        public const string SenderID = "440868453679";

        public static string PortalURL = "https://fanword-api-beta.azurewebsites.net/";
        public static string MvcPortalURL = "https://fanwordportalbeta.azurewebsites.net/";

#else
        //public static string PortalURL = "https://fanwordapiprod.azurewebsites.net";
        //public static string MvcPortalURL = "https://portal.fanword.com";

        public static string PortalURL = "https://fanword-api-beta.azurewebsites.net"; 
        public static string MvcPortalURL = "https://fanwordportalbeta.azurewebsites.net";

        public const string HubName = "fanwordBeta";
        public const string AzureConnectionString = "Endpoint=sb://fanwordbeta.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=Z030s1TggFodcapvdeWN6P1MzZqigTYz70FgcvjXXj0=";

        public const string SenderID = "440868453679"; // Google API Project Number

        //public const string HubName = "fanwordprod";
        //public const string AzureConnectionString = "Endpoint=sb://fanword.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=UM+iWUpoD3WN9SlrbdCBkdvV6PPmXW1Iw/8ZUNamuCc=";

#endif

        protected HttpClient _Client;

        public ServiceApiBase()
        {
            //_apiService = apiService;
            _Client = new HttpClient(new NativeMessageHandler())
            {
                BaseAddress = new Uri(PortalURL)
            };

            NotificationsConfiguration.ApiBaseUri = new Uri(PortalURL);
        }

        protected async Task<T> Get<T>(string route, Dictionary<string, object> values)
        {
            var accessToken = CrossSettings.Current.GetValueOrDefault("AccessToken", "");
            if (CrossConnectivity.Current.IsConnected)
            {
                HttpClient c = new HttpClient(new NativeMessageHandler()) { BaseAddress = new Uri(PortalURL) };
                string queryString = "?";
                foreach (var item in values)
                {
                    if (queryString != "?")
                    {
                        queryString += "&";
                    }
                    queryString += item.Key + "=" + WebUtility.UrlEncode(item.Value.ToString());
                }
                if (!string.IsNullOrEmpty(accessToken))
                {
                    c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }
                var t = c.GetAsync(PortalURL + route + queryString);
                var message = await Policy
                .Handle<WebException>().Or<IOException>()
                .WaitAndRetryAsync(retryCount: 2, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(async () => await t);

                string content = await message.Content.ReadAsStringAsync();
                if (!message.IsSuccessStatusCode)
                {
                    bool status = IsJson(content);

                    if (status)
                    {
                        var error = JsonConvert.DeserializeObject<JObject>(content);

                        try
                        {
                            var err = error["Message"].ToString();
                            throw new WebException(err);
                        }
                        catch (NullReferenceException ex)
                        { }
                    }
                }
                return JsonConvert.DeserializeObject<T>(content);

            }
            else
            {
                throw new Exception("No internet connection.");
            }
        }

        protected async Task Get(string route, Dictionary<string, object> values)
        {
            var accessToken = CrossSettings.Current.GetValueOrDefault("AccessToken", "");
            if (CrossConnectivity.Current.IsConnected)
            {
                HttpClient c = new HttpClient(new NativeMessageHandler()) { BaseAddress = new Uri(PortalURL) };
                string queryString = "?";
                foreach (var item in values)
                {
                    if (queryString != "?")
                    {
                        queryString += "&";
                    }
                    queryString += item.Key + "=" + WebUtility.UrlEncode(item.Value.ToString());
                }
                if (!string.IsNullOrEmpty(accessToken))
                {
                    c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }
                var t = c.GetAsync(PortalURL + route + queryString);
                var message = await Policy
                .Handle<WebException>().Or<IOException>()
                .WaitAndRetryAsync(retryCount: 2, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(async () => await t);


                string content = await message.Content.ReadAsStringAsync();
                if (!message.IsSuccessStatusCode)
                {
                    bool status = IsJson(content);

                    if (status)
                    {
                        var error = JsonConvert.DeserializeObject<JObject>(content);

                        try
                        {
                            var err = error["Message"].ToString();
                            throw new WebException(err);
                        }
                        catch (NullReferenceException ex)
                        { }
                    }
                }
            }
            else
            {
                throw new Exception("No internet connection.");
            }
        }

        protected async Task Delete(string route, Dictionary<string, object> values)
        {
            var accessToken = CrossSettings.Current.GetValueOrDefault("AccessToken", "");
            if (CrossConnectivity.Current.IsConnected)
            {
                HttpClient c = new HttpClient(new NativeMessageHandler()) { BaseAddress = new Uri(PortalURL) };
                string queryString = "?";
                foreach (var item in values)
                {
                    if (queryString != "?")
                    {
                        queryString += "&";
                    }
                    queryString += item.Key + "=" + WebUtility.UrlEncode(item.Value.ToString());
                }
                if (!string.IsNullOrEmpty(accessToken))
                {
                    c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }
                var t = c.DeleteAsync(PortalURL + route + queryString);
                var message = await Policy
                .Handle<WebException>().Or<IOException>()
                .WaitAndRetryAsync(retryCount: 2, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(async () => await t);

                string content = await message.Content.ReadAsStringAsync();
                if (!message.IsSuccessStatusCode)
                {
                    bool status = IsJson(content);

                    if (status)
                    {
                        var error = JsonConvert.DeserializeObject<JObject>(content);

                        try
                        {
                            var err = error["Message"].ToString();
                            throw new WebException(err);
                        }
                        catch (NullReferenceException ex)
                        { }

                        throw new Exception(message.ReasonPhrase);
                    }
                }
            }
            else
            {
                throw new Exception("No internet connection.");
            }
        }

        protected async Task Post(string route, object obj)
        {
            var accessToken = CrossSettings.Current.GetValueOrDefault("AccessToken", "");

            if (CrossConnectivity.Current.IsConnected)
            {
                HttpClient c = new HttpClient(new NativeMessageHandler()) { BaseAddress = new Uri(PortalURL) };
                c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var t = c.PostAsJsonAsync(PortalURL + route, obj);
                var message = await Policy
                .Handle<WebException>().Or<IOException>()
                .WaitAndRetryAsync(retryCount: 2, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(async () => await t);

                string content = await message.Content.ReadAsStringAsync();
                if (!message.IsSuccessStatusCode)
                {
                    bool status = IsJson(content);

                    if (status)
                    {
                        var error = JsonConvert.DeserializeObject<JObject>(content);

                        try
                        {
                            var err = error["Message"].ToString();
                            throw new WebException(err);
                        }
                        catch (NullReferenceException ex)
                        { }
                    }
                }
            }
            else
            {
                throw new Exception("No internet connection.");
            }
        }

        protected async Task<T> Post<T>(string route, object obj)
        {
            var accessToken = CrossSettings.Current.GetValueOrDefault("AccessToken", "");

            if (CrossConnectivity.Current.IsConnected)
            {
                HttpClient c = new HttpClient(new NativeMessageHandler()) { BaseAddress = new Uri(PortalURL) };
                c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var t = c.PostAsJsonAsync(PortalURL + route, obj);
                var message = await Policy
                .Handle<WebException>().Or<IOException>()
                .WaitAndRetryAsync(retryCount: 2, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(async () => await t);

                string content = await message.Content.ReadAsStringAsync();
                if (!message.IsSuccessStatusCode)
                {
                    bool status = IsJson(content);

                    if (status)
                    {
                        var error = JsonConvert.DeserializeObject<JObject>(content);

                        try
                        {
                            var err = error["Message"].ToString();
                            throw new WebException(err);
                        }
                        catch (NullReferenceException ex)
                        { }
                    }
                }
                return JsonConvert.DeserializeObject<T>(content);

            }
            else
            {
                throw new Exception("No internet connection.");
            }
        }

        public static bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                   || input.StartsWith("[") && input.EndsWith("]");
        }

    }
}
