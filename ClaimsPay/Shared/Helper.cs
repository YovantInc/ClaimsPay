using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NLog.Config;
using NLog.Targets;
using NLog.Web;
using NLog;
using System.Text;
using ClaimsPay.Modules.ClaimsPay.Models;

using LogLevel = NLog.LogLevel;

namespace ClaimsPay.Shared
{
    public class Helper
    {

        AppHttpClient appHttpClient = new();
        HttpClient objhttpClient = new();
        string baseURL = string.Empty;
        string ClaimsPayType = string.Empty;
        string ClaimsPayTypeRequest = string.Empty;
        string LoanAccountNumber = string.Empty;
        string ClaimsPayMethod = string.Empty;
        string approvalRequired = string.Empty;
        #region Get Loss Details
        public async Task<string> GetLossDetails(string LossID, LoggingConfiguration config)
        {
            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();
            JObject? objResponse = null;
            try
            {

                _logger.Info("\r\n");
                _logger.Info("\r\n");
                _logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated Get Party Detail ");
                _logger.Info("");

                string? baseURI = AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIURI"];
                string? URI = baseURI + "/v3/losses/lossids?includeExtendedData=false";

                _logger.Info("\r\n");
                _logger.Info("Request");
                _logger.Info(URI);
                objhttpClient.DefaultRequestHeaders.Clear();
                objhttpClient.DefaultRequestHeaders.Add("userid", AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIDefaultUser"]);
                objhttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIKEY"]);
                var content = JObject.Parse("{\"Identifiers\":[\"" + LossID + "\"]}");
                var stringContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
                var result = await objhttpClient.PostAsync(URI, stringContent).Result.Content.ReadAsStringAsync();
                objResponse = JObject.Parse(result.ToString());

                _logger.Info("\r\n");
                _logger.Info("Response");
                _logger.Info(result.ToString());

                //return result.ToString();
                _logger.Info("\r\n");
                _logger.Info("Loss Details Executed Successfully");
            }

            catch (Exception ex)
            {

                _logger.Info("\r\n");
                _logger.Info("Loss Execution failed");
                _logger.Error(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + ex, "ERROR" + ex.ToString());

            }
            finally
            {
                //_logger.Info("------------------------------------------------------------------ || Log End || -------------------------------------------------------------------------");
                // NLog.LogManager.Shutdown();
            }

            return objResponse["loss"][0]["lossDate"].ToString();
        }
        #endregion

        #region Get Party Details
        public async Task<string> GetPartyDetails(string partyID, LoggingConfiguration config)
        {
            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();
            try
            {

                _logger.Info("\r\n");

                _logger.Info("\r\n");
                _logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated Get Party Detail ");
                _logger.Info("");

                string baseURI = AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIURI"];
                string URI = baseURI + "/v2/parties/" + partyID + "/partyDetails";

                _logger.Info("\r\n");
                _logger.Info("Request");
                _logger.Info(URI);
                objhttpClient.DefaultRequestHeaders.Clear();
                objhttpClient.DefaultRequestHeaders.Add("userid", AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIDefaultUser"]);
                objhttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIKEY"]);

                var result = await objhttpClient.GetAsync(URI).Result.Content.ReadAsStringAsync();

                _logger.Info("\r\n");
                _logger.Info("Response");
                _logger.Info(result.ToString());

                return result.ToString();
                _logger.Info("\r\n");
                _logger.Info("Party Details Executed Successfully");
            }

            catch (Exception ex)
            {

                _logger.Info("\r\n");
                _logger.Info("Party Detail Execution failed");
                _logger.Error(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + ex, "ERROR" + ex.ToString());

            }
            finally
            {
                //_logger.Info("------------------------------------------------------------------ || Log End || -------------------------------------------------------------------------");
                // NLog.LogManager.Shutdown();
            }

            return "";
        }

        #endregion

        #region Get Payment Header Details
        public async Task<string> GetPaymentHeaderDetails(string paymentID, LoggingConfiguration config)
        {
            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();
            try
            {

                _logger.Info("\r\n");

                _logger.Info("\r\n");
                _logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated Get Payment Header Details ");
                _logger.Info("");

                string? baseURI = AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIURI"];
                string? URI = baseURI + "/v2/payments/" + paymentID + "/paymentHeader?includeExtendedData=true";

                _logger.Info("\r\n");
                _logger.Info("Request");
                _logger.Info(URI);
                objhttpClient.DefaultRequestHeaders.Clear();
                objhttpClient.DefaultRequestHeaders.Add("userid", AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIDefaultUser"]);
                objhttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIKEY"]);

                var result = await objhttpClient.GetAsync(URI).Result.Content.ReadAsStringAsync();

                _logger.Info("\r\n");
                _logger.Info("Response");
                _logger.Info(result.ToString());

                return result.ToString();
                _logger.Info("\r\n");
                _logger.Info("Get Payment Header Details Executed Successfully");
            }

            catch (Exception ex)
            {

                _logger.Info("\r\n");
                _logger.Info("Get Payment Header Details Execution failed");
                _logger.Error(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + ex, "ERROR" + ex.ToString());

            }
            finally
            {
                //_logger.Info("------------------------------------------------------------------ || Log End || -------------------------------------------------------------------------");
                // NLog.LogManager.Shutdown();
            }

            return "";
        }

        #endregion

        #region Get Country
        public async Task<string> GetCountry(string key_name)
        {
            JObject? objStateKeys = JObject.Parse(File.ReadAllText(@"Shared\\CountryKeys.json"));
            string? value = objStateKeys[key_name].ToString();
            return value;
        }

        #endregion

        #region Get State
        public async Task<string> GetState(string key_name)
        {
            JObject? objStateKeys = JObject.Parse(File.ReadAllText(@"Shared\\StateKeys.json"));
            string? value = objStateKeys[key_name].ToString();
            return value;
        }

        #endregion

        #region Get Cause Of Loss
        public async Task<string> GetCauseOfLoss(string key_name)
        {
            JObject objStateKeys = JObject.Parse(File.ReadAllText(@"Shared\\CauseOfLoss.json"));
            string value = objStateKeys[key_name].ToString();
            return value;
        }

        #endregion

        #region Read Claimspay Field from Extended Data
        public async Task ReadClaimsPayFields(JObject extendedData)
        {
            foreach (dynamic Column in extendedData["column"])
            {
                if (Column["name"].Value == string.Concat("PaymentCstm.", Constants.DATAITEM_ClaimsPayType))
                {
                    if ((Column.value["@xsi:nil"] == null) || (Column.value["@xsi:nil"].Value == "false"))
                    {
                        ClaimsPayType = Column.value["#text"].Value;
                    }
                }
                else if (Column["name"].Value == string.Concat("PaymentCstm.", Constants.DATAITEM_ClaimsPayRequestType))
                {
                    if ((Column.value["@xsi:nil"] == null) || (Column.value["@xsi:nil"].Value == "false"))
                    {
                        ClaimsPayTypeRequest = Column.value["#text"].Value;
                    }
                }
                else if (Column["name"].Value == string.Concat("PaymentCstm.", Constants.DATAITEM_LoanAccountNumber))
                {
                    if ((Column.value["@xsi:nil"] == null) || (Column.value["@xsi:nil"].Value == "false"))
                    {
                        LoanAccountNumber = Column.value["#text"].Value;
                    }
                }
                else if (Column["name"].Value == string.Concat("PaymentCstm.", Constants.DATAITEM_ClaimsPayMethod))
                {
                    if ((Column.value["@xsi:nil"] == null) || (Column.value["@xsi:nil"].Value == "false"))
                    {
                        ClaimsPayMethod = Column.value["#text"].Value;
                    }

                }
                else if (Column["name"].Value == string.Concat("PaymentCstm.", Constants.DATAITEM_ApprovalRequired))
                {
                    if ((Column.value["@xsi:nil"] == null) || (Column.value["@xsi:nil"].Value == "false"))
                    {
                        approvalRequired = Column.value["#text"].Value;
                    }

                }

            }

        }
        #endregion

        #region Set Nlog Config 
        public LoggingConfiguration SetNlogConfig(string logPath, string logName)
        {
            LoggingConfiguration config = new LoggingConfiguration();
            var consoleTarget = new FileTarget
            {
                Name = "logfile",
                FileName = "" + logPath + "//" + logName + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt",
                Layout = "${message}"
            };
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, consoleTarget, "*");
            LogManager.Configuration = config;
            return config;
        }
        #endregion

        #region Get Participant Role
        public async Task<string> GetParticipantRole(string key_name)
        {
            JObject objStateKeys = JObject.Parse(File.ReadAllText(@"Shared\\ParticipantRole.json"));
            string value = objStateKeys[key_name].ToString();
            return value;
        }

        #endregion
    }
}
