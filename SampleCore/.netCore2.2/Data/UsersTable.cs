using System;
using System.ComponentModel.DataAnnotations;

namespace SampleCore.Data
{
    public class UsersTable
    {
        [Key]
        public string Id { get; set; }
        public string City { get; set; }
        public string DisplayName { get; set; }
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string Username { get; set; }
        public string MobilePhone { get; set; }
        public string LastName { get; set; }
    }
}
