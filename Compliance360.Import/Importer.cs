using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Compliance360.ImportStarter
{
    //
    // Instructions/Requirements: (see README.md)
    //

    public class Importer
    {
        private const string argBaseUri = "--baseuri";
        private const string argOrganization = "--organization";
        private const string argIntegrationKey = "--integrationkey";
        private const string argModule = "--module";
        private const string argComponent = "--component";
        private const string argFilePath = "--filepath";

        Dictionary<string, string> _importArgs;

        public Importer(Dictionary<string, string> importArgs)
        {
            _importArgs = importArgs;
        }

        public void Import()
        {
            if (!_importArgs.ContainsKey(argBaseUri))
            {
                throw new ArgumentException("Missing argument [--baseuri]");
            }
            if (!_importArgs.ContainsKey(argOrganization))
            {
                throw new ArgumentException("Missing argument [--organization]");
            }
            if (!_importArgs.ContainsKey(argIntegrationKey))
            {
                throw new ArgumentException("Missing argument [--integrationkey]");
            }
            if (!_importArgs.ContainsKey(argModule))
            {
                throw new ArgumentException("Missing argument [--module]");
            }
            if (!_importArgs.ContainsKey(argComponent))
            {
                throw new ArgumentException("Missing argument [--component]");
            }
            if (!_importArgs.ContainsKey(argFilePath))
            {
                throw new ArgumentException("Missing argument [--filepath]");
            }

            var uri = C360_Uri();

            Console.WriteLine("Uploading {0} to {1} ...", _importArgs[argFilePath], uri);

            var result = PostData(_importArgs[argFilePath], uri);

            Console.WriteLine("\nResponse Received:\n{0}", result);
        }

        public string PostData(string filepath, string url)
        {
            var result = string.Empty;
            HttpWebRequest request = null;

            try
            {
                var uri = new Uri(url);
                request = (HttpWebRequest)WebRequest.Create(uri);
            }
            catch (Exception)
            {
                throw new Exception(string.Format("Error contacting URL: {0}", url));
            }

            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/x-binary";

            var inputStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            var outputStream = request.GetRequestStream();
            inputStream.CopyTo(outputStream);

            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    result = reader.ReadToEnd();
                }
            }
            catch (WebException webException)
            {
                if (((HttpWebResponse)(webException.Response)).StatusCode == HttpStatusCode.BadRequest)
                {
                    var exceptionReader = new StreamReader(webException.Response.GetResponseStream());
                    result = exceptionReader.ReadToEnd();
                }
            }

            return result.Replace("\0", "");
        }

        public string C360_Uri()
        {
           return $@"{_importArgs[argBaseUri]}/API/2.0/Security/Authenticate?organization={_importArgs[argOrganization]}&integrationkey={_importArgs[argIntegrationKey]}&module={_importArgs[argModule]}&component={_importArgs[argComponent]}";
        }
    }
}