using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Idempotency;

public interface IIdempotencyRepository
{
    Task<IdempotencyRecord?> GetByKeyAsync(
        string key,
        CancellationToken cancellationToken);

    Task AddAsync(
        IdempotencyRecord record);
}
