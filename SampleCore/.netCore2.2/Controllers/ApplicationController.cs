using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SampleCore.Data;
using SampleCore.Filters;
using SampleCore.Models;

namespace SampleCore.Controllers
{
    public class ApplicationController : Controller
    {
        public Context db = new Context();
        [HttpPost, HttpPut, HttpDelete]
        [ServiceFilter(typeof(WebhookFilters))]
        public void Reciever(WebHookDataRequest model, WebHookDeleteRequest delete)
        {
            if (delete.Id == null)
            {
                ActionModel action = JsonConvert.DeserializeObject<ActionModel>(model.Action);
                switch (action.Target.ToLower())
                {
                    //case "role":
                    //    ApplicationRoleModel role = JsonConvert.DeserializeObject<ApplicationRoleModel>(model.Data);
                    //    UserGroupsTable dataRole = db.UserGroups.Find(role.Id);
                    //    switch (action.Method.ToLower())
                    //    {
                    //        case "put":
                    //            if (dataRole != null)
                    //            {
                    //                dataRole.Name = role.Name;
                    //                db.SaveChanges();
                    //            }
                    //            break;
                    //        case "post":
                    //            UserGroupsTable add = new UserGroupsTable()
                    //            {
                    //                UserGroupId = role.Id,
                    //                Name = role.Name
                    //            };
                    //            if (dataRole == null)
                    //            {
                    //                db.UserGroups.Add(add);
                    //                db.SaveChanges();
                    //            }
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //    break;
                    case "user":
                        UserModel user = JsonConvert.DeserializeObject<UserModel>(model.Data);
                        UsersTable dataUser = db.Users.Find(user.Id);
                        UsersTable newUser = new UsersTable()
                        {
                            Id = user.Id,
                            EmployeeId = user.EmployeeId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            DisplayName = user.DisplayName,
                            City = user.City,
                            MobilePhone = user.MobilePhone,
                            Username = user.Username
                        };
                        switch (action.Method.ToLower())
                        {
                            case "put":
                                if (dataUser != null)
                                {
                                    dataUser.EmployeeId = user.EmployeeId;
                                    dataUser.FirstName = user.FirstName;
                                    dataUser.LastName = user.LastName;
                                    dataUser.DisplayName = user.DisplayName;
                                    dataUser.City = user.City;
                                    dataUser.MobilePhone = user.MobilePhone;
                                    dataUser.Username = user.Username;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    db.Users.Add(newUser);
                                    db.SaveChanges();
                                }
                                break;
                            case "post":

                                if (dataUser == null)
                                {
                                    db.Users.Add(newUser);
                                    db.SaveChanges();
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    //case "organization":
                    //    OrganizationModel organization = JsonConvert.DeserializeObject<OrganizationModel>(model.Data);
                    //    OrganizationsTable dataOrganization = db.Organization.Find(organization.Id);
                    //    OrganizationsTable newOrganization = new OrganizationsTable()
                    //    {
                    //        OrganizationId = organization.Id,
                    //        Name = organization.Name,
                    //        Parent = organization.Parent,
                    //        //Title = workUnit.Title,
                    //        //OrganizationCode = workUnit.OrganizationCode,
                    //        IsPublish = organization.IsPublished
                    //    };
                    //    switch (action.Method.ToLower())
                    //    {
                    //        case "put":
                    //            if (dataOrganization != null)
                    //            {
                    //                dataOrganization.Name = organization.Name;
                    //                dataOrganization.Parent = organization.Parent;
                    //                //dataOrganization.Title = workUnit.Title;
                    //                //dataOrganization.OrganizationCode = workUnit.OrganizationCode;
                    //                dataOrganization.IsPublish = organization.IsPublished;
                    //                db.SaveChanges();
                    //            }
                    //            else
                    //            {
                    //                db.Organization.Add(newOrganization);
                    //                db.SaveChanges();
                    //            }
                    //            break;
                    //        case "post":

                    //            if (dataOrganization == null)
                    //            {
                    //                db.Organization.Add(newOrganization);
                    //                db.SaveChanges();
                    //            }
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //    break;
                    //case "position":
                    //    PositionModel position = JsonConvert.DeserializeObject<PositionModel>(model.Data);
                    //    PositionsTable dataPosition = db.Positions.Find(position.Id);
                    //    PositionsTable newPosition = new PositionsTable()
                    //    {
                    //        PositionId = position.Id,
                    //        Name = position.Name,
                    //        OrganizationId = position.OrganizationId,
                    //        //OwnerStatus = position.OwnerStatus,
                    //        //IsDeleted = position.IsDeleted,
                    //        IsPublish = position.IsPublished,
                    //    };
                    //    switch (action.Method.ToLower())
                    //    {
                    //        case "put":
                    //            if (dataPosition != null)
                    //            {
                    //                dataPosition.Name = position.Name;
                    //                dataPosition.OrganizationId = position.OrganizationId;
                    //                //dataPosition.OwnerStatus = position.OwnerStatus;
                    //                //dataPosition.IsDeleted = position.IsDeleted;
                    //                dataPosition.IsPublish = position.IsPublished;
                    //                db.SaveChanges();
                    //            }
                    //            else
                    //            {
                    //                db.Positions.Add(newPosition);
                    //                db.SaveChanges();
                    //            }
                    //            break;
                    //        case "post":

                    //            if (dataPosition == null)
                    //            {
                    //                db.Positions.Add(newPosition);
                    //                db.SaveChanges();
                    //            }
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //    break;
                    default:
                        break;
                }
            }
            else
            {
                switch (delete.Target.ToLower())
                {
                    case "user":
                        UsersTable user = db.Users.Find(delete.Id);
                        if (user != null)
                        {
                            db.Users.Remove(user);
                            db.SaveChanges();
                        }
                        break;
                    //case "organization":
                    //    OrganizationsTable organization = db.Organization.Find(delete.Id);
                    //    if (organization != null)
                    //    {
                    //        organization.IsDeleted = true;
                    //        organization.IsPublish = false;
                    //        db.SaveChanges();
                    //    }
                    //    break;
                    //case "position":
                    //    PositionsTable positions = db.Positions.Find(delete.Id);
                    //    if (positions != null)
                    //    {
                    //        positions.IsDeleted = true;
                    //        positions.IsPublish = false;
                    //        db.SaveChanges();
                    //    }
                    //    break;
                    //case "role":
                    //    UserGroupsTable role = db.UserGroups.Find(delete.Id);
                    //    if (role != null)
                    //    {
                    //        db.UserGroups.Remove(role);
                    //        db.SaveChanges();
                    //    }
                    //    break;
                    default:
                        break;
                }
            }

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
