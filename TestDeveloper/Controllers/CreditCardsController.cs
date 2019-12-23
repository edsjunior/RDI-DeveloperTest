using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestDeveloper.API.NewFolder;

namespace TestDeveloper.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CreditCardsController : ControllerBase
	{
		private static string _key = "token";
		private static readonly System.Runtime.Caching.MemoryCache _cache = System.Runtime.Caching.MemoryCache.Default;
		private static int cardLength = 16;
		private static int dateLength = 12;

		// POST api/CreditCards
		/// <summary>
		/// Post credit card number and cvv code to generate token
		/// </summary>
		/// <returns>Os itens da To-do list</returns>
		/// <response code="200">Return token, array smaller 4 and array with rotation values</response>
		[HttpPost]
		public ActionResult<string> Post([FromBody] CardViewModel card)
		{
			var numCartao = card.CardNumber.ToString();
			var numCVV = card.CVV.ToString();

			if (numCartao.Length != cardLength)
				return BadRequest();

			if (numCVV.Length < 3 || numCVV.Length > 5)
				return BadRequest();

			var date = DateTime.Now.ToString("yyyyMMddHHmm");

			var token = numCartao + numCVV + date;
			StoreTokenInCache(token);

			var formatedArray = FilterArray(token);
			var rotationArray = RotateArray(token);

			var Json = CreateJson(token, formatedArray, rotationArray);

			return Ok(Json);
		}

		public static void StoreTokenInCache(string tokenId)
		{
			var cacheItemPolicy = new CacheItemPolicy()
			{
				AbsoluteExpiration = DateTime.Now.AddMinutes(30)
			};
			_cache.Add(_key, tokenId, cacheItemPolicy);
		}

		private static string FilterArray(string token)
		{
			int[] tokenArray = Array.ConvertAll(token.ToArray(), c => (int)Char.GetNumericValue(c));
			IEnumerable<int> filteredArray = tokenArray.AsQueryable().Where((a, index) => a < 5);
			return (string.Join(",", filteredArray));
		}

		private static string RotateArray(string token)
		{
			var cvv = token.Substring(cardLength, token.Length - cardLength - dateLength);

			int[] tokenArray = Array.ConvertAll(cvv.ToArray(), c => (int)Char.GetNumericValue(c));

			var listArray = (tokenArray).ToList();

			for (int i = 0; i < listArray.Count - 1; i++)
			{
				listArray.Insert(0, listArray.LastOrDefault());
				listArray.RemoveAt(listArray.Count - 1);
			}

			string InvertedArray = string.Join(",", listArray);
			return (InvertedArray);
		}

		private static JObject CreateJson(string token, string formatedArray, string rotationArray)
		{
			var dataJson = new
			{
				Token = token,
				FormatedArray = formatedArray,
				RotationArray = rotationArray,
			};

			string json_data = JsonConvert.SerializeObject(dataJson);
			JObject json_object = JObject.Parse(json_data);

			return (json_object);
		}
	}
}
