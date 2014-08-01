using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.Diagnostics;

namespace pblib.NET
{
	public class Playbasis
	{
		private static readonly string BASE_URL = "https://api.pbapp.net/";
		private string token;
		private string apiKeyParam;

		public bool auth(string apiKey, string apiSecret)
		{
			apiKeyParam = "?api_key=" + apiKey;
			var param = "api_key=" + apiKey + "&api_secret=" + apiSecret;
			dynamic result = JsonToDynamic(call("Auth", param));
			if ((bool)result.success)
			{
				token = result.response.token;
				Debug.Assert(!string.IsNullOrEmpty(token));
				return true;
			}
			return false;
		}

        public bool renew(string apiKey, string apiSecret)
        {
            apiKeyParam = "?api_key=" + apiKey;
            var param = "api_key=" + apiKey + "&api_secret=" + apiSecret;
            dynamic result = JsonToDynamic(call("Auth/renew", param));
            if ((bool)result.success)
            {
                token = result.response.token;
                Debug.Assert(!string.IsNullOrEmpty(token));
                return true;
            }
            return false;
        }



		public string player(string playerId)
		{
			return call("Player/" + playerId, "token=" + token);
		}

		/// <summary>
		/// Get detail of list player
		/// </summary>
		/// <param name="playerListId">player id as used in client's website separate with ',' example '1,2,3'</param>
		/// <returns></returns>
		public string playerList(string playerListId)
		{
			return call("Player/list", "token=" + token + "&list_player_id=" + playerListId);
		}
		
		/// <summary>
		/// Get detailed information about a player, including points and badges
		/// </summary>
		/// <param name="playerId"></param>
		/// <returns></returns>
		public string playerDetail(string playerId)
		{
			return call("Player/" + playerId + "/data/all", "token=" + token);
		}

		/// <summary>
		/// Register a new user
		/// </summary>
		/// <param name="playerId"></param>
		/// <param name="username"></param>
		/// <param name="email"></param>
		/// <param name="imageUrl"></param>
		/// <param name="optionalData"> Varargs of String for additional parameters to be sent to the register method.
		/// Each element is a string in the format of key=value, for example: first_name=john
		/// The following keys are supported:
		/// - facebook_id
		/// - twitter_id
		/// - password		assumed hashed
		/// - first_name
		/// - last_name
		/// - nickname
		/// - gender		1=Male, 2=Female
		/// - birth_date	format YYYY-MM-DD</param>
		/// <returns></returns>
		public string register(string playerId, string username, string email, string imageUrl, params string[] optionalData)
		{
			var param = new StringBuilder();
			param.Append("token=");
			param.Append(token);
			param.Append("&username=");
			param.Append(username);
			param.Append("&email=");
			param.Append(email);
			param.Append("&image=");
			param.Append(imageUrl);

			for (int i = 0; i < optionalData.Length; ++i)
				param.Append("&" + optionalData[i]);

			return call("Player/" + playerId + "/register", param.ToString());
		}
		public void register_async(string playerId, string username, string email, string imageUrl, UploadStringCompletedEventHandler onComplete = null, params string[] optionalData)
		{
			var param = new StringBuilder();
			param.Append("token=");
			param.Append(token);
			param.Append("&username=");
			param.Append(username);
			param.Append("&email=");
			param.Append(email);
			param.Append("&image=");
			param.Append(imageUrl);

			for (int i = 0; i < optionalData.Length; ++i)
				param.Append("&" + optionalData[i]);

			call_async("Player/" + playerId + "/register", param.ToString(), onComplete);
		}

		/// <summary>
		/// Update user data
		/// </summary>
		/// <param name="playerId"></param>
		/// <param name="updateData"> Varargs of String data to be updated.
		/// Each element is a string in the format of key=value, for example: first_name=john
		/// The following keys are supported:
		/// - username
		/// - email
		/// - image
		/// - exp
		/// - level
		/// - facebook_id
		/// - twitter_id
		/// - password		assumed hashed
		/// - first_name
		/// - last_name
		/// - nickname
		/// - gender		1=Male, 2=Female
		/// - birth_date	format YYYY-MM-DD</param>
		/// <returns></returns>
		public string update(string playerId, params string[] updateData)
		{
			var param = new StringBuilder();
			param.Append("token=");
			param.Append(token);

			for (int i = 0; i < updateData.Length; ++i)
				param.Append("&" + updateData[i]);

			return call("Player/" + playerId + "/update", param.ToString());
		}
		public void update_async(string playerId, UploadStringCompletedEventHandler onComplete = null, params string[] updateData)
		{
			var param = new StringBuilder();
			param.Append("token=");
			param.Append(token);

			for (int i = 0; i < updateData.Length; ++i)
				param.Append("&" + updateData[i]);

			call_async("Player/" + playerId + "/update", param.ToString(), onComplete);
		}

