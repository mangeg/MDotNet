using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUtils.Model
{
	public class StatusMessage
	{
		public DateTime TimeStamp { get; set; }
		public String Message { get; set; }

		public StatusMessage()
		{
			TimeStamp = DateTime.Now;
		}
	}
}
