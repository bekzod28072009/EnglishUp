using Auth.Domain.Entities.Tests;
using Auth.Service.DTOs.Tests.MockTestsDto;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface IMockTestService
{
    Task<IEnumerable<MockTestForViewDto>> GetAllAsync(Expression<Func<MockTest, bool>> filter = null, string[] includes = null);
    Task<MockTestForViewDto> GetAsync(Expression<Func<MockTest, bool>> filter, string[] includes = null);
    Task<MockTestForViewDto> CreateAsync(MockTestForCreationDto dto);
    Task<MockTestForViewDto> UpdateAsync(long id, MockTestForUpdateDto dto);
    Task<bool> DeleteAsync(Expression<Func<MockTest, bool>> filter);
}
