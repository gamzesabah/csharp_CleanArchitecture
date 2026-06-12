using System;
using System.Collections.Generic;
using System.Text;
using Domain.Idempotency;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Idempotency;

internal sealed class IdempotencyRepository
    : IIdempotencyRepository
{
    private readonly ApplicationDbContext _context;

    public IdempotencyRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IdempotencyRecord?>
        GetByKeyAsync(
            string key,
            CancellationToken cancellationToken)
    {
        return await _context
            .IdempotencyRecords
            .FirstOrDefaultAsync(
                x => x.Key == key,
                cancellationToken);
    }

    public async Task AddAsync(
        IdempotencyRecord record)
    {
        await _context
            .IdempotencyRecords
            .AddAsync(record);
    }
}
