using Newtonsoft.Json;
using System.Text;

namespace ClaimsPay.Shared
{
    abstract class ClaimsAPI
    {
        protected static HttpClient httpClient = null;

        abstract protected HttpRequestMessage CreateRequest(HttpMethod method, ILogger log, string uri, string authToken, string userID);

        public abstract string BaseURI { get; }

        private Task<string> ProcessJSONResult(string loggingName, Task<HttpResponseMessage> task, ILogger log, System.Net.HttpStatusCode[] expectedResponses = null)
        {
            task.Wait();

            Helpers.ProcessTaskCompletion(loggingName + " Task", task, log, true);

            HttpResponseMessage response = task.Result;

            if (expectedResponses == null || expectedResponses.Length == 0)
                Helpers.ProcessHttpResponseCompletion(loggingName + " Response", response, log, true);
            else
            {
                bool found = false;
                foreach (System.Net.HttpStatusCode statusCode in expectedResponses)
                {
                    if (statusCode == response.StatusCode)
                    {
                        Helpers.ProcessHttpResponseCompletion(loggingName + " Response", response, log, false, statusCode);
                        found = true;
                        break;
                    }
                }

                if (!found)
                    Helpers.ProcessHttpResponseCompletion(loggingName + " Response", response, log, true);
            }


            return response.Content.ReadAsStringAsync();
        }

        public dynamic GetLossData(string lossID, ILogger log, string authToken, string userID)
        {
            string requestBody = "{\"identifiers\": [\"" + lossID + "\"]}";

            StringContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            string uri = string.Format("{0}v2/losses/lossids?includeExtendedData=false", BaseURI);

            HttpRequestMessage httpRequestMessage = CreateRequest(HttpMethod.Post, log, uri, authToken, userID);

            httpRequestMessage.Content = content;

            Helpers.ProcessHttpRequest("Get Loss", httpRequestMessage, log);

            Task<HttpResponseMessage> task = httpClient.SendAsync(httpRequestMessage);
            task.Wait();

            Helpers.ProcessTaskCompletion("Get Losses", task, log, true);

            HttpResponseMessage response = task.Result;

            Helpers.ProcessHttpResponseCompletion("Get Losses Response", response, log, true);

            Task<string> taskResponse = response.Content.ReadAsStringAsync();
            taskResponse.Wait();

            Helpers.ProcessTaskCompletion("GetLossData Read Response", taskResponse, log, true);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(taskResponse.Result);
        }

        public dynamic GetClaimData(string claimID, ILogger log, string authToken, string userID)
        {
            string uri = string.Format("{0}v2/claims/{1}", BaseURI, claimID);

            HttpRequestMessage httpRequestMessage = CreateRequest(HttpMethod.Get, log, uri, authToken, userID);

            Helpers.ProcessHttpRequest("Get Claim", httpRequestMessage, log);

            Task<HttpResponseMessage> task = httpClient.SendAsync(httpRequestMessage);
            task.Wait();

            Helpers.ProcessTaskCompletion("Get Claim", task, log, true);

            HttpResponseMessage response = task.Result;

            Helpers.ProcessHttpResponseCompletion("Get Claim Response", response, log, true);

            Task<string> taskResponse = response.Content.ReadAsStringAsync();
            taskResponse.Wait();

            Helpers.ProcessTaskCompletion("GetClaimData Read Response", taskResponse, log, true);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(taskResponse.Result);
        }

        public dynamic GetPartyDetails(string partyID, ILogger log, string authToken, string userID)
        {
            string uri = string.Format("{0}v2/parties/{1}/partydetails", BaseURI, partyID);

            HttpRequestMessage httpRequestMessage = CreateRequest(HttpMethod.Get, log, uri, authToken, userID);

            Helpers.ProcessHttpRequest("Get Party", httpRequestMessage, log);

            Task<HttpResponseMessage> task = httpClient.SendAsync(httpRequestMessage);
            task.Wait();

            Helpers.ProcessTaskCompletion("Get Party", task, log, true);

            HttpResponseMessage response = task.Result;

            Helpers.ProcessHttpResponseCompletion("Get Party Response", response, log, true);

            Task<string> taskResponse = response.Content.ReadAsStringAsync();
            taskResponse.Wait();

            Helpers.ProcessTaskCompletion("GetPartyDetails Read Response", taskResponse, log, true);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(taskResponse.Result);
        }

