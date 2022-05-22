using System;
using System.Threading;
using System.Threading.Tasks;
using KesselRun.Business.DataTransferObjects;
using KesselRun.Web.Api.Messaging.Queries;
using KesselRunFramework.Core.Cqrs.Queries;

namespace KesselRun.Web.Api.Messaging.QueryHandlers
{
    public class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserPayloadDto>
    {

        public GetUserQueryHandler()
        {
            
        }

        
        public async Task<UserPayloadDto> HandleAsync(GetUserQuery query, CancellationToken cancellationToken)
        {
            // normally I'd hit a database and retrieve the user by id.
            // Here, if the Id of 10 is passed in, a User with UserName RonBurgandy will be returned.

            if (query.UserId.Equals(10))
                return await Task.FromResult(new UserPayloadDto
                {
                    UserName = "RonBurgandy"
                });

            throw new Exception("User does not exist.");
        }
    }
}
