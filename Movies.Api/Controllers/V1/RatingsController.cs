﻿using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers.V1
{
   
    [ApiController]
    [ApiVersion(1.0)]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingsController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }


        [Authorize]
        [HttpPut(ApiEndpoints.Movies.Rate)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult > RateMovie([FromRoute] Guid id  , RateMovieRequest request, CancellationToken token =default)
        {
            var userId = HttpContext.GetUserId();

            var result = await _ratingService.RateMovieAsync(id, request.Rate, userId!.Value, token);
       
            return result?Ok():NotFound();
           
        }


        [Authorize]
        [HttpDelete(ApiEndpoints.Movies.DeleteRating)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> DeleteRating([FromRoute] Guid id , CancellationToken cancellationToken = default)
        {
            var userId = HttpContext.GetUserId();

            var result = await _ratingService.DeleteRatingAsync(id, userId!.Value, cancellationToken);

            return result?Ok():NotFound();
        }

        [Authorize]
        [HttpGet(ApiEndpoints.Ratings.GetUserRatings)]
        [ProducesResponseType(typeof(IEnumerable<MovieRatingReponse>),StatusCodes.Status200OK)]
       

        public async Task<IActionResult> GetUserRating( CancellationToken cancellationToken = default)
        {
            var userId = HttpContext.GetUserId();
            var ratings = await _ratingService.GetRatingForUserAsync(userId.Value , cancellationToken);
            var ratingReponse = ratings.MapToResponse();
            return Ok(ratingReponse);
        }
    }
}