        public dynamic GetClaimDataByClaimNumber(string claimNumber, ILogger log, string authToken, string userID)
        {
            string uri = string.Format("{0}v2/claims/claimnumber/{1}", BaseURI, claimNumber);

            HttpRequestMessage httpRequestMessage = CreateRequest(HttpMethod.Get, log, uri, authToken, userID);

            Helpers.ProcessHttpRequest("Get Claim by Claim Number", httpRequestMessage, log);

            Task<HttpResponseMessage> task = httpClient.SendAsync(httpRequestMessage);
            task.Wait();

            Helpers.ProcessTaskCompletion("Get Claim by Claim Number", task, log, true);

            HttpResponseMessage response = task.Result;

            Helpers.ProcessHttpResponseCompletion("Get Claim by Claim Number Response", response, log, true);

            Task<string> taskResponse = response.Content.ReadAsStringAsync();
            taskResponse.Wait();

            Helpers.ProcessTaskCompletion("Get Claim by Claim Number Read Response", taskResponse, log, true);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(taskResponse.Result);
        }

        public dynamic PublishEvent(ILogger log, string authToken, string userID, string eventType, string entityID, string entityType, string eventData = null, string processEntityId = null, string processEntityType = null, int userTimeZoneOffset = 0)
        {
            string uri = string.Format("{0}v2/eventprocess/publishevent/eventtype/{1}?entityId={2}&entityType={3}",
                BaseURI, eventType, entityID, entityType);
            //&eventData={3}&processEntityId={4}&processEntityType{5}&userTimeZoneOffset={6}"

            uri += "&eventData";
            if (!string.IsNullOrWhiteSpace(eventData))
                uri += "=" + eventData;

            uri += "&processEntityId";
            if (!string.IsNullOrWhiteSpace(processEntityId))
                uri += "=" + processEntityId;

            uri += "&processEntityType";
            if (!string.IsNullOrWhiteSpace(processEntityType))
                uri += "=" + processEntityType;

            if (userTimeZoneOffset != 0)
                uri += "&userTimeZoneOffset=" + userTimeZoneOffset;

            HttpRequestMessage httpRequestMessage = CreateRequest(HttpMethod.Post, log, uri, authToken, userID);

            Helpers.ProcessHttpRequest("PublishEvent", httpRequestMessage, log);

            Task<HttpResponseMessage> task = httpClient.SendAsync(httpRequestMessage);
            task.Wait();

            Helpers.ProcessTaskCompletion("PublishEvent", task, log, true);

            HttpResponseMessage response = task.Result;

            Helpers.ProcessHttpResponseCompletion("PublishEvent Response", response, log, true);

            Task<string> taskResponse = response.Content.ReadAsStringAsync();
            taskResponse.Wait();

            Helpers.ProcessTaskCompletion("PublishEvent Response", taskResponse, log, true);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(taskResponse.Result);
        }

        public dynamic CreateFileNote(string claimID, string category, string subcategory, string subject, string text, string[] participants, ILogger log, string authToken, string userID)
        {
            StringBuilder sb = new StringBuilder();

            string now = JsonConvert.SerializeObject(DateTime.Now);

            sb.Append("{");
            sb.AppendFormat(" \"ClaimID\":\"{0}\",", claimID);
            sb.Append("\"FileNoteWebDTO\" : {");
            sb.Append("\"Recepients\" : null, ");

            if (participants != null && participants.Length > 0)
            {
                sb.Append("                     \"Participants\": [");

                bool first = true;

                foreach (string participant in participants)
                {
                    if (!first)
                        sb.Append(",");

                    sb.AppendFormat("{ \"IsSelected\": true, \"ID\": \"{0}\"}", participant);
                }

                sb.Append("                     ],");
            }
            else
                sb.Append("\"Participants\" : null,");

            sb.Append("\"FileNoteDetails\" : {");

            sb.Append("  \"FileNote\" : {");
            sb.AppendFormat(" \"DateLastUpdated\":{0},", now);
            sb.AppendFormat(" \"ApplicationCode\":\"CLAIMS\",");
            sb.AppendFormat(" \"CategoryCode\":\"{0}\",", category);
            sb.AppendFormat(" \"SubCategoryCode\":\"{0}\",", subcategory);
            sb.AppendFormat(" \"FileNoteType\":\"01\",");
            sb.AppendFormat(" \"DateFileNoteCreated\":{0},", now);
            sb.AppendFormat(" \"DateFileNoteFinal\":{0},", now);
            sb.AppendFormat(" \"DateFileNoteDiscussion\":{0},", now);
            sb.AppendFormat(" \"ImportanceCode\":\"M\",");
            sb.AppendFormat(" \"AccessTypeCode\":\"GEN\",");
            sb.AppendFormat(" \"IsSystemGenerated\":false ");

            sb.Append("},  \"FileNoteTextDetails\" : [{");
            sb.Append("  \"FileNoteText\" : {");
            sb.AppendFormat(" \"TextType\":\"OR\",");
            sb.AppendFormat(" \"NoteText\":\"{0}\",", text);
            sb.AppendFormat(" \"NoteTextWithHTML\":null,");
            sb.AppendFormat(" \"Subject\":\"{0}\",", subject);
            sb.AppendFormat(" \"DateCreated\":{0},", now);
            sb.AppendFormat(" \"DateLastUpdated\":{0}", now);
            sb.Append("}}]}}}");

            string requestBody = sb.ToString();

            StringContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            string uri = string.Format("{0}v2/filenotes", BaseURI);

            HttpRequestMessage httpRequestMessage = CreateRequest(HttpMethod.Post, log, uri, authToken, userID);

            httpRequestMessage.Content = content;

            Helpers.ProcessHttpRequest("CreateFileNote", httpRequestMessage, log);

            Task<HttpResponseMessage> task = httpClient.SendAsync(httpRequestMessage);
            task.Wait();

            Helpers.ProcessTaskCompletion("CreateFileNote", task, log, true);

            HttpResponseMessage response = task.Result;

            Helpers.ProcessHttpResponseCompletion("CreateFileNote", response, log, true);

            Task<string> taskResponse = response.Content.ReadAsStringAsync();
            taskResponse.Wait();

            Helpers.ProcessTaskCompletion("CreateFileNote", taskResponse, log, true);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(taskResponse.Result);
        }

