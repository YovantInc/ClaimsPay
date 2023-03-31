using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ClaimsPay.Modules.ClaimsPay.Models;

namespace ClaimsPay.Shared
{
    class Helpers
    {


        internal static void ProcessTaskCompletion(string description, Task t, ILogger log, bool throwExceptionOnError)
        {
            if (t.Status == TaskStatus.RanToCompletion)
                return;

            string error;

            if (t.Exception != null)
            {
                error = string.Format("Execption from Task calling {0}", description);
                log.LogError(t.Exception, error);
            }
            else
            {
                error = string.Format("Error calling Task {0}", description);
                log.LogError(error);
            }

            if (throwExceptionOnError)
                throw new ApplicationException(error);
        }

        internal static void ProcessHttpRequest(string description, HttpRequestMessage request, ILogger log)
        {
            string logMessage = string.Format("{0} - {1} -  To: {2}", description, request.Method, request.RequestUri.ToString());
            log.LogInformation(logMessage);

#if DEBUG
            if (request.Content != null)
            {
                Task<string> task = request.Content.ReadAsStringAsync();
                task.Wait();

                ProcessTaskCompletion(description + " Get Request Content for Debug", task, log, false);

                if (task.Status == TaskStatus.RanToCompletion)
                {
                    logMessage = string.Format("{0} - {1} -  To:{2} - Request Body: {3}", description, request.Method, request.RequestUri.ToString(), task.Result);
                    log.LogDebug(logMessage);
                    System.Diagnostics.Trace.WriteLine(logMessage);
                }
            }
#endif
        }

        internal static void ProcessHttpResponseCompletion(string description, HttpResponseMessage response, ILogger log, bool throwExceptionOnError, System.Net.HttpStatusCode expectedResult = System.Net.HttpStatusCode.OK)
        {
            string error = "Unknown";

            if (response.StatusCode == expectedResult)
            {
                string logMessage = string.Format("{0} - Succesfully Recieved {1} From {2} - To: {3}", description, response.StatusCode, response.RequestMessage.Method, response.RequestMessage.RequestUri.ToString());
                log.LogInformation(logMessage);
                throwExceptionOnError = false;
            }
            else
            {
                error = string.Format("{0} call exepcted {1}, recieved {2}", description, expectedResult, response.StatusCode);
                log.LogError(error);
            }

            Task<string> task = response.Content.ReadAsStringAsync();
            task.Wait();

            ProcessTaskCompletion(description + " Get Response for Debug", task, log, false);

#if DEBUG
            if (task.Status == TaskStatus.RanToCompletion)
            {
                string logMessage = string.Format("{0} - Response Body:{1}", description, task.Result);
                log.LogDebug(logMessage);
                System.Diagnostics.Trace.WriteLine(logMessage);
            }
#endif
            if (throwExceptionOnError)
                throw new ApplicationException(error);
        }

        //internal static string ConstructLoginParams()
        //{
        //    JObject loginObj =
        //        new JObject(
        //            new JProperty("user_auth",
        //                new JObject(
        //                    new JProperty("user_name", System.Configuration.ConfigurationManager.AppSettings["ClaimsPayUserName"]),
        //                    new JProperty("password", System.Configuration.ConfigurationManager.AppSettings["ClaimsPayPassword"]),
        //                    new JProperty("version", "1")
        //                    )
        //                )
        //        );
        //    return loginObj.ToString();
        //}

        internal static string ConstructGetPaymentStatusRequest(string paymentId)
        {

            JObject createPaymentObj =
                new JObject(
                    new JProperty("PM_PaymentID", paymentId)
                );
            return createPaymentObj.ToString();

        }

        internal static string ConstructResendEmailRequest(string paymentId)
        {

            JObject createPaymentObj =
                new JObject(
                    new JProperty("PM_PaymentID", paymentId)
                );
            return createPaymentObj.ToString();
        }

        internal static string ConstructStopPaymentRequest(string paymentID)
        {
            JObject createPaymentObj =
                new JObject(
                    new JProperty("PM_PaymentID", paymentID)
               );
            return createPaymentObj.ToString();
        }

