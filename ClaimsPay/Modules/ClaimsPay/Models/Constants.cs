using System.Security.Principal;

namespace ClaimsPay.Modules.ClaimsPay.Models
{
    public class Constants
    {
        //Constants for Ref
        public static string BUS_TINType = "TIN";
        public static string BUS_Type = "Vendor";
        public static string BUS_Status = "Active";
        public static string PartyType = "BUS";

        //Payment Methods
        public const string C_KEY_Mail_Prepaid_Card = "2d0b9";
        public const string C_KEY_Virtual_Card = "380e3";
        public const string C_KEY_Debit_Card = "4c227";
        public const string C_KEY_Field_Payment = "54ee6";
        public const string C_KEY_Check = "6755e";
        public const string C_KEY_Direct_Deposit = "967b9";
        public const string C_KEY_Instant_Prepaid_Card = "a4959";
        public const string C_KEY_Let_Customer_Pickup = "b3146";
        public const string C_KEY_Prepaid_Card = "e3c11";


        //Claimspay Type
        public const string C_KEY_Contacts = "00180";
        public const string C_KEY_Other = "08b32";
        public const string C_KEY_Vendor = "22c81";
        public const string C_KEY_Lienholder = "9c581";
        public const string C_KEY_Contacts_and_Vendor = "c04bd";
        public const string C_KEY_Contacts_and_Mortgagee = "e1e44";


        //Request Type
        public const string C_KEY_Recoverable_Depreciation = "0daa5";
        public const string C_KEY_Emergency_Funds = "b3893";
        public const string C_KEY_Loss_Payment = "b6cbb";
        public const string C_KEY_Supplemental_Payment = "c52fc";


        //DataItem
        public const string DATAITEM_PM_Selection = "DI08ae41df43940a1a5130cc4cdd944e";
        public const string DATAITEM_ClaimsPayCheckMemo = "DI0df942bd6184426bf47fce10af48ef";
        public const string DATAITEM_ClaimsPayRequestType = "DI147b9c1c5a743b5b03fea857d34127";
        public const string DATAITEM_PM_IP_PaymentID = "DI1bbf82ad9c84ca793023bae087511b";
        public const string DATAITEM_Proxy_Number = "DI21b0d0c2d13411eb3ad7faef1d26b0";
        public const string DATAITEM_PM_RejectReason = "DI23018ab44df4272898a1fb23416fae";
        public const string DATAITEM_ClaimsPayType = "DI241c58c6c0b4c6c9d1ada23b82f08a";
        public const string DATAITEM_PM_MailTrackingNumber = "DI247267f97734d61b671ad3434e388b";
        public const string DATAITEM_PM_Monitored = "DI25a0655e7214369ad5620df3c6585d";
        public const string DATAITEM_LoanAccountNumber = "DI406304e430e46aa9292107be7cb0c2";
        public const string DATAITEM_ClaimsPayMethod = "DI4192eb1d86243a7b6a1f6f0b96e468";
        public const string DATAITEM_PM_Method_Last4Digit = "DI5b322eb0f034ac295cb42b04ebb0de";
        public const string DATAITEM_PM_Funded = "DI5c9cad420d94a7686c21bd41c31d71";
        public const string DATAITEM_PM_Orig_Method = "DI5fdf982e202463bbf3c426d1e70dab";
        public const string DATAITEM_PM_ClearedDate = "DI7cb3023dc804c01a461712f33686dd";
        public const string DATAITEM_ApprovalRequired = "DI80006fd2a054f65ae9870cf0c3f048";
        public const string DATAITEM_PM_ErrorMessage = "DI88b08ff4d194b45ba7237be7030236";
        public const string DATAITEM_PM_PaidDate = "DI8f60b983c7d497c88f0560617f04f3";
        public const string DATAITEM_PM_RejectPayeeID = "DI9279277e1e54fedb7ef7254d4a6a68";
        public const string DATAITEM_Expedite = "DIb313d485ad54b59be91063cb676973";
        public const string DATAITEM_PrintNow = "DIbe838441b8d4f6dbbbb9a129d95470";
        public const string DATAITEM_PM_Status = "DIbfbca1a4f40470196c2a05f3b272f5";
        public const string DATAITEM_PM_EscheatDate = "DId02749dd9644b39ad1bedcc915a967";
        public const string DATAITEM_PM_ErrorCode = "DIe318c50aec0436898306eb1acbb381";
        public const string DATAITEM_Certified = "DIf8be4dc9a9440449fab10964ca0d4c";



