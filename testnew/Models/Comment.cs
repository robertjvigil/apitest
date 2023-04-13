using System;

namespace testnew.Models
{
	public class Comment
	{
        public Comment()
        {
            UserID = -1;
            PostID = -1;
            PostText = "";
            DatePosted = "";
            FirstName = "";
            LastName = "";
        }

        public int ID { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        public string PostText { get; set; }
        public string DatePosted { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}

