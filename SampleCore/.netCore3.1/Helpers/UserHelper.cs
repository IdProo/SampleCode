using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Helpers
{
    public static class UserHelper
    {
        public static async Task<List<ManageRolesModel>> Roles(string userId, string token)
        {
            var url = "https://rest.domain.com/v1/Roles/" + userId;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                client.Dispose();
                string json = await response.Content.ReadAsStringAsync();
                List<ManageRolesModel> value = JsonConvert.DeserializeObject<List<ManageRolesModel>>(json);
                return value;
            }
            client.Dispose();
            return null;
        }

        public static async Task<UserModel> User(string userId, string token)
        {
            var url = "https://rest.domain.com/v1/Users/" + userId;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                client.Dispose();
                string json = await response.Content.ReadAsStringAsync();
                UserModel value = JsonConvert.DeserializeObject<UserModel>(json);
                return value;
            }
            client.Dispose();
            return null;
        }
    }
}
