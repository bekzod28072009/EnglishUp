using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Tests;
using Auth.Service.DTOs.Tests.MockTestsDto;
using Auth.Service.Exceptions;
using Auth.Service.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class MockTestService : IMockTestService
{
    private readonly IGenericRepository<MockTest> repository;
    private readonly IMapper mapper;

    public MockTestService(IGenericRepository<MockTest> repository, IMapper mapper)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    public async Task<IEnumerable<MockTestForViewDto>> GetAllAsync(Expression<Func<MockTest, bool>> filter = null, string[] includes = null)
    {
        var mockTests = await repository.GetAll(filter, includes).ToListAsync();
        return mapper.Map<IEnumerable<MockTestForViewDto>>(mockTests);
    }

    public async Task<MockTestForViewDto> GetAsync(Expression<Func<MockTest, bool>> filter, string[] includes = null)
    {
        var mockTest = await repository.GetAsync(filter, includes);
        if (mockTest is null)
            throw new HttpStatusCodeException(404, "MockTest not found");

        return mapper.Map<MockTestForViewDto>(mockTest);
    }

    public async Task<MockTestForViewDto> CreateAsync(MockTestForCreationDto dto)
    {
        var mockTest = mapper.Map<MockTest>(dto);
        await repository.CreateAsync(mockTest);
        await repository.SaveChangesAsync();

        return mapper.Map<MockTestForViewDto>(mockTest);
    }

    public async Task<MockTestForViewDto> UpdateAsync(long id, MockTestForUpdateDto dto)
    {
        var mockTest = await repository.GetAsync(x => x.Id == id);
        if (mockTest is null)
            throw new HttpStatusCodeException(404, "MockTest not found");

        mapper.Map(dto, mockTest);
        repository.Update(mockTest);
        await repository.SaveChangesAsync();

        return mapper.Map<MockTestForViewDto>(mockTest);
    }

    public async Task<bool> DeleteAsync(Expression<Func<MockTest, bool>> filter)
    {
        var mockTest = await repository.GetAsync(filter);
        if (mockTest is null)
            return false;

        await repository.DeleteAsync(mockTest);
        await repository.SaveChangesAsync();
        return true;
    }
}
