namespace ClaimsPay.Modules.ClaimsPay.Models
{
    public class Constants
    {
        public const string EventUnableToStopPayment = "IP_UnableToStopPayment";
        public const string EventUnableToCorrectPayment = "IP_UnableToCorrectPayment";
        public const string EventSendPayment = "IP_SendPayment";
        public const string EventStopVoid = "IP_StopVoidPayment";
        public const string EventSendUpdate = "IP_SendUpdate";

        public const string ErrorUnableToStopPayment = "IP_ErrorUnableToStopPayment";
        public const string ErrorUnableToCorrectPayment = "IP_ErrorUnableToCorrectPayment";
        public const string ErrorNoPhone = "IP_NoPhone";
        public const string ErrorNoEmail = "IP_NoEmail";
        public const string ErrorVendorPaymentMethod = "IP_VendorPaymentMethod";
        public const string ErrorContactsNotPerson = "IP_ContactsNotPerson";
        public const string ErrorNotCompanyVendor = "IP_NotCompanyVendor";
        public const string ErrorNotEnabledForInsurPay = "IP_NotEnabledForInsurPay";
        public const string ErrorNotLienholder = "IP_NotLienholder";
        public const string ErrorLienholderPaymentMethod = "IP_LienholderPaymentMethod";
        public const string ErrorAdjusterEmail = "IP_AdjusterEmail";
        public const string ErrorStoredMethodNotUsed = "IP_StoredMethodNotUsed";
        public const string ErrorAddress = "IP_Address";
        public const string ErrorInvoiceNumber = "IP_InvoiceNumber";
        public const string ErrorContactAndVendorPayee = "IP_ContactAndVendorPayee";
        public const string ErrorContactAndMortgageeRole = "IP_ContactAndMortgageeRole";
        public const string ErrorContactAndMortgageePayee = "IP_ContactAndMortgageePayee";
        public const string ErrorContactAndMortgageeNumIndPayee = "IP_ContactAndMortgageeNumIndPayee";
        public const string ErrorNotBusinessBulkPayment = "IP_NotBusinessBulkPayment";

        public const string DataItem_ClaimsPayType = "DIfe75453702d40bd9b907438a720400";
        public const string Category_ClaimsPayType = "fa5a4d381b44fdf818fb856531543e";
        public const string DataItem_ClaimsPayMethod = "DI6b3b0d6709e406f832c7100a51c798";
        public const string Category_ClaimsPayMethod = "f8951e7bf2c4a6b9e04cdb7cd2a144";
        public const string DataItem_ClaimsPayCheckStatus = "DI6d79718b02746368e5ce487ae73ce4";
        public const string DataItem_ClaimsPayTrackingNumber = "DI23fec276ddc444793634b4b152b13e";
        public const string DataItem_ClaimsPayPaymentMethodId = "DId0a03e3d3d241418c6e69c54840126";
        public const string DataItem_ClaimsPayLoanAccountNumber = "DI005be6c61024cb3ba27e5e4e92f8cf";
        public const string DataItem_ClaimsPayICProxyNumber = "DI6b2b37594334c7eb98273135138a38";
        public const string DataItem_ClaimsPayDCExpYear = "DIb631051b90e4cb6bdcfe834ee8d321";
        public const string DataItem_ClaimsPayDCExpMonth = "DIc97a316a50e49efbc057689e814024";
        public const string DataItem_ClaimsPayDCCardNumber = "DI447fca8f0164db69f31207022bdd24";
        public const string DataItem_ClaimsPayAcctNumLast4 = "DI81fffbe795a49e490e6e5d168d4457";
        public const string DataItem_ClaimsPayComments = "DI6201b58d4ec4e85a3fd8c0879caf53";
        public const string DataItem_ClaimsPayCheckMemo = "DIa5812086384466ca9adc1e29b7aa3b";
        public const string DataItem_ClaimsPayApprovalRequired = "DI3f3e19dd01a4522a5b6695c74c0ceb";
        public const string DataItem_ClaimsPayPaymentReason = "DIe2341cefef643b98ab1c4c99d636e9";
        public const string DataItem_ClaimsPayRequestType = "DI8a2976a16cf4b198ac716957528fd9";
        public const string DataItem_ClaimsPayBulkPayment = "DIca9740c09ac49829278e5444b6ff78";

        public const string C_Key_Prepaid_Card = "c1eb6";
        public const string C_Key_Instant_Prepaid_Card = "9b442";
        public const string C_Key_Direct_Deposit = "d2a28";
        public const string C_Key_Debit_Card = "e586a";
        public const string C_Key_Virtual_Card = "a74fc";
        public const string C_Key_Total_Loss = "0015b";
        public const string C_Key_Check = "ef5cd";
        public const string C_Key_Field_Payment = "6eded";
        public const string C_Key_Let_Customer_Pickup = "14d9b";
        public const string C_Key_Contact = "b557c";
        public const string C_Key_Vendor = "4cc75";
        public const string C_Key_Lienholder = "16e12";
        public const string C_Key_Other = "c47f2";
        public const string C_Key_ContactAndMortgagee = "a1694";
        public const string C_Key_ContactAndVendor = "6777b";
        public const string C_Key_Role_Lienholder = "fc0e4";
        public const string C_Key_Role_Mortgagee = "MTGE";
        public const string C_Key_PhoneType_Business_Fax = "10";
        public const string C_Key_PhoneType_Home_Fax = "11";

        public const string C_Key_Loss_Payment = "0a3e1";
        public const string C_Key_Supplemental_Payment = "0870d";
        public const string C_Key_Recoverable_Depreciation = "5d6e0";
        public const string T_VAL_Loss_Payment = "Loss Payment";
        public const string T_VAL_Supplemental_Payment = "Supplemental Payment";
        public const string T_VAL_Recoverable_Depreciation = "Recoverable Depreciation";

        public static string Status_New = "NEW";
        public static string Status_DoneNoItems = "DONE0";
        public static string Status_Done = "DONE";
        public static string Status_NoClaim = "NOCLM";
        public static string BUS_TINType = "SSN";

        public static string EventProcessBatchItem = "IP_ProcessItem";
        public static string EventProcessBulkBatch = "IPB_ProcessBatch";
        public static string EventProcessBatch = "IP_ProcessBatch";
        public static string BatchEntityID = "INSURPAY";
        public static string DummyEntityID = "CLM_NOT_FOUND";
        public static string SystemUserID = "3C3935991D0CD783";

        public static string RefData_Label_UBASP_FNContinued = "IP_FNContinued";
        public static string RefData_Label_UBASP_AttachmentName = "IP_AttachmentName";
        public static string RefData_Label_UBASP_AttachmentDesc = "IP_AttachmentDesc";
    }

}