        public dynamic GetPolicyData(string policyID, ILogger log, string authToken, string userID)
        {
            string uri = string.Format("{0}v2/policies/{1}", BaseURI, policyID);

            HttpRequestMessage httpRequestMessage = CreateRequest(HttpMethod.Get, log, uri, authToken, userID);

            Helpers.ProcessHttpRequest("Get Policy", httpRequestMessage, log);

            Task<HttpResponseMessage> task = httpClient.SendAsync(httpRequestMessage);
            task.Wait();

            Helpers.ProcessTaskCompletion("Get Policy", task, log, true);

            HttpResponseMessage response = task.Result;

            Helpers.ProcessHttpResponseCompletion("Get Policy Response", response, log, true);

            Task<string> taskResponse = response.Content.ReadAsStringAsync();
            taskResponse.Wait();

            Helpers.ProcessTaskCompletion("GetPolicyData Read Response", taskResponse, log, true);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(taskResponse.Result);
        }

        public dynamic GetClaimGroupAssocData(string claimID, ILogger log, string authToken, string userID)
        {
            string uri = string.Format("{0}v2/claims/{1}/claimclaimgroupassociations", BaseURI, claimID);

            HttpRequestMessage httpRequestMessage = CreateRequest(HttpMethod.Get, log, uri, authToken, userID);

            Helpers.ProcessHttpRequest("GetClaimGroupAssocData", httpRequestMessage, log);

            Task<HttpResponseMessage> task = httpClient.SendAsync(httpRequestMessage);
            task.Wait();

            Helpers.ProcessTaskCompletion("GetClaimGroupAssocData Task", task, log, true);

            HttpResponseMessage response = task.Result;

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Helpers.ProcessHttpResponseCompletion("GetClaimGroupAssocData Response", response, log, false, System.Net.HttpStatusCode.NotFound);
                return null;
            }
            else
            {
                Helpers.ProcessHttpResponseCompletion("GetClaimGroupAssocData Response", response, log, true);

                Task<string> taskResponse = response.Content.ReadAsStringAsync();
                taskResponse.Wait();

                Helpers.ProcessTaskCompletion("GetClaimGroupAssocData Read Response", taskResponse, log, true);

                return Newtonsoft.Json.JsonConvert.DeserializeObject(taskResponse.Result);
            }
        }

        public dynamic GetClaimGroupData(string claimGroupID, ILogger log, string authToken, string userID)
        {
            string uri = string.Format("{0}v2/claimgroups/{1}", BaseURI, claimGroupID);

            HttpRequestMessage httpRequestMessage = CreateRequest(HttpMethod.Get, log, uri, authToken, userID);

            Helpers.ProcessHttpRequest("GetClaimGroupData", httpRequestMessage, log);

            Task<HttpResponseMessage> task = httpClient.SendAsync(httpRequestMessage);
            task.Wait();

            Helpers.ProcessTaskCompletion("GetClaimGroupData Task", task, log, true);

            HttpResponseMessage response = task.Result;

            Helpers.ProcessHttpResponseCompletion("GetClaimGroupData Response", response, log, true);

            Task<string> taskResponse = response.Content.ReadAsStringAsync();
            taskResponse.Wait();

            Helpers.ProcessTaskCompletion("GetClaimGroupData Read Response", taskResponse, log, true);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(taskResponse.Result);
        }

        private static System.Collections.Generic.Dictionary<string, string> _RefDataCache = new Dictionary<string, string>();
        private static System.Collections.Generic.Dictionary<string, string> _RefDataCacheCategoryResponse = new Dictionary<string, string>();
        private static DateTime _cacheCreated = DateTime.UtcNow;

