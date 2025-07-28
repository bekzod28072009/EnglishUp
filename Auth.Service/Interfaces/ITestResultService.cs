using Auth.Domain.Entities.Tests;
using Auth.Service.DTOs.Tests.MockTestResultsDto;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface ITestResultService
{
    Task<ResultForViewDto> CreateAsync(ResultForCreationDto dto);
    Task<ResultForViewDto> UpdateAsync(long id, ResultForUpdateDto dto);
    Task<bool> DeleteAsync(Expression<Func<TestResult, bool>> filter);
    Task<ResultForViewDto> GetAsync(Expression<Func<TestResult, bool>> filter, string[] includes = null);
    Task<IEnumerable<ResultForViewDto>> GetAllAsync(Expression<Func<TestResult, bool>> filter = null, string[] includes = null);

    // ✅ Additional useful methods
    Task<IEnumerable<ResultForViewDto>> GetByUserIdAsync(long userId);
    Task<IEnumerable<ResultForViewDto>> GetByMockTestIdAsync(long mockTestId);
    Task<double> CalculateAverageScoreAsync(long userId);
}
