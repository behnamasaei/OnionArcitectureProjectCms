using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArcitectureProject.Entities
{
    public class BlogPost
    {
        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
