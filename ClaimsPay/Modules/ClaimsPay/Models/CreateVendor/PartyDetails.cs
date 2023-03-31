namespace ClaimsPay.Modules.ClaimsPay.Models.CreateVendor
{
   
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Address
    {
        public string? addressID { get; set; }
        public string? addressType { get; set; }
        public List<AddressTypeAssociation> addressTypeAssociations { get; set; }
        public string? adminDivisionPrimary { get; set; }
        public object? adminDivisionSecondary { get; set; }
        public object? adminDivisionTertiary { get; set; }
        public bool? allowPartial { get; set; }
        public object? attention { get; set; }
        public string? countryCode { get; set; }
        public DateTime? effectiveDate { get; set; }
        public DateTime? expirationDate { get; set; }
        public string? fullAddress { get; set; }
        public string? geographicFormatCode { get; set; }
        public string? lastTransactionID { get; set; }
        public DateTime? lastUpdatedDate { get; set; }
        public double? latitude { get; set; }
        public string? locationDetailsLine1 { get; set; }
        public string? locationDetailsLine2 { get; set; }
        public object? locationDetailsLine3 { get; set; }
        public object? locationDetailsLine4 { get; set; }
        public double? longitude { get; set; }
        public string nationalDivisionPrimary { get; set; }
        public string nationalDivisionSecondary { get; set; }
        public object oldAddressCountryCode { get; set; }
        public string?  postalCode { get; set; }
        public object? subAddressType { get; set; }
        public object? subAdminDivisionPrimary { get; set; }
        public object? subAdminDivisionSecondary { get; set; }
        public object? subAdminDivisionTertiary { get; set; }
        public object? subAttention { get; set; }
        public object? subCountryCode { get; set; }
        public DateTime? subEffectiveDate { get; set; }
        public DateTime? subExpirationDate { get; set; }
        public object? subLastTransactionID { get; set; }
        public DateTime? subLastUpdatedDate { get; set; }
        public object? subLatitude { get; set; }
        public object? subLocationDetailsLine1 { get; set; }
        public object? subLocationDetailsLine2 { get; set; }
        public object? subLocationDetailsLine3 { get; set; }
        public object? subLocationDetailsLine4 { get; set; }
        public object? subLongitude { get; set; }
        public object? subNationalDivisionPrimary { get; set; }
        public object? subNationalDivisionSecondary { get; set; }
        public object? subPostalCode { get; set; }
    }

    public class AddressTypeAssociation
    {
        public string? addressID { get; set; }
        public string? addressType { get; set; }
        public string? addressTypeAssociationID { get; set; }
        public DateTime? effectiveDate { get; set; }
        public DateTime? expirationDate { get; set; }
        public string?  lastTransactionID { get; set; }
        public DateTime? lastUpdatedDate { get; set; }
    }

    public class Party
    {
        public object? achPartyExternalID { get; set; }
        public string? businessPermitted { get; set; }
        public string? countryCode { get; set; }
        public object? externalID { get; set; }
        public string? geographicFormatCode { get; set; }
        public bool? isInActive { get; set; }
        public string lastTransactionID { get; set; }
        public DateTime? lastUpdatedDate { get; set; }
        public string? number { get; set; }
        public string? partyID { get; set; }
        public string? partyTypeCode { get; set; }
        public bool? skipLoadingExtendedData { get; set; }
        public object? timeZone { get; set; }
    }

    public class PartyAddress
    {
        public string? addressID { get; set; }
        public object? comment { get; set; }
        public DateTime? effectiveDate { get; set; }
        public DateTime? expirationDate { get; set; }
        public object? externalPartyAddressID { get; set; }
        public bool? isPrimary { get; set; }
        public bool? isTaxAddress { get; set; }
        public string? lastTransactionID { get; set; }
        public DateTime? lastUpdatedDate { get; set; }
        public string? partyAddressID { get; set; }
        public string? partyID { get; set; }
        public object? subComment { get; set; }
        public bool? isDefault { get; set; }
    }

    public class PartyAddressDetail
    {
        public Address address { get; set; }
        public PartyAddress partyAddress { get; set; }
    }

    public class PartyAgency
    {
        public object? agencySpecialtyType { get; set; }
        public object? agencySubCode { get; set; }
        public DateTime? effectiveDate { get; set; }
        public DateTime? expirationDate { get; set; }
        public object? lastTransactionID { get; set; }
        public DateTime? lastUpdatedDate { get; set; }
        public object? partyID { get; set; }
        public object? producerNumber { get; set; }
    }

    public class PartyBusiness
    {
        public object? businessTypeCode { get; set; }
        public DateTime? effectiveDate { get; set; }
        public DateTime? expirationDate { get; set; }
        public bool? isAgency { get; set; }
        public bool? isSupplier { get; set; }
        public string? lastTransactionID { get; set; }
        public DateTime? lastUpdatedDate { get; set; }
        public string? partyID { get; set; }
        public object? preferredContactType { get; set; }
        public object? preferredLanguage { get; set; }
        public string? registrationID1 { get; set; }
        public object? registrationID2 { get; set; }
        public object? registrationID3 { get; set; }
    }

    public class PartyBusinessDetail
    {
        public Party party { get; set; }
        public List<PartyAddressDetail> partyAddressDetail { get; set; }
        public PartyAgency partyAgency { get; set; }
        public PartyBusiness partyBusiness { get; set; }
        public List<PartyBusNameDetail>  partyBusNameDetail { get; set; }
        public PartyComment partyComment { get; set; }
        public object? partyEmail { get; set; }
        public object? partyLicense { get; set; }
        public object? partyMembership { get; set; }
        public List<PartyPhone> partyPhone { get; set; }
        public object? partyRelation { get; set; }
    }

    public class PartyBusName
    {
        public object? comment { get; set; }
        public string? lastTransactionID { get; set; }
        public DateTime? lastUpdatedDate { get; set; }
        public string? name { get; set; }
        public object? namePhonetic { get; set; }
        public string? nameSoundex { get; set; }
        public string? nameTypeCode { get; set; }
        public string? nameUpperCase { get; set; }
        public string? partyBusNameID { get; set; }
    }

    public class PartyBusNameDetail
    {
        public PartyBusName partyBusName { get; set; }
        public PartyName partyName { get; set; }
    }

    public class PartyComment
    {
        public object? comment { get; set; }
        public DateTime? effectiveDate { get; set; }
        public DateTime? expirationDate { get; set; }
        public string? lastTransactionID { get; set; }
        public DateTime? lastUpdatedDate { get; set; }
        public string? partyID { get; set; }
    }

    public class PartyName
    {
        public bool? allowPartial { get; set; }
        public string? countryCode { get; set; }
        public DateTime? effectiveDate { get; set; }
        public DateTime? expirationDate { get; set; }
        public object? externalPartyNameID { get; set; }
        public string? geographicFormatCode { get; set; }
        public bool? isPrimary { get; set; }
        public bool? isTaxName { get; set; }
        public string? lastTransactionID { get; set; }
        public DateTime? lastUpdatedDate { get; set; }
        public string? partyID { get; set; }
        public string? partyNameID { get; set; }
        public bool? isDefault { get; set; }
    }

    public class PartyPhone
    {
        public object? comment { get; set; }
        public string? countryCode { get; set; }
        public DateTime? effectiveDate { get; set; }
        public DateTime? expirationDate { get; set; }
        public object? externalPartyPhoneID { get; set; }
        public string? fullPhoneNumber { get; set; }
        public string? geographicFormatCode { get; set; }
        public bool? isPrimary { get; set; }
        public string? lastTransactionID { get; set; }
        public DateTime? lastUpdatedDate { get; set; }
        public string? partyID { get; set; }
        public string? partyPhoneID { get; set; }
        public object? phoneExtension { get; set; }
        public string? phoneTypeCode { get; set; }
    }

    public class PartyIndividualDetail
    {
        public Party party { get; set; }
    }
    public class PartyDetails
    {
        public bool? isPerformer { get; set; }
        public PartyBusinessDetail partyBusinessDetail { get; set; }
        public string? partyID { get; set; }
        public PartyIndividualDetail partyIndividualDetail { get; set; }
        public object? partyInternalDetail { get; set; }
        public string? partyType { get; set; }
        public bool? canUserViewParty { get; set; }
        public bool? canUserListParty { get; set; }
    }


}
