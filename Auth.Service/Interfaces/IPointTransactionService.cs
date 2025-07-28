using Auth.Domain.Entities.Homeworks;
using Auth.Service.DTOs.Homeworks.PointTransactionsDto;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface IPointTransactionService
{
    Task<IEnumerable<PointTransactionForViewDto>> GetAllAsync(Expression<Func<PointTransaction, bool>> filter = null, string[] includes = null);
    Task<PointTransactionForViewDto> GetAsync(Expression<Func<PointTransaction, bool>> filter, string[] includes = null);
    Task<PointTransactionForViewDto> CreateAsync(PointTransactionForCreationDto dto);
    Task<bool> DeleteAsync(Expression<Func<PointTransaction, bool>> filter);
    Task<PointTransactionForViewDto> UpdateAsync(long id, PointTransactionForUpdateDto dto);
}
