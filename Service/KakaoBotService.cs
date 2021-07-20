using EngineIOSharp.Common.Enum;
using KakaoManagerBeta.Models;
using KakaoManagerBeta.Util;
using SocketIOSharp.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace KakaoManagerBeta.Service
{
    public class Message
    {
        public string Sender { get; set; }
        public string Room { get; set; }
        public string Msg { get; set; }
        public bool IsGroupChat { get; set; }
    }

    public static class KakaoBotService
    {
        const string SERVER_URI = "***REMOVED***";
        const int SERVER_PORT = 9200;

        private static SocketIOClient client_;
        private static HttpClient httpClient_;

        public static void Run()
        {
            httpClient_ = new HttpClient();
            client_ = new SocketIOClient(new SocketIOClientOption(EngineIOScheme.http, SERVER_URI, SERVER_PORT));
            
            Console.WriteLine("start");

            var registerData = new Dictionary<string, string>()
            {
                ["password"] = "4321"
            };

            client_.On("connect", () =>
            {
                client_.Emit("register", registerData);
            });

            client_.On("receive message", (data) =>
            {
                Console.WriteLine(data[0].ToString());
                var message = data[0].ToObject<Message>();

                var domains = Globals.Domains;
                domains.AsParallel()
                    .ForAll(address =>
                    {
                        try
                        {
                            httpClient_.PostAsJsonAsync<Message>(address.ConvertBase64ToText(), message);
                        }
                        catch (Exception err)
                        {
                            Console.WriteLine(err);
                        }
                    });
            });

            client_.Connect();
        }

        public static void Close()
        {
            client_.Dispose();
            httpClient_.Dispose();
        }
    }
}
