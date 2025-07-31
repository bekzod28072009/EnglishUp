using Auth.DataAccess.Interface;
using Auth.Domain.Configurations;
using Auth.Domain.Entities.Roles;
using Auth.Domain.Entities.Tokens;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.Users;
using Auth.Service.Exceptions;
using Auth.Service.Extensions;
using Auth.Service.Helpers;
using Auth.Service.Interfaces;
using Auth.Service.Security;
using AutoMapper;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class UserService : IUserService
{
    private readonly IGenericRepository<User> _repository;
    private readonly IGenericRepository<Token> _tokentRepository;
    private readonly IGenericRepository<Role> _roleRepository;
    private readonly IMapper _mapper;

    public UserService(IGenericRepository<User> repository, IMapper mapper,
        IGenericRepository<Role> roleRepository, IGenericRepository<Token> tokentRepository)
    {
        this._repository = repository;
        this._mapper = mapper;
        this._roleRepository = roleRepository;
        this._tokentRepository = tokentRepository;
    }




    public async Task<PagedResult<UserForViewDto>> GetAllAsync(
            PaginationParams @params,
            Expression<Func<User, bool>> filter = null,
            string[] includes = null)
    {
        var query = _repository.GetAll(filter, includes: new[] { "Role" });

        var entities = await query.ToPagedListAsync(@params);


        var res = _mapper.Map<List<UserForViewDto>>(entities.Data).OrderBy(t => t.Id).ToList();



        foreach (var item in res)
        {
            var token = await _tokentRepository.GetAsync(res => res.UserId == item.Id);
        }

        PagedResult<UserForViewDto> result = new PagedResult<UserForViewDto>()
        {
            Data = res,
            TotalItems = entities.TotalItems,
            TotalPages = entities.TotalPages,
            CurrentPage = entities.CurrentPage,
            PageSize = entities.PageSize
        };


        return result;
    }

    public async Task<UserForViewDto> GetAsync(Expression<Func<User, bool>> filter, string[] includes = null)
    {
        var res = await _repository.GetAsync(filter, includes: ["Role"]);
        if (res == null)
            throw new HttpStatusCodeException(404, "User not found");

        return _mapper.Map<UserForViewDto>(res);
    }

    public async Task<UserForViewDto> CreateAsync(UserForCreationDto entity)
    {

        var existUser = await _repository.GetAsync(p => p.Email == entity.Email);
        if (existUser != null)
            throw new HttpStatusCodeException(400, "User is already exist");

        var roleRes = await _roleRepository.GetAsync(item => item.Id == entity.RoleId);
        if (roleRes == null)
            throw new HttpStatusCodeException(404, "Role is not exist");


        var user = _mapper.Map<User>(entity);

        user.PasswordHash = SecurePasswordHasher.Hash(entity.Password);
        user.Role = roleRes;

        _repository.CreateAsync(user);
        await _repository.SaveChangesAsync();

        return _mapper.Map<UserForViewDto>(user);
    }

    public async Task<bool> DeleteAsync(Expression<Func<User, bool>> filter)
    {
        var res = await _repository.GetAsync(filter);

        if (res == null)
            throw new HttpStatusCodeException(404, "User not found");

        res.DeletedBy = HttpContextHelper.UserId;
        await _repository.DeleteAsync(res);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<UserForViewDto> UpdateAsync(long id, UserForUpdateDto dto)
    {
        var res = await _repository.GetAsync(item => item.Id == id);

        if (res == null)
            throw new HttpStatusCodeException(404, "User not found");

        res = _mapper.Map(dto, res);
        res.UpdatedAt = DateTime.UtcNow;

        res.UpdatedBy = HttpContextHelper.UserId;


        _repository.Update(res);

        await _repository.SaveChangesAsync();
        return _mapper.Map<UserForViewDto>(res);
    }

    public async Task<bool> ChangePassword(string email, string password)
    {
        var user = await _repository.GetAsync(item => item.Email == email);

        if (user == null)
        {
            throw new KeyNotFoundException($"User with email {email} does not exist.");
        }

        var hashedPassword = SecurePasswordHasher.Hash(password);

        user.PasswordHash = hashedPassword;

        _repository.Update(user);

        await _repository.SaveChangesAsync();

        return true;
    }
    public async Task AddPointsAsync(long userId, int points)
    {
        var user = await _repository.GetAsync(u => u.Id == userId)
            ?? throw new HttpStatusCodeException(404, "User not found");

        user.Points += points;

        await _repository.SaveChangesAsync();
    }

}
