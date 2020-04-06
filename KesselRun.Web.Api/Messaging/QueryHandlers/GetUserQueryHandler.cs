using System;
using System.Threading;
using System.Threading.Tasks;
using KesselRun.Business.DataTransferObjects;
using KesselRun.Web.Api.Messaging.Queries;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace KesselRun.Web.Api.Messaging.QueryHandlers
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserPayloadDto>
    {

        public GetUserQueryHandler()
        {
            
        }

        public async Task<UserPayloadDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            // normally I'd hit a database and retrieve the user by id.
            // Here, if the Id of 10 is passed in, a User with UserName RonBurgandy will be returned.

            if(request.UserId.Equals(10))
                return new UserPayloadDto
                {
                    UserName = "RonBurgandy"
                };

            throw new Exception("");
        }
    }
}
