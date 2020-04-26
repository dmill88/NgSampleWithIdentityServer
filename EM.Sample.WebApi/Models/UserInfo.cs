using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EM.Sample.WebApi.Models
{
    public class UserInfo
    {
        public bool IsAuthenticated { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public List<SimpleClaim> Claims { get; set; } = new List<SimpleClaim>();
    }

}
