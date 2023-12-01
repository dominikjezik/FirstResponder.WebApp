using FirstResponder.ApplicationCore.Users.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Queries;

public class GetUserItems :  IRequest<IEnumerable<UserItemDTO>>
{
    public int PageNumber { get; set; }
    
    public UserItemFiltersDTO Filters { get; set; }
}