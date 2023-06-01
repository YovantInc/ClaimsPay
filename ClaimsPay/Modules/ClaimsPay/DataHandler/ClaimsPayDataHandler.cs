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
using System.Linq.Expressions;
using System.Linq;
using System.Collections.Generic;

namespace ClaimsPay.Modules.ClaimsPay.DataHandler
{
    public class ClaimsPayDataHandler
    {

        #region Initialize Variables

        AppHttpClient appHttpClient = new();
        HttpClient objhttpClient = new();
        Helper helper = new Helper();
        string? baseURL = string.Empty;


        #endregion

        #region Endpoints Methods        

        #region Update Profile

        public async Task<JObject> ClaimsPayDataHandlerUpdateProfile(JObject objJsonRequest)
        {
            RestData? objRequest = new RestData();
            Str_Json? objStr_Json = new Str_Json();

            string? LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];
            var config = helper.SetNlogConfig(LogPath, "UpdateProfile", "");

            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();

            _logger.Info("\r\n");
            _logger.Info("-------------------------------------------------------------------|| Log Start || -------------------------------------------------------------------------");
            _logger.Info("\r\n");
            _logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated Update Profile");
            _logger.Info("\r\n");
            _logger.Info("Request");
            //log input recieved from DC Claims
            _logger.Info(objJsonRequest.Root.ToString());

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
                    objStr_Json.PM_PaymentID = objJsonRequest["PaymentID"].ToString();
                    objRequest.session = sessionID;
                    objRequest.str_json = objStr_Json;

                    var opt = new JsonSerializerOptions() { WriteIndented = true };
                    string strJson = System.Text.Json.JsonSerializer.Serialize<Str_Json>(objStr_Json, opt);
                    JObject objUpdateProfileRequest = new JObject(
                                    new JProperty("session", sessionID),
                                    new JProperty("str_json", strJson));

                    //value.Remove();
                    //Remove(objUpdateProfileRequest["BUS_BusinessId"].ToString());

                    _logger.Info("\r\n");
                    _logger.Info("Request");
                    _logger.Info(strJson);

                    baseURL = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["ClaimsPayURI"]; ;
                    string lURL = baseURL + "?method=UpdateProfile&input_type=JSON&response_type=JSON&rest_data=" + System.Web.HttpUtility.UrlEncode(objUpdateProfileRequest.ToString());

                    var result = objhttpClient.PostAsJsonAsync(lURL, "").Result.Content.ReadAsStringAsync();
                    response = JObject.Parse(result.Result.ToString());

                    _logger.Info("\r\n");
                    _logger.Info("Response");
                    _logger.Info(response.ToString());

                    _logger.Info("\r\n");
                    _logger.Info("Create Update Profile Executed Successfully");

                }
                else
                {
                    _logger.Info("\r\n");
                    _logger.Info("Session id not created");
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return response;
        }

        #endregion

        #region Create Payment Master
        public async Task<JObject> ClaimsPayDataHandlerCreatePaymentMaster(JObject requestJson)
        {
            
            RestData? objRestData = new RestData();
            Str_Json? objStrJson = new Str_Json();
            DTOModel? objDTOModel = new DTOModel();
            PartyDetails? objPartyDetails = new PartyDetails();
            JObject? json = new JObject();

            string? LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];
            var config = helper.SetNlogConfig(LogPath!, "CreatePaymentMaster_"+ requestJson["PaymentHeader"]["PaymentType"].ToString() +" ", requestJson["PaymentHeader"]["PaymentID"].ToString());
            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();
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

                objDTOModel = JsonConvert.DeserializeObject<DTOModel>(requestJson.ToString());

                string sessionID = await GetSessionID(config);


