using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ClaimsPay.Modules.ClaimsPay.Models.Comman_Model
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AddressDTOMailTo
    {
        [JsonProperty("AddressID")]
        public string AddressID { get; set; }

        [JsonProperty("AddressType")]
        public string AddressType { get; set; }

        [JsonProperty("AddressTypeAssociations")]
        public List<AddressTypeAssociation> AddressTypeAssociations { get; set; }

        [JsonProperty("AdminDivisionPrimary")]
        public string AdminDivisionPrimary { get; set; }

        [JsonProperty("AdminDivisionSecondary")]
        public object AdminDivisionSecondary { get; set; }

        [JsonProperty("AdminDivisionTertiary")]
        public object AdminDivisionTertiary { get; set; }

        [JsonProperty("AllowPartial")]
        public bool? AllowPartial { get; set; }

        [JsonProperty("Attention")]
        public object Attention { get; set; }

        [JsonProperty("CountryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("EffectiveDate")]
        public DateTime? EffectiveDate { get; set; }

        [JsonProperty("ExpirationDate")]
        public DateTime? ExpirationDate { get; set; }

        [JsonProperty("FullAddress")]
        public string FullAddress { get; set; }

        [JsonProperty("GeographicFormatCode")]
        public string GeographicFormatCode { get; set; }

        [JsonProperty("LastTransactionID")]
        public string LastTransactionID { get; set; }

        [JsonProperty("LastUpdatedDate")]
        public DateTime? LastUpdatedDate { get; set; }

        [JsonProperty("Latitude")]
        public object Latitude { get; set; }

        [JsonProperty("LocationDetailsLine1")]
        public string LocationDetailsLine1 { get; set; }

        [JsonProperty("LocationDetailsLine2")]
        public object LocationDetailsLine2 { get; set; }

        [JsonProperty("LocationDetailsLine3")]
        public object LocationDetailsLine3 { get; set; }

        [JsonProperty("LocationDetailsLine4")]
        public object LocationDetailsLine4 { get; set; }

        [JsonProperty("Longitude")]
        public object Longitude { get; set; }

        [JsonProperty("NationalDivisionPrimary")]
        public string NationalDivisionPrimary { get; set; }

        [JsonProperty("NationalDivisionSecondary")]
        public object NationalDivisionSecondary { get; set; }

        [JsonProperty("OldAddressCountryCode")]
        public object OldAddressCountryCode { get; set; }

        [JsonProperty("PostalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("SubAddressType")]
        public object SubAddressType { get; set; }

        [JsonProperty("SubAdminDivisionPrimary")]
        public object SubAdminDivisionPrimary { get; set; }

        [JsonProperty("SubAdminDivisionSecondary")]
        public object SubAdminDivisionSecondary { get; set; }

        [JsonProperty("SubAdminDivisionTertiary")]
        public object SubAdminDivisionTertiary { get; set; }

        [JsonProperty("SubAttention")]
        public object SubAttention { get; set; }

        [JsonProperty("SubCountryCode")]
        public object SubCountryCode { get; set; }

        [JsonProperty("SubEffectiveDate")]
        public DateTime? SubEffectiveDate { get; set; }

        [JsonProperty("SubExpirationDate")]
        public DateTime? SubExpirationDate { get; set; }

        [JsonProperty("SubLastTransactionID")]
        public object SubLastTransactionID { get; set; }

        [JsonProperty("SubLastUpdatedDate")]
        public DateTime? SubLastUpdatedDate { get; set; }

        [JsonProperty("SubLatitude")]
        public object SubLatitude { get; set; }

        [JsonProperty("SubLocationDetailsLine1")]
        public object SubLocationDetailsLine1 { get; set; }

        [JsonProperty("SubLocationDetailsLine2")]
        public object SubLocationDetailsLine2 { get; set; }

        [JsonProperty("SubLocationDetailsLine3")]
        public object SubLocationDetailsLine3 { get; set; }

        [JsonProperty("SubLocationDetailsLine4")]
        public object SubLocationDetailsLine4 { get; set; }

        [JsonProperty("SubLongitude")]
        public object SubLongitude { get; set; }

        [JsonProperty("SubNationalDivisionPrimary")]
        public object SubNationalDivisionPrimary { get; set; }

        [JsonProperty("SubNationalDivisionSecondary")]
        public object SubNationalDivisionSecondary { get; set; }

        [JsonProperty("SubPostalCode")]
        public object SubPostalCode { get; set; }
    }

    public class AddressTypeAssociation
    {
        [JsonProperty("AddressID")]
        public string AddressID { get; set; }

        [JsonProperty("AddressType")]
        public string AddressType { get; set; }

        [JsonProperty("AddressTypeAssociationID")]
        public string AddressTypeAssociationID { get; set; }

        [JsonProperty("EffectiveDate")]
        public DateTime? EffectiveDate { get; set; }

        [JsonProperty("ExpirationDate")]
        public DateTime? ExpirationDate { get; set; }

        [JsonProperty("LastTransactionID")]
        public string LastTransactionID { get; set; }

        [JsonProperty("LastUpdatedDate")]
        public DateTime? LastUpdatedDate { get; set; }
    }

    public class ApprovedAmount
    {
        [JsonProperty("UnitOfMoney")]
        public string UnitOfMoney { get; set; }

        [JsonProperty("MonetaryAmount")]
        public double? MonetaryAmount { get; set; }

        [JsonProperty("EffectiveDate")]
        public DateTime? EffectiveDate { get; set; }
    }

    public class ClaimDTO
    {
        [JsonProperty("ClaimID")]
        public string ClaimID { get; set; }

        [JsonProperty("LossID")]
        public string LossID { get; set; }

        [JsonProperty("PolicyID")]
        public string PolicyID { get; set; }

        [JsonProperty("MarkInErrorIndicator")]
        public bool? MarkInErrorIndicator { get; set; }

        [JsonProperty("ClaimStatus")]
        public string ClaimStatus { get; set; }

        [JsonProperty("Sensitivity")]
        public string Sensitivity { get; set; }

        [JsonProperty("LineOfBusinessCategory")]
        public string LineOfBusinessCategory { get; set; }

        [JsonProperty("LineOfBusiness")]
        public string LineOfBusiness { get; set; }

        [JsonProperty("ClaimNumber")]
        public string ClaimNumber { get; set; }

        [JsonProperty("FileType")]
        public string FileType { get; set; }

        [JsonProperty("DateEntered")]
        public DateTime? DateEntered { get; set; }

        [JsonProperty("EnteredByID")]
        public string EnteredByID { get; set; }

        [JsonProperty("DateClosed")]
        public object DateClosed { get; set; }

        [JsonProperty("DateReopened")]
        public object DateReopened { get; set; }

        [JsonProperty("Complexity")]
        public string Complexity { get; set; }

        [JsonProperty("RegulatoryJurisdiction")]
        public string RegulatoryJurisdiction { get; set; }

        [JsonProperty("RegulatoryTerritory")]
        public object RegulatoryTerritory { get; set; }

        [JsonProperty("RegulatoryCountry")]
        public string RegulatoryCountry { get; set; }

        [JsonProperty("HandlingCompany")]
        public object HandlingCompany { get; set; }

        [JsonProperty("SuitFiledIndicator")]
        public bool? SuitFiledIndicator { get; set; }

        [JsonProperty("InsuredNotificationDate")]
        public object InsuredNotificationDate { get; set; }

        [JsonProperty("AccountLocation")]
        public object AccountLocation { get; set; }

        [JsonProperty("AccountLocationDescription")]
        public object AccountLocationDescription { get; set; }

        [JsonProperty("ClaimGroupPushDate")]
        public object ClaimGroupPushDate { get; set; }

        [JsonProperty("ConvertedClaimIndicator")]
        public bool? ConvertedClaimIndicator { get; set; }

        [JsonProperty("InternationalClaimNumber")]
        public object InternationalClaimNumber { get; set; }

        [JsonProperty("ClaimToSuitIndicator")]
        public bool? ClaimToSuitIndicator { get; set; }

        [JsonProperty("HandlingDepartment")]
        public object HandlingDepartment { get; set; }

        [JsonProperty("DefaultCurrency")]
        public string DefaultCurrency { get; set; }

        [JsonProperty("AllowAutomatedPaymentIndicator")]
        public bool? AllowAutomatedPaymentIndicator { get; set; }

        [JsonProperty("RecoveryScore")]
        public object RecoveryScore { get; set; }

        [JsonProperty("InsuredReportNumber")]
        public object InsuredReportNumber { get; set; }

        [JsonProperty("ClaimSourceType")]
        public object ClaimSourceType { get; set; }

        [JsonProperty("LastTransactionID")]
        public string LastTransactionID { get; set; }

        [JsonProperty("ClientSatisfaction")]
        public int? ClientSatisfaction { get; set; }

        [JsonProperty("ProductLines")]
        public object ProductLines { get; set; }
    }

    public class Column
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("datatype")]
        public string datatype { get; set; }

        [JsonProperty("value")]
        public Value value { get; set; }

        [JsonProperty("originalvalue")]
        public Originalvalue originalvalue { get; set; }

        [JsonProperty("begindate")]
        public DateTime? begindate { get; set; }

        [JsonProperty("enddate")]
        public DateTime? enddate { get; set; }
    }

    public class Columns
    {
        [JsonProperty("column")]
        public List<Column> column { get; set; }
    }

    public class ContactAddressDetail
    {
        [JsonProperty("AddressID")]
        public string AddressID { get; set; }

        [JsonProperty("AddressType")]
        public string AddressType { get; set; }

        [JsonProperty("AddressTypeAssociations")]
        public List<AddressTypeAssociation> AddressTypeAssociations { get; set; }

        [JsonProperty("AdminDivisionPrimary")]
        public string AdminDivisionPrimary { get; set; }

        [JsonProperty("AdminDivisionSecondary")]
        public object AdminDivisionSecondary { get; set; }

        [JsonProperty("AdminDivisionTertiary")]
        public object AdminDivisionTertiary { get; set; }

        [JsonProperty("AllowPartial")]
        public bool? AllowPartial { get; set; }

        [JsonProperty("Attention")]
        public object Attention { get; set; }

        [JsonProperty("CountryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("EffectiveDate")]
        public DateTime? EffectiveDate { get; set; }

        [JsonProperty("ExpirationDate")]
        public DateTime? ExpirationDate { get; set; }

        [JsonProperty("FullAddress")]
        public string FullAddress { get; set; }

        [JsonProperty("GeographicFormatCode")]
        public string GeographicFormatCode { get; set; }

        [JsonProperty("LastTransactionID")]
        public string LastTransactionID { get; set; }

        [JsonProperty("LastUpdatedDate")]
        public DateTime? LastUpdatedDate { get; set; }

        [JsonProperty("Latitude")]
        public object Latitude { get; set; }

        [JsonProperty("LocationDetailsLine1")]
        public string LocationDetailsLine1 { get; set; }

        [JsonProperty("LocationDetailsLine2")]
        public object LocationDetailsLine2 { get; set; }

        [JsonProperty("LocationDetailsLine3")]
        public object LocationDetailsLine3 { get; set; }

        [JsonProperty("LocationDetailsLine4")]
        public object LocationDetailsLine4 { get; set; }

        [JsonProperty("Longitude")]
        public object Longitude { get; set; }

        [JsonProperty("NationalDivisionPrimary")]
        public string NationalDivisionPrimary { get; set; }

        [JsonProperty("NationalDivisionSecondary")]
        public object NationalDivisionSecondary { get; set; }

        [JsonProperty("OldAddressCountryCode")]
        public object OldAddressCountryCode { get; set; }

        [JsonProperty("PostalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("SubAddressType")]
        public object SubAddressType { get; set; }

        [JsonProperty("SubAdminDivisionPrimary")]
        public object SubAdminDivisionPrimary { get; set; }

        [JsonProperty("SubAdminDivisionSecondary")]
        public object SubAdminDivisionSecondary { get; set; }

        [JsonProperty("SubAdminDivisionTertiary")]
        public object SubAdminDivisionTertiary { get; set; }

        [JsonProperty("SubAttention")]
        public object SubAttention { get; set; }

        [JsonProperty("SubCountryCode")]
        public object SubCountryCode { get; set; }

        [JsonProperty("SubEffectiveDate")]
        public DateTime? SubEffectiveDate { get; set; }

        [JsonProperty("SubExpirationDate")]
        public DateTime? SubExpirationDate { get; set; }

        [JsonProperty("SubLastTransactionID")]
        public object SubLastTransactionID { get; set; }

        [JsonProperty("SubLastUpdatedDate")]
        public DateTime? SubLastUpdatedDate { get; set; }

        [JsonProperty("SubLatitude")]
        public object SubLatitude { get; set; }

        [JsonProperty("SubLocationDetailsLine1")]
        public object SubLocationDetailsLine1 { get; set; }

        [JsonProperty("SubLocationDetailsLine2")]
        public object SubLocationDetailsLine2 { get; set; }

        [JsonProperty("SubLocationDetailsLine3")]
        public object SubLocationDetailsLine3 { get; set; }

        [JsonProperty("SubLocationDetailsLine4")]
        public object SubLocationDetailsLine4 { get; set; }

        [JsonProperty("SubLongitude")]
        public object SubLongitude { get; set; }

        [JsonProperty("SubNationalDivisionPrimary")]
        public object SubNationalDivisionPrimary { get; set; }

        [JsonProperty("SubNationalDivisionSecondary")]
        public object SubNationalDivisionSecondary { get; set; }

        [JsonProperty("SubPostalCode")]
        public object SubPostalCode { get; set; }
    }

    public class ContactInfoDTO
    {
        [JsonProperty("ContactParticipantID")]
        public string ContactParticipantID { get; set; }

        [JsonProperty("ContactPartyType")]
        public string ContactPartyType { get; set; }

        [JsonProperty("ContactFirstName")]
        public string ContactFirstName { get; set; }

        [JsonProperty("ContactMiddleName")]
        public string ContactMiddleName { get; set; }

        [JsonProperty("ContactLastName")]
        public string ContactLastName { get; set; }

        [JsonProperty("ContactFirstNamePhonetic")]
        public string ContactFirstNamePhonetic { get; set; }

        [JsonProperty("ContactMiddleNamePhonetic")]
        public string ContactMiddleNamePhonetic { get; set; }

        [JsonProperty("ContactLastNamePhonetic")]
        public string ContactLastNamePhonetic { get; set; }

        [JsonProperty("ContactFullName")]
        public string ContactFullName { get; set; }

        [JsonProperty("ContactBusinessName")]
        public string ContactBusinessName { get; set; }

        [JsonProperty("ContactBusinessNamePhonetic")]
        public object ContactBusinessNamePhonetic { get; set; }

        [JsonProperty("ContactPhoneType")]
        public string ContactPhoneType { get; set; }

        [JsonProperty("ContactFullPhone")]
        public string ContactFullPhone { get; set; }

        [JsonProperty("ContactPhoneExt")]
        public object ContactPhoneExt { get; set; }

        [JsonProperty("ContactEmailAddress")]
        public string ContactEmailAddress { get; set; }

        [JsonProperty("ContactRegistrationType")]
        public string ContactRegistrationType { get; set; }

        [JsonProperty("ContactRegistrationID")]
        public string ContactRegistrationID { get; set; }

        [JsonProperty("ContactFullAddress")]
        public string ContactFullAddress { get; set; }

        [JsonProperty("ContactISOCountry")]
        public string ContactISOCountry { get; set; }

        [JsonProperty("ContactAddressDetail")]
        public ContactAddressDetail ContactAddressDetail { get; set; }
    }

    public class Entitydata
    {
        [JsonProperty("keys")]
        public Keys keys { get; set; }

        [JsonProperty("columns")]
        public Columns columns { get; set; }
    }

    public class ExtendedData
    {
        [JsonProperty("extendeddata")]
        public Extendeddata2 extendeddata { get; set; }
    }

    public class Extendeddata2
    {
        [JsonProperty("@xmlns:xs")]
        public string xmlnsxs { get; set; }

        [JsonProperty("@xmlns:xsi")]
        public string xmlnsxsi { get; set; }

        [JsonProperty("table")]
        public Table table { get; set; }
    }

    public class Key
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("datatype")]
        public string datatype { get; set; }

        [JsonProperty("value")]
        public Value value { get; set; }
    }

    public class Keys
    {
        [JsonProperty("key")]
        public Key key { get; set; }
    }

    public class LineDTO
    {
        [JsonProperty("LineID")]
        public string LineID { get; set; }

        [JsonProperty("AddressID")]
        public string AddressID { get; set; }

        [JsonProperty("InvolvementRoleID")]
        public string InvolvementRoleID { get; set; }

        [JsonProperty("ClaimID")]
        public string ClaimID { get; set; }

        [JsonProperty("ClaimantName")]
        public string ClaimantName { get; set; }

        [JsonProperty("CauseOfLoss")]
        public string CauseOfLoss { get; set; }

        [JsonProperty("LineType")]
        public string LineType { get; set; }

        [JsonProperty("LineTypeDescription")]
        public string LineTypeDescription { get; set; }

        [JsonProperty("LineComplexity")]
        public string LineComplexity { get; set; }

        [JsonProperty("LineRegulatoryState")]
        public string LineRegulatoryState { get; set; }

        [JsonProperty("HandlingStatus")]
        public string HandlingStatus { get; set; }

        [JsonProperty("LineCloseReason")]
        public object LineCloseReason { get; set; }

        [JsonProperty("ReserveCategory")]
        public object ReserveCategory { get; set; }

        [JsonProperty("LineRegulatoryCountry")]
        public string LineRegulatoryCountry { get; set; }

        [JsonProperty("LineRegulatoryCounty")]
        public object LineRegulatoryCounty { get; set; }

        [JsonProperty("LineUOM")]
        public string LineUOM { get; set; }

        [JsonProperty("LineFinancialStatus")]
        public string LineFinancialStatus { get; set; }

        [JsonProperty("LineFinancialCloseReason")]
        public object LineFinancialCloseReason { get; set; }

        [JsonProperty("LineFinancialsCloseDate")]
        public object LineFinancialsCloseDate { get; set; }

        [JsonProperty("LineFinancialsReopenDate")]
        public object LineFinancialsReopenDate { get; set; }

        [JsonProperty("CoverageMatchStatus")]
        public string CoverageMatchStatus { get; set; }

        [JsonProperty("RecoveryIndicator")]
        public bool? RecoveryIndicator { get; set; }

        [JsonProperty("LegacySalvageStatus")]
        public object LegacySalvageStatus { get; set; }

        [JsonProperty("LegacySubrogationStatus")]
        public object LegacySubrogationStatus { get; set; }

        [JsonProperty("AdditionalFinalPayment")]
        public object AdditionalFinalPayment { get; set; }

        [JsonProperty("LineReserveMethod")]
        public object LineReserveMethod { get; set; }

        [JsonProperty("SettlementMethod")]
        public object SettlementMethod { get; set; }

        [JsonProperty("ExpenseOnlyIndicator")]
        public bool? ExpenseOnlyIndicator { get; set; }

        [JsonProperty("SIUIndicator")]
        public object SIUIndicator { get; set; }

        [JsonProperty("NatureOfOperation")]
        public object NatureOfOperation { get; set; }

        [JsonProperty("PrimaryExcessLine")]
        public object PrimaryExcessLine { get; set; }

        [JsonProperty("LineLocationOfLoss")]
        public object LineLocationOfLoss { get; set; }

        [JsonProperty("PropertyType")]
        public object PropertyType { get; set; }

        [JsonProperty("LossCharacteristic")]
        public object LossCharacteristic { get; set; }

        [JsonProperty("ThirdPartyIndicator")]
        public bool? ThirdPartyIndicator { get; set; }

        [JsonProperty("ValidProductCatalogLineIndicator")]
        public bool? ValidProductCatalogLineIndicator { get; set; }

        [JsonProperty("AutoReserveIndicator")]
        public bool? AutoReserveIndicator { get; set; }

        [JsonProperty("AutomatedLineGenIndicator")]
        public bool? AutomatedLineGenIndicator { get; set; }

        [JsonProperty("ContributingFactor")]
        public object ContributingFactor { get; set; }

        [JsonProperty("DamageDescription")]
        public object DamageDescription { get; set; }

        [JsonProperty("VehicleLocationDetail")]
        public object VehicleLocationDetail { get; set; }

        [JsonProperty("MedicareIndicator")]
        public bool? MedicareIndicator { get; set; }

        [JsonProperty("LineTypeOverrideIndicator")]
        public bool? LineTypeOverrideIndicator { get; set; }

        [JsonProperty("LineTypeOverridingOrgEntity")]
        public object LineTypeOverridingOrgEntity { get; set; }

        [JsonProperty("LastTransactionID")]
        public string LastTransactionID { get; set; }

        [JsonProperty("LiableItemID")]
        public object LiableItemID { get; set; }
    }

    public class Originalvalue
    {
        [JsonProperty("@xsi:nil")]
        public string xsinil { get; set; }
    }

    public class ParticipantDataObject
    {
        [JsonProperty("ParticipantID")]
        public string ParticipantID { get; set; }

        [JsonProperty("ClaimID")]
        public string ClaimID { get; set; }

        [JsonProperty("PartyID")]
        public string PartyID { get; set; }

        [JsonProperty("PolicyAdded")]
        public bool? PolicyAdded { get; set; }

        [JsonProperty("PrefNameID")]
        public string PrefNameID { get; set; }

        [JsonProperty("PrefAddressID")]
        public string PrefAddressID { get; set; }

        [JsonProperty("PrefEmailID")]
        public string PrefEmailID { get; set; }

        [JsonProperty("PrefPhoneID")]
        public string PrefPhoneID { get; set; }

        [JsonProperty("EFTActive")]
        public object EFTActive { get; set; }

        [JsonProperty("ContactNote")]
        public object ContactNote { get; set; }

        [JsonProperty("DriversLicenseNumber")]
        public object DriversLicenseNumber { get; set; }

        [JsonProperty("DriverState")]
        public object DriverState { get; set; }

        [JsonProperty("AlternateID")]
        public object AlternateID { get; set; }

        [JsonProperty("AlternateIDType")]
        public object AlternateIDType { get; set; }

        [JsonProperty("BeneDependencyType")]
        public object BeneDependencyType { get; set; }

        [JsonProperty("DriverCountry")]
        public object DriverCountry { get; set; }

        [JsonProperty("ColBargAgreementCode")]
        public object ColBargAgreementCode { get; set; }

        [JsonProperty("CIBRequestDate")]
        public object CIBRequestDate { get; set; }

        [JsonProperty("ReportedAge")]
        public object ReportedAge { get; set; }

        [JsonProperty("DateOfBirth")]
        public object DateOfBirth { get; set; }

        [JsonProperty("TaxID")]
        public string TaxID { get; set; }

        [JsonProperty("PartialTaxID")]
        public object PartialTaxID { get; set; }

        [JsonProperty("SSN")]
        public object SSN { get; set; }

        [JsonProperty("Gender")]
        public object Gender { get; set; }

        [JsonProperty("DependentsUnder18")]
        public object DependentsUnder18 { get; set; }

        [JsonProperty("EmployerUINumber")]
        public object EmployerUINumber { get; set; }

        [JsonProperty("PreferredContactType")]
        public string PreferredContactType { get; set; }

        [JsonProperty("AnalyticScore")]
        public object AnalyticScore { get; set; }

        [JsonProperty("LastTransactionID")]
        public string LastTransactionID { get; set; }

        [JsonProperty("ParticipantRolesDTO")]
        public List<ParticipantRolesDTO> ParticipantRolesDTO { get; set; }

        [JsonProperty("ParticipantFullName")]
        public string ParticipantFullName { get; set; }

        [JsonProperty("InsuranceExist")]
        public object InsuranceExist { get; set; }

        [JsonProperty("ThirdPartyPolicyNumber")]
        public object ThirdPartyPolicyNumber { get; set; }

        [JsonProperty("CoverageConfirmed")]
        public object CoverageConfirmed { get; set; }

        [JsonProperty("ParticipantNameDetailDTO")]
        public ParticipantNameDetailDTO ParticipantNameDetailDTO { get; set; }
    }

    public class ParticipantNameDetailDTO
    {
        [JsonProperty("PrefixCode")]
        public object PrefixCode { get; set; }

        [JsonProperty("SuffixCode")]
        public object SuffixCode { get; set; }

        [JsonProperty("FirstName")]
        public object FirstName { get; set; }

        [JsonProperty("MiddleName")]
        public object MiddleName { get; set; }

        [JsonProperty("LastName")]
        public object LastName { get; set; }

        [JsonProperty("CountryCode")]
        public string CountryCode { get; set; }
    }

    public class ParticipantRolesDTO
    {
        [JsonProperty("ParticipantRoleID")]
        public string ParticipantRoleID { get; set; }

        [JsonProperty("ParticipantID")]
        public string ParticipantID { get; set; }

        [JsonProperty("ClaimID")]
        public string ClaimID { get; set; }

        [JsonProperty("ParticipantRole")]
        public string ParticipantRole { get; set; }

        [JsonProperty("ActivationDate")]
        public object ActivationDate { get; set; }

        [JsonProperty("DeactivationDate")]
        public object DeactivationDate { get; set; }

        [JsonProperty("LastTransactionID")]
        public string LastTransactionID { get; set; }
    }

    public class PaymentDetailDTO
    {
        [JsonProperty("PaymentDetailID")]
        public string PaymentDetailID { get; set; }

        [JsonProperty("LineID")]
        public string LineID { get; set; }

        [JsonProperty("InvoiceDetailID")]
        public object InvoiceDetailID { get; set; }

        [JsonProperty("RecurringPaymentDetailID")]
        public object RecurringPaymentDetailID { get; set; }

        [JsonProperty("PaymentID")]
        public string PaymentID { get; set; }

        [JsonProperty("ItemArticleID")]
        public object ItemArticleID { get; set; }

        [JsonProperty("PaymentFrequency")]
        public object PaymentFrequency { get; set; }

        [JsonProperty("FinancialCategory")]
        public string FinancialCategory { get; set; }

        [JsonProperty("BenefitOffset")]
        public object BenefitOffset { get; set; }

        [JsonProperty("Service")]
        public string Service { get; set; }

        [JsonProperty("ServiceStartDate")]
        public object ServiceStartDate { get; set; }

        [JsonProperty("ServiceEndDate")]
        public object ServiceEndDate { get; set; }

        [JsonProperty("LumpSumIndicator")]
        public bool? LumpSumIndicator { get; set; }

        [JsonProperty("ApprovedAmount")]
        public ApprovedAmount ApprovedAmount { get; set; }

        [JsonProperty("ApprovedAmountWithoutVAT")]
        public object ApprovedAmountWithoutVAT { get; set; }

        [JsonProperty("VATPercentage")]
        public object VATPercentage { get; set; }

        [JsonProperty("VATNumeric")]
        public object VATNumeric { get; set; }

        [JsonProperty("MedicalEvalutator")]
        public object MedicalEvalutator { get; set; }

        [JsonProperty("DeductionReason")]
        public object DeductionReason { get; set; }

        [JsonProperty("Reportable1099")]
        public string Reportable1099 { get; set; }

        [JsonProperty("ErodeLimit")]
        public bool? ErodeLimit { get; set; }

        [JsonProperty("TaxType")]
        public object TaxType { get; set; }

        [JsonProperty("GSTType")]
        public object GSTType { get; set; }

        [JsonProperty("LastTransactionID")]
        public string LastTransactionID { get; set; }

        [JsonProperty("VATMode")]
        public object VATMode { get; set; }

        [JsonProperty("VATType")]
        public object VATType { get; set; }

        [JsonProperty("VATApplicableComponent")]
        public bool? VATApplicableComponent { get; set; }

        [JsonProperty("InactiveIndicator")]
        public bool? InactiveIndicator { get; set; }

        [JsonProperty("NumberOfVisits")]
        public object NumberOfVisits { get; set; }

        [JsonProperty("ParticipantRoleID")]
        public string ParticipantRoleID { get; set; }

        [JsonProperty("LastUpdate")]
        public DateTime? LastUpdate { get; set; }

        [JsonProperty("ExtendedData")]
        public object ExtendedData { get; set; }
    }

    public class PaymentHeader
    {
        [JsonProperty("PaymentID")]
        public string PaymentID { get; set; }

        [JsonProperty("RecurringPaymentID")]
        public object RecurringPaymentID { get; set; }

        [JsonProperty("IssueDate")]
        public DateTime? IssueDate { get; set; }

        [JsonProperty("FinalPaymentIndicator")]
        public bool? FinalPaymentIndicator { get; set; }

        [JsonProperty("AuthorityTaskExist")]
        public bool? AuthorityTaskExist { get; set; }

        [JsonProperty("PaymentMethod")]
        public string PaymentMethod { get; set; }

        [JsonProperty("StopVoid")]
        public object StopVoid { get; set; }

        [JsonProperty("StopVoidReason")]
        public object StopVoidReason { get; set; }

        [JsonProperty("PaymentType")]
        public string PaymentType { get; set; }

        [JsonProperty("AttachmentIndicator")]
        public bool? AttachmentIndicator { get; set; }

        [JsonProperty("PrimaryPayeeInsuranceInvolvementID")]
        public string PrimaryPayeeInsuranceInvolvementID { get; set; }

        [JsonProperty("MailToInsuranceInvolvementID")]
        public string MailToInsuranceInvolvementID { get; set; }

        [JsonProperty("PrimaryPayeeClientID")]
        public string PrimaryPayeeClientID { get; set; }

        [JsonProperty("MailTOClientID")]
        public string MailTOClientID { get; set; }

        [JsonProperty("ExplainationOfBenefitComments")]
        public object ExplainationOfBenefitComments { get; set; }

        [JsonProperty("BankAccountNumber")]
        public string BankAccountNumber { get; set; }

        [JsonProperty("BankTransitNumber")]
        public string BankTransitNumber { get; set; }

        [JsonProperty("HoldReason")]
        public object HoldReason { get; set; }

        [JsonProperty("HandlingNotes")]
        public object HandlingNotes { get; set; }

        [JsonProperty("ElectronFundsTransferID")]
        public object ElectronFundsTransferID { get; set; }

        [JsonProperty("PayeeName")]
        public object PayeeName { get; set; }

        [JsonProperty("MailToName")]
        public string MailToName { get; set; }

        [JsonProperty("MailToAddressID")]
        public string MailToAddressID { get; set; }

        [JsonProperty("ReconciliationStatus")]
        public object ReconciliationStatus { get; set; }

        [JsonProperty("PaymentNumber")]
        public string PaymentNumber { get; set; }

        [JsonProperty("PaymentStatus")]
        public string PaymentStatus { get; set; }

        [JsonProperty("CheckStatus")]
        public string CheckStatus { get; set; }

        [JsonProperty("PrinterLocation")]
        public object PrinterLocation { get; set; }

        [JsonProperty("CheckMemo")]
        public object CheckMemo { get; set; }

        [JsonProperty("PaymentGroupNumber")]
        public object PaymentGroupNumber { get; set; }

        [JsonProperty("InvoiceID")]
        public object InvoiceID { get; set; }

        [JsonProperty("PaymentCurrency")]
        public string PaymentCurrency { get; set; }

        [JsonProperty("TotalApprovedAmount")]
        public double? TotalApprovedAmount { get; set; }

        [JsonProperty("CoversionDate")]
        public DateTime? CoversionDate { get; set; }

        [JsonProperty("Language")]
        public string Language { get; set; }

        [JsonProperty("OfficeOrgEntityID")]
        public string OfficeOrgEntityID { get; set; }

        [JsonProperty("UserOrgEntityID")]
        public string UserOrgEntityID { get; set; }

        [JsonProperty("CustomCheckPartyId")]
        public object CustomCheckPartyId { get; set; }

        [JsonProperty("LastTransactionID")]
        public string LastTransactionID { get; set; }

        [JsonProperty("ReportablePartyID")]
        public string ReportablePartyID { get; set; }

        [JsonProperty("LastUpdate")]
        public DateTime? LastUpdate { get; set; }

        [JsonProperty("ExtendedData")]
        public object ExtendedData { get; set; }
    }

    public class PaymentPayeeDataObjectsList
    {
        [JsonProperty("PaymentPayeeID")]
        public string PaymentPayeeID { get; set; }

        [JsonProperty("PaymentID")]
        public string PaymentID { get; set; }

        [JsonProperty("InsuranceInvolvementID")]
        public string InsuranceInvolvementID { get; set; }

        [JsonProperty("ClientID")]
        public string ClientID { get; set; }

        [JsonProperty("DateSentToOFAC")]
        public DateTime? DateSentToOFAC { get; set; }

        [JsonProperty("DateResponseRecievedFromOFAC")]
        public DateTime? DateResponseRecievedFromOFAC { get; set; }

        [JsonProperty("MatchReportReceivedIndicator")]
        public bool? MatchReportReceivedIndicator { get; set; }

        [JsonProperty("BatchNumber")]
        public int? BatchNumber { get; set; }

        [JsonProperty("LastTransactionID")]
        public string LastTransactionID { get; set; }
    }

    public class PerformerDTO
    {
        [JsonProperty("PerformerID")]
        public string PerformerID { get; set; }

        [JsonProperty("LineID")]
        public object LineID { get; set; }

        [JsonProperty("ClaimantRoleID")]
        public object ClaimantRoleID { get; set; }

        [JsonProperty("PerformerActivationDate")]
        public DateTime? PerformerActivationDate { get; set; }

        [JsonProperty("PerformerDeactivationDate")]
        public object PerformerDeactivationDate { get; set; }

        [JsonProperty("PerformerRole")]
        public string PerformerRole { get; set; }

        [JsonProperty("ClaimID")]
        public string ClaimID { get; set; }

        [JsonProperty("OrganizationEntityID")]
        public string OrganizationEntityID { get; set; }

        [JsonProperty("LastTransactionID")]
        public string LastTransactionID { get; set; }

        [JsonProperty("OrganizationType")]
        public object OrganizationType { get; set; }

        [JsonProperty("OrganizationCorporateName")]
        public object OrganizationCorporateName { get; set; }

        [JsonProperty("OrganizationFirstName")]
        public object OrganizationFirstName { get; set; }

        [JsonProperty("OrganizationMiddleName")]
        public object OrganizationMiddleName { get; set; }

        [JsonProperty("OrganizationLastName")]
        public object OrganizationLastName { get; set; }

        [JsonProperty("OrganizationActivationDate")]
        public DateTime? OrganizationActivationDate { get; set; }

        [JsonProperty("OrganizationDeactivationDate")]
        public object OrganizationDeactivationDate { get; set; }

        [JsonProperty("OrganizationEmail")]
        public object OrganizationEmail { get; set; }

        [JsonProperty("PerformerFullName")]
        public string PerformerFullName { get; set; }

        [JsonProperty("CanUserViewParty")]
        public bool? CanUserViewParty { get; set; }

        [JsonProperty("CanUserListParty")]
        public bool? CanUserListParty { get; set; }

        [JsonProperty("PerformerNameDetailDTO")]
        public PerformerNameDetailDTO PerformerNameDetailDTO { get; set; }
    }

    public class PerformerNameDetailDTO
    {
        [JsonProperty("PrefixCode")]
        public object PrefixCode { get; set; }

        [JsonProperty("SuffixCode")]
        public object SuffixCode { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("MiddleName")]
        public object MiddleName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("CountryCode")]
        public string CountryCode { get; set; }
    }

    public class DTOModel
    {
        [JsonProperty("PaymentHeader")]
        public PaymentHeader PaymentHeader { get; set; }

        [JsonProperty("ParticipantDataObject")]
        public ParticipantDataObject ParticipantDataObject { get; set; }

        [JsonProperty("PaymentPayeeDataObjectsList")]
        public List<PaymentPayeeDataObjectsList> PaymentPayeeDataObjectsList { get; set; }

        [JsonProperty("ClaimDTO")]
        public ClaimDTO ClaimDTO { get; set; }

        [JsonProperty("LineDTOs")]
        public List<LineDTO> LineDTOs { get; set; }

        [JsonProperty("ContactInfoDTO")]
        public ContactInfoDTO ContactInfoDTO { get; set; }

        [JsonProperty("PerformerDTO")]
        public PerformerDTO PerformerDTO { get; set; }

        [JsonProperty("PaymentDetailDTOs")]
        public List<PaymentDetailDTO> PaymentDetailDTOs { get; set; }

        [JsonProperty("AddressDTO_MailTo")]
        public AddressDTOMailTo AddressDTO_MailTo { get; set; }

        [JsonProperty("__extendedData")]
        public ExtendedData __extendedData { get; set; }
    }

    public class Table
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("entitydata")]
        public Entitydata entitydata { get; set; }
    }

    public class Value
    {
        [JsonProperty("@xsi:type")]
        public string xsitype { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }

        [JsonProperty("@xsi:nil")]
        public string xsinil { get; set; }
    }



}
