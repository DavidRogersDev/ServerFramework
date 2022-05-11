using System.Collections.Generic;
using System.Net;
using KesselRunFramework.AspNet.Infrastructure.Extensions;
using KesselRunFramework.AspNet.Response;
using KesselRunFramework.Core.Infrastructure.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KesselRunFramework.AspNet.Infrastructure.Controllers
{
    [ApiController]
    public class AppApiController : ControllerBase
    {
        protected readonly ICurrentUser _currentUser;

        public AppApiController(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
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

        public IActionResult CreatedResponse<T>(T data, string url, string message = null)
        {
            var outcome = OperationOutcome.SuccessfulOutcome;
            outcome.Message = message ?? string.Empty;

            var apiResponse = new ApiResponse<T>
            {
                Data = data,
                Outcome = outcome
            };

            return Created(url, apiResponse);
        }

        public IActionResult CreatedResponse<T>(T data, string url, OperationOutcome operationOutcome)
        {
            var apiResponse = new ApiResponse<T>
            {
                Data = data,
                Outcome = operationOutcome
            };

            return Created(url, apiResponse);
        }

        public IActionResult CreatedResponse<T>(string url, ApiResponse<T> apiResponse)
        {
            return Created(url, apiResponse);
        }

        public IActionResult UnprocessableEntityResponse<T>(T data, string errorMessage = null, IEnumerable<string> errors = null)
        {
            var outcome = OperationOutcome.ValidationFailOutcome(errors, errorMessage);

            var apiResponse = new ApiResponse<T>
            {
                Data = data,
                Outcome = outcome
            };

            return StatusCode(StatusCodes.Status422UnprocessableEntity, apiResponse);
        }

        public IActionResult UnprocessableEntityResponse<T>(T data, OperationOutcome operationOutcome)
        {
            var apiResponse = new ApiResponse<T>
            {
                Data = data,
                Outcome = operationOutcome
            };

            return StatusCode(StatusCodes.Status422UnprocessableEntity, apiResponse);
        }

        public IActionResult UnprocessableEntityResponse<T>(ApiResponse<T> apiResponse)
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, apiResponse);
        }

        public IActionResult InternalServerErrorResponse<T>(T data, string errorMessage = null, IEnumerable<string> errors = null)
        {
            var outcome = OperationOutcome.ValidationFailOutcome(errors, errorMessage);

            var apiResponse = new ApiResponse<T>
            {
                Data = data,
                Outcome = outcome
            };

            return this.ServerError(apiResponse);
        }

        public IActionResult InternalServerErrorResponse<T>(T data, OperationOutcome operationOutcome)
        {
            var apiResponse = new ApiResponse<T>
            {
                Data = data,
                Outcome = operationOutcome
            };

            return this.ServerError(apiResponse);
        }

        public IActionResult InternalServerErrorResponse<T>(ApiResponse<T> apiResponse)
        {
            return this.ServerError(apiResponse);
        }

        protected void PrepareInvalidResult<T>(ValidateableResponse<T> result)
            where T : class
        {
            // The ModelState may be valid at this point, BUT the result has validation errors (possibly from the Business layer)
            if (!result.IsValidResponse && ModelState.IsValid) result.AddToModelState(ModelState);
        }
    }
}
