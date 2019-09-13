using System;
using System.IO;
using System.Text;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Compliance360.ImportStarter
{
    //
    //
    // Instructions/Requirements:
    //
    // 1. Client must create an Integration defintion within the C360 website
    //
    // 2. The Integration should have configurations for;
    //    a. Unique identifier field (e.g. Import Id)
    //    b. Default Module Type (if other than 'Default')
    //    c. Default Workflow Template
    //    d. Default Division/Folder
    //
    // 3. Provide the following values for use here;
    //    a. Organization
    //    b. ApplicationKey
    //
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

            var url = Url();

            var dataJson = ConvertCsvFileToJsonObject();

            var bodyJson = "{\"" + _importArgs[argModule] + "\": {\"" + _importArgs[argComponent] + "\":" + dataJson + "}}";

            PostData(url, bodyJson);
        }

        public string ConvertCsvFileToJsonObject()
        {
            var csv = new List<string[]>();
            var lines = File.ReadAllLines(_importArgs[argFilePath]);

            foreach (string line in lines)
                csv.Add(line.Split(','));

            var properties = lines[0].Split(',');

            var listObjResult = new List<Dictionary<string, string>>();

            for (int i = 1; i < lines.Length; i++)
            {
                var objResult = new Dictionary<string, string>();
                for (int j = 0; j < properties.Length; j++)
                    objResult.Add(properties[j], csv[i][j]);

                listObjResult.Add(objResult);
            }

            return JsonConvert.SerializeObject(listObjResult);
        }

        public void PostData(string url, string bodyJson)
        {
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
            request.ContentType = "application/json";

            var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(bodyJson));
            var outputStream = request.GetRequestStream();
            inputStream.CopyTo(outputStream);

            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    var result = reader.ReadToEnd();
                }
            }
            catch (WebException webException)
            {
                if (((HttpWebResponse)(webException.Response)).StatusCode == HttpStatusCode.BadRequest)
                {
                    var exceptionReader = new StreamReader(webException.Response.GetResponseStream());
                    var exceptionresult = exceptionReader.ReadToEnd();
                }

                throw;
            }
        }

        public string Url()
        {
            return $@"{_importArgs[argBaseUri]}/API/2.0/Security/Authenticate?organization={_importArgs[argOrganization]}&integrationkey={_importArgs[argIntegrationKey]}";
        }
    }
}