using System.Collections.Generic;
using System.Linq;
using System.Net;
using KesselRunFramework.AspNet.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KesselRunFramework.AspNet.Infrastructure.Controllers
{
    [ApiController]
    public class KesselRunApiController : ControllerBase
    {
        protected readonly ICurrentUser _currentUser;
        protected readonly ILogger _logger;
        protected readonly IMediator _mediator;

        public KesselRunApiController(ICurrentUser currentUser, ILogger logger, IMediator mediator)
        {
            _currentUser = currentUser;
            _logger = logger;
            _mediator = mediator;
        }

        public IActionResult OkResponse<T>(T data, string message = null)
        {
            var outcome = OperationOutcome.SuccessfulOutcome;
            outcome.Message = message ?? string.Empty;

            var apiResponse = new ApiResponse<T>
            {
                Data = data,
                Outcome = outcome
            };

            return Ok(apiResponse);
        }
        
        public IActionResult OkResponse<T>(T data, OperationOutcome operationOutcome)
        {
            var apiResponse = new ApiResponse<T>
            {
                Data = data,
                Outcome = operationOutcome
            };

            return Ok(apiResponse);
        }

        public IActionResult OkResponse<T>(ApiResponse<T> apiResponse)
        {
            return Ok(apiResponse);
        }

        public IActionResult BadRequestResponse<T>(T data, string errorMessage = null, IEnumerable<string> errors = null)
        {
            var outcome = OperationOutcome.ValidationFailOutcome(errors, errorMessage);

            var apiResponse = new ApiResponse<T>
            {
                Data = data,
                Outcome = outcome
            };

            return BadRequest(apiResponse);
        }

        public IActionResult BadRequestResponse<T>(T data, OperationOutcome operationOutcome)
        {
            var apiResponse = new ApiResponse<T>
            {
                Data = data,
                Outcome = operationOutcome 
            };

            return BadRequest(apiResponse);
        }

        public IActionResult BadRequestResponse<T>(ApiResponse<T> apiResponse)
        {
            return BadRequest(apiResponse);
        }

        public IActionResult UnprocessableEntityResponse<T>(T data, string errorMessage = null, IEnumerable<string> errors = null)
        {
            var outcome = OperationOutcome.ValidationFailOutcome(errors, errorMessage);

            var apiResponse = new ApiResponse<T>
            {
                Data = data,
                Outcome = outcome
            };

            return StatusCode((int)HttpStatusCode.UnprocessableEntity, apiResponse);
        }

        public IActionResult UnprocessableEntityResponse<T>(T data, OperationOutcome operationOutcome)
        {
            var apiResponse = new ApiResponse<T>
            {
                Data = data,
                Outcome = operationOutcome
            };

            return StatusCode((int)HttpStatusCode.UnprocessableEntity, apiResponse);
        }

        public IActionResult UnprocessableEntityResponse<T>(ApiResponse<T> apiResponse)
        {
            return StatusCode((int)HttpStatusCode.UnprocessableEntity, apiResponse);
        }

        public IActionResult InternalServerErrorResponse<T>(T data, string errorMessage = null, IEnumerable<string> errors = null)
        {
            var outcome = OperationOutcome.UnSuccessfulOutcome;
            outcome.Errors = errors ?? Enumerable.Empty<string>();
            outcome.Message = errorMessage ?? string.Empty;

            var apiResponse = new ApiResponse<T>
            {
                Data = data,
                Outcome = outcome
            };

            return StatusCode((int)HttpStatusCode.InternalServerError, apiResponse);
        }

        public IActionResult InternalServerErrorResponse<T>(T data, OperationOutcome operationOutcome)
        {
            var apiResponse = new ApiResponse<T>
            {
                Data = data,
                Outcome = operationOutcome
            };

            return StatusCode((int)HttpStatusCode.InternalServerError, apiResponse);
        }

        public IActionResult InternalServerErrorResponse<T>(ApiResponse<T> apiResponse)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, apiResponse);
        }
    }
}
