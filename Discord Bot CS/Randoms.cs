using System.Net.Http;
using System.Collections;
using Newtonsoft.Json.Linq;
using System;
using Discord;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord_Bot_CS
{
	public class Randoms
	{

		private static Dictionary<string, JObject> imageArchives = new Dictionary<string, JObject>();
		private static Random random = new Random();
		public static string Kanye(HttpClient client)
		{
			var req = JObject.Parse(client.GetStringAsync("https://api.kanye.rest").Result);
			return (string) req["quote"];
		}

		public static async System.Threading.Tasks.Task<string> ImageAsync(HttpClient client, string tag, string key)
        {
			var req = new JObject();
			if(imageArchives.ContainsKey(tag) && DateTime.Now.Subtract((DateTime)imageArchives[tag]["datetime"]).TotalMinutes < 60)
            {
				req = imageArchives[tag];
            }
			else
            {
				Dictionary<string, string> auth = new Dictionary<string, string>();
				auth.Add("Authorization", "Client-ID " + key);
				var headers = JsonConvert.SerializeObject(auth);
				var url = "https://api.imgur.com/3/gallery/t/" + tag;
				HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, url);
				message.Headers.Add("Authorization", "Client-ID " + key);
				var response = await client.SendAsync(message).Result.Content.ReadAsStringAsync();
				Console.WriteLine("begin parsing");
				req = JObject.Parse(response);
				Console.WriteLine("finished parsing");
				req.Add("datetime", DateTime.Now);
				imageArchives.Add(tag, req);
            }
			//while (true)
			{
				try
				{
					Console.WriteLine("attempting");
					Console.WriteLine(req["data"]["items"][random.Next(0, 39)]);
					return (string)req["data"]["items"][random.Next(0, 39)]["images"][0]["link"];
				}
				catch (NullReferenceException)
				{
					Console.WriteLine("failure");
					return "Failed";
				}
			}
        }
	}
}