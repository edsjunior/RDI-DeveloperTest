using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestDeveloper.API.NewFolder
{
	public class CardViewModel
	{
		[Required]
		public long CardNumber { get; set; }

		[Required]
		public int CVV { get; set; }
	}
}
