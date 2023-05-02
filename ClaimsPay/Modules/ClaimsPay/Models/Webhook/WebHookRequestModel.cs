using ClaimsPay.Modules.ClaimsPay.Models.Comman_Model;
using System.Text.Json.Serialization;

namespace ClaimsPay.Modules.ClaimsPay.Models.Webhook
{
    

    public class Column
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("datatype")]
        public string datatype { get; set; }

        [JsonPropertyName("value")]
        public Value value { get; set; }

        [JsonPropertyName("originalvalue")]
        public Originalvalue originalvalue { get; set; }

        [JsonPropertyName("begindate")]
        public DateTime begindate { get; set; }

        [JsonPropertyName("enddate")]
        public DateTime enddate { get; set; }
    }

    public class Columns
    {
        [JsonPropertyName("column")]
        public List<Column> column { get; set; }
    }

    public class Entitydata
    {
        [JsonPropertyName("keys")]
        public Keys keys { get; set; }

        [JsonPropertyName("columns")]
        public Columns columns { get; set; }
    }

    public class ExtendedData
    {
        [JsonPropertyName("extendeddata")]
        public Extendeddata2 extendeddata2 { get; set; }
    }

    public class Extendeddata2
    {
        [JsonPropertyName("@xmlns:xs")]
        public string xmlnsxs { get; set; }

        [JsonPropertyName("@xmlns:xsi")]
        public string xmlnsxsi { get; set; }

        [JsonPropertyName("table")]
        public Table table { get; set; }
    }

    public class Key
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("datatype")]
        public string datatype { get; set; }

        [JsonPropertyName("value")]
        public Value value { get; set; }
    }

    public class Keys
    {
        [JsonPropertyName("key")]
        public Key key { get; set; }
    }

    public class Originalvalue
    {
        [JsonPropertyName("@xsi:nil")]
        public string xsinil { get; set; }
    }

    public class PaymentHeaderDTO
    {
        [JsonPropertyName("paymentID")]
        public string paymentID { get; set; }

        [JsonPropertyName("recurringPaymentID")]
        public object recurringPaymentID { get; set; }

        [JsonPropertyName("issueDate")]
        public DateTime issueDate { get; set; }

        [JsonPropertyName("finalPaymentIndicator")]
        public bool finalPaymentIndicator { get; set; }

        [JsonPropertyName("authorityTaskExist")]
        public bool authorityTaskExist { get; set; }

        [JsonPropertyName("paymentMethod")]
        public string paymentMethod { get; set; }

        [JsonPropertyName("stopVoid")]
        public object stopVoid { get; set; }

        [JsonPropertyName("stopVoidReason")]
        public object stopVoidReason { get; set; }

        [JsonPropertyName("paymentType")]
        public string paymentType { get; set; }

        [JsonPropertyName("attachmentIndicator")]
        public bool attachmentIndicator { get; set; }

        [JsonPropertyName("primaryPayeeInsuranceInvolvementID")]
        public string primaryPayeeInsuranceInvolvementID { get; set; }

        [JsonPropertyName("mailToInsuranceInvolvementID")]
        public string mailToInsuranceInvolvementID { get; set; }

        [JsonPropertyName("primaryPayeeClientID")]
        public string primaryPayeeClientID { get; set; }

        [JsonPropertyName("mailTOClientID")]
        public string mailTOClientID { get; set; }

        [JsonPropertyName("explainationOfBenefitComments")]
        public object explainationOfBenefitComments { get; set; }

        [JsonPropertyName("bankAccountNumber")]
        public string bankAccountNumber { get; set; }

        [JsonPropertyName("bankTransitNumber")]
        public string bankTransitNumber { get; set; }

        [JsonPropertyName("holdReason")]
        public object holdReason { get; set; }

        [JsonPropertyName("handlingNotes")]
        public object handlingNotes { get; set; }

        [JsonPropertyName("electronFundsTransferID")]
        public object electronFundsTransferID { get; set; }

        [JsonPropertyName("payeeName")]
        public object payeeName { get; set; }

        [JsonPropertyName("mailToName")]
        public string mailToName { get; set; }

        [JsonPropertyName("mailToAddressID")]
        public string mailToAddressID { get; set; }

        [JsonPropertyName("reconciliationStatus")]
        public object reconciliationStatus { get; set; }

        [JsonPropertyName("paymentNumber")]
        public string paymentNumber { get; set; }

        [JsonPropertyName("paymentStatus")]
        public string paymentStatus { get; set; }

        [JsonPropertyName("checkStatus")]
        public string checkStatus { get; set; }

        [JsonPropertyName("printerLocation")]
        public object printerLocation { get; set; }

        [JsonPropertyName("checkMemo")]
        public object checkMemo { get; set; }

        [JsonPropertyName("paymentGroupNumber")]
        public object paymentGroupNumber { get; set; }

        [JsonPropertyName("invoiceID")]
        public object invoiceID { get; set; }

        [JsonPropertyName("paymentCurrency")]
        public string paymentCurrency { get; set; }

        [JsonPropertyName("totalApprovedAmount")]
        public double totalApprovedAmount { get; set; }

        [JsonPropertyName("coversionDate")]
        public DateTime coversionDate { get; set; }

        [JsonPropertyName("language")]
        public string language { get; set; }

        [JsonPropertyName("officeOrgEntityID")]
        public string officeOrgEntityID { get; set; }

        [JsonPropertyName("userOrgEntityID")]
        public string userOrgEntityID { get; set; }

        [JsonPropertyName("customCheckPartyId")]
        public object customCheckPartyId { get; set; }

        [JsonPropertyName("lastTransactionID")]
        public string lastTransactionID { get; set; }

        [JsonPropertyName("reportablePartyID")]
        public object reportablePartyID { get; set; }

        [JsonPropertyName("lastUpdate")]
        public DateTime lastUpdate { get; set; }

        [JsonPropertyName("extendedData")]
        public object extendedData { get; set; }
    }

    public class WebHookRequestModel
    {
        [JsonPropertyName("paymentHeaderDTO")]
        public PaymentHeaderDTO paymentHeaderDTO { get; set; }

        [JsonPropertyName("__extendedData")]
        public ExtendedData __extendedData { get; set; }
    }

    public class Table
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("entitydata")]
        public Entitydata entitydata { get; set; }
    }

    public class Value
    {
        [JsonPropertyName("@xsi:type")]
        public string xsitype { get; set; }

        [JsonPropertyName("#text")]
        public string text { get; set; }

        [JsonPropertyName("@xsi:nil")]
        public string xsinil { get; set; }
    }
}
