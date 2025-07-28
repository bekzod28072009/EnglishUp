using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Homeworks;
using Auth.Service.DTOs.Homeworks.PointTransactionsDto;
using Auth.Service.Exceptions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class PointTransactionService
{
    private readonly IGenericRepository<PointTransaction> repository;
    private readonly IMapper mapper;

    public PointTransactionService(IGenericRepository<PointTransaction> repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<PointTransactionForViewDto>> GetAllAsync(Expression<Func<PointTransaction, bool>> filter = null, string[] includes = null)
    {
        var transactions = repository.GetAll(filter, includes).ToListAsync();
        return mapper.Map<IEnumerable<PointTransactionForViewDto>>(transactions);
    }

    public async Task<PointTransactionForViewDto> GetAsync(Expression<Func<PointTransaction, bool>> filter, string[] includes = null)
    {
        var transaction = await repository.GetAsync(filter, includes);
        if (transaction is null)
            throw new HttpStatusCodeException(404, "Point transaction not found");

        return mapper.Map<PointTransactionForViewDto>(transaction);
    }

    public async Task<PointTransactionForViewDto> CreateAsync(PointTransactionForCreationDto dto)
    {
        var created = await repository.CreateAsync(mapper.Map<PointTransaction>(dto));
        await repository.SaveChangesAsync();
        return mapper.Map<PointTransactionForViewDto>(created);
    }

    public async Task<PointTransactionForViewDto> UpdateAsync(long id, PointTransactionForUpdateDto dto)
    {
        var transaction = await repository.GetAsync(t => t.Id == id);
        if (transaction is null)
            throw new HttpStatusCodeException(404, "Point transaction not found");

        mapper.Map(dto, transaction); // updates the entity with values from dto
        transaction.UpdatedAt = DateTime.UtcNow;

        await repository.SaveChangesAsync();
        return mapper.Map<PointTransactionForViewDto>(transaction);
    }

    public async Task<bool> DeleteAsync(Expression<Func<PointTransaction, bool>> filter)
    {
        var transaction = await repository.GetAsync(filter);
        if (transaction is null)
            return false;

        await repository.DeleteAsync(transaction);
        await repository.SaveChangesAsync();
        return true;
    }
}
