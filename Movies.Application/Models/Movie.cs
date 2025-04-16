using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Movies.Application.Models
{
    public  class Movie
    {
       public required Guid Id { get; init; }
        
       public required string Title { get; set; }

        public string Slug {get; private set; } = String.Empty;

        public required int YearOfRelease { get; set; }

        public  List<Genre> Genres { get; init; } = new();
        public List<Rating> Ratings { get; init; } = new();

        public void GenerateAndSetSlug()
        {
            var sluggedTitle = Regex.Replace(Title,"[^0-9A-Za-z_-]", string.Empty)
                                     .ToLower().Replace(" ", "-");

            Slug = $"{sluggedTitle}-{YearOfRelease}";
        }


    }
}
