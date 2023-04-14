using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using testnew.Models;
using System.Net.Http.Headers;
using Microsoft.Extensions.Hosting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace testnew.Controllers
{
    [Route("api/[controller]")]
    public class SessionController : Controller
    {


        private readonly IConfiguration _configuration;

        public SessionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/values
        [HttpGet]
        public User Get()
        {
            return isAuthenticated(Request);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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

        private User isAuthenticated(HttpRequest request)
        {
            StringValues values;

            var userid = -1;

            string username = string.Empty;
            string pwd = string.Empty;

            if (request.Headers.TryGetValue("username", out values))
            {
                username = values.FirstOrDefault();

            }

            if (request.Headers.TryGetValue("password", out values))
            {
                pwd = values.FirstOrDefault();

            }
            var sql = "SELECT * from Users where UserName='" + username + "' and Password='" + pwd + "'";
            User user = new User();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("UserDatabase")))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(sql, connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    user.ID = (int)reader["ID"];
                    user.FirstName = reader["FirstName"].ToString();
                    user.LastName = reader["LastName"].ToString();
                    user.UserName = reader["UserName"].ToString();
                    user.Password = reader["Password"].ToString();
                    user.Gender = reader["Gender"].ToString();
                }
            }


            return user;
        }
    }
}

