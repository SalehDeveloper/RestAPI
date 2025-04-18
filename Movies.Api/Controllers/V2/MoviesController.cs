using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers.V2
{
    [ApiVersion(2.0)]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

       
      
        [Authorize]
        [HttpGet(ApiEndpoints.Movies.GetAll)]
        [ProducesResponseType(typeof(MoviesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllMoviesRequest request,CancellationToken cancellationToken)
        {
            var userId = HttpContext.GetUserId();

            var options = request.MapToOptions().WithUser(userId);


            var movies = await _movieService.GetAllAsync(options, cancellationToken);
          
            var movieCount = await _movieService.GetCountAsync(request.Title, request.YearOfRelease, cancellationToken);

            var moviesResponse = movies.MapToResponse(request.Page , request.PageSize , movieCount);

            return Ok(moviesResponse);
        }



    }
}