        public string RefDataLookup(string category, string key, ILogger log, string authToken, string userID)
        {
            if (string.IsNullOrWhiteSpace(key))
                return string.Empty;

            string cacheKey = string.Format("{0}|{1}", category, key);
            System.Collections.Generic.Dictionary<string, string> refDataCache = _RefDataCache;

            if (refDataCache.ContainsKey(cacheKey))
                return refDataCache[cacheKey];
            else
            {
                lock (_RefDataCache)
                {
                    if (refDataCache.ContainsKey(cacheKey))  //added while we waited for the lock
                        return refDataCache[cacheKey];

                    string uri = string.Format("{0}v1/referencedata/categories/{1}/keys/{2}", BaseURI, category, key);

                    HttpRequestMessage httpRequestMessage = CreateRequest(HttpMethod.Get, log, uri, authToken, userID);

                    Helpers.ProcessHttpRequest("RefDataLookup", httpRequestMessage, log);

                    Task<HttpResponseMessage> taskHttpResponse = httpClient.SendAsync(httpRequestMessage);

                    taskHttpResponse.Wait();

                    Helpers.ProcessTaskCompletion("RefDataLookup", taskHttpResponse, log, true);

                    Helpers.ProcessHttpResponseCompletion("RefDataLookup", taskHttpResponse.Result, log, true);

                    Task<string> taskStringResponse = taskHttpResponse.Result.Content.ReadAsStringAsync();

                    Helpers.ProcessTaskCompletion("RefDataLookup taskStringResponse", taskStringResponse, log, true);

                    string jsonSring = taskStringResponse.Result;

                    dynamic jsonObject = JsonConvert.DeserializeObject(jsonSring);

                    string val = (string)jsonObject.value;

                    if (!string.IsNullOrWhiteSpace(val))
                    {
                        if (_cacheCreated.AddHours(1) < DateTime.UtcNow || _RefDataCache.Count > 5000)
                        {
                            _RefDataCache = new Dictionary<string, string>();
                            _cacheCreated = DateTime.UtcNow;
                        }

                        _RefDataCache[cacheKey] = val;
                    }

                    return val;
                }
            }
        }

        public dynamic GetPaymentHeaderByPaymentID(string paymentId, ILogger log, string authToken, string userID)
        {
            string uri = string.Format("{0}v2/payments/{1}/paymentHeader?includeExtendedData=true", BaseURI, paymentId);

            HttpRequestMessage httpRequestMessage = CreateRequest(HttpMethod.Get, log, uri, authToken, userID);

            Helpers.ProcessHttpRequest("InsurPay_GetPaymentHeaderByPaymentIDV3", httpRequestMessage, log);

            Task<HttpResponseMessage> task = httpClient.SendAsync(httpRequestMessage);
            task.Wait();

            Helpers.ProcessTaskCompletion("GetPaymentHeaderByPaymentID Task", task, log, true);

            HttpResponseMessage response = task.Result;

            Helpers.ProcessHttpResponseCompletion("GetPaymentHeaderByPaymentID Response", response, log, true);

            Task<string> taskResponse = response.Content.ReadAsStringAsync();
            taskResponse.Wait();

            Helpers.ProcessTaskCompletion("GetPaymentHeaderByPaymentID Read Response", taskResponse, log, true);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(taskResponse.Result);
        }

        public dynamic UpdatePaymentHeader(dynamic paymentHeader, ILogger log, string authToken, string userID)
        {
            string uri = string.Format("{0}v3/paymentHeader", BaseURI);

            HttpRequestMessage httpRequestMessage = CreateRequest(HttpMethod.Put, log, uri, authToken, userID);

            //Try this
            var jsonPayload = JsonConvert.SerializeObject(paymentHeader, Newtonsoft.Json.Formatting.Indented);

            httpRequestMessage.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            string sample = httpRequestMessage.ToString();

            Helpers.ProcessHttpRequest("InsurPay_UpdatePaymentHeaderV3", httpRequestMessage, log);

            Task<HttpResponseMessage> task = httpClient.SendAsync(httpRequestMessage);
            task.Wait();

            Helpers.ProcessTaskCompletion("UpdatePaymentHeader Task", task, log, true);

            HttpResponseMessage response = task.Result;

            Helpers.ProcessHttpResponseCompletion("UpdatePaymentHeader Response", response, log, true);

            Task<string> taskResponse = response.Content.ReadAsStringAsync();
            taskResponse.Wait();

            Helpers.ProcessTaskCompletion("UpdatePaymentHeader Read Response", taskResponse, log, true);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(taskResponse.Result);
        }
    }

}
