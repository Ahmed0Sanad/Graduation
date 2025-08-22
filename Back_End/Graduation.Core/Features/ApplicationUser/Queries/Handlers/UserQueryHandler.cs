using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Graduation.Core.Bases;
using Graduation.Core.Features.ApplicationUser.Queries.Models;
using Graduation.Core.Features.ApplicationUser.Queries.Results;
using Graduation.Core.Resources;
using Graduation.Core.Warppars;
using Graduation.Data.Entities.Identity;

namespace Graduation.Core.Features.ApplicationUser.Queries.Handlers
{
    public class UserQueryHandler : ResponseHandler,
         IRequestHandler<GetUserPaginationQuery, PaginatedResult<GetUserPaginationReponse>>,
         IRequestHandler<GetUserByIdQuery, Response<GetUserByIdResponse>>
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _sharedResources;
        private readonly UserManager<AppUser> _userManager;
        #endregion

        #region Constructors
        public UserQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                  IMapper mapper,
                                  UserManager<AppUser> userManager) 
        {
            _mapper = mapper;
            _sharedResources = stringLocalizer;
            _userManager= userManager;
        }
        #endregion

        #region Handle Functions
        public async Task<PaginatedResult<GetUserPaginationReponse>> Handle(GetUserPaginationQuery request, CancellationToken cancellationToken)
        {
            var users = _userManager.Users.Skip((request.PageNumber -1) * request.PageSize).Take(request.PageSize).ToList();
            var count = _userManager.Users.Count();
            var usersToReturn = _mapper.Map<List<GetUserPaginationReponse>>(users);
            //var paginatedList = await _mapper.ProjectTo<GetUserPaginationReponse>(users)
            //                                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            var paginatedList = new PaginatedResult<GetUserPaginationReponse>(page: request.PageNumber, 
                                                                           pageSize: request.PageSize, 
                                                                           count: count,
                                                                           data: usersToReturn

                                                                           );

            return paginatedList;
        }

        public async Task<Response<GetUserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            //var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id==request.Id);
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user==null) return NotFound<GetUserByIdResponse>(_sharedResources[SharedResourcesKeys.NotFound]);
            var result = _mapper.Map<GetUserByIdResponse>(user);
            return Success(result);
        }
        #endregion
    }
}
