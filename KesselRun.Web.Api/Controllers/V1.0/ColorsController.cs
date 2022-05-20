﻿using FluentValidation.Results;
using KesselRun.Business.DataTransferObjects;
using KesselRun.Web.Api.New;
using KesselRunFramework.AspNet.Infrastructure;
using KesselRunFramework.AspNet.Infrastructure.Controllers;
using KesselRunFramework.AspNet.Infrastructure.Invariants;
using KesselRunFramework.AspNet.Response;
using KesselRunFramework.Core.Infrastructure.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace KesselRun.Web.Api.Controllers.V1._0
{
    [ApiVersion(Swagger.Versions.v1_0)]
    [Route(AspNet.Mvc.DefaultControllerTemplate)]
    [Produces(MediaTypeNames.Application.Json)]
    public class ColorsController : AppApiController
    {
        private readonly IQueryHandler<GetColorsQuery, Either<IEnumerable<ColorPayloadDto>, ValidationResult>> _queryHandler;

        public ColorsController(
            ICurrentUser currentUser,
            ILogger logger,
            IQueryHandler<GetColorsQuery, Either<IEnumerable<ColorPayloadDto>, ValidationResult>> queryHandler
            )
            : base(currentUser)
        {
            _queryHandler = queryHandler;
        }

        [HttpGet]
        [Route(AspNet.Mvc.ActionTemplate)]
        [MapToApiVersion(Swagger.Versions.v1_0)]
        //[ApiExplorerSettings(GroupName = Swagger.DocVersions.v1_0)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetColors()
        {
            var colors = await _queryHandler.HandleAsync(new GetColorsQuery(), CancellationToken.None);

            return colors.Match(
                result => OkResponse(colors.LeftOrDefault()), 
                error => BadRequestResponse(string.Empty, OperationOutcome.ValidationFailOutcome(colors.RightOrDefault().Errors.Select(e => e.ErrorMessage)))                
                );
        }
    }
}