		public string delete(string playerId)
		{
			return call("Player/" + playerId + "/delete", "token=" + token);
		}
		public void delete_async(string playerId, UploadStringCompletedEventHandler onComplete = null)
		{
			call_async("Player/" + playerId + "/delete", "token=" + token, onComplete);
		}

		public string login(string playerId)
		{
			return call("Player/" + playerId + "/login", "token=" + token);
		}
		public void login_async(string playerId, UploadStringCompletedEventHandler onComplete = null)
		{
			call_async("Player/" + playerId + "/login", "token=" + token, onComplete);
		}

		public string logout(string playerId)
		{
			return call("Player/" + playerId + "/logout", "token=" + token);
		}
		public void logout_async(string playerId, UploadStringCompletedEventHandler onComplete = null)
		{
			call_async("Player/" + playerId + "/logout", "token=" + token, onComplete);
		}

		public string points(string playerId)
		{
			return call("Player/" + playerId + "/points" + apiKeyParam);
		}

		public string point(string playerId, string pointName)
		{
			return call("Player/" + playerId + "/point/" + pointName + apiKeyParam);
		}

        public string pointHistory(string playerId, string pointName=null, int offset=0, int limit=20)
        {
            string stringQuery = "&offset=" + offset + "&limit=" + limit;
            if(pointName != null){
                stringQuery=stringQuery+"&point_name"+pointName;
            }
            return call("Player/" + playerId + "/point_history" + apiKeyParam + stringQuery);
        }

		public string actionLastPerformed(string playerId)
		{
			return call("Player/" + playerId + "/action/time" + apiKeyParam);
		}

		public string actionLastPerformedTime(string playerId, string actionName)
		{
			return call("Player/" + playerId + "/action/" + actionName + "/time" + apiKeyParam);
		}

		public string actionPerformedCount(string playerId, string actionName)
		{
			return call("Player/" + playerId + "/action/" + actionName + "/count" + apiKeyParam);
		}

		public string badgeOwned(string playerId)
		{
			return call("Player/" + playerId + "/badge" + apiKeyParam);
		}

		public string rank(string rankedBy, int limit)
		{
			return call("Player/rank/" + rankedBy + "/" + limit.ToString() + apiKeyParam);
		}

		public string ranks(int limit)
		{
			return call("Player/ranks/" + limit.ToString() + apiKeyParam);
		}

        public string level(int level)
        {
            return call("Player/level/" + level.ToString() + apiKeyParam);
        }

        public string levels()
        {
            return call("Player/levels" + apiKeyParam);
        }

        public string claimBadge(string playerId, string badgeId)
        {
            return call("Player/" + playerId + "/badge/" + badgeId + "/claim", "token=" + token);
        }

        public string redeemBadge(string playerId, string badgeId)
        {
            return call("Player/" + playerId + "/badge/" + badgeId + "/redeem", "token=" + token);
        }

        public string goodsOwned(string playerId)
        {
            return call("Player/" + playerId + "/goods" + apiKeyParam);
        }

        public string questOfPlayer(string playerId, string questId)
        {
            return call("Player/quest/" + questId + apiKeyParam + "&player_id=" + playerId);
        }

        public string questListOfPlayer(string playerId)
        {
            return call("Player/quest" + apiKeyParam + "&player_id=" + playerId);
        }

		public string badges()
		{
			return call("Badge" + apiKeyParam);
		}

		public string badge(string badgeId)
		{
			return call("Badge/" + badgeId + apiKeyParam);
		}

        public string goods(string goodsId)
        {
            return call("Goods/" + goodsId + apiKeyParam);
        }

        public string goodsList()
        {
            return call("Goods" + apiKeyParam);
        }

		public string actionConfig()
		{
			return call("Engine/actionConfig" + apiKeyParam);
		}

		/// <summary>
		/// Trigger an action and process related rules for the specified user
		/// </summary>
		/// <param name="playerId"></param>
		/// <param name="action"></param>
		/// <param name="optionalData">Varargs of String for additional parameters to be sent to the rule method.
		/// Each element is a string in the format of key=value, for example: url=playbasis.com
		/// The following keys are supported:
		/// - url		url or filter string (for triggering non-global actions)
		/// - reward	name of the custom-point reward to give (for triggering rules with custom-point reward)
		/// - quantity	amount of points to give (for triggering rules with custom-point reward)</param>
		/// <returns></returns>
		public string rule(string playerId, string action, params string[] optionalData)
		{
			var param = new StringBuilder();
			param.Append("token=");
			param.Append(token);
			param.Append("&player_id=");
			param.Append(playerId);
			param.Append("&action=");
			param.Append(action);

			for (int i = 0; i < optionalData.Length; ++i)
				param.Append("&" + optionalData[i]);

			return call("Engine/rule", param.ToString());
		}
		public void rule_async(string playerId, string action, UploadStringCompletedEventHandler onComplete, params string[] optionalData)
		{
			var param = new StringBuilder();
			param.Append("token=");
			param.Append(token);
			param.Append("&player_id=");
			param.Append(playerId);
			param.Append("&action=");
			param.Append(action);

			for (int i = 0; i < optionalData.Length; ++i)
				param.Append("&" + optionalData[i]);

			call_async("Engine/rule", param.ToString(), onComplete);
		}

