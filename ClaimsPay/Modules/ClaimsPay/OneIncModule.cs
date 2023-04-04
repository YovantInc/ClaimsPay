using ClaimsPay.Filters;
using ClaimsPay.Modules.ClaimsPay.ClaimsPayFilter;
using ClaimsPay.Shared;
using Newtonsoft.Json.Linq;
using ClaimsPay.Modules.ClaimsPay.DataHandler;
using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;
using ClaimsPay.Modules.ClaimsPay.Models.Comman_Model;
using FluentValidation;
using ClaimsPay.Modules.ClaimsPay.Filters;
using System.Text;
using System;

namespace ClaimsPay.Modules.OneInc
{
    public class OneIncModule : IModule
    {
        AppHttpClient objhttpclient = new AppHttpClient();
        ClaimsPayDataHandler objClaimsPayDataHandler = new ClaimsPayDataHandler();
        public IServiceCollection RegisterModule(IServiceCollection services)
        {
            return services;
        }
        
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {

            #region Create Payment Master
            endpoints.MapPost($"CreatePaymentMaster", async Task<JObject> (HttpRequest request, IValidator<RestData> validator, RestData objRestData) =>
            {
                var body = new StreamReader(request.Body);
                var requestJson = await body.ReadToEndAsync();

                //var validationResult = await validator.ValidateAsync(objRestData);
                //if (!validationResult.IsValid)
                //{
                var result = "";// Results.ValidationProblem(validationResult.ToDictionary());

                //}

                string str = await objClaimsPayDataHandler.GetSessionID();
                JObject objResponse = JObject.Parse(result.ToString());

                return await Task.FromResult(objResponse);
            }).AddEndpointFilter<ClaimsPayFilter>()
              .AddEndpointFilter<ClaimsPayIPFilter>();
            #endregion

            #region Create Vendor
            endpoints.MapPost($"CreateVendor", async Task<JObject> (HttpRequest request) =>
            {
                var body = new StreamReader(request.Body);
                var requestJson = await body.ReadToEndAsync();
                JObject objRequest = JObject.Parse(requestJson);
                var result = await objClaimsPayDataHandler.ClaimsPayDataHandlerCreateVendor(objRequest);

                string objResponse = string.Empty;
                
                JObject jobj = JObject.Parse(result.ToString());

                return await Task.FromResult(jobj);
            }).AddEndpointFilter<ClaimsPayFilter>()
            .AddEndpointFilter<ClaimsPayIPFilter>();
            #endregion

            #region Stop Payment
            endpoints.MapPost("stopPayment", async Task<JObject> (HttpRequest request) => {
                var body = new StreamReader(request.Body);
                var requestJson = await body.ReadToEndAsync();
                JObject objRequest = JObject.Parse(requestJson);


                var result = await objClaimsPayDataHandler.ClaimsPayDataHandlerStopPayment(objRequest);
                var objrespons = JObject.Parse(result.ToString());
                return objrespons;
                //return "{\r\n\t\"Status\": \"Success\",\r\n\t\"Message\": \"Endpoint Hit Successfully\"\r\n}";
            })
            .AddEndpointFilter<ClaimsPayFilter>()
            .AddEndpointFilter<ClaimsPayIPFilter>();

            #endregion

            #region Get Payment Status
            endpoints.MapPost("getPaymentStatus", async Task<string> (HttpRequest request) => {
                var body = new StreamReader(request.Body);
                var requestJson = await body.ReadToEndAsync();
                JObject objRequest = JObject.Parse(requestJson);
              

                var result = await objClaimsPayDataHandler.ClaimsPayDataHandlerGetPaymentStatus(objRequest);
                return result.ToString();
                //return "{\r\n\t\"Status\": \"Success\",\r\n\t\"Message\": \"Endpoint Hit Successfully\"\r\n}";

            })
            .AddEndpointFilter<ClaimsPayFilter>()
            .AddEndpointFilter<ClaimsPayIPFilter>();

            #endregion

            #region Update Profile
            endpoints.MapPost("updateProfile", async Task<JObject> (HttpRequest request) => {
                var body = new StreamReader(request.Body);
                var requestJson = await body.ReadToEndAsync();

                var objResponse=JObject.Parse("{\r\n\t\"Status\": \"Success\",\r\n\t\"Message\": \"Endpoint Hit Successfully\"\r\n}");
                return objResponse;
            })
            .AddEndpointFilter<ClaimsPayFilter>()
            .AddEndpointFilter<ClaimsPayIPFilter>();

            #endregion

            #region Resend Email
            endpoints.MapPost("resendEmail", async Task<JObject> (HttpRequest request) => {
                var body = new StreamReader(request.Body);
                var requestJson = await body.ReadToEndAsync();
                JObject objRequest = JObject.Parse(requestJson);
                var result = await objClaimsPayDataHandler.ClaimsPayDataHandlerResendEmail(objRequest);
                var objResponse = JObject.Parse("{\r\n\t\"Status\": \"Success\",\r\n\t\"Message\": \"Endpoint Hit Successfully\"\r\n}");
                return objResponse;
            })
            .AddEndpointFilter<ClaimsPayFilter>()
            .AddEndpointFilter<ClaimsPayIPFilter>();

            #endregion

            #region Webhook
            endpoints.MapPost("webhook", async Task<JObject> (HttpRequest request) => {
                var body = new StreamReader(request.Body);
                var requestJson = await body.ReadToEndAsync();

                var objResponse = JObject.Parse("{\r\n\t\"Status\": \"Success\",\r\n\t\"Message\": \"Endpoint Hit Successfully\"\r\n}");
                return objResponse;
            })
            .AddEndpointFilter<ClaimsPayFilter>()
            .AddEndpointFilter<ClaimsPayIPFilter>();

            #endregion

            #region Bulk Payment Master
            endpoints.MapPost("bulkPaymentMaster", async Task<string> (HttpRequest request) => {
                var body = new StreamReader(request.Body);
                var requestJson = await body.ReadToEndAsync();

                return "{\r\n\t\"Status\": \"Success\",\r\n\t\"Message\": \"Endpoint Hit Successfully\"\r\n}";
            })
            .AddEndpointFilter<ClaimsPayFilter>()
            .AddEndpointFilter<ClaimsPayIPFilter>();

            #endregion

            #region Sample Endpoint
            endpoints.MapPost($"SampleEndpoint", async Task<string> (HttpRequest request) =>
            {
                //var temp = await objClaimsPayDataHandler.GetSessionID();
                //var body = new StreamReader(request.Body);
                //var requestJson = await body.ReadToEndAsync();
                //var authHeader = request.Headers["Authorization"].ToString();
                //var token = authHeader.Substring("Basic ".Length).Trim();
                
                //System.Console.WriteLine(token);
                //var credentialstring = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                //var credentials = credentialstring.Split(':');
                //if (authHeader != null && authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
                //{
                //    if (credentials[0] == "ClaimsPayAdmin" && credentials[1] == "jshbkj36#9shg#hfh7")
                //    { 

                //    }
                //}


               

                return "{\r\n\t\"Status\": \"Success\",\r\n\t\"Message\": \"Endpoint Hit Successfully\"\r\n}";
            }).AddEndpointFilter<ClaimsPayFilter>();
            #endregion

            #region Default 
            endpoints.MapGet($"/", async Task<string> (HttpRequest request) =>
            {
                return "Claims Pay Middleware Started";
            });
            #endregion

            return endpoints;
        }

    }
}
