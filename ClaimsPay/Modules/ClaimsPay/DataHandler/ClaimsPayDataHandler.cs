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

using Microsoft.Extensions.Logging;
using System.Text;
using System.Runtime.InteropServices;
using ClaimsPay.Modules.ClaimsPay.Models.Webhook;

namespace ClaimsPay.Modules.ClaimsPay.DataHandler
{
    public class ClaimsPayDataHandler
    {

        #region Initialize Variables

        AppHttpClient appHttpClient = new();
        HttpClient objhttpClient = new();
        Helper helper = new Helper();
        string? baseURL = string.Empty;
        string? ClaimsPayType = string.Empty;
        string? ClaimsPayTypeRequest = string.Empty;
        string? LoanAccountNumber = string.Empty;
        string? ClaimsPayMethod = string.Empty;
        string? approvalRequired = string.Empty;

        #endregion

        #region Endpoints Methods        

        #region Update Profile

        public async Task<JObject> ClaimsPayDataHandlerUpdateProfile(JObject requestJson)
        {
            string? LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];
            var config = helper.SetNlogConfig(LogPath!, "UpdateProfile");
            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();

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
            string? LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];
            var config = helper.SetNlogConfig(LogPath!, "CreatePaymentMaster");
            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();

            RestData? objRestData = new RestData();
            Str_Json? objStrJson = new Str_Json();
            DTOModel? objDTOModel = new DTOModel();
            PartyDetails? objPartyDetails = new PartyDetails();
            JObject? json = new JObject();
            try
            {
                _logger.Info("\r\n");
                _logger.Info("-------------------------------------------------------------------|| Log Start || -------------------------------------------------------------------------");
                _logger.Info("\r\n");
                _logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated Create Payment Master ");
                _logger.Info("\r\n");
                _logger.Info("Request");
                //log input recieved from DC Claims
                _logger.Info(requestJson.Root);

               
               
                var result = await helper.GetPartyDetails(requestJson["ParticipantDataObject"]["PartyID"].ToString(), config);
                objPartyDetails = JsonConvert.DeserializeObject<PartyDetails>(result.ToString())!;

                objDTOModel = JsonConvert.DeserializeObject<DTOModel>(requestJson.ToString());

                string sessionID = await GetSessionID(config);


                if (sessionID.Length > 0)
                {    

                    //Read Payment Headers
                    JObject? objextendedData = JObject.Parse(requestJson["__extendedData"]["extendeddata"]["table"]["entitydata"]["columns"].ToString());
                    await helper.ReadClaimsPayFields(objextendedData!);


                    //Mapping From extended data
                    objStrJson.PM_LoanAccountNumber = LoanAccountNumber;
                    objStrJson.PM_CarrierId = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["CarrierID"]; ;
                    objStrJson.PM_PaymentID = objDTOModel.PaymentHeader.PaymentID;
                    objStrJson.PM_Amount = objDTOModel.PaymentHeader.TotalApprovedAmount;

                    if (ClaimsPayType == Constants.C_KEY_Contacts)
                    {
                        objStrJson.PM_PaymentType = Constants.C_KEY_Contacts_Value;
                    }
                    else if (ClaimsPayType == Constants.C_KEY_Other)
                    {
                        objStrJson.PM_PaymentType = Constants.C_KEY_Other_Value;
                    }
                    else if (ClaimsPayType == Constants.C_KEY_Vendor)
                    {
                        objStrJson.PM_PaymentType = Constants.C_KEY_Vendor_Value;
                    }
                    else if (ClaimsPayType == Constants.C_KEY_Lienholder)
                    {
                        objStrJson.PM_PaymentType = Constants.C_KEY_Lienholder_Value;
                    }
                    else if (ClaimsPayType == Constants.C_KEY_Contacts_and_Vendor)
                    {
                        objStrJson.PM_PaymentType = Constants.C_KEY_Contacts_and_Vendor_Value;
                    }
                    else if (ClaimsPayType == Constants.C_KEY_Contacts_and_Mortgagee)
                    {
                        objStrJson.PM_PaymentType = Constants.C_KEY_Contacts_and_Mortgagee_Value;
                    }

                    if (ClaimsPayTypeRequest == Constants.C_KEY_Recoverable_Depreciation)
                    {
                        objStrJson.PM_RequestReason = Constants.C_KEY_Recoverable_Depreciation_Value;
                    }
                    else if (ClaimsPayTypeRequest == Constants.C_KEY_Emergency_Funds)
                    {
                        objStrJson.PM_RequestReason = Constants.C_KEY_Emergency_Funds_Value;
                    }
                    else if (ClaimsPayTypeRequest == Constants.C_KEY_Loss_Payment)
                    {
                        objStrJson.PM_RequestReason = Constants.C_KEY_Loss_Payment_Value;
                    }
                    else if (ClaimsPayTypeRequest == Constants.C_KEY_Supplemental_Payment)
                    {
                        objStrJson.PM_RequestReason = Constants.C_KEY_Supplemental_Payment_Value;
                    }

                    //Mapping For payment method Detail
                    if (ClaimsPayMethod == Constants.C_KEY_Mail_Prepaid_Card)
                    {
                        objStrJson.PMETHOD = Constants.C_KEY_Mail_Prepaid_Card_Value;
                    }
                    else if (ClaimsPayMethod == Constants.C_KEY_Virtual_Card)
                    {
                        objStrJson.PMETHOD = Constants.C_KEY_Virtual_Card_Value;
                    }
                    else if (ClaimsPayMethod == Constants.C_KEY_Debit_Card)
                    {
                        objStrJson.PMETHOD = Constants.C_KEY_Debit_Card_Value;
                    }
                    else if (ClaimsPayMethod == Constants.C_KEY_Field_Payment)
                    {
                        objStrJson.PMETHOD = Constants.C_KEY_Field_Payment_Value;
                    }
                    else if (ClaimsPayMethod == Constants.C_KEY_Check)
                    {
                        objStrJson.PMETHOD = Constants.C_KEY_Check_Value;
                    }
                    else if (ClaimsPayMethod == Constants.C_KEY_Direct_Deposit)
                    {
                        objStrJson.PMETHOD = Constants.C_KEY_Direct_Deposit_Value;
                    }
                    else if (ClaimsPayMethod == Constants.C_KEY_Instant_Prepaid_Card)
                    {
                        objStrJson.PMETHOD = Constants.C_KEY_Instant_Prepaid_Card_Value;
                    }
                    else if (ClaimsPayMethod == Constants.C_KEY_Let_Customer_Pickup)
                    {
                        objStrJson.PMETHOD = Constants.C_KEY_Let_Customer_Pickup_Value;
                    }
                    else if (ClaimsPayMethod == Constants.C_KEY_Prepaid_Card)
                    {
                        objStrJson.PMETHOD = Constants.C_KEY_Prepaid_Card_Value;
                    }

                    if (objStrJson.PM_PaymentType == Constants.C_KEY_Contacts_and_Mortgagee_Value)
                    {
                        objStrJson.PA_City = objDTOModel.AddressDTO_MailTo.AdminDivisionPrimary;
                        objStrJson.PA_Country = "USA";
                        objStrJson.PA_State = "CA";
                        objStrJson.PA_Street = objDTOModel.AddressDTO_MailTo.LocationDetailsLine1;
                        objStrJson.PA_Zipcode = objDTOModel.AddressDTO_MailTo.PostalCode;
                    }



                    //Mapping From Claim DTO
                    objStrJson.CL_ClaimNumber = objDTOModel.ClaimDTO.ClaimID;
                    objStrJson.CL_DateofLoss = Convert.ToDateTime(await helper.GetLossDetails(objDTOModel.ClaimDTO.LossID, config)).ToString("yyyy-MM-dd");
                    objStrJson.CL_PolicyNumber = objDTOModel.ClaimDTO.PolicyID;
                    // objStrJson.CL_DateofLoss = objDTOModel.ClaimDTO.LossID;

                    //Mapping From Line DTO
                    objStrJson.CL_CauseofLoss = await helper.GetCauseOfLoss(objDTOModel.LineDTOs[0].CauseOfLoss);

                    //Mapping From ParticipantDTO
                    objStrJson.PCON_ContactId = objDTOModel.ParticipantDataObject.PartyID;
                    objStrJson.SCON_ContactId = null;
                    objStrJson.BUS_BusinessId = objDTOModel.ParticipantDataObject.PartyID;
                    objStrJson.BUS_Type = await helper.GetParticipantRole(objDTOModel.ParticipantDataObject.ParticipantRolesDTO[0].ParticipantRole.ToString() == "" ? "" : objDTOModel.ParticipantDataObject.ParticipantRolesDTO[0].ParticipantRole);
                    objStrJson.BUS_TIN = objDTOModel.ParticipantDataObject.TaxID;

                    //Get Party Details 
                    if (!string.IsNullOrEmpty(objStrJson.PCON_ContactId))
                    {
                        var responsePartyDetails = await helper.GetPartyDetails(objStrJson.PCON_ContactId, config);
                        objPartyDetails = JsonConvert.DeserializeObject<PartyDetails>(responsePartyDetails.ToString());

                        //Mapping From PartyDetail 
                        if (objPartyDetails != null)
                        {
                            //Party Business Detail
                            if (objPartyDetails.partyBusinessDetail != null)
                            {
                                //partyPhone array mapping
                                if (objPartyDetails.partyBusinessDetail.partyPhone != null)
                                {
                                    for (int i = 0; i < objPartyDetails.partyBusinessDetail.partyPhone.Count; i++)
                                    {
                                        if (objPartyDetails.partyBusinessDetail.partyPhone[i].isPrimary == true)
                                        {
                                            objStrJson.PCON_MobilePhone = "";
                                            objStrJson.SCON_MobilePhone = "";
                                        }
                                    }
                                }

                                //partyEmail array mapping
                                if (objPartyDetails.partyBusinessDetail.partyEmail != null)
                                {

                                    for (int i = 0; i < objPartyDetails.partyBusinessDetail.partyEmail.Count; i++)
                                    {
                                        if (objPartyDetails.partyBusinessDetail.partyEmail[i].isPrimary == true)
                                        {
                                            objStrJson.PCON_EmailAddress = "";
                                            objStrJson.SCON_EmailAddress = "";
                                        }
                                    }
                                }

                                //partyBusNameDetail array mapping
                                if (objPartyDetails.partyBusinessDetail.partyBusNameDetail != null)
                                {
                                    for (int i = 0; i < objPartyDetails.partyBusinessDetail.partyBusNameDetail.Count; i++)
                                    {
                                        if (objPartyDetails.partyBusinessDetail.partyBusNameDetail[i].partyBusName.partyBusNameID ==
                                            objPartyDetails.partyBusinessDetail.partyBusNameDetail[i].partyName.partyNameID
                                            && objPartyDetails.partyBusinessDetail.partyBusNameDetail[i].partyName.isPrimary == true)
                                        {
                                            objStrJson.BUS_Name = objPartyDetails.partyBusinessDetail.partyBusNameDetail[i].partyBusName.name;
                                           
                                        }
                                    }
                                }

                                //partyAddressDetail array mapping
                                for (int i = 0; i < objPartyDetails.partyBusinessDetail.partyAddressDetail.Count; i++)
                                {
                                    if (objPartyDetails.partyBusinessDetail.partyAddressDetail[i].partyAddress.isPrimary == true &&
                                        objPartyDetails.partyBusinessDetail.partyAddressDetail[i].partyAddress.addressID == objPartyDetails.partyBusinessDetail.partyAddressDetail[i].address.addressID)
                                    {
                                        objStrJson.BUS_Street = objPartyDetails.partyBusinessDetail.partyAddressDetail[i].address.locationDetailsLine1;
                                        objStrJson.BUS_City = objPartyDetails.partyBusinessDetail.partyAddressDetail[i].address.adminDivisionPrimary;
                                        objStrJson.BUS_State = await helper.GetState(objPartyDetails.partyBusinessDetail.partyAddressDetail[i].address.nationalDivisionPrimary);
                                        objStrJson.BUS_Zipcode = objPartyDetails.partyBusinessDetail.partyAddressDetail[i].address.postalCode;
                                        objStrJson.BUS_Country = await helper.GetCountry(objPartyDetails.partyBusinessDetail.partyAddressDetail[i].address.countryCode.ToString().ToUpper());
                                    }

                                }

                            }
                            //Party Individual Detail
                            else if (objPartyDetails.partyIndividualDetail != null)
                            {

                                //partyIndNameDetail array mapping
                                if (objPartyDetails.partyIndividualDetail.partyIndNameDetail != null)
                                {
                                    for (int i = 0; i < objPartyDetails.partyIndividualDetail.partyIndNameDetail.Count; i++)
                                    {
                                        if (objPartyDetails.partyIndividualDetail.partyIndNameDetail[i].partyIndName.partyIndNameID ==
                                            objPartyDetails.partyIndividualDetail.partyIndNameDetail[i].partyName.partyNameID
                                            && objPartyDetails.partyIndividualDetail.partyIndNameDetail[i].partyName.isPrimary == true)
                                        {
                                            objStrJson.PCON_FirstName = objPartyDetails.partyIndividualDetail.partyIndNameDetail[i].partyIndName.firstName;
                                            objStrJson.PCON_LastName = objPartyDetails.partyIndividualDetail.partyIndNameDetail[i].partyIndName.lastName;

                                            objStrJson.SCON_FirstName = "";
                                            objStrJson.SCON_LastName = "";
                                        }
                                    }
                                }

                                //partyEmail array mapping
                                if (objPartyDetails.partyIndividualDetail.partyEmail != null)
                                {

                                    for (int i = 0; i < objPartyDetails.partyIndividualDetail.partyEmail.Count; i++)
                                    {
                                        if (objPartyDetails.partyIndividualDetail.partyEmail[i].isPrimary == true)
                                        {
                                            objStrJson.PCON_EmailAddress = objPartyDetails.partyIndividualDetail.partyEmail[i].emailAddress;
                                            objStrJson.SCON_EmailAddress = objPartyDetails.partyIndividualDetail.partyEmail[i].emailAddress;
                                        }
                                    }
                                }

                                //partyPhone array mapping
                                if (objPartyDetails.partyIndividualDetail.partyPhone != null)
                                {

                                    for (int i = 0; i < objPartyDetails.partyIndividualDetail.partyPhone.Count; i++)
                                    {
                                        if (objPartyDetails.partyIndividualDetail.partyPhone[i].isPrimary == true)
                                        {
                                            objStrJson.SCON_MobilePhone = "";
                                            objStrJson.PCON_MobilePhone = objPartyDetails.partyIndividualDetail.partyPhone[i].fullPhoneNumber;
                                        }
                                    }
                                }

                                //partyAddressDetail array mapping
                                for (int i = 0; i < objPartyDetails.partyIndividualDetail.partyAddressDetail.Count; i++)
                                {
                                    if (objPartyDetails.partyIndividualDetail.partyAddressDetail[i].partyAddress.isPrimary == true &&
                                        objPartyDetails.partyIndividualDetail.partyAddressDetail[i].partyAddress.addressID == objPartyDetails.partyIndividualDetail.partyAddressDetail[i].address.addressID)
                                    {
                                        objStrJson.BUS_Street = objPartyDetails.partyIndividualDetail.partyAddressDetail[i].address.locationDetailsLine1;
                                        objStrJson.BUS_City = objPartyDetails.partyIndividualDetail.partyAddressDetail[i].address.adminDivisionPrimary;
                                        objStrJson.BUS_State = await helper.GetState(objPartyDetails.partyIndividualDetail.partyAddressDetail[i].address.nationalDivisionPrimary);
                                        objStrJson.BUS_Zipcode = objPartyDetails.partyIndividualDetail.partyAddressDetail[i].address.postalCode;
                                        objStrJson.BUS_Country = await helper.GetCountry(objPartyDetails.partyIndividualDetail.partyAddressDetail[i].address.countryCode.ToString().ToUpper());
                                    }

                                }

                            }
                        }
                        int numIndividual = 0;

                        if (objDTOModel.PaymentPayeeDataObjectsList.Count > 1)
                        {
                            for (int i = 0; i < objDTOModel.PaymentPayeeDataObjectsList.Count; i++)
                            {
                                var responsePartyDetails2 = await helper.GetPartyDetails(objDTOModel.PaymentPayeeDataObjectsList[i].ClientID, config);
                                PartyDetails? objPartyDetails2 = JsonConvert.DeserializeObject<PartyDetails>(responsePartyDetails2.ToString());

                                if (objPartyDetails2.partyType == Constants.PartyTypeIndividual)
                                {
                                    if (objPartyDetails.partyType == Constants.PartyType && numIndividual < 1)
                                    {
                                        for (int j = 0; j < objPartyDetails2.partyIndividualDetail.partyIndNameDetail.Count; j++)
                                        {
                                            if (objPartyDetails2.partyIndividualDetail.partyIndNameDetail[j].partyIndName.partyIndNameID ==
                                                objPartyDetails2.partyIndividualDetail.partyIndNameDetail[j].partyName.partyNameID
                                                && objPartyDetails2.partyIndividualDetail.partyIndNameDetail[j].partyName.isPrimary == true)
                                            {
                                                objStrJson.PCON_FirstName = objPartyDetails2.partyIndividualDetail.partyIndNameDetail[j].partyIndName.firstName;
                                                objStrJson.PCON_LastName = objPartyDetails2.partyIndividualDetail.partyIndNameDetail[j].partyIndName.lastName;

                                            }

                                            if (objPartyDetails2.partyIndividualDetail.partyPhone[j].isPrimary == true)
                                            {
                                                objStrJson.PCON_MobilePhone = objPartyDetails2.partyIndividualDetail.partyPhone[j].fullPhoneNumber;
                                            }
                                            if (objPartyDetails2.partyIndividualDetail.partyEmail[j].isPrimary == true)
                                            {
                                                objStrJson.PCON_EmailAddress = objPartyDetails2.partyIndividualDetail.partyEmail[j].emailAddress;
                                            }
                                        }

                                        if (objStrJson.PM_PaymentType == Constants.C_KEY_Contacts_and_Vendor_Value)
                                        {
                                            if (string.IsNullOrEmpty(approvalRequired))
                                                objStrJson.PCON_Approval_Reqd = "Y";
                                            else
                                                objStrJson.PCON_Approval_Reqd = "N";
                                        }


                                    }
                                    else
                                    {
                                        for (int j = 0; j < objPartyDetails2.partyIndividualDetail.partyIndNameDetail.Count; j++)
                                        {
                                            if (objPartyDetails2.partyIndividualDetail.partyIndNameDetail[j].partyIndName.partyIndNameID ==
                                                objPartyDetails2.partyIndividualDetail.partyIndNameDetail[j].partyName.partyNameID
                                                && objPartyDetails2.partyIndividualDetail.partyIndNameDetail[j].partyName.isPrimary == true)
                                            {
                                                objStrJson.SCON_FirstName = "";
                                                objStrJson.SCON_LastName = "";

                                            }

                                            if (objPartyDetails2.partyIndividualDetail.partyPhone[j].isPrimary == true)
                                            {
                                                objStrJson.SCON_MobilePhone = "";
                                            }
                                            if (objPartyDetails2.partyIndividualDetail.partyEmail[j].isPrimary == true)
                                            {
                                                objStrJson.SCON_EmailAddress = "";
                                            }
                                        }

                                        if (objStrJson.PM_PaymentType == Constants.C_KEY_Contacts_and_Vendor_Value)
                                        {
                                            if (string.IsNullOrEmpty(approvalRequired))
                                            {
                                                objStrJson.SCON_Approval_Reqd = "Y";

                                            }

                                            else
                                            {
                                                objStrJson.SCON_Approval_Reqd = "N";
                                            }
                                        }
                                    }
                                }
                                else if (objPartyDetails2.partyType == Constants.PartyType)
                                {
                                    if (objStrJson.PM_PaymentType == Constants.C_KEY_Contacts_and_Vendor_Value)
                                    {
                                        if (string.IsNullOrEmpty(approvalRequired))
                                        {
                                            objStrJson.BUS_Type = Constants.BUS_Type;
                                            objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
                                        }

                                        else
                                        {
                                            objStrJson.SCON_Approval_Reqd = "N";
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //Mapping From Performer DTO
                    objStrJson.PM_UserId = objDTOModel.ParticipantDataObject.PartyID;
                    objStrJson.PM_User_FirstName = objDTOModel.PerformerDTO.PerformerNameDetailDTO.FirstName;
                    objStrJson.PM_User_LastName = objDTOModel.PerformerDTO.PerformerNameDetailDTO.LastName;
                    objStrJson.PM_User_EmailAddress = objStrJson.PCON_EmailAddress;

                    //Mapping From AddressDTO_MailTo
                    objStrJson.PMA_Street = string.Concat(objDTOModel.AddressDTO_MailTo.LocationDetailsLine1.ToString(), " "
                        , objDTOModel.AddressDTO_MailTo.LocationDetailsLine2 == null ? "" : objDTOModel.AddressDTO_MailTo.LocationDetailsLine2);
                    objStrJson.PMA_City = objDTOModel.AddressDTO_MailTo.AdminDivisionPrimary;
                    objStrJson.PMA_State = await helper.GetState(objDTOModel.AddressDTO_MailTo.NationalDivisionPrimary);
                    objStrJson.PMA_Zipcode = objDTOModel.AddressDTO_MailTo.PostalCode;
                    objStrJson.PMA_Country = await helper.GetCountry(objDTOModel.AddressDTO_MailTo.CountryCode.ToString());
                    if (objStrJson.PM_PaymentType ==Constants.C_KEY_Contacts_Value)
                    {
                        objStrJson.PMA_MailTo = string.Concat(objStrJson.PCON_FirstName, " ", objStrJson.PCON_LastName);
                    }
                    else if (objStrJson.PM_PaymentType == Constants.C_KEY_Contacts_and_Vendor_Value)
                    {
                        objStrJson.PMA_MailTo = objStrJson.BUS_Name;
                    }
                    else if (objStrJson.PM_PaymentType == Constants.C_KEY_Contacts_and_Mortgagee_Value)
                    {
                        objStrJson.PMA_MailTo = objStrJson.BUS_Name;
                    }


                    //objStrJson.PMETHOD_DD_AccountNumber = "";
                    //objStrJson.PMETHOD_DD_RoutingNumber = "";
                    //objStrJson.PMETHOD_DD_AccountType = "";


                    var opt = new JsonSerializerOptions() { WriteIndented = true };
                    string strJson = System.Text.Json.JsonSerializer.Serialize<Str_Json>(objStrJson, opt);
                    var loginParams = JObject.Parse(strJson);

                    JObject createPaymentMasterRequest = new JObject(
                      new JProperty("session", sessionID),
                      new JProperty("str_json", strJson.ToString()));
                    _logger.Info("\r\n");
                    _logger.Info("Request To OneInc");
                    _logger.Info(strJson);
                    //createPaymentMasterRequest.Add(new JProperty("PM_PaymentType", "Contact(s) and Vendor"));

                    baseURL = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["ClaimsPayURI"];
                    string lURL = baseURL + "?method=CreatePaymentMaster&input_type=JSON&response_type=JSON&rest_data=" + System.Web.HttpUtility.UrlEncode(createPaymentMasterRequest.ToString());

                    var response = objhttpClient.PostAsJsonAsync(lURL, "").Result.Content.ReadAsStringAsync();
                    json = JObject.Parse(response.Result.ToString());

                    _logger.Info("\r\n");
                    _logger.Info("Response From OneInc");
                    _logger.Info(response.Result.ToString());

                    _logger.Info("\r\n");
                    _logger.Info("Create Payment Master Executed Successfully");
                }
                else
                {
                    _logger.Info("\r\n");
                    _logger.Info("Session id not created");
                    //Log error session id not created
                }

            }
            catch (Exception ex)
            {
                _logger.Info("\r\n");
                _logger.Info("Create Payment Master Execution failed");
                _logger.Error(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + ex, "ERROR" + ex.ToString());

            }
            finally
            {
                _logger.Info("------------------------------------------------------------------ || Log End || -------------------------------------------------------------------------");
                NLog.LogManager.Shutdown();
            }
            //json = JObject.Parse(requestJson.Root.ToString());
            return json;
        }
        #endregion

        #region Create Vendor
        public async Task<JObject> ClaimsPayDataHandlerCreateVendor(JObject objJsonRequest)
        {
            string? LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];
            var config = helper.SetNlogConfig(LogPath!, "CreateVendor");
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
                    var result = await helper.GetPartyDetails(objJsonRequest["PartyID"].ToString(), config);
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
                                objStr_Json.BUS_SubType = await helper.GetParticipantRole(objJsonRequest["ParticipantRolesDTO"][0]["ParticipantRole"].ToString() == "" ? "" : objJsonRequest["ParticipantRolesDTO"][0]["ParticipantRole"].ToString());

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
                                            objStr_Json.BUS_State = await helper.GetState(objPartyDetails.partyBusinessDetail.partyAddressDetail[i].address.nationalDivisionPrimary);
                                            objStr_Json.BUS_Zipcode = objPartyDetails.partyBusinessDetail.partyAddressDetail[i].address.postalCode;
                                            objStr_Json.BUS_Country = await helper.GetCountry(objPartyDetails.partyBusinessDetail.partyAddressDetail[i].address.countryCode.ToString().ToUpper());
                                        }

                                    }



                                    for (int i = 0; i < objPartyDetails.partyBusinessDetail.partyBusNameDetail.Count; i++)
                                    {

                                        if (objPartyDetails.partyBusinessDetail.partyBusNameDetail[i].partyName.isPrimary == true &&
                                            objPartyDetails.partyBusinessDetail.partyBusNameDetail[i].partyName.partyNameID == objPartyDetails.partyBusinessDetail.partyBusNameDetail[i].partyBusName.partyBusNameID)
                                        {
                                            objStr_Json.BUS_Name = objPartyDetails.partyBusinessDetail.partyBusNameDetail[i].partyBusName.name;
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

        #region WebHook
        public async Task<JsonDocument> ClaimsPayDataHandlerWebHook(JObject objJsonRequest)
        {
            JsonDocument? response = null;
            RestData? objRequest = new RestData();
            Str_Json? objStr_Json = new Str_Json();

            string? LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];
            var config = helper.SetNlogConfig(LogPath, "Webhook");
            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();

            _logger.Info("\r\n");
            _logger.Info("-------------------------------------------------------------------|| Log Start || -------------------------------------------------------------------------");
            _logger.Info("\r\n");
            _logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated WebHook ");
            _logger.Info("\r\n");
            _logger.Info("Request");
            //log input recieved from DC Claims
            _logger.Info(objJsonRequest.Root);

            try
            {

                var responsePaymentHeader = await helper.GetPaymentHeaderDetails(objJsonRequest["PM_CR_PaymentID"].ToString(), config);
                JObject? objreq = JObject.Parse(responsePaymentHeader.ToString());

                WebHookRequestModel? myDeserializedClass = System.Text.Json.JsonSerializer.Deserialize<WebHookRequestModel>(responsePaymentHeader.ToString());

                JObject? colObj = JObject.Parse(objreq["__extendedData"]["extendeddata"]["table"]["entitydata"]["columns"].ToString());
                JArray? columArray = JArray.Parse(colObj["column"].ToString());

                var jObjects = columArray.ToObject<List<JObject>>();

                var PM_Method_Last4Digit = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
                                         .Single(x => x.Name == Constants.DATAITEM_FQN_PM_Method_Last4Digit)
                                         .Index;
                var PM_Funded = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
                                         .Single(x => x.Name == Constants.DATAITEM_FQN_PM_Funded)
                                         .Index;
                var PM_Status = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
                                         .Single(x => x.Name == Constants.DATAITEM_FQN_PM_Status)
                                         .Index;
                var PM_PaidDate = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
                                         .Single(x => x.Name == Constants.DATAITEM_FQN_PM_PaidDate)
                                         .Index;
                var PM_ClearedDate = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
                                         .Single(x => x.Name == Constants.DATAITEM_FQN_PM_ClearedDate)
                                         .Index;
                var PM_EscheatDate = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
                                         .Single(x => x.Name == Constants.DATAITEM_FQN_PM_EscheatDate)
                                         .Index;
                var PM_MailTrackingNumber = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
                                         .Single(x => x.Name == Constants.DATAITEM_FQN_PM_MailTrackingNumber)
                                         .Index;
                var PM_Monitored = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
                                         .Single(x => x.Name == Constants.DATAITEM_FQN_PM_Monitored)
                                         .Index;
                var PM_RejectPayeeID = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
                                         .Single(x => x.Name == Constants.DATAITEM_FQN_PM_RejectPayeeID)
                                         .Index;
                var PM_RejectReason = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
                                        .Single(x => x.Name == Constants.DATAITEM_FQN_PM_RejectReason)
                                        .Index;
                var PM_Orig_Method = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
                                        .Single(x => x.Name == Constants.DATAITEM_FQN_PM_Orig_Method)
                                        .Index;
                var PM_Selection = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
                                        .Single(x => x.Name == Constants.DATAITEM_FQN_PM_Selection)
                                        .Index;
                var PM_ErrorCode = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
                                        .Single(x => x.Name == Constants.DATAITEM_FQN_PM_ErrorCode)
                                        .Index;
                var PM_ErrorMessage = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
                                        .Single(x => x.Name == Constants.DATAITEM_FQN_PM_ErrorMessage)
                                        .Index;

                foreach (var obj in jObjects)
                {
                    //For PM_Method_Last4Digit
                    if (jObjects.IndexOf(obj) == PM_Method_Last4Digit)
                    {
                        foreach (var prop in obj.Properties())
                        {
                            if (prop.Name == "value")                     //Get desired property
                            {
                                obj.Remove("value");
                                obj.Add(new JProperty("value", new JObject()));

                                JObject? value = (JObject)obj["value"];

                                //Add needed properties
                                value.Add(new JProperty("@xsi:type", new JObject()));
                                value.Add(new JProperty("#text", new JObject()));

                                //Give them values
                                value["@xsi:type"] = "xs:string";
                                value["#text"] = objJsonRequest["PM_Method_Last4Digit"].ToString();
                                break;
                            }
                        }
                    }
                    else if (jObjects.IndexOf(obj) == PM_Funded)
                    {
                        foreach (var prop in obj.Properties())
                        {
                            if (prop.Name == "value")                     //Get desired property
                            {
                                obj.Remove("value");
                                obj.Add(new JProperty("value", new JObject()));

                                JObject? value = (JObject)obj["value"];

                                //Add needed properties
                                value.Add(new JProperty("@xsi:type", new JObject()));
                                value.Add(new JProperty("#text", new JObject()));

                                //Give them values
                                value["@xsi:type"] = "xs:string";
                                value["#text"] = objJsonRequest["PM_Funded"].ToString();
                                break;
                            }
                        }
                    }
                    else if (jObjects.IndexOf(obj) == PM_Status)
                    {
                        foreach (var prop in obj.Properties())
                        {
                            if (prop.Name == "value")                     //Get desired property
                            {
                                obj.Remove("value");
                                obj.Add(new JProperty("value", new JObject()));

                                JObject? value = (JObject)obj["value"];

                                //Add needed properties
                                value.Add(new JProperty("@xsi:type", new JObject()));
                                value.Add(new JProperty("#text", new JObject()));

                                //Give them values
                                value["@xsi:type"] = "xs:string";
                                value["#text"] = objJsonRequest["PM_Status"].ToString();
                                break;
                            }
                        }
                    }
                    else if (jObjects.IndexOf(obj) == PM_PaidDate)
                    {
                        foreach (var prop in obj.Properties())
                        {
                            if (prop.Name == "value")                     //Get desired property
                            {
                                obj.Remove("value");
                                obj.Add(new JProperty("value", new JObject()));

                                JObject? value = (JObject)obj["value"];

                                //Add needed properties
                                value.Add(new JProperty("@xsi:type", new JObject()));
                                value.Add(new JProperty("#text", new JObject()));

                                //Give them values
                                value["@xsi:type"] = "xs:datetime";
                                value["#text"] = objJsonRequest["PM_PaidDate"].ToString();
                                break;
                            }
                        }
                    }
                    else if (jObjects.IndexOf(obj) == PM_ClearedDate)
                    {
                        foreach (var prop in obj.Properties())
                        {
                            if (prop.Name == "value")                     //Get desired property
                            {
                                obj.Remove("value");
                                obj.Add(new JProperty("value", new JObject()));

                                JObject? value = (JObject)obj["value"];

                                //Add needed properties
                                value.Add(new JProperty("@xsi:type", new JObject()));
                                value.Add(new JProperty("#text", new JObject()));

                                //Give them values
                                value["@xsi:type"] = "xs:datetime";
                                value["#text"] = objJsonRequest["PM_ClearedDate"].ToString();
                                break;
                            }
                        }
                    }
                    else if (jObjects.IndexOf(obj) == PM_EscheatDate)
                    {
                        foreach (var prop in obj.Properties())
                        {
                            if (prop.Name == "value")                     //Get desired property
                            {
                                obj.Remove("value");
                                obj.Add(new JProperty("value", new JObject()));

                                JObject? value = (JObject)obj["value"];

                                //Add needed properties
                                value.Add(new JProperty("@xsi:type", new JObject()));
                                value.Add(new JProperty("#text", new JObject()));

                                //Give them values
                                value["@xsi:type"] = "xs:datetime";
                                value["#text"] = objJsonRequest["PM_EscheatDate"].ToString();
                                break;
                            }
                        }
                    }
                    else if (jObjects.IndexOf(obj) == PM_MailTrackingNumber)
                    {
                        foreach (var prop in obj.Properties())
                        {
                            if (prop.Name == "value")                     //Get desired property
                            {
                                obj.Remove("value");
                                obj.Add(new JProperty("value", new JObject()));

                                JObject? value = (JObject)obj["value"];

                                //Add needed properties
                                value.Add(new JProperty("@xsi:type", new JObject()));
                                value.Add(new JProperty("#text", new JObject()));

                                //Give them values
                                value["@xsi:type"] = "xs:string";
                                value["#text"] = objJsonRequest["PM_MailTrackingNumber"].ToString();
                                break;
                            }
                        }
                    }
                    else if (jObjects.IndexOf(obj) == PM_Monitored)
                    {
                        foreach (var prop in obj.Properties())
                        {
                            if (prop.Name == "value")                     //Get desired property
                            {
                                obj.Remove("value");
                                obj.Add(new JProperty("value", new JObject()));

                                JObject? value = (JObject)obj["value"];

                                //Add needed properties
                                value.Add(new JProperty("@xsi:type", new JObject()));
                                value.Add(new JProperty("#text", new JObject()));

                                //Give them values
                                value["@xsi:type"] = "xs:string";
                                value["#text"] = objJsonRequest["PM_Monitored"].ToString();
                                break;
                            }
                        }
                    }
                    else if (jObjects.IndexOf(obj) == PM_RejectPayeeID)
                    {
                        foreach (var prop in obj.Properties())
                        {
                            if (prop.Name == "value")                     //Get desired property
                            {
                                obj.Remove("value");
                                obj.Add(new JProperty("value", new JObject()));

                                JObject? value = (JObject)obj["value"];

                                //Add needed properties
                                value.Add(new JProperty("@xsi:type", new JObject()));
                                value.Add(new JProperty("#text", new JObject()));

                                //Give them values
                                value["@xsi:type"] = "xs:string";
                                value["#text"] = objJsonRequest["PM_RejectPayeeID"].ToString();
                                break;
                            }
                        }
                    }
                    else if (jObjects.IndexOf(obj) == PM_RejectReason)
                    {
                        foreach (var prop in obj.Properties())
                        {
                            if (prop.Name == "value")                     //Get desired property
                            {
                                obj.Remove("value");
                                obj.Add(new JProperty("value", new JObject()));

                                JObject? value = (JObject)obj["value"];

                                //Add needed properties
                                value.Add(new JProperty("@xsi:type", new JObject()));
                                value.Add(new JProperty("#text", new JObject()));

                                //Give them values
                                value["@xsi:type"] = "xs:string";
                                value["#text"] = objJsonRequest["PM_RejectReason"].ToString();
                                break;
                            }
                        }
                    }
                    else if (jObjects.IndexOf(obj) == PM_Selection)
                    {
                        foreach (var prop in obj.Properties())
                        {
                            if (prop.Name == "value")                     //Get desired property
                            {
                                obj.Remove("value");
                                obj.Add(new JProperty("value", new JObject()));

                                JObject? value = (JObject)obj["value"];

                                //Add needed properties
                                value.Add(new JProperty("@xsi:type", new JObject()));
                                value.Add(new JProperty("#text", new JObject()));

                                //Give them values
                                value["@xsi:type"] = "xs:string";
                                value["#text"] = objJsonRequest["PM_Selection"].ToString();
                                break;
                            }
                        }
                    }
                    else if (jObjects.IndexOf(obj) == PM_ErrorMessage)
                    {
                        foreach (var prop in obj.Properties())
                        {
                            if (prop.Name == "value")                     //Get desired property
                            {
                                obj.Remove("value");
                                obj.Add(new JProperty("value", new JObject()));

                                JObject? value = (JObject)obj["value"];

                                //Add needed properties
                                value.Add(new JProperty("@xsi:type", new JObject()));
                                value.Add(new JProperty("#text", new JObject()));

                                //Give them values
                                value["@xsi:type"] = "xs:string";
                                value["#text"] = objJsonRequest["PM_ErrorMessage"].ToString();
                                break;
                            }
                        }
                    }



                }


                JArray? outputArray = JArray.FromObject(jObjects);         //Output array

                objreq["paymentHeaderDTO"]["totalApprovedAmount"] = objJsonRequest["PM_Amount"];
                objreq["paymentHeaderDTO"]["paymentNumber"] = objJsonRequest["PM_CheckNumber"];
                objreq["paymentHeaderDTO"]["paymentID"] = objJsonRequest["PM_CR_PaymentID"];
                //Create new object for the new array
                JObject? updatedJson = new JObject();
                updatedJson["name"] = outputArray;
                objreq["__extendedData"]["extendeddata"]["table"]["entitydata"]["columns"].Replace(updatedJson);



                string? temp = objreq.ToString();

                string? baseURI = AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIURI"];
                string? URI = baseURI + "/v3/paymentheader";

                _logger.Info("\r\n");
                _logger.Info("Request URI");
                _logger.Info(URI);

                _logger.Info("\r\n");
                _logger.Info("Request");
                _logger.Info(objreq.ToString());

                objhttpClient.DefaultRequestHeaders.Clear();
                objhttpClient.DefaultRequestHeaders.Add("userid", AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIDefaultUser"]);
                objhttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AppConfig.configuration?.GetSection($"Modules:DuckcreekConfig")["ClaimAPIKEY"]);

                var content = JObject.Parse(objreq.ToString());
                var stringContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                var responsePutPaymentHeader = await objhttpClient.PutAsync(URI, stringContent);
                var result = responsePutPaymentHeader.Content.ReadAsStringAsync();
                if (responsePutPaymentHeader.IsSuccessStatusCode)
                {
                    response = JsonDocument.Parse("{\n  \"PM_IP_PaymentID\": \"" + objJsonRequest["PM_IP_PaymentID"] + "\",\n  \"Status\": \"Success\"\n}");
                }
                else
                {
                    response = JsonDocument.Parse(result.ToString());
                }
                _logger.Info("\r\n");
                _logger.Info("Response");
                _logger.Info(response.RootElement.ToString());


                _logger.Info("\r\n");
                _logger.Info("Webhook Executed Successfully");

            }
            catch (Exception ex)
            {
                _logger.Info("\r\n");
                _logger.Info("Webhook Execution failed");
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

        #region Get Payment Status
        public async Task<JObject> ClaimsPayDataHandlerGetPaymentStatus(JObject objJsonRequest)
        {

            RestData objRequest = new RestData();
            Str_Json objStr_Json = new Str_Json();

            string LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];
            var config = helper.SetNlogConfig(LogPath, "GetPaymentStatus");
            JObject response = null;
            string sessionID = await GetSessionID(config);
            if (sessionID.Length > 0)
            {
                objStr_Json.PM_PaymentID = "08DB3FD571F4683B";
                objRequest.session = sessionID;
                objRequest.str_json = objStr_Json;

                var opt = new JsonSerializerOptions() { WriteIndented = true };
                string strJson = System.Text.Json.JsonSerializer.Serialize<Str_Json>(objStr_Json, opt);


                JObject getPaymentStatusRequest = new JObject(
                                 new JProperty("session", sessionID),
                                 new JProperty("str_json", strJson.ToString()));
                baseURL = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["ClaimsPayURI"]; ;
                string lURL = baseURL + "?method=GetPaymentStatus&input_type=JSON&response_type=JSON&rest_data=" + System.Web.HttpUtility.UrlEncode(getPaymentStatusRequest.ToString());

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
            RestData? objRequest = new RestData();
            Str_Json? objStr_Json = new Str_Json();

            string? LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];
            var config = helper.SetNlogConfig(LogPath, "StopPayment");

            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();

            _logger.Info("\r\n");
            _logger.Info("-------------------------------------------------------------------|| Log Start || -------------------------------------------------------------------------");
            _logger.Info("\r\n");
            _logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated Stop Payment ");
            _logger.Info("\r\n");
            _logger.Info("Request");
            //log input recieved from DC Claims
            _logger.Info(objJsonRequest.Root);

            JObject? response = null;
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
                    objStr_Json.PM_PaymentID = objJsonRequest["PaymentID"].ToString();
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
            JObject? response = null;
            RestData? objRequest = new RestData();
            Str_Json? objStr_Json = new Str_Json();

            string LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];
            var config = helper.SetNlogConfig(LogPath, "ResendEmail");
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
                    objStr_Json.PM_PaymentID = objJsonRequest["PaymentID"].ToString();
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
                    string lURL = baseURL + "?method=ResendEmails&input_type=JSON&response_type=JSON&rest_data=" + System.Web.HttpUtility.UrlEncode(objResendEmailsRequest.ToString());

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
            string? LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];
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
                string? strJson = System.Text.Json.JsonSerializer.Serialize<UserAuthenticationModel>(objUserAuthenticationModel, opt);
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

        
    }
}
