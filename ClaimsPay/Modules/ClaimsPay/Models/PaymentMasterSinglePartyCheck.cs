using System.Text.Json.Serialization;

namespace ClaimsPay.Modules.ClaimsPay.Models
{
    public class PaymentMasterSinglePartyCheck
    {    
        public string session { get; set; }
        [JsonPropertyName("str_json")]
        public StrJson strjson { get; set; }
    }

    public class StrJson
    {
        public string PM_CarrierId { get; set; }
        public string PM_PaymentID { get; set; }
        public string PM_PaymentType { get; set; }
        public string PM_Amount { get; set; }
        public string PM_UserId { get; set; }
        public string PM_User_FirstName { get; set; }
        public string PM_User_LastName { get; set; }
        public string PM_User_EmailAddress { get; set; }
        public string CL_ClaimNumber { get; set; }
        public string PCON_ContactId { get; set; }
        public string PCON_FirstName { get; set; }
        public string PCON_LastName { get; set; }
        public string PCON_EmailAddress { get; set; }
        public string PCON_MobilePhone { get; set; }
        public string PMA_Street { get; set; }
        public string PMA_City { get; set; }
        public string PMA_State { get; set; }
        public string PMA_Zipcode { get; set; }
        public string PMA_Country { get; set; }
        public string PMETHOD { get; set; }
        public string PMA_MailTo { get; set; }
        public string PM_Additional_Text_3 { get; set; }
    }


}
