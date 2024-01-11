using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWT_AuthApplication.Models
{
	public class User
	{
        public string UserName { get; set;  }
        public string Password { get; set; }
        public string? Role { get; set; }

		public User()
        {
            
        }
		public User(string u, string p)
		{
			this.UserName = u;
			this.Password = p;
		}
		
	}
}
