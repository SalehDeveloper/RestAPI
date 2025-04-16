using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Models
{
    public class Rating
    {
        public int Rate {  get; set; }
        public Guid UserId { get; set; }
        

        public Guid MovieId { get; set; }

        //navigation properties 

        public Movie Movie { get; set; }


    }
}
