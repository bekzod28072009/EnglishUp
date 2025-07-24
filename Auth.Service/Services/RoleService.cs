using Auth.DataAccess.Interface;
using Auth.Domain.Configurations;
using Auth.Domain.Entities.Permissions;
using Auth.Domain.Entities.Roles;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.Roles;
using Auth.Service.Exceptions;
using Auth.Service.Extensions;
using Auth.Service.Helpers;
using Auth.Service.Interfaces;
using AutoMapper;
using System.Linq.Expressions;
using System.Text.Json;

namespace Auth.Service.Services;

public class RoleService : IRoleService
{
    private readonly IGenericRepository<Role> _repository;
    private readonly IGenericRepository<Permission> _permissionRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IPermissionService _permissionService;
    private readonly IMapper _mapper;

    public RoleService(IGenericRepository<Role> repository, IMapper mapper, IGenericRepository<Permission> permissionRepository,
        IPermissionService permissionService, IGenericRepository<User> userRepository)
    {
        this._repository = repository;
        this._mapper = mapper;
        this._permissionRepository = permissionRepository;
        this._permissionService = permissionService;
        this._userRepository = userRepository;
    }

    public async Task<PagedResult<RoleForViewDto>> GetAllAsync(PaginationParams @params, Expression<Func<Role, bool>> filter = null, string[] includes = null)
    {
        var roles = _repository.GetAll(filter, includes: includes ?? new[] { "RolePermissions", "UserRoles" });


        var entities = await roles.ToPagedListAsync(@params);

        var rolesForViewDtos = new List<RoleForViewDto>();

        foreach (var role in entities.Data)
        {
            var permissionNames = role.RolePermission.Select(rp => rp.Name).ToList();
            var rolePermissions = await _permissionService.GetPermissionsAsync(permissionNames);

            var roleForViewDto = new RoleForViewDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                RolePermissions = rolePermissions,
                UserCount = role.UserRole.Count()
            };

            rolesForViewDtos.Add(roleForViewDto);
        }

        PagedResult<RoleForViewDto> result = new PagedResult<RoleForViewDto>
        {
            Data = rolesForViewDtos,
            TotalItems = entities.TotalItems,
            TotalPages = entities.TotalPages,
            CurrentPage = entities.CurrentPage,
            PageSize = entities.PageSize
        };

        return result;

    }


    public async Task<RoleForViewGetDto> GetAsync(Expression<Func<Role, bool>> filter, string[] includes = null)
    {
        var role = await _repository.GetAsync(filter, includes: includes ?? new[] { "UserRoles", "RolePermissions" });
        if (role == null)
            throw new HttpStatusCodeException(404, "Role not found");


        //var permissionNames = role.RolePermissions.Select(rp => rp.Name).ToList();

        //var rolePermissions = await _permissionService.GetPermissionsAsync(permissionNames);

        List<string> strings = new List<string>();

        foreach (var item in role.UserRole)
        {
            strings.Add(item.FullName);
        }

        var resUser = await _userRepository.GetAsync(item => item.Id == role.CreatedBy);
        var permission = await _permissionService.GetPermissionsAsync(role.RolePermission.Select(item => item.Name).ToList());


        var res = new RoleForViewGetDto
        {
            Id = role.Id,
            Name = role.Name,
            CreatedAt = role.CreatedAt,
            Users = strings,
            Permissions = permission,
            CreatedBy = resUser.FullName

        };

        return res;
    }

    public async Task<RoleForViewDto> CreateAsync(RoleForCreationDto dto)
    {
        var res = await _repository.GetAsync(iten => iten.Name == dto.Name);
        if (res != null)
            throw new HttpStatusCodeException(404, "Group is exit");

        var result = new List<string>();

        if (dto.Permissions is JsonElement jsonElement)
        {
            var permissions = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, bool>>>(jsonElement.GetRawText());
            if (permissions == null)
                throw new ArgumentException("Invalid PermissionId format.");

            foreach (var section in permissions)
            {
                foreach (var action in section.Value)
                {
                    if (action.Value == true)
                    {
                        string key = $"{section.Key}_{action.Key}";
                        result.Add(key.ToString());
                    }
                }
            }
        }
        else
        {
            throw new ArgumentException("Invalid PermissionId format.");
        }

        List<Permission> permissionsResult = new List<Permission>();
        foreach (var Id in result)
        {
            var permission = await _permissionRepository.GetAsync(item => item.Name.ToLower() == Id.ToLower());
            if (permission == null)
                throw new HttpStatusCodeException(404, $"Permission with Id {Id} not found");

            permissionsResult.Add(permission);
        }

        var role = _mapper.Map<Role>(dto);

        role.RolePermission = permissionsResult;

        await _repository.CreateAsync(role);
        await _repository.SaveChangesAsync();

        role.RolePermission = null;


        return _mapper.Map<RoleForViewDto>(role);
    }
    public async Task<bool> DeleteAsync(Expression<Func<Role, bool>> filter)
    {
        var role = await _repository.GetAsync(filter);
        if (role == null)
            throw new HttpStatusCodeException(404, "Role not found");

        role.DeletedBy = HttpContextHelper.UserId;
        await _repository.DeleteAsync(role);  
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<RoleForViewDto> UpdateAsync(long id, RoleForUpdateDto dto)
    {
        // Rolni topish
        var role = await _repository.GetAsync(r => r.Id == id, includes: new[] { "RolePermissions" });
        if (role == null)
            throw new HttpStatusCodeException(404, "Role not found");



        var result = new List<string>();

        if (dto.Permissions is JsonElement jsonElement)
        {
            var permissions = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, bool>>>(jsonElement.GetRawText());
            if (permissions == null)
                throw new ArgumentException("Invalid PermissionId format.");

            foreach (var section in permissions)
            {
                foreach (var action in section.Value)
                {
                    if (action.Value == true)
                    {
                        string key = $"{section.Key}_{action.Key}";
                        result.Add(key.ToString());
                    }
                }
            }
        }
        else
        {
            throw new ArgumentException("Invalid PermissionId format.");
        }

        List<Permission> permissionsResult = new List<Permission>();
        foreach (var Id in result)
        {
            var permission = await _permissionRepository.GetAsync(item => item.Name.ToLower() == Id.ToLower());
            if (permission == null)
                throw new HttpStatusCodeException(404, $"Permission with Id {Id} not found");

            permissionsResult.Add(permission);
        }


        _mapper.Map(dto, role);

        role.RolePermission.Clear();

        role.RolePermission = permissionsResult;

        role.UpdatedBy = HttpContextHelper.UserId;
        role.UpdatedAt = DateTime.UtcNow;

        _repository.Update(role);
        await _repository.SaveChangesAsync();

        role.RolePermission = null;

        return _mapper.Map<RoleForViewDto>(role);
    }
}