                if (sessionID.Length > 0)
                {
                    string? ClaimsPayType = string.Empty;
                    string? ClaimsPayTypeRequest = string.Empty;
                    string? LoanAccountNumber = string.Empty;
                    string? ClaimsPayMethod = string.Empty;
                    string? approvalRequired = string.Empty;
                    string? printNow = string.Empty;
                    string? expedite = string.Empty;
                    string? certified = string.Empty;
                    //Read Payment Headers
                    JObject? objextendedData = JObject.Parse(requestJson["__extendedData"]["extendeddata"]["table"]["entitydata"]["columns"].ToString());
                    helper.ReadClaimsPayFields(objextendedData!, ref ClaimsPayType, ref ClaimsPayTypeRequest, ref LoanAccountNumber, ref ClaimsPayMethod, ref approvalRequired, ref printNow, ref certified, ref expedite);


                    //Mapping From extended data
                    objStrJson.PM_LoanAccountNumber = LoanAccountNumber;
                    objStrJson.PM_CarrierId = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["CarrierID"]; ;
                    objStrJson.PM_PaymentID = objDTOModel.PaymentHeader.PaymentID;
                    objStrJson.PM_Amount = objDTOModel.PaymentHeader.TotalApprovedAmount;
                    objStrJson.PM_Certified = certified;
                    objStrJson.PM_Expedite = expedite;
                    objStrJson.PM_PrintNow = printNow;

                    //Mapping From Performer DTO
                    
                    if (objDTOModel.PaymentHeader.PaymentType.ToUpper() == "BLK")
                    {

                        objStrJson.PM_UserId = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["Default_PM_User_ID"];
                        objStrJson.PM_User_FirstName = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["Default_PM_User_FirstName"];
                        objStrJson.PM_User_LastName = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["PM_User_LastName"];

                    }
                    else if (objDTOModel.PaymentHeader.PaymentType.ToUpper() == "REG")
                    {
                        objStrJson.PM_UserId = objDTOModel.PerformerDTO.PerformerID;
                        objStrJson.PM_User_FirstName = objDTOModel.PerformerDTO.PerformerNameDetailDTO.FirstName;
                        objStrJson.PM_User_LastName = objDTOModel.PerformerDTO.PerformerNameDetailDTO.LastName;
                        
                    }
                    var result = await helper.GetPartyDetails(objDTOModel.PaymentHeader.UserOrgEntityID.ToString(), config);
                    objPartyDetails = JsonConvert.DeserializeObject<PartyDetails>(result.ToString())!;
                    if (objPartyDetails.partyIndividualDetail.partyEmail != null)
                    {
                        objStrJson.PM_User_EmailAddress = objPartyDetails.partyIndividualDetail.partyEmail!.Where(a => a.isPrimary == true).Select(b => b.emailAddress).FirstOrDefault();
                    }
                    //Mapping From AddressDTO_MailTo
                    objStrJson.PMA_Street = string.Concat(objDTOModel.AddressDTO_MailTo.LocationDetailsLine1.ToString(), " "
                        , objDTOModel.AddressDTO_MailTo.LocationDetailsLine2 == null ? "" : objDTOModel.AddressDTO_MailTo.LocationDetailsLine2);
                    objStrJson.PMA_City = objDTOModel.AddressDTO_MailTo.AdminDivisionPrimary;
                    objStrJson.PMA_State = await helper.GetState(objDTOModel.AddressDTO_MailTo.NationalDivisionPrimary);
                    objStrJson.PMA_Zipcode = objDTOModel.AddressDTO_MailTo.PostalCode;
                    objStrJson.PMA_Country = await helper.GetCountry(objDTOModel.AddressDTO_MailTo.CountryCode.ToString());
                    objStrJson.PMA_MailTo = objDTOModel.PaymentHeader.MailToName;


                    //Mapping From Claim DTO
                    if (objDTOModel.PaymentHeader.PaymentType != "BLK")
                    {
                        objStrJson.CL_ClaimNumber = objDTOModel.ClaimDTO.ClaimID;
                        objStrJson.CL_DateofLoss = Convert.ToDateTime(await helper.GetLossDetails(objDTOModel.ClaimDTO.LossID, config)).ToString("yyyy-MM-dd");
                        objStrJson.CL_PolicyNumber = objDTOModel.ClaimDTO.PolicyID;
                        //Mapping From Line DTO
                        objStrJson.CL_CauseofLoss = await helper.GetCauseOfLoss(objDTOModel.LineDTOs[0].CauseOfLoss);
                    }


                    



                    switch (ClaimsPayType)
                    {
                        case Constants.C_KEY_Contacts:
                            objStrJson.PM_PaymentType = Constants.C_KEY_Contacts_Value;
                            break;
                        case Constants.C_KEY_Other:
                            objStrJson.PM_PaymentType = Constants.C_KEY_Other_Value;
                            break;
                        case Constants.C_KEY_Vendor:
                            objStrJson.PM_PaymentType = Constants.C_KEY_Vendor_Value;
                            break;
                        case Constants.C_KEY_Lienholder:
                            objStrJson.PM_PaymentType = Constants.C_KEY_Lienholder_Value;
                            break;
                        case Constants.C_KEY_Contacts_and_Vendor:
                            objStrJson.PM_PaymentType = Constants.C_KEY_Contacts_and_Vendor_Value;
                            break;
                        case Constants.C_KEY_Contacts_and_Mortgagee:
                            objStrJson.PM_PaymentType = Constants.C_KEY_Contacts_and_Mortgagee_Value;
                            break;

                    }

                    if (objStrJson.PM_PaymentType == Constants.C_KEY_Contacts_and_Mortgagee_Value)
                    {
                        objStrJson.PA_City = objDTOModel.AddressDTO_MailTo.AdminDivisionPrimary;
                        objStrJson.PA_Country = await helper.GetCountry(objDTOModel.AddressDTO_MailTo.CountryCode.ToString());
                        objStrJson.PA_State = await helper.GetState(objDTOModel.AddressDTO_MailTo.NationalDivisionPrimary);
                        objStrJson.PA_Street = objDTOModel.AddressDTO_MailTo.LocationDetailsLine1;
                        objStrJson.PA_Zipcode = objDTOModel.AddressDTO_MailTo.PostalCode;
                    }

                    switch (ClaimsPayMethod)
                    {
                        case Constants.C_KEY_Let_Customer_Pickup:
                            objStrJson.PMETHOD = Constants.C_KEY_Let_Customer_Pickup_Value;
                            break;
                        case Constants.C_KEY_Check:
                            objStrJson.PMETHOD = Constants.C_KEY_Check_Value;
                            break;
                        default:
                            objStrJson.PMETHOD = "";
                            break;
                    }

                    switch (ClaimsPayTypeRequest)
                    {
                        case Constants.C_KEY_Recoverable_Depreciation:
                            objStrJson.PM_RequestReason = Constants.C_KEY_Recoverable_Depreciation_Value;
                            break;
                        case Constants.C_KEY_Emergency_Funds:
                            objStrJson.PM_RequestReason = Constants.C_KEY_Emergency_Funds_Value;
                            break;
                        case Constants.C_KEY_Loss_Payment:
                            objStrJson.PM_RequestReason = Constants.C_KEY_Loss_Payment_Value;
                            break;
                        case Constants.C_KEY_Supplemental_Payment:
                            objStrJson.PM_RequestReason = Constants.C_KEY_Supplemental_Payment_Value;
                            break;
                    }

                    PartyDetails? objReportablePartyDetails = new PartyDetails();
                    string? Reportable_PartyId = objDTOModel.PaymentHeader.ReportablePartyID;
                    if (!string.IsNullOrEmpty(Reportable_PartyId))
                    {
                        var reportablePartyDetails = await helper.GetPartyDetails(Reportable_PartyId, config);
                        objReportablePartyDetails = JsonConvert.DeserializeObject<PartyDetails>(reportablePartyDetails.ToString());
                    }


                    //check how many Payees are present
                    if (objDTOModel.PaymentPayeeDataObjectsList.Count > 1)
                    {
                        if (objDTOModel.PaymentPayeeDataObjectsList.Count > 2)
                        {
                            ClaimsPayMethod = Constants.C_KEY_Check;
                            objStrJson.PMETHOD = Constants.C_KEY_Check_Value;
                        }

                        bool IsBussiness = false;
                        bool IsPCONFilled = false;
                        bool IsCurrentPCON = false;
                        int i = 1;
                        foreach (var item in objDTOModel.PaymentPayeeDataObjectsList)
                        {
                            IsCurrentPCON = false;
                            var responsePartyDetails = await helper.GetPartyDetails(item.ClientID, config);
                            objPartyDetails = JsonConvert.DeserializeObject<PartyDetails>(responsePartyDetails.ToString());
                            //claims pay type and claims pay method value
                            //Get Payee type
                            switch (ClaimsPayType + ClaimsPayMethod)
                            {

                                case Constants.C_KEY_Contacts + Constants.C_KEY_Let_Customer_Pickup:
                                    objStrJson.PMETHOD = Constants.C_KEY_Let_Customer_Pickup_Value;
                                    if (i == 1)
                                        FillPCON(objStrJson, objPartyDetails);
                                    if (i == 2)
                                        FillSCON(objStrJson, objPartyDetails);
                                    break;
                                case Constants.C_KEY_Contacts + Constants.C_KEY_Check:
                                    objStrJson.PMETHOD = Constants.C_KEY_Check_Value;
                                    objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
                                    if (i == 1)
                                        FillPCON(objStrJson, objPartyDetails);
                                    if (i == 2)
                                        FillSCON(objStrJson, objPartyDetails);
                                    break;
                                case Constants.C_KEY_Contacts_and_Vendor:
                                    objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
                                    objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

                                    if (objPartyDetails.partyType == Constants.PartyType && IsBussiness == false)
                                    {
                                        objStrJson.BUS_Type = Constants.C_KEY_Vendor_Value;
                                        objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
                                        objStrJson.BUS_TINType = Constants.BUS_TINType;
                                        FillBus(objStrJson, objPartyDetails);
                                        IsBussiness = true;
                                    }
                                    if (objPartyDetails.partyType == Constants.PartyTypeIndividual && IsPCONFilled == false)
                                    {
                                        objStrJson.PCON_Approval_Reqd = approvalRequired;
                                        FillPCON(objStrJson, objPartyDetails);
                                        IsPCONFilled = true;
                                        IsCurrentPCON = true;
                                    }
                                    if (objPartyDetails.partyType == Constants.PartyTypeIndividual && IsPCONFilled == true && !IsCurrentPCON)
                                    {
                                        objStrJson.SCON_Approval_Reqd = approvalRequired;
                                        FillSCON(objStrJson, objPartyDetails);
                                    }
                                    break;
                                case Constants.C_KEY_Contacts_and_Vendor + Constants.C_KEY_Check:
                                    objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
                                    objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

                                    if (objPartyDetails.partyType == Constants.PartyType && IsBussiness == false)
                                    {
                                        objStrJson.BUS_Type = Constants.C_KEY_Vendor_Value;
                                        objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
                                        objStrJson.BUS_TINType = Constants.BUS_TINType;
                                        FillBus(objStrJson, objPartyDetails);
                                        IsBussiness = true;
                                    }
                                    if (objPartyDetails.partyType == Constants.PartyTypeIndividual && IsPCONFilled == false)
                                    {
                                        objStrJson.PCON_Approval_Reqd = approvalRequired;
                                        FillPCON(objStrJson, objPartyDetails);
                                        IsPCONFilled = true;
                                        IsCurrentPCON = true;
                                    }
                                    if (objPartyDetails.partyType == Constants.PartyTypeIndividual && IsPCONFilled == true && !IsCurrentPCON)
                                    {
                                        objStrJson.SCON_Approval_Reqd = approvalRequired;
                                        FillSCON(objStrJson, objPartyDetails);
                                    }
                                    break;
                                case Constants.C_KEY_Contacts_and_Vendor + Constants.C_KEY_Let_Customer_Pickup:
                                    objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
                                    objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

                                    if (objPartyDetails.partyType == Constants.PartyType && IsBussiness == false)
                                    {
                                        objStrJson.BUS_Type = Constants.C_KEY_Vendor_Value;
                                        objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
                                        objStrJson.BUS_TINType = Constants.BUS_TINType;
                                        FillBus(objStrJson, objPartyDetails);
                                        IsBussiness = true;
                                    }
                                    if (objPartyDetails.partyType == Constants.PartyTypeIndividual && IsPCONFilled == false)
                                    {
                                        objStrJson.PCON_Approval_Reqd = approvalRequired;
                                        FillPCON(objStrJson, objPartyDetails);
                                        IsPCONFilled = true;
                                        IsCurrentPCON = true;
                                    }
                                    if (objPartyDetails.partyType == Constants.PartyTypeIndividual && IsPCONFilled == true && !IsCurrentPCON)
                                    {
                                        objStrJson.SCON_Approval_Reqd = approvalRequired;
                                        FillSCON(objStrJson, objPartyDetails);
                                    }
                                    break;

                                case Constants.C_KEY_Contacts_and_Mortgagee + Constants.C_KEY_Let_Customer_Pickup:
                                    objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
                                    objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;
                                    string? ParticipantId = objDTOModel.ParticipantDataObject.ParticipantRolesDTO!.Where(a => a.ParticipantRole == "MTGE").Select(b => b.ParticipantID).FirstOrDefault();

                                    if (IsBussiness == false && item.InsuranceInvolvementID == ParticipantId)
                                    {
                                        objStrJson.BUS_Type = Constants.C_KEY_Mortgagee_Value;
                                        objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
                                        objStrJson.BUS_TINType = Constants.BUS_TINType;
                                        FillBus(objStrJson, objPartyDetails);
                                        IsBussiness = true;
                                    }
                                    if (IsPCONFilled == false && item.InsuranceInvolvementID != ParticipantId)
                                    {
                                        objStrJson.PCON_Approval_Reqd = approvalRequired;
                                        FillPCON(objStrJson, objPartyDetails);
                                        IsPCONFilled = true;
                                        IsCurrentPCON = true;
                                    }
                                    if (item.InsuranceInvolvementID != ParticipantId && IsPCONFilled == true && !IsCurrentPCON)
                                    {
                                        objStrJson.SCON_Approval_Reqd = approvalRequired;
                                        FillSCON(objStrJson, objPartyDetails);
                                    }
                                    break;


                            }
                            i++;
                        }
                    }
                    else
                    {
                        //check for ClaimsPay Type
                        //Check if Reportable Paty value is available
                        var responsePartyDetails = await helper.GetPartyDetails(objDTOModel.PaymentPayeeDataObjectsList[0].ClientID, config);
                        objPartyDetails = JsonConvert.DeserializeObject<PartyDetails>(responsePartyDetails.ToString());

                        switch (ClaimsPayType + ClaimsPayMethod)
                        {
                            case Constants.C_KEY_Contacts + Constants.C_KEY_Let_Customer_Pickup:
                                objStrJson.PMETHOD = Constants.C_KEY_Let_Customer_Pickup_Value;

                                FillPCON(objStrJson, objPartyDetails);

                                break;
                            case Constants.C_KEY_Contacts + Constants.C_KEY_Check:
                                objStrJson.PMETHOD = Constants.C_KEY_Check_Value;
                                objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;

                                FillPCON(objStrJson, objPartyDetails);

                                break;

                            case Constants.C_KEY_Contacts:
                                objStrJson.PMETHOD = Constants.C_KEY_Check_Value;
                                objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;

                                FillPCON(objStrJson, objPartyDetails);

                                break;
                            case Constants.C_KEY_Contacts_and_Vendor:
                                objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
                                objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

                                objStrJson.BUS_Type = Constants.C_KEY_Vendor_Value;
                                objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
                                objStrJson.BUS_TINType = Constants.BUS_TINType;
                                FillBus(objStrJson, objPartyDetails);
                                if (!string.IsNullOrEmpty(Reportable_PartyId))
                                {
                                    objStrJson.PCON_Approval_Reqd = approvalRequired;
                                    FillPCON(objStrJson, objReportablePartyDetails);
                                }
                                break;
                            case Constants.C_KEY_Contacts_and_Vendor + Constants.C_KEY_Check:
                                objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
                                objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

                                objStrJson.BUS_Type = Constants.C_KEY_Vendor_Value;
                                objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
                                objStrJson.BUS_TINType = Constants.BUS_TINType;
                                FillBus(objStrJson, objPartyDetails);
                                if (!string.IsNullOrEmpty(Reportable_PartyId))
                                {
                                    objStrJson.PCON_Approval_Reqd = approvalRequired;
                                    FillPCON(objStrJson, objReportablePartyDetails);
                                }
                                break;
                            case Constants.C_KEY_Contacts_and_Vendor + Constants.C_KEY_Let_Customer_Pickup:
                                objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
                                objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

                                objStrJson.BUS_Type = Constants.C_KEY_Vendor_Value;
                                objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
                                objStrJson.BUS_TINType = Constants.BUS_TINType;
                                FillBus(objStrJson, objPartyDetails);
                                if (!string.IsNullOrEmpty(Reportable_PartyId))
                                {
                                    objStrJson.PCON_Approval_Reqd = approvalRequired;
                                    FillPCON(objStrJson, objReportablePartyDetails);
                                }
                                break;

                            case Constants.C_KEY_Contacts_and_Mortgagee + Constants.C_KEY_Let_Customer_Pickup:
                                objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
                                objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

                                objStrJson.BUS_Type = Constants.C_KEY_Mortgagee_Value;
                                objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
                                objStrJson.BUS_TINType = Constants.BUS_TINType;
                                FillBus(objStrJson, objPartyDetails);
                                if (!string.IsNullOrEmpty(Reportable_PartyId))
                                {
                                    objStrJson.PCON_Approval_Reqd = approvalRequired;
                                    FillPCON(objStrJson, objReportablePartyDetails);
                                }
                                break;

                            case Constants.C_KEY_Lienholder + Constants.C_KEY_Let_Customer_Pickup:
                                objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
                                objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

                                objStrJson.BUS_Type = Constants.C_KEY_Lienholder_Value;
                                objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
                                objStrJson.BUS_TINType = Constants.BUS_TINType;
                                FillBus(objStrJson, objPartyDetails);
                                if (!string.IsNullOrEmpty(Reportable_PartyId))
                                {
                                    objStrJson.PCON_Approval_Reqd = approvalRequired;
                                    FillPCON(objStrJson, objReportablePartyDetails);
                                }
                                break;
                            case Constants.C_KEY_Lienholder + Constants.C_KEY_Check:
                                objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
                                objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

                                objStrJson.BUS_Type = Constants.C_KEY_Lienholder_Value;
                                objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
                                objStrJson.BUS_TINType = Constants.BUS_TINType;
                                FillBus(objStrJson, objPartyDetails);
                                if (!string.IsNullOrEmpty(Reportable_PartyId))
                                {
                                    objStrJson.PCON_Approval_Reqd = approvalRequired;
                                    FillPCON(objStrJson, objReportablePartyDetails);
                                }
                                break;
                            case Constants.C_KEY_Lienholder:
                                objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
                                objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

                                objStrJson.BUS_Type = Constants.C_KEY_Lienholder_Value;
                                objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
                                objStrJson.BUS_TINType = Constants.BUS_TINType;
                                FillBus(objStrJson, objPartyDetails);
                                if (!string.IsNullOrEmpty(Reportable_PartyId))
                                {
                                    objStrJson.PCON_Approval_Reqd = approvalRequired;
                                    FillPCON(objStrJson, objReportablePartyDetails);
                                }
                                break;
                            case Constants.C_KEY_Vendor + Constants.C_KEY_Check:
                                objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
                                objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

                                objStrJson.BUS_Type = Constants.C_KEY_Vendor_Value;
                                objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
                                objStrJson.BUS_TINType = Constants.BUS_TINType;
                                FillBus(objStrJson, objPartyDetails);
                                if (!string.IsNullOrEmpty(Reportable_PartyId))
                                {
                                    objStrJson.PCON_Approval_Reqd = approvalRequired;
                                    FillPCON(objStrJson, objReportablePartyDetails);
                                }
                                break;
                            case Constants.C_KEY_Vendor:
                                objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
                                objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

                                objStrJson.BUS_Type = Constants.C_KEY_Vendor_Value;
                                objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
                                objStrJson.BUS_TINType = Constants.BUS_TINType;
                                FillBus(objStrJson, objPartyDetails);
                                if (!string.IsNullOrEmpty(Reportable_PartyId))
                                {
                                    objStrJson.PCON_Approval_Reqd = approvalRequired;
                                    FillPCON(objStrJson, objReportablePartyDetails);
                                }
                                break;
                        }

                    }

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


                    //Get Payment Header Detail

                    var responsePaymentHeaderDetail = await helper.GetPaymentHeaderDetails(json["PM_PaymentID"].ToString(), config);

                    JObject? objreq = JObject.Parse(responsePaymentHeaderDetail.ToString());

                    JObject? colObj = JObject.Parse(objreq["__extendedData"]["extendeddata"]["table"]["entitydata"]["columns"].ToString());
                    JArray? columArray = JArray.Parse(colObj["column"].ToString());

                    var jObjects = columArray.ToObject<List<JObject>>();

                    var DATAITEM_PM_Orig_Method = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
                                        .Single(x => x.Name == Constants.DATAITEM_FQN_PM_Orig_Method)
                                        .Index;
                    var DATAITEM_PM_Status = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
                                        .Single(x => x.Name == Constants.DATAITEM_FQN_PM_Status)
                                        .Index;
                    var DATAITEM_PM_MailTrackingNumber = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
                                        .Single(x => x.Name == Constants.DATAITEM_FQN_PM_MailTrackingNumber)
                                        .Index;

                    var DATAITEM_IP_PaymentID = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
                                        .Single(x => x.Name == Constants.DATAITEM_FQN_IP_PaymentID)
                                        .Index;

                    foreach (var obj in jObjects)
                    {
                        //For PM_Method_Last4Digit
                        if (jObjects.IndexOf(obj) == DATAITEM_PM_Orig_Method)
                        {
                            foreach (var prop in obj.Properties())
                            {
                                if (json.ToString().Contains("PM_PaymentMethod"))
                                {
                                    if (prop.Name == "value")                     //Get desired property
                                    {
                                        obj.Remove("value");
                                        obj.Add(new JProperty("value", new JObject()));

                                        JObject? value = obj["value"] as JObject;

                                        //Add needed properties
                                        value.Add(new JProperty("@xsi:type", new JObject()));
                                        value.Add(new JProperty("#text", new JObject()));

                                        //Give them values
                                        value["@xsi:type"] = "xs:string";
                                        value["#text"] = json["PM_PaymentMethod"].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                        else if (jObjects.IndexOf(obj) == DATAITEM_PM_Status)
                        {
                            foreach (var prop in obj.Properties())
                            {
                                if (json.ToString().Contains("PM_Status"))
                                {
                                    if (prop.Name == "value")                     //Get desired property
                                    {
                                        obj.Remove("value");
                                        obj.Add(new JProperty("value", new JObject()));

                                        JObject? value = obj["value"] as JObject;

                                        //Add needed properties
                                        value.Add(new JProperty("@xsi:type", new JObject()));
                                        value.Add(new JProperty("#text", new JObject()));

                                        //Give them values
                                        value["@xsi:type"] = "xs:string";
                                        value["#text"] = json["PM_Status"].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                        else if (jObjects.IndexOf(obj) == DATAITEM_PM_MailTrackingNumber)
                        {
                            foreach (var prop in obj.Properties())
                            {
                                if (json.ToString().Contains("PM_Id"))
                                {
                                    if (prop.Name == "value")                     //Get desired property
                                    {
                                        obj.Remove("value");
                                        obj.Add(new JProperty("value", new JObject()));

                                        JObject? value = obj["value"] as JObject;

                                        //Add needed properties
                                        value.Add(new JProperty("@xsi:type", new JObject()));
                                        value.Add(new JProperty("#text", new JObject()));

                                        //Give them values
                                        value["@xsi:type"] = "xs:string";
                                        value["#text"] = json["PM_Id"].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                        else if (jObjects.IndexOf(obj) == DATAITEM_IP_PaymentID)
                        {
                            foreach (var prop in obj.Properties())
                            {
                                if (json.ToString().Contains("PM_TranId"))
                                {

                                    if (prop.Name == "value")                     //Get desired property
                                    {
                                        obj.Remove("value");
                                        obj.Add(new JProperty("value", new JObject()));

                                        JObject? value = obj["value"] as JObject;

                                        //Add needed properties
                                        value.Add(new JProperty("@xsi:type", new JObject()));
                                        value.Add(new JProperty("#text", new JObject()));

                                        //Give them values
                                        value["@xsi:type"] = "xs:string";
                                        value["#text"] = json["PM_TranId"].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                    }


                    JArray? outputArray = JArray.FromObject(jObjects);         //Output array
                    JObject? updatedJson = new JObject();
                    updatedJson["column"] = outputArray;
                    objreq["__extendedData"]["extendeddata"]["table"]["entitydata"]["columns"].Replace(updatedJson);

                    Thread t = new Thread(() => WorkThreadFunction(objreq.ToString(), "CreatePaymentHeaderUpdate", requestJson["PaymentHeader"]["PaymentID"].ToString()));
                    t.Start();

                    //helper.UpdatePaymentHeaderDetails(objreq.ToString(), config);

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
            var config = helper.SetNlogConfig(LogPath!, "CreateVendor","");
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


                RestData? objRequestCreateVendor = new RestData();
                Str_Json? objStr_Json = new Str_Json();
                PartyDetails? objPartyDetails = new PartyDetails();



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
            var config = helper.SetNlogConfig(LogPath, "Webhook", objJsonRequest["PM_CR_PaymentID"].ToString());
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

                                JObject? value = obj["value"] as JObject;

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
                                value["@xsi:type"] = "xs:boolean";
                                value["#text"] = objJsonRequest["PM_Monitored"].ToString() == "Y" ? "true" : "false";
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
                                value["@xsi:type"] = "xs:boolean";
                                value["#text"] = objJsonRequest["PM_Selection"].ToString() == "Y" ? "true" : "false";
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
                updatedJson["column"] = outputArray;
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

            string? LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];
            var config = helper.SetNlogConfig(LogPath!, "GetPaymentStatus", objJsonRequest["PM_CR_PaymentID"].ToString());
            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();
            RestData objRequest = new RestData();
            Str_Json objStr_Json = new Str_Json();
            _logger.Info("\r\n");
            _logger.Info("-------------------------------------------------------------------|| Log Start || -------------------------------------------------------------------------");
            _logger.Info("\r\n");
            _logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated Get Payment Status");
            _logger.Info("\r\n");
            _logger.Info("Request");
            _logger.Info(objJsonRequest.Root);
            JObject response = null;

            try
            {
                string sessionID = await GetSessionID(config);
                if (sessionID.Length > 0)
                {
                    objStr_Json.PM_PaymentID = objJsonRequest["PaymentID"].ToString();
                    objRequest.session = sessionID;
                    objRequest.str_json = objStr_Json;

                    var opt = new JsonSerializerOptions() { WriteIndented = true };
                    string strJson = System.Text.Json.JsonSerializer.Serialize<Str_Json>(objStr_Json, opt);

                    _logger.Info("\r\n");
                    _logger.Info("Request To OneInc");
                    _logger.Info(strJson);
                    JObject getPaymentStatusRequest = new JObject(
                                     new JProperty("session", sessionID),
                                     new JProperty("str_json", strJson.ToString()));
                    baseURL = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["ClaimsPayURI"];
                    string lURL = baseURL + "?method=GetPaymentStatus&input_type=JSON&response_type=JSON&rest_data=" + System.Web.HttpUtility.UrlEncode(getPaymentStatusRequest.ToString());

                    var result = objhttpClient.PostAsJsonAsync(lURL, "").Result.Content.ReadAsStringAsync();
                    response = JObject.Parse(result.Result.ToString());
                    _logger.Info("\r\n");
                    _logger.Info("Response From OneInc");
                    _logger.Info(strJson);
                }
                else
                {
                    response = JObject.Parse("{\"Status\":\"failed\",\"Message\":\"Invalid Session ID\"}");
                }
            }
            catch (Exception ex)
            {

                _logger.Info("\r\n");
                _logger.Info("Get Payment Status Execution failed");
                _logger.Error(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + ex, "ERROR" + ex.ToString());
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
            var config = helper.SetNlogConfig(LogPath, "StopPayment", objJsonRequest["PM_CR_PaymentID"].ToString());

            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();

            _logger.Info("\r\n");
            _logger.Info("-------------------------------------------------------------------|| Log Start || -------------------------------------------------------------------------");
            _logger.Info("\r\n");
            _logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated Stop Payment ");
            _logger.Info("\r\n");
            _logger.Info("Request");

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

                    baseURL = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["ClaimsPayURI"];
                    string lURL = baseURL + "?method=StopPayment&input_type=JSON&response_type=JSON&rest_data=" + System.Web.HttpUtility.UrlEncode(objStopPaymentRequest.ToString());

                    var result = objhttpClient.PostAsJsonAsync(lURL, "").Result.Content.ReadAsStringAsync();
                    response = JObject.Parse(result.Result.ToString());

                    _logger.Info("\r\n");
                    _logger.Info("Response");
                    _logger.Info(response.ToString());

                    _logger.Info("\r\n");
                    _logger.Info("Stop Payment Executed Successfully");

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
            var config = helper.SetNlogConfig(LogPath, "ResendEmail", objJsonRequest["PaymentID"].ToString());
            var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();

            _logger.Info("\r\n");
            _logger.Info("-------------------------------------------------------------------|| Log Start || -------------------------------------------------------------------------");
            _logger.Info("\r\n");
            _logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated Resend Email ");
            _logger.Info("\r\n");
            _logger.Info("Request");

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

                    baseURL = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["ClaimsPayURI"];
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
                _logger.Info("Resend Email Execution failed");
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

        //#region Bulk Payment Master
        //public async Task<JObject> ClaimsPayDataHandlerBulkPaymentMaster(JObject requestJson)
        //{
        //    string? LogPath = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["LogPath"];
        //    var config = helper.SetNlogConfig(LogPath!, "CreatePaymentMaster Bulk","");
        //    var _logger = NLogBuilder.ConfigureNLog(config).GetCurrentClassLogger();

        //    RestData? objRestData = new RestData();
        //    Str_Json? objStrJson = new Str_Json();
        //    DTOModel? objDTOModel = new DTOModel();
        //    PartyDetails? objPartyDetails = new PartyDetails();
        //    JObject? json = new JObject();
        //    try
        //    {
        //        _logger.Info("\r\n");
        //        _logger.Info("-------------------------------------------------------------------|| Log Start || -------------------------------------------------------------------------");
        //        _logger.Info("\r\n");
        //        _logger.Info(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " INFO Initiated Create Payment Master ");
        //        _logger.Info("\r\n");
        //        _logger.Info("Request");
        //        //log input recieved from DC Claims
        //        _logger.Info(requestJson.Root);

        //        objDTOModel = JsonConvert.DeserializeObject<DTOModel>(requestJson.ToString());

        //        string sessionID = await GetSessionID(config);


        //        if (sessionID.Length > 0)
        //        {
        //            string? ClaimsPayType = string.Empty;
        //            string? ClaimsPayTypeRequest = string.Empty;
        //            string? LoanAccountNumber = string.Empty;
        //            string? ClaimsPayMethod = string.Empty;
        //            string? approvalRequired = string.Empty;
        //            string? printNow = string.Empty;
        //            string? expedite = string.Empty;
        //            string? certified = string.Empty;
        //            //Read Payment Headers
        //            JObject? objextendedData = JObject.Parse(requestJson["__extendedData"]["extendeddata"]["table"]["entitydata"]["columns"].ToString());
        //            helper.ReadClaimsPayFields(objextendedData!, ref ClaimsPayType, ref ClaimsPayTypeRequest, ref LoanAccountNumber, ref ClaimsPayMethod, ref approvalRequired, ref printNow, ref certified, ref expedite);


        //            //Mapping From extended data
        //            objStrJson.PM_LoanAccountNumber = LoanAccountNumber;
        //            objStrJson.PM_CarrierId = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["CarrierID"];
        //            objStrJson.PM_PaymentID = objDTOModel.PaymentHeader.PaymentID;
        //            objStrJson.PM_Amount = objDTOModel.PaymentHeader.TotalApprovedAmount;
        //            objStrJson.PM_Certified = certified;
        //            objStrJson.PM_Expedite = expedite;
        //            objStrJson.PM_PrintNow = printNow;


        //           // objStrJson.PM_UserId = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["Default_PM_User_ID"];//objDTOModel.PerformerDTO.PerformerID;
        //            //objStrJson.PM_User_FirstName = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["Default_PM_User_FirstName"];// objDTOModel.PerformerDTO.PerformerNameDetailDTO.FirstName;
        //            //objStrJson.PM_User_LastName = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["PM_User_LastName"];// objDTOModel.PerformerDTO.PerformerNameDetailDTO.LastName;

        //            var result = await helper.GetPartyDetails(objDTOModel.PaymentHeader.UserOrgEntityID.ToString(), config);
        //            objPartyDetails = JsonConvert.DeserializeObject<PartyDetails>(result.ToString())!;
        //            if (objPartyDetails.partyIndividualDetail.partyEmail != null)
        //            {
        //                objStrJson.PM_User_EmailAddress = objPartyDetails.partyIndividualDetail.partyEmail!.Where(a => a.isPrimary == true).Select(b => b.emailAddress).FirstOrDefault();
        //            }
        //            //Mapping From AddressDTO_MailTo
        //            objStrJson.PMA_Street = string.Concat(objDTOModel.AddressDTO_MailTo.LocationDetailsLine1.ToString(), " "
        //                , objDTOModel.AddressDTO_MailTo.LocationDetailsLine2 == null ? "" : objDTOModel.AddressDTO_MailTo.LocationDetailsLine2);
        //            objStrJson.PMA_City = objDTOModel.AddressDTO_MailTo.AdminDivisionPrimary;
        //            objStrJson.PMA_State = await helper.GetState(objDTOModel.AddressDTO_MailTo.NationalDivisionPrimary);
        //            objStrJson.PMA_Zipcode = objDTOModel.AddressDTO_MailTo.PostalCode;
        //            objStrJson.PMA_Country = await helper.GetCountry(objDTOModel.AddressDTO_MailTo.CountryCode.ToString());
        //            objStrJson.PMA_MailTo = objDTOModel.PaymentHeader.MailToName;


        //            //Mapping From Claim DTO
        //            objStrJson.CL_ClaimNumber = "";// objDTOModel.ClaimDTO.ClaimID;
        //            objStrJson.CL_DateofLoss = "";// Convert.ToDateTime(await helper.GetLossDetails(objDTOModel.ClaimDTO.LossID, config)).ToString("yyyy-MM-dd");
        //            objStrJson.CL_PolicyNumber = "";//objDTOModel.ClaimDTO.PolicyID;

        //            //Mapping From Line DTO
        //            objStrJson.CL_CauseofLoss = "";//await helper.GetCauseOfLoss(objDTOModel.LineDTOs[0].CauseOfLoss);

        //            switch (ClaimsPayType)
        //            {
        //                case Constants.C_KEY_Contacts:
        //                    objStrJson.PM_PaymentType = Constants.C_KEY_Contacts_Value;
        //                    break;
        //                case Constants.C_KEY_Other:
        //                    objStrJson.PM_PaymentType = Constants.C_KEY_Other_Value;
        //                    break;
        //                case Constants.C_KEY_Vendor:
        //                    objStrJson.PM_PaymentType = Constants.C_KEY_Vendor_Value;
        //                    break;
        //                case Constants.C_KEY_Lienholder:
        //                    objStrJson.PM_PaymentType = Constants.C_KEY_Lienholder_Value;
        //                    break;
        //                case Constants.C_KEY_Contacts_and_Vendor:
        //                    objStrJson.PM_PaymentType = Constants.C_KEY_Contacts_and_Vendor_Value;
        //                    break;
        //                case Constants.C_KEY_Contacts_and_Mortgagee:
        //                    objStrJson.PM_PaymentType = Constants.C_KEY_Contacts_and_Mortgagee_Value;
        //                    break;

        //            }

        //            if (objStrJson.PM_PaymentType == Constants.C_KEY_Contacts_and_Mortgagee_Value)
        //            {
        //                objStrJson.PA_City = objDTOModel.AddressDTO_MailTo.AdminDivisionPrimary;
        //                objStrJson.PA_Country = await helper.GetCountry(objDTOModel.AddressDTO_MailTo.CountryCode.ToString());
        //                objStrJson.PA_State = await helper.GetState(objDTOModel.AddressDTO_MailTo.NationalDivisionPrimary);
        //                objStrJson.PA_Street = objDTOModel.AddressDTO_MailTo.LocationDetailsLine1;
        //                objStrJson.PA_Zipcode = objDTOModel.AddressDTO_MailTo.PostalCode;
        //            }

        //            switch (ClaimsPayMethod)
        //            {
        //                case Constants.C_KEY_Let_Customer_Pickup:
        //                    objStrJson.PMETHOD = Constants.C_KEY_Let_Customer_Pickup_Value;
        //                    break;
        //                case Constants.C_KEY_Check:
        //                    objStrJson.PMETHOD = Constants.C_KEY_Check_Value;
        //                    break;
        //                default:
        //                    objStrJson.PMETHOD = "";
        //                    break;
        //            }

        //            switch (ClaimsPayTypeRequest)
        //            {
        //                case Constants.C_KEY_Recoverable_Depreciation:
        //                    objStrJson.PM_RequestReason = Constants.C_KEY_Recoverable_Depreciation_Value;
        //                    break;
        //                case Constants.C_KEY_Emergency_Funds:
        //                    objStrJson.PM_RequestReason = Constants.C_KEY_Emergency_Funds_Value;
        //                    break;
        //                case Constants.C_KEY_Loss_Payment:
        //                    objStrJson.PM_RequestReason = Constants.C_KEY_Loss_Payment_Value;
        //                    break;
        //                case Constants.C_KEY_Supplemental_Payment:
        //                    objStrJson.PM_RequestReason = Constants.C_KEY_Supplemental_Payment_Value;
        //                    break;
        //            }

        //            PartyDetails? objReportablePartyDetails = new PartyDetails();
        //            string? Reportable_PartyId = objDTOModel.PaymentHeader.ReportablePartyID;
        //            if (!string.IsNullOrEmpty(Reportable_PartyId))
        //            {
        //                var reportablePartyDetails = await helper.GetPartyDetails(Reportable_PartyId, config);
        //                objReportablePartyDetails = JsonConvert.DeserializeObject<PartyDetails>(reportablePartyDetails.ToString());
        //            }

        //            //check how many Payees are present
        //            if (objDTOModel.PaymentPayeeDataObjectsList.Count > 1)
        //            {
        //                if (objDTOModel.PaymentPayeeDataObjectsList.Count > 2)
        //                {
        //                    ClaimsPayMethod = Constants.C_KEY_Check;
        //                    objStrJson.PMETHOD = Constants.C_KEY_Check_Value;
        //                }

        //                bool IsBussiness = false;
        //                bool IsPCONFilled = false;
        //                bool IsCurrentPCON = false;
        //                int i = 1;
        //                foreach (var item in objDTOModel.PaymentPayeeDataObjectsList)
        //                {
        //                    IsCurrentPCON = false;
        //                    var responsePartyDetails = await helper.GetPartyDetails(item.ClientID, config);
        //                    objPartyDetails = JsonConvert.DeserializeObject<PartyDetails>(responsePartyDetails.ToString());
        //                    //claims pay type and claims pay method value
        //                    //Get Payee type
        //                    switch (ClaimsPayType + ClaimsPayMethod)
        //                    {

        //                        case Constants.C_KEY_Contacts + Constants.C_KEY_Let_Customer_Pickup:
        //                            objStrJson.PMETHOD = Constants.C_KEY_Let_Customer_Pickup_Value;
        //                            if (i == 1)
        //                                FillPCON(objStrJson, objPartyDetails);
        //                            if (i == 2)
        //                                FillSCON(objStrJson, objPartyDetails);
        //                            break;
        //                        case Constants.C_KEY_Contacts + Constants.C_KEY_Check:
        //                            objStrJson.PMETHOD = Constants.C_KEY_Check_Value;
        //                            objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
        //                            if (i == 1)
        //                                FillPCON(objStrJson, objPartyDetails);
        //                            if (i == 2)
        //                                FillSCON(objStrJson, objPartyDetails);
        //                            break;
        //                        case Constants.C_KEY_Contacts_and_Vendor:
        //                            objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
        //                            objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

        //                            if (objPartyDetails.partyType == Constants.PartyType && IsBussiness == false)
        //                            {
        //                                objStrJson.BUS_Type = Constants.C_KEY_Vendor_Value;
        //                                objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
        //                                objStrJson.BUS_TINType = Constants.BUS_TINType;
        //                                FillBus(objStrJson, objPartyDetails);
        //                                IsBussiness = true;
        //                            }
        //                            if (objPartyDetails.partyType == Constants.PartyTypeIndividual && IsPCONFilled == false)
        //                            {
        //                                objStrJson.PCON_Approval_Reqd = approvalRequired;
        //                                FillPCON(objStrJson, objPartyDetails);
        //                                IsPCONFilled = true;
        //                                IsCurrentPCON = true;
        //                            }
        //                            if (objPartyDetails.partyType == Constants.PartyTypeIndividual && IsPCONFilled == true && !IsCurrentPCON)
        //                            {
        //                                objStrJson.SCON_Approval_Reqd = approvalRequired;
        //                                FillSCON(objStrJson, objPartyDetails);
        //                            }
        //                            break;
        //                        case Constants.C_KEY_Contacts_and_Vendor + Constants.C_KEY_Check:
        //                            objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
        //                            objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

        //                            if (objPartyDetails.partyType == Constants.PartyType && IsBussiness == false)
        //                            {
        //                                objStrJson.BUS_Type = Constants.C_KEY_Vendor_Value;
        //                                objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
        //                                objStrJson.BUS_TINType = Constants.BUS_TINType;
        //                                FillBus(objStrJson, objPartyDetails);
        //                                IsBussiness = true;
        //                            }
        //                            if (objPartyDetails.partyType == Constants.PartyTypeIndividual && IsPCONFilled == false)
        //                            {
        //                                objStrJson.PCON_Approval_Reqd = approvalRequired;
        //                                FillPCON(objStrJson, objPartyDetails);
        //                                IsPCONFilled = true;
        //                                IsCurrentPCON = true;
        //                            }
        //                            if (objPartyDetails.partyType == Constants.PartyTypeIndividual && IsPCONFilled == true && !IsCurrentPCON)
        //                            {
        //                                objStrJson.SCON_Approval_Reqd = approvalRequired;
        //                                FillSCON(objStrJson, objPartyDetails);
        //                            }
        //                            break;
        //                        case Constants.C_KEY_Contacts_and_Vendor + Constants.C_KEY_Let_Customer_Pickup:
        //                            objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
        //                            objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

        //                            if (objPartyDetails.partyType == Constants.PartyType && IsBussiness == false)
        //                            {
        //                                objStrJson.BUS_Type = Constants.C_KEY_Vendor_Value;
        //                                objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
        //                                objStrJson.BUS_TINType = Constants.BUS_TINType;
        //                                FillBus(objStrJson, objPartyDetails);
        //                                IsBussiness = true;
        //                            }
        //                            if (objPartyDetails.partyType == Constants.PartyTypeIndividual && IsPCONFilled == false)
        //                            {
        //                                objStrJson.PCON_Approval_Reqd = approvalRequired;
        //                                FillPCON(objStrJson, objPartyDetails);
        //                                IsPCONFilled = true;
        //                                IsCurrentPCON = true;
        //                            }
        //                            if (objPartyDetails.partyType == Constants.PartyTypeIndividual && IsPCONFilled == true && !IsCurrentPCON)
        //                            {
        //                                objStrJson.SCON_Approval_Reqd = approvalRequired;
        //                                FillSCON(objStrJson, objPartyDetails);
        //                            }
        //                            break;

        //                        case Constants.C_KEY_Contacts_and_Mortgagee + Constants.C_KEY_Let_Customer_Pickup:
        //                            objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
        //                            objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;
        //                            string? ParticipantId = objDTOModel.ParticipantDataObject.ParticipantRolesDTO!.Where(a => a.ParticipantRole == "MTGE").Select(b => b.ParticipantID).FirstOrDefault();

        //                            if (IsBussiness == false && item.InsuranceInvolvementID == ParticipantId)
        //                            {
        //                                objStrJson.BUS_Type = Constants.C_KEY_Mortgagee_Value;
        //                                objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
        //                                objStrJson.BUS_TINType = Constants.BUS_TINType;
        //                                FillBus(objStrJson, objPartyDetails);
        //                                IsBussiness = true;
        //                            }
        //                            if (IsPCONFilled == false && item.InsuranceInvolvementID != ParticipantId)
        //                            {
        //                                objStrJson.PCON_Approval_Reqd = approvalRequired;
        //                                FillPCON(objStrJson, objPartyDetails);
        //                                IsPCONFilled = true;
        //                                IsCurrentPCON = true;
        //                            }
        //                            if (item.InsuranceInvolvementID != ParticipantId && IsPCONFilled == true && !IsCurrentPCON)
        //                            {
        //                                objStrJson.SCON_Approval_Reqd = approvalRequired;
        //                                FillSCON(objStrJson, objPartyDetails);
        //                            }
        //                            break;


        //                    }
        //                    i++;
        //                }
        //            }
        //            else
        //            {
        //                //check for ClaimsPay Type
        //                //Check if Reportable Paty value is available
        //                var responsePartyDetails = await helper.GetPartyDetails(objDTOModel.PaymentPayeeDataObjectsList[0].ClientID, config);
        //                objPartyDetails = JsonConvert.DeserializeObject<PartyDetails>(responsePartyDetails.ToString());

        //                switch (ClaimsPayType + ClaimsPayMethod)
        //                {
        //                    case Constants.C_KEY_Contacts + Constants.C_KEY_Let_Customer_Pickup:
        //                        objStrJson.PMETHOD = Constants.C_KEY_Let_Customer_Pickup_Value;

        //                        FillPCON(objStrJson, objPartyDetails);

        //                        break;
        //                    case Constants.C_KEY_Contacts + Constants.C_KEY_Check:
        //                        objStrJson.PMETHOD = Constants.C_KEY_Check_Value;
        //                        objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;

        //                        FillPCON(objStrJson, objPartyDetails);

        //                        break;

        //                    case Constants.C_KEY_Contacts:
        //                        objStrJson.PMETHOD = Constants.C_KEY_Check_Value;
        //                        objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;

        //                        FillPCON(objStrJson, objPartyDetails);

        //                        break;
        //                    case Constants.C_KEY_Contacts_and_Vendor:
        //                        objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
        //                        objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

        //                        objStrJson.BUS_Type = Constants.C_KEY_Vendor_Value;
        //                        objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
        //                        objStrJson.BUS_TINType = Constants.BUS_TINType;
        //                        FillBus(objStrJson, objPartyDetails);
        //                        if (!string.IsNullOrEmpty(Reportable_PartyId))
        //                        {
        //                            objStrJson.PCON_Approval_Reqd = approvalRequired;
        //                            FillPCON(objStrJson, objReportablePartyDetails);
        //                        }
        //                        break;
        //                    case Constants.C_KEY_Contacts_and_Vendor + Constants.C_KEY_Check:
        //                        objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
        //                        objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

        //                        objStrJson.BUS_Type = Constants.C_KEY_Vendor_Value;
        //                        objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
        //                        objStrJson.BUS_TINType = Constants.BUS_TINType;
        //                        FillBus(objStrJson, objPartyDetails);
        //                        if (!string.IsNullOrEmpty(Reportable_PartyId))
        //                        {
        //                            objStrJson.PCON_Approval_Reqd = approvalRequired;
        //                            FillPCON(objStrJson, objReportablePartyDetails);
        //                        }
        //                        break;
        //                    case Constants.C_KEY_Contacts_and_Vendor + Constants.C_KEY_Let_Customer_Pickup:
        //                        objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
        //                        objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

        //                        objStrJson.BUS_Type = Constants.C_KEY_Vendor_Value;
        //                        objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
        //                        objStrJson.BUS_TINType = Constants.BUS_TINType;
        //                        FillBus(objStrJson, objPartyDetails);
        //                        if (!string.IsNullOrEmpty(Reportable_PartyId))
        //                        {
        //                            objStrJson.PCON_Approval_Reqd = approvalRequired;
        //                            FillPCON(objStrJson, objReportablePartyDetails);
        //                        }
        //                        break;

        //                    case Constants.C_KEY_Contacts_and_Mortgagee + Constants.C_KEY_Let_Customer_Pickup:
        //                        objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
        //                        objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

        //                        objStrJson.BUS_Type = Constants.C_KEY_Mortgagee_Value;
        //                        objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
        //                        objStrJson.BUS_TINType = Constants.BUS_TINType;
        //                        FillBus(objStrJson, objPartyDetails);
        //                        if (!string.IsNullOrEmpty(Reportable_PartyId))
        //                        {
        //                            objStrJson.PCON_Approval_Reqd = approvalRequired;
        //                            FillPCON(objStrJson, objReportablePartyDetails);
        //                        }
        //                        break;

        //                    case Constants.C_KEY_Lienholder + Constants.C_KEY_Let_Customer_Pickup:
        //                        objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
        //                        objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

        //                        objStrJson.BUS_Type = Constants.C_KEY_Lienholder_Value;
        //                        objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
        //                        objStrJson.BUS_TINType = Constants.BUS_TINType;
        //                        FillBus(objStrJson, objPartyDetails);
        //                        if (!string.IsNullOrEmpty(Reportable_PartyId))
        //                        {
        //                            objStrJson.PCON_Approval_Reqd = approvalRequired;
        //                            FillPCON(objStrJson, objReportablePartyDetails);
        //                        }
        //                        break;
        //                    case Constants.C_KEY_Lienholder + Constants.C_KEY_Check:
        //                        objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
        //                        objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

        //                        objStrJson.BUS_Type = Constants.C_KEY_Lienholder_Value;
        //                        objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
        //                        objStrJson.BUS_TINType = Constants.BUS_TINType;
        //                        FillBus(objStrJson, objPartyDetails);
        //                        if (!string.IsNullOrEmpty(Reportable_PartyId))
        //                        {
        //                            objStrJson.PCON_Approval_Reqd = approvalRequired;
        //                            FillPCON(objStrJson, objReportablePartyDetails);
        //                        }
        //                        break;
        //                    case Constants.C_KEY_Lienholder:
        //                        objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
        //                        objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

        //                        objStrJson.BUS_Type = Constants.C_KEY_Lienholder_Value;
        //                        objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
        //                        objStrJson.BUS_TINType = Constants.BUS_TINType;
        //                        FillBus(objStrJson, objPartyDetails);
        //                        if (!string.IsNullOrEmpty(Reportable_PartyId))
        //                        {
        //                            objStrJson.PCON_Approval_Reqd = approvalRequired;
        //                            FillPCON(objStrJson, objReportablePartyDetails);
        //                        }
        //                        break;
        //                    case Constants.C_KEY_Vendor + Constants.C_KEY_Check:
        //                        objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
        //                        objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

        //                        objStrJson.BUS_Type = Constants.C_KEY_Vendor_Value;
        //                        objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
        //                        objStrJson.BUS_TINType = Constants.BUS_TINType;
        //                        FillBus(objStrJson, objPartyDetails);
        //                        if (!string.IsNullOrEmpty(Reportable_PartyId))
        //                        {
        //                            objStrJson.PCON_Approval_Reqd = approvalRequired;
        //                            FillPCON(objStrJson, objReportablePartyDetails);
        //                        }
        //                        break;
        //                    case Constants.C_KEY_Vendor:
        //                        objStrJson.PM_Additional_Text_3 = objStrJson.PMA_MailTo;
        //                        objStrJson.PM_Additional_Text_1 = objDTOModel.PaymentHeader.InvoiceID;

        //                        objStrJson.BUS_Type = Constants.C_KEY_Vendor_Value;
        //                        objStrJson.BUS_SubType = Constants.BUS_Sub_Type;
        //                        objStrJson.BUS_TINType = Constants.BUS_TINType;
        //                        FillBus(objStrJson, objPartyDetails);
        //                        if (!string.IsNullOrEmpty(Reportable_PartyId))
        //                        {
        //                            objStrJson.PCON_Approval_Reqd = approvalRequired;
        //                            FillPCON(objStrJson, objReportablePartyDetails);
        //                        }
        //                        break;
        //                }

        //            }

        //            var opt = new JsonSerializerOptions() { WriteIndented = true };
        //            string strJson = System.Text.Json.JsonSerializer.Serialize<Str_Json>(objStrJson, opt);
        //            var loginParams = JObject.Parse(strJson);

        //            JObject createPaymentMasterRequest = new JObject(
        //              new JProperty("session", sessionID),
        //              new JProperty("str_json", strJson.ToString()));
        //            _logger.Info("\r\n");
        //            _logger.Info("Request To OneInc");
        //            _logger.Info(strJson);
        //            //createPaymentMasterRequest.Add(new JProperty("PM_PaymentType", "Contact(s) and Vendor"));

        //            baseURL = AppConfig.configuration?.GetSection($"Modules:ClaimsPay")["ClaimsPayURI"];
        //            string lURL = baseURL + "?method=CreatePaymentMaster&input_type=JSON&response_type=JSON&rest_data=" + System.Web.HttpUtility.UrlEncode(createPaymentMasterRequest.ToString());

        //            var response = objhttpClient.PostAsJsonAsync(lURL, "").Result.Content.ReadAsStringAsync();
        //            json = JObject.Parse(response.Result.ToString());

        //            _logger.Info("\r\n");
        //            _logger.Info("Response From OneInc");
        //            _logger.Info(json.ToString());


        //            //Get Payment Header Detail
        //            string responsePaymentHeaderDetail = string.Empty;
        //            if (json["Status"].ToString() != "Error")
        //            {
        //                responsePaymentHeaderDetail = await helper.GetPaymentHeaderDetails(json["PM_PaymentID"].ToString(), config);

        //                JObject? objreq = JObject.Parse(responsePaymentHeaderDetail.ToString());

        //                JObject? colObj = JObject.Parse(objreq["__extendedData"]["extendeddata"]["table"]["entitydata"]["columns"].ToString());
        //                JArray? columArray = JArray.Parse(colObj["column"].ToString());

        //                var jObjects = columArray.ToObject<List<JObject>>();

        //                var DATAITEM_PM_Orig_Method = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
        //                                    .Single(x => x.Name == Constants.DATAITEM_FQN_PM_Orig_Method)
        //                                    .Index;
        //                var DATAITEM_PM_Status = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
        //                                    .Single(x => x.Name == Constants.DATAITEM_FQN_PM_Status)
        //                                    .Index;
        //                var DATAITEM_PM_MailTrackingNumber = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
        //                                    .Single(x => x.Name == Constants.DATAITEM_FQN_PM_MailTrackingNumber)
        //                                    .Index;

        //                var DATAITEM_IP_PaymentID = columArray.Select((x, index) => new { Name = x.Value<string>("name"), Node = x, Index = index })
        //                                    .Single(x => x.Name == Constants.DATAITEM_FQN_IP_PaymentID)
        //                                    .Index;

        //                foreach (var obj in jObjects)
        //                {
        //                    //For PM_Method_Last4Digit
        //                    if (jObjects.IndexOf(obj) == DATAITEM_PM_Orig_Method)
        //                    {
        //                        foreach (var prop in obj.Properties())
        //                        {
        //                            if (json.ToString().Contains("PM_PaymentMethod"))
        //                            {
        //                                if (prop.Name == "value")                     //Get desired property
        //                                {
        //                                    obj.Remove("value");
        //                                    obj.Add(new JProperty("value", new JObject()));

        //                                    JObject? value = obj["value"] as JObject;

        //                                    //Add needed properties
        //                                    value.Add(new JProperty("@xsi:type", new JObject()));
        //                                    value.Add(new JProperty("#text", new JObject()));

        //                                    //Give them values
        //                                    value["@xsi:type"] = "xs:string";
        //                                    value["#text"] = json["PM_PaymentMethod"].ToString();
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else if (jObjects.IndexOf(obj) == DATAITEM_PM_Status)
        //                    {
        //                        foreach (var prop in obj.Properties())
        //                        {
        //                            if (json.ToString().Contains("PM_Status"))
        //                            {
        //                                if (prop.Name == "value")                     //Get desired property
        //                                {
        //                                    obj.Remove("value");
        //                                    obj.Add(new JProperty("value", new JObject()));

        //                                    JObject? value = obj["value"] as JObject;

        //                                    //Add needed properties
        //                                    value.Add(new JProperty("@xsi:type", new JObject()));
        //                                    value.Add(new JProperty("#text", new JObject()));

        //                                    //Give them values
        //                                    value["@xsi:type"] = "xs:string";
        //                                    value["#text"] = json["PM_Status"].ToString();
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else if (jObjects.IndexOf(obj) == DATAITEM_PM_MailTrackingNumber)
        //                    {
        //                        foreach (var prop in obj.Properties())
        //                        {
        //                            if (json.ToString().Contains("PM_Id"))
        //                            {
        //                                if (prop.Name == "value")                     //Get desired property
        //                                {
        //                                    obj.Remove("value");
        //                                    obj.Add(new JProperty("value", new JObject()));

        //                                    JObject? value = obj["value"] as JObject;

        //                                    //Add needed properties
        //                                    value.Add(new JProperty("@xsi:type", new JObject()));
        //                                    value.Add(new JProperty("#text", new JObject()));

        //                                    //Give them values
        //                                    value["@xsi:type"] = "xs:string";
        //                                    value["#text"] = json["PM_Id"].ToString();
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else if (jObjects.IndexOf(obj) == DATAITEM_IP_PaymentID)
        //                    {
        //                        foreach (var prop in obj.Properties())
        //                        {
        //                            if (json.ToString().Contains("PM_TranId"))
        //                            {

        //                                if (prop.Name == "value")                     //Get desired property
        //                                {
        //                                    obj.Remove("value");
        //                                    obj.Add(new JProperty("value", new JObject()));

        //                                    JObject? value = obj["value"] as JObject;

        //                                    //Add needed properties
        //                                    value.Add(new JProperty("@xsi:type", new JObject()));
        //                                    value.Add(new JProperty("#text", new JObject()));

        //                                    //Give them values
        //                                    value["@xsi:type"] = "xs:string";
        //                                    value["#text"] = json["PM_TranId"].ToString();
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }


        //                JArray? outputArray = JArray.FromObject(jObjects);         //Output array
        //                JObject? updatedJson = new JObject();
        //                updatedJson["column"] = outputArray;
        //                objreq["__extendedData"]["extendeddata"]["table"]["entitydata"]["columns"].Replace(updatedJson);

        //                Thread t = new Thread(() => WorkThreadFunction(objreq.ToString(), config));
        //                t.Start();

        //                //helper.UpdatePaymentHeaderDetails(objreq.ToString(), config);
        //            }
        //            _logger.Info("\r\n");
        //            _logger.Info("Create Payment Master Executed Successfully");
        //        }
        //        else
        //        {
        //            _logger.Info("\r\n");
        //            _logger.Info("Session id not created");
        //            //Log error session id not created
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Info("\r\n");
        //        _logger.Info("Create Payment Master Execution failed");
        //        _logger.Error(DateTime.Now.ToString("dd-MM-yyyy HH:mm") + ex, "ERROR" + ex.ToString());

        //    }
        //    finally
        //    {
        //        _logger.Info("------------------------------------------------------------------ || Log End || -------------------------------------------------------------------------");
        //        NLog.LogManager.Shutdown();
        //    }
        //    //json = JObject.Parse(requestJson.Root.ToString());
        //    return json;
        //}
        //#endregion

        public void FillPCON(Str_Json objStrJson, PartyDetails objPartyDetails)
        {
            if (objPartyDetails.partyType == Constants.PartyTypeIndividual)
            {
                objStrJson.PCON_ContactId = objPartyDetails.partyIndividualDetail.party.partyID;
                objStrJson.PCON_FirstName = objPartyDetails.partyIndividualDetail.partyIndNameDetail.Where(a => a.partyIndName.partyIndNameID == a.partyName.partyNameID && a.partyName.isPrimary == true).Select(b => b.partyIndName.firstName).FirstOrDefault();
                objStrJson.PCON_LastName = objPartyDetails.partyIndividualDetail.partyIndNameDetail.Where(a => a.partyIndName.partyIndNameID == a.partyName.partyNameID && a.partyName.isPrimary == true).Select(b => b.partyIndName.lastName).FirstOrDefault();
                if (objPartyDetails.partyIndividualDetail.partyEmail != null)
                {
                    objStrJson.PCON_EmailAddress = objPartyDetails.partyIndividualDetail.partyEmail.Where(a => a.isPrimary == true).Select(b => b.emailAddress).FirstOrDefault();
                }
                if (objPartyDetails.partyIndividualDetail.partyPhone != null)
                {
                    objStrJson.PCON_MobilePhone = objPartyDetails.partyIndividualDetail.partyPhone.Where(a => a.isPrimary == true).Select(b => b.fullPhoneNumber).FirstOrDefault();
                }
            }
            if (objPartyDetails.partyType == Constants.PartyType)
            {
                objStrJson.PCON_ContactId = objPartyDetails.partyBusinessDetail.party.partyID;

                objStrJson.PCON_LastName = objPartyDetails.partyBusinessDetail.partyBusNameDetail.Where(a => a.partyBusName.partyBusNameID == a.partyName.partyNameID && a.partyName.isPrimary == true).Select(b => b.partyBusName.name).FirstOrDefault();
                if (objPartyDetails.partyBusinessDetail.partyEmail != null)
                {
                    objStrJson.PCON_EmailAddress = objPartyDetails.partyBusinessDetail.partyEmail.Where(a => a.isPrimary == true).Select(b => b.emailAddress).FirstOrDefault();
                }
                if (objPartyDetails.partyBusinessDetail.partyPhone != null)
                {
                    objStrJson.PCON_MobilePhone = objPartyDetails.partyBusinessDetail.partyPhone.Where(a => a.isPrimary == true).Select(b => b.fullPhoneNumber).FirstOrDefault();
                }
                objStrJson.PCON_Business = "Y";
            }
        }

        public void FillSCON(Str_Json objStrJson, PartyDetails objPartyDetails)
        {

            objStrJson.SCON_ContactId = objPartyDetails.partyIndividualDetail.party.partyID; ;
            objStrJson.SCON_FirstName = objPartyDetails.partyIndividualDetail.partyIndNameDetail!.Where(a => a.partyIndName.partyIndNameID == a.partyName.partyNameID && a.partyName.isPrimary == true).Select(b => b.partyIndName.firstName).FirstOrDefault();
            objStrJson.SCON_LastName = objPartyDetails.partyIndividualDetail.partyIndNameDetail!.Where(a => a.partyIndName.partyIndNameID == a.partyName.partyNameID && a.partyName.isPrimary == true).Select(b => b.partyIndName.lastName).FirstOrDefault();
            if (objPartyDetails.partyIndividualDetail.partyEmail != null)
            {
                objStrJson.SCON_EmailAddress = objPartyDetails.partyIndividualDetail.partyEmail.Where(a => a.isPrimary == true).Select(b => b.emailAddress).FirstOrDefault();
            }
            if (objPartyDetails.partyIndividualDetail.partyPhone != null)
            {
                objStrJson.SCON_MobilePhone = objPartyDetails.partyIndividualDetail.partyPhone.Where(a => a.isPrimary == true).Select(b => b.fullPhoneNumber).FirstOrDefault();
            }


        }

        public async void FillBus(Str_Json objStrJson, PartyDetails objPartyDetails)
        {

            if (objPartyDetails.partyType == Constants.PartyType)
            {
                objStrJson.BUS_BusinessId = objPartyDetails.partyBusinessDetail.party.partyID;
                objStrJson.BUS_Name = objPartyDetails.partyBusinessDetail.partyBusNameDetail!.Where(a => a.partyBusName.partyBusNameID == a.partyName.partyNameID && a.partyName.isPrimary == true).Select(b => b.partyBusName.name).FirstOrDefault();

                objStrJson.BUS_EmailAddress = objPartyDetails.partyBusinessDetail.partyEmail!.Where(a => a.isPrimary == true).Select(b => b.emailAddress).FirstOrDefault();


                objStrJson.BUS_Street = objPartyDetails.partyBusinessDetail.partyAddressDetail!.Where(a => a.partyAddress.addressID == a.address.addressID && a.partyAddress.isPrimary == true).Select(b => b.address.locationDetailsLine1).FirstOrDefault() + objPartyDetails.partyBusinessDetail.partyAddressDetail.Where(a => a.partyAddress.addressID == a.address.addressID && a.partyAddress.isPrimary == true).Select(b => b.address.locationDetailsLine2).FirstOrDefault();

                objStrJson.BUS_City = objPartyDetails.partyBusinessDetail.partyAddressDetail!.Where(a => a.partyAddress.addressID == a.address.addressID && a.partyAddress.isPrimary == true).Select(b => b.address.adminDivisionPrimary).FirstOrDefault();

                string? state = objPartyDetails.partyBusinessDetail.partyAddressDetail!.Where(a => a.partyAddress.addressID == a.address.addressID && a.partyAddress.isPrimary == true).Select(b => b.address.nationalDivisionPrimary).FirstOrDefault();
                objStrJson.BUS_State = await helper.GetState(state!);

                objStrJson.BUS_Zipcode = objPartyDetails.partyBusinessDetail.partyAddressDetail.Where(a => a.partyAddress.addressID == a.address.addressID && a.partyAddress.isPrimary == true).Select(b => b.address.postalCode).FirstOrDefault();
                string country = objPartyDetails.partyBusinessDetail.partyAddressDetail.Where(a => a.partyAddress.addressID == a.address.addressID && a.partyAddress.isPrimary == true).Select(b => b.address.countryCode).FirstOrDefault().ToUpper();

                objStrJson.BUS_Country = await helper.GetCountry(country);

                objStrJson.BUS_TIN = objPartyDetails.partyBusinessDetail.partyBusiness.registrationID1;


            }
        }
        #endregion
        public void WorkThreadFunction(string objreq,string logName, string paymenyID)
        {
            try
            {
                var response = helper.UpdatePaymentHeaderDetails(objreq.ToString(),logName,paymenyID).ToString();
            }
            catch (Exception ex)
            {
                // log errors
            }
        }


    }
}
