using ClaimsPay.Modules.ClaimsPay.Models;
using ClaimsPay.Modules.ClaimsPay.Models.Comman_Model;
using ClaimsPay.Shared;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog.Fluent;
using NLog.Web;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using AppConfig = ClaimsPay.Shared.AppConfig;
using Constants = ClaimsPay.Modules.ClaimsPay.Models.Constants;
using ClaimsPay.Modules.ClaimsPay.Models.CreateVendor;
using NLog;
using System.Xml;
using NLog.Targets;
using NLog.Config;
using LogLevel = NLog.LogLevel;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Logging;

namespace ClaimsPay.Modules.ClaimsPay.DataHandler
{
    public class ClaimsPayDataHandler
    {

        #region Initialize Variables

        AppHttpClient appHttpClient = new();
        HttpClient objhttpClient = new();
        string baseURL = string.Empty;


        #endregion

        #region Endpoints Methods        

        #region Webhook
        public async Task<JObject> ClaimsPayDataHandlerWebhook(JObject requestJson)
        {
            string LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];

            var _logger = NLogBuilder.ConfigureNLog(SetNlogConfig(LogPath, "Webhook")).GetCurrentClassLogger();

            _logger.Info("\r\n");
            _logger.Info("-------------------------------------------------------------------|| Log Start || -------------------------------------------------------------------------");
            _logger.Info("\r\n");
            _logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated Webhook ");
            _logger.Info("\r\n");
            _logger.Info("Request");
            //log input recieved from DC Claims
            _logger.Info(requestJson.Root);
            JObject json = new JObject();

            try
            {
                //Write Logic Here
            }
            catch (Exception ex)
            {

                throw;
            }

            json = JObject.Parse(requestJson.Root.ToString());
            return json;
        }
        #endregion

        #region Update Profile

        public async Task<JObject> ClaimsPayDataHandlerUpdateProfile(JObject requestJson)
        {
            string LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];

            var _logger = NLogBuilder.ConfigureNLog(SetNlogConfig(LogPath, "UpdateProfile")).GetCurrentClassLogger();

            _logger.Info("\r\n");
            _logger.Info("-------------------------------------------------------------------|| Log Start || -------------------------------------------------------------------------");
            _logger.Info("\r\n");
            _logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated UpdateProfile");
            _logger.Info("\r\n");
            _logger.Info("Request");
            //log input recieved from DC Claims
            _logger.Info(requestJson.Root);
            JObject json = new JObject();

            try
            {
                //Write Logic Here
            }
            catch (Exception ex)
            {

                throw;
            }

