﻿using FluentValidation;
using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Validators
{

    public class GetAllMoviesOptionsValidator : AbstractValidator<GetAllMoviesOptions>
    {
        private static readonly string[] AcceptableSortFields =
         {
              "title","yearofrelease"
         };
        public GetAllMoviesOptionsValidator()
        {
            RuleFor(x => x.YearOfRelease).LessThanOrEqualTo(DateTime.UtcNow.Year);
            RuleFor(x => x.SortField)
                    .Must(x => x is null || AcceptableSortFields.Contains(x, StringComparer.OrdinalIgnoreCase))
                    .WithMessage("you can only sort by 'title' or 'yearofrelease'");


            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize).InclusiveBetween(1, 10).WithMessage("you can get between 1 and 10 movie per page");

        }
    }
}
