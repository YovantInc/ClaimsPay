﻿using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NLog.Config;
using NLog.Targets;
using NLog.Web;
using NLog;
using System.Text;
using ClaimsPay.Modules.ClaimsPay.Models;

using LogLevel = NLog.LogLevel;
using System.Security.Claims;

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
       
        #region Get Loss Details
        public async Task<string> GetLossDetails(string LossID, LoggingConfiguration config)
        {
            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();
            JObject? objResponse = null;
            try
            {

                //_logger.Info("\r\n");
                //_logger.Info("\r\n");
                //_logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated Get Party Detail ");
                //_logger.Info("");

                string? baseURI = AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIURI"];
                string? URI = baseURI + "/v3/losses/lossids?includeExtendedData=false";

                //_logger.Info("\r\n");
                //_logger.Info("Request");
                //_logger.Info(URI);
                objhttpClient.DefaultRequestHeaders.Clear();
                objhttpClient.DefaultRequestHeaders.Add("userid", AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIDefaultUser"]);
                objhttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIKEY"]);
                var content = JObject.Parse("{\"Identifiers\":[\"" + LossID + "\"]}");
                var stringContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
                var result = await objhttpClient.PostAsync(URI, stringContent).Result.Content.ReadAsStringAsync();
                objResponse = JObject.Parse(result.ToString());

                //_logger.Info("\r\n");
                //_logger.Info("Response");
                //_logger.Info(result.ToString());

                //return result.ToString();
                //_logger.Info("\r\n");
                //_logger.Info("Loss Details Executed Successfully");
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

            return objResponse["loss"][0].ToString();
        }
        #endregion

        #region Get Party Details
        public async Task<string> GetPartyDetails(string partyID, LoggingConfiguration config)
        {
            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();
            try
            {

                //_logger.Info("\r\n");

                //_logger.Info("\r\n");
                //_logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated Get Party Detail ");
                //_logger.Info("");

                string baseURI = AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIURI"];
                string URI = baseURI + "/v2/parties/" + partyID + "/partyDetails";

                //_logger.Info("\r\n");
                //_logger.Info("Request");
                //_logger.Info(URI);
                objhttpClient.DefaultRequestHeaders.Clear();
                objhttpClient.DefaultRequestHeaders.Add("userid", AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIDefaultUser"]);
                objhttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIKEY"]);

                var result = await objhttpClient.GetAsync(URI).Result.Content.ReadAsStringAsync();

                //_logger.Info("\r\n");
                //_logger.Info("Response");
                //_logger.Info(result.ToString());

                return result.ToString();
                //_logger.Info("\r\n");
                //_logger.Info("Party Details Executed Successfully");
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

                //_logger.Info("\r\n");
                //_logger.Info("Request");
                //_logger.Info(URI);
                objhttpClient.DefaultRequestHeaders.Clear();
                objhttpClient.DefaultRequestHeaders.Add("userid", AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIDefaultUser"]);
                objhttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIKEY"]);

                var result = await objhttpClient.GetAsync(URI).Result.Content.ReadAsStringAsync();

                //_logger.Info("\r\n");
                //_logger.Info("Response");
                //_logger.Info(result.ToString());

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
        public void ReadClaimsPayFields(JObject extendedData, ref string ClaimsPayType, ref string ClaimsPayTypeRequest, ref string LoanAccountNumber, ref string ClaimsPayMethod, ref string approvalRequired, ref string printNow, ref string certified, ref string expedite)
        {
            foreach (dynamic Column in extendedData["column"])
            {
                if (Column["name"].Value == string.Concat("PaymentCstm.", Constants.DATAITEM_ClaimsPayType))
                {
                    if ((Column.value["@xsi:nil"] == null) || (Column.value["@xsi:nil"].Value == "false"))
                    {
                        if (Column.value.ToString().Contains("#text"))
                        {
                            ClaimsPayType = Column.value["#text"].Value;
                        }
                    }
                }
                else if (Column["name"].Value == string.Concat("PaymentCstm.", Constants.DATAITEM_ClaimsPayRequestType))
                {
                    if ((Column.value["@xsi:nil"] == null) || (Column.value["@xsi:nil"].Value == "false"))
                    {
                        if (Column.value.ToString().Contains("#text"))
                        {
                            ClaimsPayTypeRequest = Column.value["#text"].Value;
                        }
                    }
                }
                else if (Column["name"].Value == string.Concat("PaymentCstm.", Constants.DATAITEM_LoanAccountNumber))
                {
                    if ((Column.value["@xsi:nil"] == null) || (Column.value["@xsi:nil"].Value == "false"))
                    {
                        if (Column.value.ToString().Contains("#text"))
                        {
                            LoanAccountNumber = Column.value["#text"].Value;
                        }
                    }
                }
                else if (Column["name"].Value == string.Concat("PaymentCstm.", Constants.DATAITEM_ClaimsPayMethod))
                {
                    if ((Column.value["@xsi:nil"] == null) || (Column.value["@xsi:nil"].Value == "false"))
                    {
                        if (Column.value.ToString().Contains("#text"))
                        {
                            ClaimsPayMethod = Column.value["#text"].Value;
                        }
                    }

                }
                else if (Column["name"].Value == string.Concat("PaymentCstm.", Constants.DATAITEM_ApprovalRequired))
                {
                    if ((Column.value["@xsi:nil"] == null) || (Column.value["@xsi:nil"].Value == "false"))
                    {
                        bool approvalRequiredval = false;
                        if (Column.value.ToString().Contains("#text"))
                        {
                            approvalRequiredval = Convert.ToBoolean(Column.value["#text"].Value);
                        }
                        if (approvalRequiredval)
                        {
                            approvalRequired = "Y";
                        }
                        else
                        {
                            approvalRequired = "N";
                        }

                    }

                }
                else if (Column["name"].Value == string.Concat("PaymentCstm.", Constants.DATAITEM_PrintNow))
                {
                    if ((Column.value["@xsi:nil"] == null) || (Column.value["@xsi:nil"].Value == "false"))
                    {
                        if (Column.value.ToString().Contains("#text"))
                        {
                            bool printNowVal = Convert.ToBoolean(Column.value["#text"].Value);

                            if (printNowVal)
                            {
                                printNow = "Y";
                            }
                        }

                    }

                }
                else if (Column["name"].Value == string.Concat("PaymentCstm.", Constants.DATAITEM_Expedite))
                {
                    if ((Column.value["@xsi:nil"] == null) || (Column.value["@xsi:nil"].Value == "false"))
                    {
                        if (Column.value.ToString().Contains("#text"))
                        {
                            bool expediteval = Convert.ToBoolean(Column.value["#text"].Value);
                            if (expediteval)
                            {
                                expedite = "Y";
                            }
                        }

                    }

                }
                else if (Column["name"].Value == string.Concat("PaymentCstm.", Constants.DATAITEM_Certified))
                {
                    if ((Column.value["@xsi:nil"] == null) || (Column.value["@xsi:nil"].Value == "false"))
                    {
                        if (Column.value.ToString().Contains("#text"))
                        {
                            bool certifiedval = Convert.ToBoolean(Column.value["#text"].Value);
                            if (certifiedval)
                            {
                                certified = "Y";
                            }
                        }

                    }

                }

            }

        }
        #endregion

        #region Set Nlog Config 
        public LoggingConfiguration SetNlogConfig(string logPath, string logName,string paymentID)
        {
            LoggingConfiguration config = new LoggingConfiguration();
            var consoleTarget = new FileTarget
            {
                Name = "logfile",
                FileName = "" + logPath + "//" + logName + "_" + DateTime.Now.ToString("dd-MM-yyyy")+"_"+paymentID + ".txt",
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

        #region Update Payment Header Details
        public async Task UpdatePaymentHeaderDetails(string payload, string logName,string paymentID)
        {

            string? LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];
            var config = SetNlogConfig(LogPath!, logName, paymentID);
            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();

            string str = string.Empty;
            try
            {

                _logger.Info("\r\n");

                _logger.Info("\r\n");
                _logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated Update Payment Header Details ");
                _logger.Info("");

                
                string? baseURI = AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIURI"];
                string? URI = baseURI + "/v3/paymentheader";

                _logger.Info("\r\n");
                _logger.Info("Request URI");
                _logger.Info(URI);

                _logger.Info("\r\n");
                _logger.Info("Request to DC Payment Header Update");
                _logger.Info(payload.ToString());

                objhttpClient.DefaultRequestHeaders.Clear();
                objhttpClient.DefaultRequestHeaders.Add("userid", AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIDefaultUser"]);
                objhttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIKEY"]);

                var content = JObject.Parse(payload);
                var stringContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                var responsePutPaymentHeader = await objhttpClient.PutAsync(URI, stringContent);
                var resultPaymentHeaderUpdate = responsePutPaymentHeader.Content.ReadAsStringAsync().Result;

                _logger.Info("\r\n");
                _logger.Info("Response");
                _logger.Info(resultPaymentHeaderUpdate.ToString());

                //return "";// resultPaymentHeaderUpdate.ToString();
                _logger.Info("\r\n");
                _logger.Info("Update Payment Header Details Executed Successfully");
            }

            catch (Exception ex)
            {

                _logger.Info("\r\n");
                _logger.Info("Update Payment Header Details Execution failed");
                _logger.Error(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + ex, "ERROR" + ex.ToString());
                //return "";
            }
            finally
            {
                //_logger.Info("------------------------------------------------------------------ || Log End || -------------------------------------------------------------------------");
                // NLog.LogManager.Shutdown();
            }

          //return "";
        }

        #endregion

        #region Get Claims Summary
        public async Task<string> GetClaimsSummary(string claimID, LoggingConfiguration config)
        {
            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();
            try
            {

                //_logger.Info("\r\n");

                //_logger.Info("\r\n");
                //_logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated Get Party Detail ");
                //_logger.Info("");

                string baseURI = AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIURI"];

                string URI = baseURI + "/v3/Claims/" + claimID + "/ClaimSummary";

                //_logger.Info("\r\n");
                //_logger.Info("Request");
                //_logger.Info(URI);
                objhttpClient.DefaultRequestHeaders.Clear();
                objhttpClient.DefaultRequestHeaders.Add("userid", AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIDefaultUser"]);
                objhttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIKEY"]);

                var content = JObject.Parse("{\"ClaimID\":\""+claimID+"\",\"IncludeAgentsData\":false,\"IncludePerformerData\":false,\"IncludeFinancialSummaryData\":false,\"IncludeLitigationDetails\":false,\"IncludeLiabilityDetails\":false,\"IncludeExtendedDataforClaimAndClaimGroup\":false,\"includeExtendedDataForAgent\":false,\"IncludeExtendedDataForPolicy\":false,\"IncludeExtendedDataForLine\":false,\"IncludeExtendedDataForParticipantData\":false,\"IncludeExtendedDataForPerformer\":false,\"IncludeExtendedDataForClaimFinSummary\":false,\"IncludeExtendedDataForLitigationDetails\":false,\"IncludeExtendedDataForLiabilityDetails\":false,\"IncludeExtendedDataforItems\":false}");
                var stringContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                var result = await objhttpClient.PostAsync(URI,stringContent).Result.Content.ReadAsStringAsync();

                //_logger.Info("\r\n");
                //_logger.Info("Response");
                //_logger.Info(result.ToString());

                return result.ToString();
                //_logger.Info("\r\n");
                //_logger.Info("Party Details Executed Successfully");
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


    }
}
