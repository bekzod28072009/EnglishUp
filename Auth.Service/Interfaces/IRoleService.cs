using Auth.Domain.Configurations;
using Auth.Domain.Entities.Roles;
using Auth.Service.DTOs.Roles;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface IRoleService
{
    Task<PagedResult<RoleForViewDto>> GetAllAsync(PaginationParams @params, Expression<Func<Role, bool>> filter = null, string[] includes = null);
    Task<RoleForViewGetDto> GetAsync(Expression<Func<Role, bool>> filter = null, string[] includes = null);
    Task<RoleForViewDto> CreateAsync(RoleForCreationDto dto);
    Task<bool> DeleteAsync(Expression<Func<Role, bool>> filter);
    Task<RoleForViewDto> UpdateAsync(long id, RoleForUpdateDto dto);
}
