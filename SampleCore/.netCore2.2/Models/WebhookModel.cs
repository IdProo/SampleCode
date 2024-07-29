using Newtonsoft.Json;
using System;

namespace SampleCore.Models
{

    public class WebHookDataRequest
    {
        [JsonProperty("Action")]
        public string Action { get; set; }
        [JsonProperty("Webhook")]
        public string Webhook { get; set; }
        [JsonProperty("Data")]
        public string Data { get; set; }
    }

    public class WebHookDeleteRequest
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("Target")]
        public string Target { get; set; }
    }
    public class OrganizationModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        public string Title { get; set; }
        public string OrganizationCode { get; set; }
        public bool IsPublished { get; set; }
    }

    public class PositionModel
    {
        public string Id { get; set; }
        [JsonProperty("OrganizationId")]
        public string OrganizationId { get; set; }
        public string Name { get; set; }
        public int OwnerStatus { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPublished { get; set; }
    }

    public class ApplicationRoleModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ApplicationId { get; set; }
    }
    public class UserModel
    {
        public string Id { get; set; }
        [JsonProperty("OrganizationId")]
        public string OrganizationId { get; set; }
        public string PositionId { get; set; }
        public DateTimeOffset? Birthday { get; set; }
        public string City { get; set; }
        public string CompanyName { get; set; }
        public string Country { get; set; }
        public string Department { get; set; }
        public string DisplayName { get; set; }
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public DateTimeOffset? HireDate { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string MobilePhone { get; set; }
        public string MySite { get; set; }
        public string AboutMe { get; set; }
        public string OfficeLocation { get; set; }
        public string PostalCode { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string UserType { get; set; }
        public string UserPrincipalName { get; set; }
        public string Photo { get; set; }
        public string ExtensionAttributes { get; set; }
        public string Idp { get; set; }
    }


    public class ActionModel
    {
        [JsonProperty("Method")]
        public string Method { get; set; }
        [JsonProperty("Target")]
        public string Target { get; set; }
        [JsonProperty("Message")]
        public string Message { get; set; }
    }
    public class WebhookModel
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("AppId")]
        public string AppId { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Key")]
        public string Key { get; set; }
        [JsonProperty("TargetUrl")]
        public string TargetUrl { get; set; }
    }
}
