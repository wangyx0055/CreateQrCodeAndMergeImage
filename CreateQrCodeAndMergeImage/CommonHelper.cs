using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using RestSharp;

namespace CreateQrCodeAndMergeImage
{
    public static class CommonHelper
    {
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        #region RestGet HtppGet请求

        /// <summary>
        /// HtppGet请求
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="resource"></param>
        /// <param name="parameters"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string RestGet(string baseUrl, string resource, Dictionary<string, string> parameters,
            Dictionary<string, string> headers = null)
        {
            const Method method = Method.GET;
            return RestRequest(baseUrl, resource, method, parameters, headers);
        }

        #endregion

        #region RestPost HtppPost请求

        /// <summary>
        /// HtppGet请求
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="resource"></param>
        /// <param name="parameters"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string RestPost(string baseUrl, string resource, Dictionary<string, string> parameters,
            Dictionary<string, string> headers = null)
        {
            const Method method = Method.POST;
            return RestRequest(baseUrl, resource, method, parameters, headers);
        }

        #endregion

        #region RestRequest Http Rest请求

        /// <summary>
        /// Http Rest请求
        /// </summary>
        /// <param name="baseUrl">Host</param>
        /// <param name="resource">RecoureUrl</param>
        /// <param name="method">Method</param>
        /// <param name="parameters">Parameters</param>
        /// <param name="headers">Headers</param>
        /// <returns></returns>
        public static string RestRequest(string baseUrl, string resource, Method method,
            Dictionary<string, string> parameters, Dictionary<string, string> headers = null)
        {
            string strResult = "";

            var client = new RestClient(baseUrl);
            var request = new RestRequest(resource, method);
            if (headers != null)
            {
                if (headers.Count > 0)
                {
                    foreach (var headItem in headers)
                    {
                        request.AddHeader(headItem.Key, headItem.Value);
                    }
                }
            }

            if (method == Method.POST || method == Method.PUT)
            {
                if (parameters != null)
                {
                    if (parameters.Count > 0)
                    {
                        {
                            foreach (var item in parameters)
                            {
                                request.AddParameter(item.Key, item.Value);
                            }
                        }
                    }
                }
            }

            if (method == Method.GET)
            {
                if (parameters != null)
                {
                    if (parameters.Count > 0)
                    {
                        foreach (var item in parameters)
                        {
                            request.AddQueryParameter(item.Key, item.Value);
                        }
                    }
                }
            }

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                strResult = response.Content;
            }

            return strResult;
        }

        #endregion
    }
}