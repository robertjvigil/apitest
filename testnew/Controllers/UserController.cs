using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Data.SqlClient;
using testnew.Models;
using System.Net.Http.Headers;
using System.Text;
using Azure.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace testnew.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
           _configuration = configuration;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<User> Get()
        {
            var users = GetUsers();
            return users;

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            var user = GetUser(id);
            return user;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private int isAuthenticated()
        {
            StringValues values;

            var userid = -1;

            string username = string.Empty;
            string pwd = string.Empty;

            if (Request.Headers.TryGetValue("username", out values))
            {
                username = values.FirstOrDefault();

            }

            if (Request.Headers.TryGetValue("password", out values))
            {
                pwd = values.FirstOrDefault();

            }

            return userid;
        }
        private IEnumerable<User> GetUsers()
         {

            var users = new List<User>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("UserDatabase")))
            {
                var sql = "SELECT * from Users";

                connection.Open();
                using SqlCommand command = new SqlCommand(sql, connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var user = new User()
                    {
                        ID = (int) reader["ID"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Gender = reader["Gender"].ToString(),
                    };
                     users.Add(user);
                }
            }
            return users;
        }
        private User GetUser(int id)
        {
            var user = new User();
            var aid = isAuthenticated();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("UserDatabase")))
            {
                var sql = "SELECT * from Users WHERE ID=" + id;
                
                connection.Open();
                using SqlCommand command = new SqlCommand(sql, connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    user.ID = (int)reader["ID"];
                    user.FirstName = reader["FirstName"].ToString();
                    user.LastName = reader["LastName"].ToString();
                    user.Gender = reader["Gender"].ToString();
                    
                }
            }
            return user;
        }
    }
}

