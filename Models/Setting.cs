
using System;

namespace servicedesk.api
{
    /*
    public class Setting
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public string Code { get { return this.Key; } }
		public string Content  { get { return Base64Decode(this.Value); } }

		private static string Base64Decode(string base64EncodedData)
		{
			var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}

	}
    */

    public class SMTPSettings {
        public string Server { get; set; }
        public string Port { get; set; }
    }

    
}
