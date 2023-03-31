using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ClaimsPay.Modules.ClaimsPay.Models.Comman_Model
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AddressDTOMailTo
    {
        public string AddressID { get; set; }
        public string AddressType { get; set; }
        public List<AddressTypeAssociation> AddressTypeAssociations { get; set; }
        public string AdminDivisionPrimary { get; set; }
        public object AdminDivisionSecondary { get; set; }
        public object AdminDivisionTertiary { get; set; }
        public bool AllowPartial { get; set; }
        public object Attention { get; set; }
        public string CountryCode { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string FullAddress { get; set; }
        public string GeographicFormatCode { get; set; }
        public string LastTransactionID { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public double Latitude { get; set; }
        public string LocationDetailsLine1 { get; set; }
        public object LocationDetailsLine2 { get; set; }
        public object LocationDetailsLine3 { get; set; }
        public object LocationDetailsLine4 { get; set; }
        public double Longitude { get; set; }
        public string NationalDivisionPrimary { get; set; }
        public string NationalDivisionSecondary { get; set; }
        public object OldAddressCountryCode { get; set; }
        public string PostalCode { get; set; }
        public object SubAddressType { get; set; }
        public object SubAdminDivisionPrimary { get; set; }
        public object SubAdminDivisionSecondary { get; set; }
        public object SubAdminDivisionTertiary { get; set; }
        public object SubAttention { get; set; }
        public object SubCountryCode { get; set; }
        public DateTime SubEffectiveDate { get; set; }
        public DateTime SubExpirationDate { get; set; }
        public string SubLastTransactionID { get; set; }
        public DateTime SubLastUpdatedDate { get; set; }
        public object SubLatitude { get; set; }
        public object SubLocationDetailsLine1 { get; set; }
        public object SubLocationDetailsLine2 { get; set; }
        public object SubLocationDetailsLine3 { get; set; }
        public object SubLocationDetailsLine4 { get; set; }
        public object SubLongitude { get; set; }
        public object SubNationalDivisionPrimary { get; set; }
        public object SubNationalDivisionSecondary { get; set; }
        public object SubPostalCode { get; set; }
    }

    public class AddressTypeAssociation
    {
        public string AddressID { get; set; }
        public string AddressType { get; set; }
        public string AddressTypeAssociationID { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string LastTransactionID { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }

    public class ApprovedAmount
    {
        public string m_unitOfMoney { get; set; }

        [JsonProperty("<MonetaryAmount>k__BackingField")]
        public double MonetaryAmountk__BackingField { get; set; }

        [JsonProperty("<EffectiveDate>k__BackingField")]
        public DateTime EffectiveDatek__BackingField { get; set; }
    }

    public class ClaimDTO
    {
        public string LineOfBusinessCategory { get; set; }
        public string LineOfBusiness { get; set; }
        public string ClaimStatus { get; set; }
        public string ClaimID { get; set; }
        public object DateClosed { get; set; }
        public object DateReopened { get; set; }
        public object RecoveryScore { get; set; }
        public string ClaimNumber { get; set; }
        public string LossID { get; set; }
        public string PolicyID { get; set; }
        public bool MarkInErrorIndicator { get; set; }
        public string Sensitivity { get; set; }
        public string FileType { get; set; }
        public DateTime DateEntered { get; set; }
        public string EnteredByID { get; set; }
        public string Complexity { get; set; }
        public string RegulatoryJurisdiction { get; set; }
        public object RegulatoryTerritory { get; set; }
        public string RegulatoryCountry { get; set; }
        public object HandlingCompany { get; set; }
        public bool SuitFiledIndicator { get; set; }
        public object InsuredNotificationDate { get; set; }
        public object AccountLocation { get; set; }
        public object AccountLocationDescription { get; set; }
        public object ClaimGroupPushDate { get; set; }
        public bool ConvertedClaimIndicator { get; set; }
        public object InternationalClaimNumber { get; set; }
        public bool ClaimToSuitIndicator { get; set; }
        public object HandlingDepartment { get; set; }
        public string DefaultCurrency { get; set; }
        public bool AllowAutomatedPaymentIndicator { get; set; }
        public object ClaimSourceType { get; set; }
        public string LastTransactionID { get; set; }
        public int ClientSatisfaction { get; set; }
        public object ProductLines { get; set; }
    }

    public class Columns
    {
        public object column { get; set; }
    }

    public class ContactAddressDetail
    {
        public string AddressID { get; set; }
        public string AddressType { get; set; }
        public List<AddressTypeAssociation> AddressTypeAssociations { get; set; }
        public string AdminDivisionPrimary { get; set; }
        public object AdminDivisionSecondary { get; set; }
        public object AdminDivisionTertiary { get; set; }
        public bool AllowPartial { get; set; }
        public object Attention { get; set; }
        public string CountryCode { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string FullAddress { get; set; }
        public string GeographicFormatCode { get; set; }
        public string LastTransactionID { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public double Latitude { get; set; }
        public string LocationDetailsLine1 { get; set; }
        public object LocationDetailsLine2 { get; set; }
        public object LocationDetailsLine3 { get; set; }
        public object LocationDetailsLine4 { get; set; }
        public double Longitude { get; set; }
        public string NationalDivisionPrimary { get; set; }
        public string NationalDivisionSecondary { get; set; }
        public object OldAddressCountryCode { get; set; }
        public string PostalCode { get; set; }
        public object SubAddressType { get; set; }
        public object SubAdminDivisionPrimary { get; set; }
        public object SubAdminDivisionSecondary { get; set; }
        public object SubAdminDivisionTertiary { get; set; }
        public object SubAttention { get; set; }
        public object SubCountryCode { get; set; }
        public DateTime SubEffectiveDate { get; set; }
        public DateTime SubExpirationDate { get; set; }
        public object SubLastTransactionID { get; set; }
        public DateTime SubLastUpdatedDate { get; set; }
        public object SubLatitude { get; set; }
        public object SubLocationDetailsLine1 { get; set; }
        public object SubLocationDetailsLine2 { get; set; }
        public object SubLocationDetailsLine3 { get; set; }
        public object SubLocationDetailsLine4 { get; set; }
        public object SubLongitude { get; set; }
        public object SubNationalDivisionPrimary { get; set; }
        public object SubNationalDivisionSecondary { get; set; }
        public object SubPostalCode { get; set; }
    }

    public class ContactInfoDTO
    {
        public string ContactPartyType { get; set; }
        public string ContactISOCountry { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactParticipantID { get; set; }
        public string ContactMiddleName { get; set; }
        public object ContactFirstNamePhonetic { get; set; }
        public object ContactMiddleNamePhonetic { get; set; }
        public object ContactLastNamePhonetic { get; set; }
        public string ContactFullName { get; set; }
        public string ContactBusinessName { get; set; }
        public string ContactBusinessNamePhonetic { get; set; }
        public string ContactPhoneType { get; set; }
        public string ContactFullPhone { get; set; }
        public object ContactPhoneExt { get; set; }
        public string ContactEmailAddress { get; set; }
        public string ContactRegistrationType { get; set; }
        public string ContactRegistrationID { get; set; }
        public string ContactFullAddress { get; set; }
        public ContactAddressDetail ContactAddressDetail { get; set; }
    }

    public class Entitydata
    {
        public Keys keys { get; set; }
        public Columns columns { get; set; }
    }

    public class ExtendedData
    {
        public Extendeddata2 extendeddata { get; set; }
    }

    public class Extendeddata2
    {
        [JsonProperty("@xmlns:xsi")]
        public string xmlnsxsi { get; set; }
        public List<Table> table { get; set; }
    }

    public class Key
    {
        public string name { get; set; }
        public string datatype { get; set; }
        public Value value { get; set; }
    }

    public class Keys
    {
        public Key key { get; set; }
    }

    public class LineDTO
    {
        public string LineType { get; set; }
        public string LineID { get; set; }
        public string HandlingStatus { get; set; }
        public string AddressID { get; set; }
        public string InvolvementRoleID { get; set; }
        public string ClaimID { get; set; }
        public string ClaimantName { get; set; }
        public string CauseOfLoss { get; set; }
        public string LineTypeDescription { get; set; }
        public string LineComplexity { get; set; }
        public string LineRegulatoryState { get; set; }
        public object LineCloseReason { get; set; }
        public object ReserveCategory { get; set; }
        public string LineRegulatoryCountry { get; set; }
        public object LineRegulatoryCounty { get; set; }
        public string LineUOM { get; set; }
        public string LineFinancialStatus { get; set; }
        public object LineFinancialCloseReason { get; set; }
        public object LineFinancialsCloseDate { get; set; }
        public object LineFinancialsReopenDate { get; set; }
        public string CoverageMatchStatus { get; set; }
        public bool RecoveryIndicator { get; set; }
        public object LegacySalvageStatus { get; set; }
        public object LegacySubrogationStatus { get; set; }
        public object AdditionalFinalPayment { get; set; }
        public object LineReserveMethod { get; set; }
        public object SettlementMethod { get; set; }
        public bool ExpenseOnlyIndicator { get; set; }
        public object SIUIndicator { get; set; }
        public object NatureOfOperation { get; set; }
        public object PrimaryExcessLine { get; set; }
        public object LineLocationOfLoss { get; set; }
        public object PropertyType { get; set; }
        public object LossCharacteristic { get; set; }
        public bool ThirdPartyIndicator { get; set; }
        public bool ValidProductCatalogLineIndicator { get; set; }
        public bool AutoReserveIndicator { get; set; }
        public bool AutomatedLineGenIndicator { get; set; }
        public object ContributingFactor { get; set; }
        public object DamageDescription { get; set; }
        public object VehicleLocationDetail { get; set; }
        public bool MedicareIndicator { get; set; }
        public bool LineTypeOverrideIndicator { get; set; }
        public object LineTypeOverridingOrgEntity { get; set; }
        public string LastTransactionID { get; set; }
        public object LiableItemID { get; set; }
    }

    public class ParticipantDTO
    {
        public List<ParticipantRolesDTO> ParticipantRolesDTO { get; set; }
        public object CIBRequestDate { get; set; }
        public string ClaimID { get; set; }
        public string ParticipantID { get; set; }
        public object AnalyticScore { get; set; }
        public object ContactNote { get; set; }
        public object DateOfBirth { get; set; }
        public object DependentsUnder18 { get; set; }
        public object DriverCountry { get; set; }
        public object DriverState { get; set; }
        public object DriversLicenseNumber { get; set; }
        public object EFTActive { get; set; }
        public object Gender { get; set; }
        public string ParticipantFullName { get; set; }
        public string PrefAddressID { get; set; }
        public object PrefEmailID { get; set; }
        public string PrefNameID { get; set; }
        public object PrefPhoneID { get; set; }
        public object PreferredContactType { get; set; }
        public object ReportedAge { get; set; }
        public string TaxID { get; set; }
        public string PartyID { get; set; }
        public bool PolicyAdded { get; set; }
        public object PartialTaxID { get; set; }
        public string LastTransactionID { get; set; }
        public object InsuranceExist { get; set; }
        public object ThirdPartyPolicyNumber { get; set; }
        public object CoverageConfirmed { get; set; }
        public ParticipantNameDetailDTO ParticipantNameDetailDTO { get; set; }
    }

    public class ParticipantNameDetailDTO
    {
        public object PrefixCode { get; set; }
        public object SuffixCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CountryCode { get; set; }
    }

    public class ParticipantRolesDTO
    {
        public string ParticipantRole { get; set; }
        public string ClaimID { get; set; }
        public string ParticipantID { get; set; }
        public string ParticipantRoleID { get; set; }
        public object ActivationDate { get; set; }
        public object DeactivationDate { get; set; }
    }

    public class PaymentDetailDTO
    {
        public string PaymentDetailID { get; set; }
        public string LineID { get; set; }
        public object InvoiceDetailID { get; set; }
        public string PaymentID { get; set; }
        public object ItemArticleID { get; set; }
        public object PaymentFrequency { get; set; }
        public string FinancialCategory { get; set; }
        public string Service { get; set; }
        public object ServiceStartDate { get; set; }
        public object ServiceEndDate { get; set; }
        public ApprovedAmount ApprovedAmount { get; set; }
        public object ApprovedAmountWithoutVAT { get; set; }
        public object VATPercentage { get; set; }
        public object VATNumeric { get; set; }
        public object MedicalEvalutator { get; set; }
        public object DeductionReason { get; set; }
        public string Reportable1099 { get; set; }
        public bool ErodeLimit { get; set; }
        public string LastTransactionID { get; set; }
        public object VATMode { get; set; }
        public object VATType { get; set; }
        public bool VATApplicableComponent { get; set; }
        public bool InactiveIndicator { get; set; }
        public object NumberOfVisits { get; set; }
        public string ParticipantRoleID { get; set; }
        public object LastUpdate { get; set; }
    }

    public class PaymentHeaderDTO
    {
        public string PaymentID { get; set; }
        public object RecurringPaymentID { get; set; }
        public DateTime IssueDate { get; set; }
        public bool FinalPaymentIndicator { get; set; }
        public bool AuthorityTaskExist { get; set; }
        public string PaymentMethod { get; set; }
        public object StopVoid { get; set; }
        public object StopVoidReason { get; set; }
        public string PaymentType { get; set; }
        public bool AttachmentIndicator { get; set; }
        public string PrimaryPayeeInsuranceInvolvementID { get; set; }
        public string MailToInsuranceInvolvementID { get; set; }
        public string PrimaryPayeeClientID { get; set; }
        public string MailTOClientID { get; set; }
        public object ExplainationOfBenefitComments { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankTransitNumber { get; set; }
        public object HoldReason { get; set; }
        public object HandlingNotes { get; set; }
        public object ElectronFundsTransferID { get; set; }
        public object PayeeName { get; set; }
        public string MailToName { get; set; }
        public string MailToAddressID { get; set; }
        public object ReconciliationStatus { get; set; }
        public string PaymentNumber { get; set; }
        public string PaymentStatus { get; set; }
        public string CheckStatus { get; set; }
        public object PrinterLocation { get; set; }
        public object CheckMemo { get; set; }
        public object PaymentGroupNumber { get; set; }
        public object InvoiceID { get; set; }
        public string PaymentCurrency { get; set; }
        public double TotalApprovedAmount { get; set; }
        public DateTime CoversionDate { get; set; }
        public string Language { get; set; }
        public string OfficeOrgEntityID { get; set; }
        public string UserOrgEntityID { get; set; }
        public object CustomCheckPartyId { get; set; }
        public string LastTransactionID { get; set; }
        public object ReportablePartyID { get; set; }
        public DateTime LastUpdate { get; set; }
    }

    public class PaymentPayeeDTO
    {
        public string PaymentPayeeID { get; set; }
        public string PaymentID { get; set; }
        public string InsuranceInvolvementID { get; set; }
        public string ClientID { get; set; }
        public object DateSentToOFAC { get; set; }
        public object DateResponseRecievedFromOFAC { get; set; }
        public bool MatchReportReceivedIndicator { get; set; }
        public int BatchNumber { get; set; }
    }

    public class PerformerDTO
    {
        public string ClaimID { get; set; }
        public string PerformerID { get; set; }
        public object LineID { get; set; }
        public object ClaimantRoleID { get; set; }
        public DateTime PerformerActivationDate { get; set; }
        public object PerformerDeactivationDate { get; set; }
        public string PerformerRole { get; set; }
        public string OrganizationEntityID { get; set; }
        public string LastTransactionID { get; set; }
        public object OrganizationType { get; set; }
        public object OrganizationCorporateName { get; set; }
        public object OrganizationFirstName { get; set; }
        public object OrganizationMiddleName { get; set; }
        public object OrganizationLastName { get; set; }
        public DateTime OrganizationActivationDate { get; set; }
        public object OrganizationDeactivationDate { get; set; }
        public object OrganizationEmail { get; set; }
        public string PerformerFullName { get; set; }
        public PerformerNameDetailDTO PerformerNameDetailDTO { get; set; }
    }

    public class PerformerNameDetailDTO
    {
        public object PrefixCode { get; set; }
        public object SuffixCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CountryCode { get; set; }
    }

    public class DTOModel
    {
        public ClaimDTO ClaimDTO { get; set; }
        public ParticipantDTO ParticipantDTO { get; set; }
        public List<LineDTO> LineDTOs { get; set; }
        public ContactInfoDTO ContactInfoDTO { get; set; }
        public PerformerDTO PerformerDTO { get; set; }
        public PaymentHeaderDTO PaymentHeaderDTO { get; set; }
        public List<PaymentDetailDTO> PaymentDetailDTOs { get; set; }
        public AddressDTOMailTo AddressDTO_MailTo { get; set; }
        public List<PaymentPayeeDTO> PaymentPayeeDTOs { get; set; }
        public ExtendedData __extendedData { get; set; }
    }

    public class Table
    {
        [Required(ErrorMessage = "Title is required")]
        public string name { get; set; }
        public Entitydata entitydata { get; set; }
    }

    public class Value
    {
        [JsonProperty("@xsi:nil")]
        public string xsinil { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }


}
