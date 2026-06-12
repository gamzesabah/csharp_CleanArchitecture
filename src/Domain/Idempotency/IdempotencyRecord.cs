using System;
using System.Collections.Generic;
using System.Text;
using SharedKernel;

namespace Domain.Idempotency;

public sealed class IdempotencyRecord : Entity
{
    public Guid Id { get; private set; }

    public string Key { get; private set; }

    public string Response { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime ExpiresAt { get; private set; }

    private IdempotencyRecord()
    {
    }

    private IdempotencyRecord(
        Guid id,
        string key,
        string response,
        DateTime createdAt,
        DateTime expiresAt)
    {
        Id = id;
        Key = key;
        Response = response;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
    }

    public static IdempotencyRecord Create(
        string key,
        string response)
    {
        return new IdempotencyRecord(
            Guid.NewGuid(),
            key,
            response,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(24));
    }
}