        public string quest(string questId)
        {
            return call("Quest/" + questId + apiKeyParam);
        }

        public string quests()
        {
            return call("Quest" + apiKeyParam);
        }

        public string mission(string questId, string missionId)
        {
            return call("Quest/" + questId + "/mission/" + missionId + apiKeyParam);
        }

        public string questAvailable(string questId, string playerId)
        {
            return call("Quest/" + questId + "/available/" + apiKeyParam + "&player_id=" + playerId);
        }

        public string questsAvailable(string playerId)
        {
            return call("Quest/available" + apiKeyParam + "&player_id=" + playerId);
        }

        public string joinQuest(string questId, string playerId)
        {
            var param = new StringBuilder();
            param.Append("token=");
            param.Append(token);
            param.Append("&player_id=");
            param.Append(playerId);
            return call("Quest/" + questId + "/join", param.ToString());
        }
        public void joinQuest_async(string questId, string playerId, UploadStringCompletedEventHandler onComplete = null)
        {
            var param = new StringBuilder();
            param.Append("token=");
            param.Append(token);
            param.Append("&player_id=");
            param.Append(playerId);
            call_async("Quest/" + questId + "/join", param.ToString(), onComplete);
        }

        public string cancelQuest(string questId, string playerId)
        {
            var param = new StringBuilder();
            param.Append("token=");
            param.Append(token);
            param.Append("&player_id=");
            param.Append(playerId);
            return call("Quest/" + questId + "/cancel", param.ToString());
        }
        public void cancelQuest_async(string questId, string playerId, UploadStringCompletedEventHandler onComplete = null)
        {
            var param = new StringBuilder();
            param.Append("token=");
            param.Append(token);
            param.Append("&player_id=");
            param.Append(playerId);
            call_async("Quest/" + questId + "/cancel", param.ToString(), onComplete);
        }

        public string redeemGoods(string goodsId, string playerId, int amount = 1)
        {
            var param = new StringBuilder();
            param.Append("token=");
            param.Append(token);
            param.Append("&goods_id=");
            param.Append(goodsId);
            param.Append("&player_id=");
            param.Append(playerId);
            param.Append("&amount=");
            param.Append(amount);
            return call("Redeem/goods", param.ToString());
        }
        public void redeemGoods_async(string goodsId, string playerId, int amount = 1, UploadStringCompletedEventHandler onComplete = null)
        {
            var param = new StringBuilder();
            param.Append("token=");
            param.Append(token);
            param.Append("&goods_id=");
            param.Append(goodsId);
            param.Append("&player_id=");
            param.Append(playerId);
            param.Append("&amount=");
            param.Append(amount);
            call_async("Redeem/goods", param.ToString(), onComplete);
        }

        public string recentPoint(int offset=0, int limit=10)
        {
            return call("Service/recent_point" + apiKeyParam + "&offset=" + offset.ToString() + "&limit=" + limit.ToString());
        }

        public string recentPointByName(string pointName, int offset = 0, int limit = 10)
        {
            return call("Service/recent_point" + apiKeyParam + "&offset=" + offset.ToString() + "&limit=" + limit.ToString() + "&point_name=" + pointName);
        }

		public static string call(string address, string data = null)
		{
			Console.WriteLine("making request to: " + address);

			WebClient client = new WebClient();
			if (!string.IsNullOrEmpty(data))
			{
				client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
				return client.UploadString(BASE_URL + address, data);
			}
			return client.DownloadString(BASE_URL + address);
		}

		public static void call_async(string address, string data, UploadStringCompletedEventHandler onComplete)
		{
			Console.WriteLine("making async request to: " + address);

			WebClient client = new WebClient();
			Debug.Assert(!string.IsNullOrEmpty(data));			
			client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
			if(onComplete != null)
				client.UploadStringCompleted += onComplete;
			client.UploadStringAsync(new Uri(BASE_URL + address), data);
		}

		public static dynamic JsonToDynamic(string json)
		{
			return JsonConvert.DeserializeObject<dynamic>(json);
		}

		public static T JsonToObject<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}
	}
}
