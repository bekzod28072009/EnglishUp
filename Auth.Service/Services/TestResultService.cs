using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Tests;
using Auth.Service.DTOs.Tests.MockTestResultsDto;
using Auth.Service.Exceptions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Auth.Service.Services;

public class TestResultService
{
    private readonly IGenericRepository<TestResult> repository;
    private readonly IMapper mapper;
    public TestResultService(IGenericRepository<TestResult> repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    // Create
    public async Task<ResultForViewDto> CreateAsync(ResultForCreationDto dto)
    {
        var result = mapper.Map<TestResult>(dto);
        var created = await repository.CreateAsync(result);
        return mapper.Map<ResultForViewDto>(created);
    }

    // Get All
    public async Task<IEnumerable<ResultForViewDto>> GetAllAsync()
    {
        var results = await repository.GetAll(includes: new[] { "User", "MockTest" }).ToListAsync();
        return mapper.Map<IEnumerable<ResultForViewDto>>(results);
    }

    // Get by ID
    public async Task<ResultForViewDto> GetByIdAsync(long id)
    {
        var result = await repository.GetAsync(x => x.Id == id, includes: new[] { "User", "MockTest" });
        if (result is null)
            throw new HttpStatusCodeException(404, $"MockTestResult with ID {id} not found");

        return mapper.Map<ResultForViewDto>(result);
    }

    // Update
    public async Task<ResultForViewDto> UpdateAsync(long id, ResultForUpdateDto dto)
    {
        var existing = await repository.GetAsync(x => x.Id == id);
        if (existing is null)
            throw new HttpStatusCodeException(404, $"MockTestResult with ID {id} not found");

        mapper.Map(dto, existing);
        existing.UpdatedAt = DateTime.UtcNow;

        repository.Update(existing);
        return mapper.Map<ResultForViewDto>(existing);
    }

    // Delete
    public async Task<bool> DeleteAsync(long id)
    {
        var result = await repository.GetAsync(x => x.Id == id);
        if (result is null)
            throw new HttpStatusCodeException(404, $"MockTestResult with ID {id} not found");

        await repository.DeleteAsync(result);
        return true;
    }

    // Get results by user
    public async Task<IEnumerable<ResultForViewDto>> GetByUserIdAsync(long userId)
    {
        var results = await repository.GetAll(x => x.UserId == userId, includes: new[] { "MockTest" }).ToListAsync();
        return mapper.Map<IEnumerable<ResultForViewDto>>(results);
    }

    // Get results by mock test
    public async Task<IEnumerable<ResultForViewDto>> GetByMockTestIdAsync(long mockTestId)
    {
        var results = await repository.GetAll(x => x.MockTestId == mockTestId, includes: new[] { "User" }).ToListAsync();
        return mapper.Map<IEnumerable<ResultForViewDto>>(results);
    }

    // Calculate average overall score
    public async Task<double> CalculateAverageScoreAsync(long userId)
    {
        var results = repository.GetAll(x => x.UserId == userId);
        if (!results.Any()) return 0;

        var avg = results.Average(r => r.OverallScore);
        return Math.Round(avg, 2);
    }
}
