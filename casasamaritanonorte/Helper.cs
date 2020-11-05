using casasamaritanonorte.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace casasamaritanonorte
{


    public class AuthFacebook
    {
        public enum Method { GET, POST };
        public const string Authorize = "https://graph.facebook.com/oauth/authorize";
        public const string Access_Token = "https://graph.facebook.com/oauth/access_token";
        public string CallBack_Url = ConfigurationManager.AppSettings["Facebook_CallbackUrl"];

        private string _aplicationKey = "";
        private string _aplicationSecret = "";
        private string _token = "";

        public string AplicationKey
        {
            get
            {
                if (_aplicationKey.Length == 0)
                {
                    _aplicationKey = ConfigurationManager.AppSettings["Facebook_aplicationKey"];
                }
                return _aplicationKey;
            }
            set { _aplicationKey = value; }
        }

        public string AplicationSecret
        {
            get
            {
                if (_aplicationSecret.Length == 0)
                {
                    _aplicationSecret = ConfigurationManager.AppSettings["Facebook_aplicationSecret"];
                }
                return _aplicationSecret;
            }
            set { _aplicationSecret = value; }
        }

        public string Token { get; set; }

        public string GetAuthorizationLink()
        {
            string t = string.Format("{0}?client_id={1}&redirect_uri={2}&scope={3}",
                Authorize, AplicationKey, CallBack_Url, "publish_video");

            return t;
        }

        public string GetAuthorizationLinkPopup()
        {
            return string.Format("{0}?client_id={1}&display=popup&redirect_uri={2}&scope={3}",
                Authorize, AplicationKey, CallBack_Url, "publish_stream");
        }

        public void GetAccessToken(string authToken)
        {
            this.Token = authToken;
            string accessTokenUrl = string.Format("{0}?client_id={1}&redirect_uri={2}&client_secret={3}&code={4}",
            Access_Token, this.AplicationKey, CallBack_Url, this.AplicationSecret, authToken);

            string response = Request(Method.GET, accessTokenUrl, String.Empty);

            if (response.Length > 0)
            {
                NameValueCollection qs = HttpUtility.ParseQueryString(response);

                if (qs["access_token"] != null)
                {
                    this.Token = qs["access_token"];
                }
            }
        }

        public string Request(Method method, string url, string postData)
        {
            HttpWebRequest webRequest = null;
            StreamWriter requestWriter = null;
            string responseData = "";

            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = method.ToString();
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.UserAgent = "MSIE 6.0";
            webRequest.Timeout = 20000;


            if (method == Method.POST)
            {
                webRequest.ContentType = "multipart/form-data";

                //POST the data.
                requestWriter = new StreamWriter(webRequest.GetRequestStream());

                try
                {
                    requestWriter.Write(postData);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    requestWriter.Close();
                    requestWriter = null;
                }
            }

            responseData = GetWebResponse(webRequest);
            webRequest = null;
            return responseData;
        }

        public string GetWebResponse(HttpWebRequest webRequest)
        {
            StreamReader responseReader = null;
            string responseData = "";

            try
            {
                responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch(Exception e)
            {
                throw;
            }
            finally
            {
                webRequest.GetResponse().GetResponseStream().Close();
                responseReader.Close();
                responseReader = null;
            }

            return responseData;
        }
    }


    public class Helper
    {
        public enum Method { GET, POST };

        public static MethodResult SubmitPost(string url, string postValues, out string response, Method metodo = Method.POST, string contentType = "application/json")
        {
            MethodResult result = new MethodResult();
            response = string.Empty;

            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(postValues);

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = metodo.ToString();
                webRequest.ContentType = contentType;
                //webRequest.Accept = accept;
                webRequest.Timeout = System.Threading.Timeout.Infinite;

                webRequest.ContentLength = data.Length;

                Stream requestStream = webRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
                requestStream.Flush();

                WebResponse webResponse = webRequest.GetResponse();
                StreamReader reader = new StreamReader(webResponse.GetResponseStream());

                var dataReceived = reader.ReadToEnd();

                webResponse.Close();
                reader.Close();
                webRequest.Abort();

                result.returnCode = MethodResult.ReturnCode.Success;
                response = dataReceived;
            }

            catch (Exception ex)
            {
                result.returnCode = MethodResult.ReturnCode.Failure;
                result.errorMessage = ex.Message;
            }

            return result;
        }

        public static string BuildPostString(Dictionary<string, string> postValues)
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> value in postValues)
            {
                if (sb.Length > 0) sb.Append("&");

                sb.Append(string.Format("{0}={1}", value.Key, value.Value));
            }

            return sb.ToString();
        }
    }
}