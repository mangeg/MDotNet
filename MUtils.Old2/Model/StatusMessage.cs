namespace MUtils.Model
{
	using System;

	public class StatusMessage
	{
		public StatusMessage() { TimeStamp = DateTime.Now; }
		public DateTime TimeStamp { get; set; }
		public String Message { get; set; }
	}
}