        internal static string ConstructUpdateProfileRequest(string paymentId, string contactId, string mobilePhone, string emailAddress)
        {
            JObject createPaymentObj =
                new JObject(

                    new JProperty("PM_PaymentID", paymentId),
                    new JProperty("PCON_ContactId", contactId),
                    new JProperty("PCON_MobilePhone", mobilePhone),
                    new JProperty("PCON_EmailAddress", emailAddress)

                );
            return createPaymentObj.ToString();

        }

        internal static string ConstructCreateVendorRequest(dynamic vendorDetail, string vendorId)
        {
            string rootDirectory = AppDomain.CurrentDomain.BaseDirectory;
            JObject objStateKeys = JObject.Parse(File.ReadAllText(@"" + rootDirectory + "Content\\StateKeys.json"));
            JObject objCountryKeys = JObject.Parse(File.ReadAllText(@"" + rootDirectory + "Content\\CountryKeys.json"));
            dynamic dtoData = JsonConvert.DeserializeObject<dynamic>(vendorDetail.ToString());
            string vendorBusId = dtoData.ParticipantDTO.PartyID;
            string vendorName = dtoData.ContactInfoDTO.ContactFullName;
            string vendortype = "Vendor";
            string faxNumber = dtoData.ContactInfoDTO.ContactFullPhone;
            string DBAName = "Honda City322";
            string emailAddress = dtoData.ContactInfoDTO.ContactEmailAddress;
            string phoneNumber = dtoData.ContactInfoDTO.ContactFullPhone;
            string streetAddress = dtoData.ContactInfoDTO.ContactAddressDetail.LocationDetailsLine1;
            string city = dtoData.ContactInfoDTO.ContactAddressDetail.AdminDivisionPrimary;
            string state = objStateKeys[dtoData.ContactInfoDTO.ContactAddressDetail.NationalDivisionPrimary.ToString()];
            string zipCode = dtoData.ContactInfoDTO.ContactAddressDetail.PostalCode;
            string TINType = Constants.BUS_TINType;
            string country = objCountryKeys[dtoData.ContactInfoDTO.ContactAddressDetail.CountryCode.ToString()];
            string TIN = dtoData.ParticipantDTO.TaxID;
            string subType = "Other vendor";
            string altVendorId = dtoData.ParticipantDTO.PartyID;
            string reportable1099 = "Y";
            string reportingType1099 = "MedHealthPay";
            string status = "Active";

            JObject createVendorObj =
                new JObject(

                    new JProperty("BUS_BusinessId", vendorId),
                    new JProperty("BUS_Name", vendorName),
                    new JProperty("BUS_Type", vendortype),
                    new JProperty("BUS_Fax", faxNumber),
                    new JProperty("BUS_DBAName", DBAName),
                    new JProperty("BUS_EmailAddress", emailAddress),
                    new JProperty("BUS_OfficePhone", phoneNumber),
                    new JProperty("BUS_Street", streetAddress),
                    new JProperty("BUS_City", city),
                    new JProperty("BUS_State", state),
                   new JProperty("BUS_Zipcode", zipCode),
                    new JProperty("BUS_TINType", TINType),
                    new JProperty("BUS_Country", country),
                    new JProperty("BUS_TIN", TIN),
                    new JProperty("BUS_SubType", subType),
                    new JProperty("BUS_AltVendorId", altVendorId),
                    new JProperty("BUS_Reportable1099", reportable1099),
                    new JProperty("BUS_ReportingType1099", reportingType1099),
                    new JProperty("BUS_Status", status)
                    );
            return createVendorObj.ToString();
        }

