using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using testnew.Models;
using Microsoft.Extensions.Primitives;
using System.Data.SqlClient;
using System.Net.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace testnew.Controllers
{
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        private readonly IConfiguration _configuration;

        public PostController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Post> Get(string ui)
        {
            var posts = GetPosts(ui);
            return posts;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Post Get(int id)
        {
            return GetPost(id);
        }

        // POST api/values
        [HttpPost]
        public int Post([FromBody] Post post)
        {
            return InsertPost(post);

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

        private int InsertPost(Post post)
        {
            int affected = 0;
            using (var connection = new SqlConnection(_configuration.GetConnectionString("UserDatabase")))
            {
                var sql = "INSERT INTO Posts (Post, UserID, DatePosted) VALUES ('" + post.PostText + "', " + post.UserID + ", GETDATE())";
                connection.Open();
                using SqlCommand command = new SqlCommand(sql, connection);
                affected = command.ExecuteNonQuery();
                
            }
            return affected;
        }

        private IEnumerable<Post> GetPosts(string userid)
        {
            var whereclause = "";
            if (userid!=null && userid!="")
            {
               whereclause = " WHERE P.UserID = " + userid;
            }
            var posts = new List<Post>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("UserDatabase")))
            {
                var sql = "SELECT * from Posts p inner join Users u on P.UserID=u.ID " + whereclause + " order by DatePosted desc ";
                connection.Open();
                using SqlCommand command = new SqlCommand(sql, connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var post = new Post()
                    {
                        ID = (int)reader["ID"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        PostText = reader["Post"].ToString(),
                        DatePosted = reader["DatePosted"].ToString(),
                        UserID = (int)reader["UserID"]
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

        private Post GetPost(int id)
        {
            var post = new Post();
            //var aid = isAuthenticated();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("UserDatabase")))
            {
                var sql = "SELECT * from Posts p inner join Users u on p.UserID=u.ID WHERE p.ID=" + id;

                connection.Open();
                using SqlCommand command = new SqlCommand(sql, connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    post.ID = (int)reader["ID"];
                    post.FirstName = reader["FirstName"].ToString();
                    post.LastName = reader["LastName"].ToString();
                    post.PostText = reader["Post"].ToString();
                    post.UserID = (int)reader["UserID"];
                    post.DatePosted = reader["DatePosted"].ToString();

                }
            }
            return post;
        }
    }
}