            json = JObject.Parse(requestJson.Root.ToString());
            return json;
        }

        #endregion

        #region Create Payment Master
        public async Task<JObject> ClaimsPayDataHandlerCreatePaymentMaster(JObject requestJson)
        {
            string LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];
            var config = SetNlogConfig(LogPath, "CreatePaymentMaster");
            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();

            _logger.Info("\r\n");
            _logger.Info("-------------------------------------------------------------------|| Log Start || -------------------------------------------------------------------------");
            _logger.Info("\r\n");
            _logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated Create Payment Master ");
            _logger.Info("\r\n");
            _logger.Info("Request");
            //log input recieved from DC Claims
            _logger.Info(requestJson.Root);
            JObject json = new JObject();


            json = JObject.Parse(requestJson.Root.ToString());
            return json;
        }
        #endregion

        #region Create Vendor
        public async Task<JObject> ClaimsPayDataHandlerCreateVendor(JObject objJsonRequest)
        {
            string LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];
            var config = SetNlogConfig(LogPath, "CreateVendor");
            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();

            _logger.Info("\r\n");
            _logger.Info("-------------------------------------------------------------------|| Log Start || -------------------------------------------------------------------------");
            _logger.Info("\r\n");
            _logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated Create Vendor ");
            _logger.Info("\r\n");
            _logger.Info("Request");
            
            _logger.Info(objJsonRequest.Root);
            JObject json = new JObject();
            try
            {

                
                RestData objRequestCreateVendor = new RestData();
                Str_Json objStr_Json = new Str_Json();
                PartyDetails objPartyDetails = new PartyDetails();



                if (!string.IsNullOrEmpty(objJsonRequest["PartyID"].ToString()))
                {
                    var result = await GetPartyDetails(objJsonRequest["PartyID"].ToString(), config);
                    objPartyDetails = JsonConvert.DeserializeObject<PartyDetails>(result.ToString());

                    if (objPartyDetails.partyBusinessDetail != null)
                    {
                        if (objPartyDetails.partyBusinessDetail.party.partyTypeCode == Constants.PartyType)
                        {
                            objStr_Json.BUS_TIN = objPartyDetails.partyBusinessDetail.partyBusiness.registrationID1;

                            string sessionID = await GetSessionID(config);

                           
                            if (sessionID.Length > 0)
                            {
                                objRequestCreateVendor.session = sessionID;
                                objStr_Json.BUS_BusinessId = objPartyDetails.partyID;
                                objStr_Json.BUS_Type = Constants.BUS_Type;
                                objStr_Json.BUS_SubType = await GetParticipantRole(objJsonRequest["ParticipantRolesDTO"][0]["ParticipantRole"].ToString() == "" ? "" : objJsonRequest["ParticipantRolesDTO"][0]["ParticipantRole"].ToString());

                                objStr_Json.BUS_TINType = Constants.BUS_TINType;

                                if (objPartyDetails.partyBusinessDetail != null)
                                {
                                    for (int i = 0; i < objPartyDetails.partyBusinessDetail.partyAddressDetail.Count; i++)
                                    {
                                        if (objPartyDetails.partyBusinessDetail.partyAddressDetail[i].partyAddress.isPrimary == true &&
                                            objPartyDetails.partyBusinessDetail.partyAddressDetail[i].partyAddress.addressID == objPartyDetails.partyBusinessDetail.partyAddressDetail[i].address.addressID)
                                        {
                                            objStr_Json.BUS_Street = objPartyDetails.partyBusinessDetail.partyAddressDetail[i].address.locationDetailsLine1;
                                            objStr_Json.BUS_City = objPartyDetails.partyBusinessDetail.partyAddressDetail[i].address.adminDivisionPrimary;
                                            objStr_Json.BUS_State = await GetState(objPartyDetails.partyBusinessDetail.partyAddressDetail[i].address.nationalDivisionPrimary);
                                            objStr_Json.BUS_Zipcode = objPartyDetails.partyBusinessDetail.partyAddressDetail[i].address.postalCode;
                                            objStr_Json.BUS_Country = await GetCountry(objPartyDetails.partyBusinessDetail.partyAddressDetail[i].address.countryCode.ToString().ToUpper());
                                        }

                                    }



                                    for (int i = 0; i < objPartyDetails.partyBusinessDetail.partyBusNameDetail.Count; i++)
                                    {

                                        if (objPartyDetails.partyBusinessDetail.partyBusNameDetail[i].partyName.isPrimary == true &&
                                            objPartyDetails.partyBusinessDetail.partyBusNameDetail[i].partyName.partyNameID == objPartyDetails.partyBusinessDetail.partyBusNameDetail[i].partyBusName.partyBusNameID)
                                        {
                                            objStr_Json.BUS_Name = objPartyDetails.partyBusinessDetail.partyBusNameDetail[i].partyBusName.name.ToUpper();
                                        }
                                    }

                                }
                                objStr_Json.BUS_Status = Constants.BUS_Status;

                                objRequestCreateVendor.str_json = objStr_Json;

                                var opt = new JsonSerializerOptions() { WriteIndented = true };
                                string strJson = System.Text.Json.JsonSerializer.Serialize<Str_Json>(objStr_Json, opt);
                                var loginParams = JObject.Parse(strJson);
                                JObject createVendorPaymentRequest = new JObject(
                                  new JProperty("session", sessionID),
                                  new JProperty("str_json", strJson.ToString()));
                                _logger.Info("\r\n");
                                _logger.Info("Request");
                                _logger.Info(strJson);

                                baseURL = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["ClaimsPayURI"];
                                string lURL = baseURL + "?method=CreateVendor&input_type=JSON&response_type=JSON&rest_data=" + System.Web.HttpUtility.UrlEncode(createVendorPaymentRequest.ToString());

                                var response = objhttpClient.PostAsJsonAsync(lURL, "").Result.Content.ReadAsStringAsync();
                                json = JObject.Parse(response.Result.ToString());

                                _logger.Info("\r\n");
                                _logger.Info("Response");
                                _logger.Info(json);

                                _logger.Info("\r\n");
                                _logger.Info("Create Vendor Executed Successfully");
                            }
                            else
                            {
                                _logger.Info("\r\n");
                                _logger.Info("Session id not created");
                                //Log error session id not created
                            }
                        }
                       
                    }
                    else
                    {
                        json = JObject.Parse("{\r\n\t\"Status\": \"Failed\",\r\n\t\"Message\": \"Party type is not bussiness\"\r\n}");
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.Info("\r\n");
                _logger.Info("Create Vendor Execution failed");
                _logger.Error(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + ex, "ERROR" + ex.ToString());

            }
            finally
            {
                _logger.Info("------------------------------------------------------------------ || Log End || -------------------------------------------------------------------------");
                NLog.LogManager.Shutdown();
            }

            return json;
        }
        #endregion

        #region Get Payment Status
        public async Task<JObject> ClaimsPayDataHandlerGetPaymentStatus(JObject objJsonRequest)
        {

            RestData objRequest = new RestData();
            Str_Json objStr_Json = new Str_Json();

            string LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];
            var config = SetNlogConfig(LogPath, "GetPaymentStatus");
            JObject response = null;
            string sessionID = await GetSessionID(config);
            if (sessionID.Length > 0)
            {
                objStr_Json.PM_PaymentID = "08DB0E7AF248CB47";
                objRequest.session = sessionID;
                objRequest.str_json = objStr_Json;

                var opt = new JsonSerializerOptions() { WriteIndented = true };
                string strJson = System.Text.Json.JsonSerializer.Serialize<RestData>(objRequest, opt);

                var loginParams = JObject.Parse(strJson);
                baseURL = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["ClaimsPayURI"]; ;
                string lURL = baseURL + "?method=GetPaymentStatus&input_type=JSON&response_type=JSON&rest_data=" + System.Web.HttpUtility.UrlEncode(strJson);

                var result = objhttpClient.PostAsJsonAsync(lURL, "").Result.Content.ReadAsStringAsync();
                response = JObject.Parse(result.Result.ToString());
            }
            else
            {
                response = JObject.Parse("{\"Status\":\"failed\",\"Message\":\"Invalid Session ID\"}");
            }
            return response;
        }
        #endregion

        #region Stop Payment
        public async Task<JObject> ClaimsPayDataHandlerStopPayment(JObject objJsonRequest)
        {
            RestData objRequest = new RestData();
            Str_Json objStr_Json = new Str_Json();

            string LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];
            var config = SetNlogConfig(LogPath, "StopPayment");

            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();

            _logger.Info("\r\n");
            _logger.Info("-------------------------------------------------------------------|| Log Start || -------------------------------------------------------------------------");
            _logger.Info("\r\n");
            _logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated Stop Payment ");
            _logger.Info("\r\n");
            _logger.Info("Request");
            //log input recieved from DC Claims
            _logger.Info(objJsonRequest.Root);

            JObject response = null;
            try
            {
                _logger.Info("\r\n");
                _logger.Info("Get Session Id Execution Started");

             
                string sessionID = await GetSessionID(config);

                _logger.Info("\r\n");
                _logger.Info("Session ID : " + sessionID);
                _logger.Info("\r\n");
                _logger.Info("Get Session Id Execution Succefully");


                if (sessionID.Length > 0)
                {
                    objStr_Json.PM_PaymentID = "PMTPMM001PL06666123";
                    objRequest.session = sessionID;
                    objRequest.str_json = objStr_Json;

                    var opt = new JsonSerializerOptions() { WriteIndented = true };
                    string strJson = System.Text.Json.JsonSerializer.Serialize<Str_Json>(objStr_Json, opt);
                    JObject objStopPaymentRequest = new JObject(
                                    new JProperty("session", sessionID),
                                    new JProperty("str_json", strJson));

                    _logger.Info("\r\n");
                    _logger.Info("Request");
                    _logger.Info(strJson);

                    baseURL = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["ClaimsPayURI"]; ;
                    string lURL = baseURL + "?method=StopPayment&input_type=JSON&response_type=JSON&rest_data=" + System.Web.HttpUtility.UrlEncode(objStopPaymentRequest.ToString());

                    var result = objhttpClient.PostAsJsonAsync(lURL, "").Result.Content.ReadAsStringAsync();
                    response = JObject.Parse(result.Result.ToString());

                    _logger.Info("\r\n");
                    _logger.Info("Response");
                    _logger.Info(response.ToString());

                    _logger.Info("\r\n");
                    _logger.Info("Create Stop Payment Executed Successfully");

                }
                else
                {
                    _logger.Info("\r\n");
                    _logger.Info("Session id not created");
                }
            }
            catch (Exception ex)
            {
                _logger.Info("\r\n");
                _logger.Info("Stop Payment Execution failed");
                _logger.Error(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + ex, "ERROR" + ex.ToString());
            }
            finally
            {
                _logger.Info("------------------------------------------------------------------ || Log End || -------------------------------------------------------------------------");
                NLog.LogManager.Shutdown();
            }

            return response;
        }
        #endregion

        #region Resend Emails
        public async Task<JObject> ClaimsPayDataHandlerResendEmail(JObject objJsonRequest)
        {
            JObject response = null;
            RestData objRequest = new RestData();
            Str_Json objStr_Json = new Str_Json();

            string LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];
            var config = SetNlogConfig(LogPath, "ResendEmail");
            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();

            _logger.Info("\r\n");
            _logger.Info("-------------------------------------------------------------------|| Log Start || -------------------------------------------------------------------------");
            _logger.Info("\r\n");
            _logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated Resend Email ");
            _logger.Info("\r\n");
            _logger.Info("Request");
            //log input recieved from DC Claims
            _logger.Info(objJsonRequest.Root);

            try
            {
                _logger.Info("\r\n");
                _logger.Info("Get Session Id Execution Started");

                string sessionID = await GetSessionID(config);

                _logger.Info("\r\n");
                _logger.Info("Session ID : " + sessionID);
                _logger.Info("\r\n");
                _logger.Info("Get Session Id Execution Succefully");

                if (sessionID.Length > 0)
                {
                    objStr_Json.PM_PaymentID = objJsonRequest["PaymentHeaderDTO"]["PaymentID"].ToString();
                    objRequest.session = sessionID;
                    objRequest.str_json = objStr_Json;

                    var opt = new JsonSerializerOptions() { WriteIndented = true };
                    string strJson = System.Text.Json.JsonSerializer.Serialize<Str_Json>(objStr_Json, opt);

                    JObject objResendEmailsRequest = new JObject(
                                  new JProperty("session", sessionID),
                                  new JProperty("str_json", strJson.ToString()));
                    _logger.Info("\r\n");
                    _logger.Info("Request");
                    _logger.Info(strJson);

                    baseURL = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["ClaimsPayURI"]; ;
                    string lURL = baseURL + "?method=StopPayment&input_type=JSON&response_type=JSON&rest_data=" + System.Web.HttpUtility.UrlEncode(objResendEmailsRequest.ToString());

                    var result = objhttpClient.PostAsJsonAsync(lURL, "").Result.Content.ReadAsStringAsync();
                    response = JObject.Parse(result.Result.ToString());

                    _logger.Info("\r\n");
                    _logger.Info("Response");
                    _logger.Info(response);

                    _logger.Info("\r\n");
                    _logger.Info("Resend Email Executed Successfully");
                }
                else
                {
                    _logger.Info("\r\n");
                    _logger.Info("Session id not created");
                }
            }
            catch (Exception ex)
            {
                _logger.Info("\r\n");
                _logger.Info("Create Vendor Execution failed");
                _logger.Error(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + ex, "ERROR" + ex.ToString());
            }
            finally
            {
                _logger.Info("------------------------------------------------------------------ || Log End || -------------------------------------------------------------------------");
                NLog.LogManager.Shutdown();
            }

            return response;

        }

        #endregion

        #region Get Session Id
        public async Task<string> GetSessionID(LoggingConfiguration config)
        {
            string LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];
            JObject json = new JObject();
            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();
            try
            {
                _logger.Info("\r\n");
                //_logger.Info("-------------------------------------------------------------------|| Log Start || -------------------------------------------------------------------------");
                _logger.Info("\r\n");
                _logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated GetSessionID ");
                _logger.Info("");

                UserAuthenticationModel objUserAuthenticationModel = new UserAuthenticationModel();
                user_auth objUserAuth = new user_auth();

                objUserAuth.user_name = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["ClaimsPayUsername"];
                objUserAuth.password = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["ClaimsPayPassword"];
                objUserAuth.version = "1";

                objUserAuthenticationModel.user_auth = objUserAuth;

                var opt = new JsonSerializerOptions() { WriteIndented = true };
                string strJson = System.Text.Json.JsonSerializer.Serialize<UserAuthenticationModel>(objUserAuthenticationModel, opt);
                var loginParams = JObject.Parse(strJson);
                baseURL = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["ClaimsPayURI"]; ;
                string lURL = baseURL + "?method=Login&input_type=JSON&response_type=JSON&rest_data=" + System.Web.HttpUtility.UrlEncode(strJson);

                _logger.Info("\r\n");
                _logger.Info("Request");
                _logger.Info(strJson);

                var result = objhttpClient.PostAsJsonAsync(lURL, "").Result.Content.ReadAsStringAsync();
                json = JObject.Parse(result.Result.ToString());

                _logger.Info("\r\n");
                _logger.Info("Response");
                _logger.Info(json);


                _logger.Info("\r\n");
                _logger.Info("GetSessionId Executed Successfully");

            }
            catch (Exception ex)
            {

                _logger.Info("\r\n");
                _logger.Info("GetSessionId Execution failed");
                _logger.Error(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + ex, "ERROR" + ex.ToString());

            }
            finally
            {
                //Logging(logRequest,logResponse,logTime,logType,logMessage,logMethodName)
                // _logger.Info("------------------------------------------------------------------ || Log End || -------------------------------------------------------------------------");
                // NLog.LogManager.Shutdown();
            }

            return json["id"].ToString();
        }

        #endregion

        #endregion

        #region Other Methods

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
                _logger.Info("Create Vendor Execution failed");
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
            JObject objStateKeys = JObject.Parse(File.ReadAllText(@"Shared\\CountryKeys.json"));
            string value = objStateKeys[key_name].ToString();
            return value;
        }

        #endregion

        #region Get State
        public async Task<string> GetState(string key_name)
        {
            JObject objStateKeys = JObject.Parse(File.ReadAllText(@"Shared\\StateKeys.json"));
            string value = objStateKeys[key_name].ToString();
            return value;
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

        #endregion
    }
}