        internal static void ReadInsurPayFields(dynamic paymentSample, ref string claimsPayType, ref string claimsPayMethod, ref bool claimsPayApprovalRequired, ref string claimsPayICProxyNumber, ref string claimsPayLoanAccNo, ref string claimsPayCheckMemo, ref string claimsPayRequestReason)
        {
            dynamic paymentExtendedData;

            //Check if the table is part of an array
            if (paymentSample.__extendedData.extendeddata.table is JArray)
            {
                //If part of an array, grab the iteration that has a payment type of PYMNT
                paymentExtendedData = GetExtDataTable(paymentSample);
            }

            //If the table isn't part of an array, just use the only table.
            else
            {
                paymentExtendedData = paymentSample.__extendedData.extendeddata.table;
            }
            

            if (paymentExtendedData == null)
                throw new Exception("Could not find payment extended data");

            foreach (dynamic extColumn in paymentExtendedData.entitydata.columns.column)
            {
                if (extColumn["name"].Value == string.Concat("PaymentCstm.", Constants.DataItem_ClaimsPayType))
                {
                    if ((extColumn.value["@xsi:nil"] == null) || (extColumn.value["@xsi:nil"].Value == "false"))
                    {
                        claimsPayType = extColumn.value["#text"].Value;
                    }
                }
                else if (extColumn["name"].Value == string.Concat("PaymentCstm.", Constants.DataItem_ClaimsPayMethod))
                {
                    if ((extColumn.value["@xsi:nil"] == null) || (extColumn.value["@xsi:nil"].Value == "false"))
                    {
                        claimsPayMethod = extColumn.value["#text"].Value;
                    }
                }
                else if (extColumn["name"].Value == string.Concat("PaymentCstm.", Constants.DataItem_ClaimsPayApprovalRequired))
                {
                    if ((extColumn.value["@xsi:nil"] == null) || (extColumn.value["@xsi:nil"].Value == "false"))
                    {
                        if (extColumn.value["#text"].Value == "True")
                        {
                            claimsPayApprovalRequired = true;
                        }
                    }
                }
                else if (extColumn["name"].Value == string.Concat("PaymentCstm.", Constants.DataItem_ClaimsPayICProxyNumber))
                {
                    if ((extColumn.value["@xsi:nil"] == null) || (extColumn.value["@xsi:nil"].Value == "false"))
                    {
                        if (extColumn.value["#text"].Value == "True")
                        {
                            claimsPayICProxyNumber = extColumn.value["#text"].Value;
                        }
                    }
                }
                else if (extColumn["name"].Value == string.Concat("PaymentCstm.", Constants.DataItem_ClaimsPayLoanAccountNumber))
                {
                    if ((extColumn.value["@xsi:nil"] == null) || (extColumn.value["@xsi:nil"].Value == "false"))
                    {
                        claimsPayLoanAccNo = extColumn.value["#text"].Value;
                    }
                }
                else if (extColumn["name"].Value == string.Concat("PaymentCstm.", Constants.DataItem_ClaimsPayCheckMemo))
                {
                    if ((extColumn.value["@xsi:nil"] == null) || (extColumn.value["@xsi:nil"].Value == "false"))
                    {
                        claimsPayCheckMemo = extColumn.value["#text"].Value;
                    }
                }
                else if (extColumn["name"].Value == string.Concat("PaymentCstm.", Constants.DataItem_ClaimsPayRequestType))
                {
                    if ((extColumn.value["@xsi:nil"] == null) || (extColumn.value["@xsi:nil"].Value == "false"))
                    {

                        switch (extColumn.value["#text"].Value)
                        {
                            case Constants.C_Key_Loss_Payment:
                                claimsPayRequestReason = Constants.T_VAL_Loss_Payment;
                                break;
                            case Constants.C_Key_Supplemental_Payment:
                                claimsPayRequestReason = Constants.T_VAL_Supplemental_Payment;
                                break;
                            case Constants.C_Key_Recoverable_Depreciation:
                                claimsPayRequestReason = Constants.T_VAL_Recoverable_Depreciation;
                                break;
                            default:
                                claimsPayRequestReason = extColumn.value["#text"].Value;
                                break;
                        }

                    }
                }
            }
        }

        internal static ClaimsAPI CreateAPI()
        {
            return new ClaimsAPIv12();
        }

        //TODO: Check this new helper method.
        internal static dynamic GetExtDataTable(dynamic paymentSample)
        {
            
            //dynamic extData = paymentSample.__extendedData.extendeddata;
            //dynamic tableData = paymentSample.__extendedData.extendeddata.table;
            //dynamic colData = paymentSample.__extendedData.extendeddata.table.entitydata.columns.column[2];
            //var countData = paymentSample.__extendedData.extendeddata.table.Count;


            //Go grab the tables from the payment sample
            try
            {
                foreach (dynamic table in paymentSample.__extendedData.extendeddata.table)
                {
                    if (table["name"].Value == "PYMNT")
                    {
                        dynamic testVariable = table;
                        dynamic testVariable2 = table["entitydata"];
                        return table;
                    }
                }
            }
            catch (Exception e)
            {
               
            }
            return null;
        }

    }

}
