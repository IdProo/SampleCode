using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class ManageRolesModel
    {
        public string Type { get; set; }

        public List<UserRolesModel> Roles { get; set; }
    }

    public class UserRolesModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public PositionsModel Position { get; set; }

        public ApplicationsModel Application { get; set; }
    }

    public class ApplicationsModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Photo { get; set; }

        public string HomePageUrl { get; set; }

        public string Description { get; set; }
    }

    public class PositionsModel
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
