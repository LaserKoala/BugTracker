using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text;

namespace BugTracker
{
    public class BugTracker
    {
        public async Task<string> Log(Exception exception)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://hooks.slack.com/services/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");
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(new
                {
                    text = "*Oops, I caught something ...*",
                    attachments = new object[]
                    {
                        new
                        {
                            title = exception.GetType().FullName,
                            color = "danger",
                            text = exception.Message
                        },

                        new
                        {
                            title = "Stack trace",
                            color = "warning",
                            text = exception.StackTrace
                        }
                    }
                });
                streamWriter.Write(json);
            }

            var response = (HttpWebResponse)await request.GetResponseAsync();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