        //DATAITEM_FQN
        public const string DATAITEM_FQN_PM_Selection = "PaymentCstm.DI08ae41df43940a1a5130cc4cdd944e";
        public const string DATAITEM_FQN_ClaimsPayCheckMemo = "PaymentCstm.DI0df942bd6184426bf47fce10af48ef";
        public const string DATAITEM_FQN_ClaimsPayRequestType = "PaymentCstm.DI147b9c1c5a743b5b03fea857d34127";
        public const string DATAITEM_FQN_IP_PaymentID = "PaymentCstm.DI1bbf82ad9c84ca793023bae087511b";
        public const string DATAITEM_FQN_Proxy_Number = "PaymentCstm.DI21b0d0c2d13411eb3ad7faef1d26b0";
        public const string DATAITEM_FQN_PM_RejectReason = "PaymentCstm.DI23018ab44df4272898a1fb23416fae";
        public const string DATAITEM_FQN_ClaimsPayType = "PaymentCstm.DI241c58c6c0b4c6c9d1ada23b82f08a";
        public const string DATAITEM_FQN_PM_MailTrackingNumber = "PaymentCstm.DI247267f97734d61b671ad3434e388b";
        public const string DATAITEM_FQN_PM_Monitored = "PaymentCstm.DI25a0655e7214369ad5620df3c6585d";
        public const string DATAITEM_FQN_LoanAccountNumber = "PaymentCstm.DI406304e430e46aa9292107be7cb0c2";
        public const string DATAITEM_FQN_ClaimsPayMethod = "PaymentCstm.DI4192eb1d86243a7b6a1f6f0b96e468";
        public const string DATAITEM_FQN_PM_Method_Last4Digit = "PaymentCstm.DI5b322eb0f034ac295cb42b04ebb0de";
        public const string DATAITEM_FQN_PM_Funded = "PaymentCstm.DI5c9cad420d94a7686c21bd41c31d71";
        public const string DATAITEM_FQN_PM_Orig_Method = "PaymentCstm.DI5fdf982e202463bbf3c426d1e70dab";
        public const string DATAITEM_FQN_PM_ClearedDate = "PaymentCstm.DI7cb3023dc804c01a461712f33686dd";
        public const string DATAITEM_FQN_ApprovalRequired = "PaymentCstm.DI80006fd2a054f65ae9870cf0c3f048";
        public const string DATAITEM_FQN_PM_ErrorMessage = "PaymentCstm.DI88b08ff4d194b45ba7237be7030236";
        public const string DATAITEM_FQN_PM_PaidDate = "PaymentCstm.DI8f60b983c7d497c88f0560617f04f3";
        public const string DATAITEM_FQN_PM_RejectPayeeID = "PaymentCstm.DI9279277e1e54fedb7ef7254d4a6a68";
        public const string DATAITEM_FQN_Expedite = "PaymentCstm.DIb313d485ad54b59be91063cb676973";
        public const string DATAITEM_FQN_PrintNow = "PaymentCstm.DIbe838441b8d4f6dbbbb9a129d95470";
        public const string DATAITEM_FQN_PM_Status = "PaymentCstm.DIbfbca1a4f40470196c2a05f3b272f5";
        public const string DATAITEM_FQN_PM_EscheatDate = "PaymentCstm.DId02749dd9644b39ad1bedcc915a967";
        public const string DATAITEM_FQN_PM_ErrorCode = "PaymentCstm.DIe318c50aec0436898306eb1acbb381";
        public const string DATAITEM_FQN_Certified = "PaymentCstm.DIf8be4dc9a9440449fab10964ca0d4c";


        public const string EventUnableToStopPayment = "IP_UnableToStopPayment";
        public const string EventUnableToCorrectPayment = "IP_UnableToCorrectPayment";
        public const string EventSendPayment = "IP_SendPayment";
        public const string EventStopVoid = "IP_StopVoidPayment";
        public const string EventSendUpdate = "IP_SendUpdate";

        
    }

}
