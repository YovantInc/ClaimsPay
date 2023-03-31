using ClaimsPay.Modules.ClaimsPay.Models.Comman_Model;
using FluentValidation;

namespace ClaimsPay.Filters
{
    public class ModelValidator : AbstractValidator<RestData>
    {

        public ModelValidator()
        {

            RuleFor(x => x.str_json.BUS_BusinessId).NotEmpty().WithMessage("BUS_BusinessId Should not be blank");
            RuleFor(x => x.str_json.BUS_Type).NotEmpty().WithMessage("BUS_Type Should not be blank");
            //RuleFor(x => x.BUS_SubType).WithMessage("BUS_Type Should not be blank");
            RuleFor(x => x.str_json.BUS_Name).NotEmpty().WithMessage("BUS_Name Should not be blank");
            RuleFor(x => x.str_json.BUS_TIN).NotEmpty().WithMessage("BUS_TIN Should not be blank");
            RuleFor(x => x.str_json.BUS_Street).NotEmpty().WithMessage("BUS_Street Should not be blank");
            RuleFor(x => x.str_json.BUS_City).NotEmpty().WithMessage("BUS_City Should not be blank");
            RuleFor(x => x.str_json.BUS_State).NotEmpty().WithMessage("BUS_State Should not be blank");
            RuleFor(x => x.str_json.BUS_Zipcode).NotEmpty().WithMessage("BUS_Zipcode Should not be blank");
            RuleFor(x => x.str_json.BUS_Country).NotEmpty().WithMessage("BUS_Country Should not be blank");
            RuleFor(x => x.str_json.BUS_Status).NotEmpty().WithMessage("BUS_Status Should not be blank");



            //RuleFor(x => x.BUS_BusinessId);
            //RuleFor(x => x.BUS_BusinessId).WithMessage("BUS_BusinessId should not be blank");
            //RuleFor(x => x.BUS_City);
            //RuleFor(x => x.BUS_Country);
            //RuleFor(x => x.BUS_EmailAddress);
            //RuleFor(x => x.BUS_Fax);
            //RuleFor(x => x.BUS_Name);
            //RuleFor(x => x.BUS_OfficePhone);
            //RuleFor(x => x.BUS_State);
            //RuleFor(x => x.BUS_Street);
            //RuleFor(x => x.BUS_SubType);
            //RuleFor(x => x.BUS_TIN);
            //RuleFor(x => x.BUS_TINType);
            //RuleFor(x => x.BUS_Type);
            //RuleFor(x => x.BUS_Zipcode);
            //RuleFor(x => x.CL_CauseofLoss);
            //RuleFor(x => x.CL_ClaimNumber);
            //RuleFor(x => x.CL_DateofLoss);
            //RuleFor(x => x.CL_PolicyNumber);
            //RuleFor(x => x.PA_City);
            //RuleFor(x => x.PA_Country);
            //RuleFor(x => x.PA_State);
            //RuleFor(x => x.PA_Street);
            //RuleFor(x => x.PA_Zipcode);
            //RuleFor(x => x.PCON_Approval_Reqd);
            //RuleFor(x => x.PCON_Business);
            //RuleFor(x => x.PCON_ContactId);
            //RuleFor(x => x.PCON_EmailAddress);
            //RuleFor(x => x.PCON_FirstName);
            //RuleFor(x => x.PCON_LastName);
            //RuleFor(x => x.PCON_MobilePhone);
            //RuleFor(x => x.PCON_OfficePhone);
            //RuleFor(x => x.PM_Additional_Text_1);
            //RuleFor(x => x.PM_Additional_Text_10);
            //RuleFor(x => x.PM_Additional_Text_2);
            //RuleFor(x => x.PM_Additional_Text_3);
            //RuleFor(x => x.PM_Additional_Text_4);
            //RuleFor(x => x.PM_Additional_Text_5);
            //RuleFor(x => x.PM_Additional_Text_6);
            //RuleFor(x => x.PM_Additional_Text_9);
            //RuleFor(x => x.PM_Amount);
            //RuleFor(x => x.PM_CarrierId);
            //RuleFor(x => x.PM_LoanAccountNumber);
            //RuleFor(x => x.PM_PaymentID);
            //RuleFor(x => x.PM_PaymentType);
            //RuleFor(x => x.PM_RequestReason);
            //RuleFor(x => x.PM_User_EmailAddress);
            //RuleFor(x => x.PM_User_FirstName);
            //RuleFor(x => x.PM_User_LastName);
            //RuleFor(x => x.PM_UserId);
            //RuleFor(x => x.PMA_City);
            //RuleFor(x => x.PMA_Country);
            //RuleFor(x => x.PMA_MailTo);
            //RuleFor(x => x.PMA_State);
            //RuleFor(x => x.PMA_Street);
            //RuleFor(x => x.PMA_Zipcode);
            //RuleFor(x => x.PMETHOD);
            //RuleFor(x => x.PMETHOD_Stored);
            //RuleFor(x => x.SCON_Approval_Reqd);
            //RuleFor(x => x.SCON_ContactId);
            //RuleFor(x => x.SCON_EmailAddress);
            //RuleFor(x => x.SCON_FirstName);
            //RuleFor(x => x.SCON_LastName);
            //RuleFor(x => x.SCON_MobilePhone);
            //RuleFor(x => x.SCON_OfficePhone);



        }

    }
}
