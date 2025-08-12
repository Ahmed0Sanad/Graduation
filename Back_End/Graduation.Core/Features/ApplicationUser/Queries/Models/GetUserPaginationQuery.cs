using MediatR;
using Graduation.Core.Features.ApplicationUser.Queries.Results;

using Graduation.Core.Warppars;

namespace Graduation.Core.Features.ApplicationUser.Queries.Models
{
    public class GetUserPaginationQuery : IRequest<PaginatedResult<GetUserPaginationReponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
