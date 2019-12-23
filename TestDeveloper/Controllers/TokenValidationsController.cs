using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace TestDeveloper.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TokenValidationsController : ControllerBase
	{
		private static string _key = "token";
		private static readonly System.Runtime.Caching.MemoryCache _cache = System.Runtime.Caching.MemoryCache.Default;
		private static int dateLength = 12;

		// GET: api/TokenValidations
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "Developer Test - Esdras M." };
		}

		// GET: api/TokenValidations/5
		[HttpGet("{tokenId}", Name = "Get")]
		public async Task<IActionResult> Get(string tokenId)
		{
			var tokenCache = GetTokenFromCache();

			if (string.IsNullOrEmpty(tokenCache))
				return BadRequest(false);

			if (tokenCache == tokenId)
			{
				var temp = tokenCache.Substring(tokenCache.Length - dateLength);
				var newDate = DateTime.ParseExact(temp,
					"yyyyMMddHHmm",
					CultureInfo.InvariantCulture);
				var compare = DateTime.Now - newDate;
				if (compare.Minutes > 15)
					return BadRequest("Token Expired");
			}
			else
			{
				return BadRequest(false);
			}

			return Ok(true);
		}

		public static string GetTokenFromCache()
		{
			var cache = _cache.Get(_key) as string;
			return cache;
		}
	}
}
