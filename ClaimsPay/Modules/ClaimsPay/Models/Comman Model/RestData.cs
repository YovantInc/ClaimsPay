namespace ClaimsPay.Modules.ClaimsPay.Models.Comman_Model
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class RestData
    {
        public string? session { get; set; }
        public Str_Json? str_json { get; set; }
    }

    public class Str_Json
    {
        public string? BUS_BusinessId { get; set; }
        public string? BUS_City { get; set; }
        public string? BUS_Country { get; set; }
        public string? BUS_EmailAddress { get; set; }
        public string? BUS_Fax { get; set; }
        public string? BUS_Name { get; set; }
        public string? BUS_OfficePhone { get; set; }
        public string? BUS_State { get; set; }
        public string? BUS_Status { get; set; }
        public string? BUS_Street { get; set; }
        public string? BUS_SubType { get; set; }
        public string? BUS_TIN { get; set; }
        public string? BUS_TINType { get; set; }
        public string? BUS_Type { get; set; }
        public string? BUS_Zipcode { get; set; }
        public string? CL_CauseofLoss { get; set; }
        public string? CL_ClaimNumber { get; set; }
        public string? CL_DateofLoss { get; set; }
        public string? CL_PolicyNumber { get; set; }
        public string? PA_City { get; set; }
        public string? PA_Country { get; set; }
        public string? PA_State { get; set; }
        public string? PA_Street { get; set; }
        public string? PA_Zipcode { get; set; }
        public string? PCON_Approval_Reqd { get; set; }
        public string? PCON_Business { get; set; }
        public string? PCON_ContactId { get; set; }
        public string? PCON_EmailAddress { get; set; }
        public string? PCON_FirstName { get; set; }
        public string? PCON_LastName { get; set; }
        public string? PCON_MobilePhone { get; set; }
        public string? PCON_OfficePhone { get; set; }
        public string? PM_Additional_Text_1 { get; set; }
        public string? PM_Additional_Text_10 { get; set; }
        public string? PM_Additional_Text_2 { get; set; }
        public string? PM_Additional_Text_3 { get; set; }
        public string? PM_Additional_Text_4 { get; set; }
        public string? PM_Additional_Text_5 { get; set; }
        public string? PM_Additional_Text_6 { get; set; }
        public string? PM_Additional_Text_9 { get; set; }
        public double? PM_Amount { get; set; }
        public string? PM_CarrierId { get; set; }
        public string? PM_LoanAccountNumber { get; set; }
        public string? PM_PaymentID { get; set; }
        public string? PM_PaymentType { get; set; }
        public string? PM_RequestReason { get; set; }
        public string? PM_User_EmailAddress { get; set; }
        public string? PM_User_FirstName { get; set; }
        public string? PM_User_LastName { get; set; }
        public string? PM_UserId { get; set; }
        public string? PMA_City { get; set; }
        public string? PMA_Country { get; set; }
        public string? PMA_MailTo { get; set; }
        public string? PMA_State { get; set; }
        public string? PMA_Street { get; set; }
        public string? PMA_Zipcode { get; set; }
        public string? PMETHOD { get; set; }
        public string? PMETHOD_Stored { get; set; }
        public string? SCON_Approval_Reqd { get; set; }
        public string? SCON_ContactId { get; set; }
        public string? SCON_EmailAddress { get; set; }
        public string? SCON_FirstName { get; set; }
        public string? SCON_LastName { get; set; }
        public string? SCON_MobilePhone { get; set; }
        public string? SCON_OfficePhone { get; set; }
        public string? PM_Expedite { get; set; }
        public string? PM_PrintNow { get; set; }
        public string? PM_Certified { get; set; }

    }


}
