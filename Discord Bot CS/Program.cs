using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;

namespace Discord_Bot_CS
{
    internal class Program
    {
        private static HttpClient client = new HttpClient();
        private DiscordSocketClient _client;
        private static void Main() => new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.MessageReceived += MessageReceived;
            var token = getKey("discord");
            Console.WriteLine(token);
            Console.ReadLine();
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task<Task> MessageReceived(SocketMessage msg)
        {
            if(!msg.ToString().StartsWith("!"))
            {
                return Task.CompletedTask;
            }
            var content = msg.ToString().Substring(1);
            var command = content.Split(" ");
            switch(command[0])
            {
                case "hello":
                    await msg.Channel.SendMessageAsync("Hello " + msg.Author.Username);
                    break;
                case "kanye":
                    await msg.Channel.SendMessageAsync(Randoms.Kanye(client));
                    break;
                case "dog":
                case "cat":
                case "lizard":
                case "giraffe":
                case "monkey":
                    await msg.Channel.SendMessageAsync(await Randoms.ImageAsync(client, command[0], getKey("imgur")));
                    break;
                default:
                    await msg.Channel.SendMessageAsync("Command not found");
                    break;
            }
            return Task.CompletedTask;
        }

        private string getKey(string platform)
        {
            using (StreamReader r = new StreamReader("keys.json"))
            {
                var json = JObject.Parse(r.ReadToEnd());
                return (string) json[platform];
            }
        }
    }
}