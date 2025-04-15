using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Models
{
    public class Genre
    {
        public Guid Id { get; init; }

        public string Name { get; set; }   

        public Guid MovieId { get; init; }   
       
        public Movie Movie { get;  }
    }
}
