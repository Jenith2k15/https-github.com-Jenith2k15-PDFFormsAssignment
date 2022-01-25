using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FormsAssignment.Models
{
    public class User
    {
        public string Fullname { get; set; }
        public IFormFile Proof { get; set; }
    }
}
