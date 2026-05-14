using Domain.Todos;
using Domain.Users;
using Infrastructure.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public static class SeedDataExtensions
{
    public static async Task SeedDataAsync(this ApplicationDbContext context)
    {
        await SeedUsersAsync(context);
        await SeedTodoItemsAsync(context);

        await context.SaveChangesAsync();
    }

    private static async Task SeedUsersAsync(ApplicationDbContext context)
    {
        if (await context.Users.AnyAsync())
        {
            return;
        }

        var passwordHasher = new PasswordHasher();

        User[] users =
        [
            new User
            {
                Id = Guid.Parse("a7f3a5c0-b8d9-4e2c-9f5b-8c7d6e5a4b3c"),
                Email = "john.doe@example.com",
                FirstName = "John",
                LastName = "Doe",
                PasswordHash = passwordHasher.Hash("Password123!")
            },
            new User
            {
                Id = Guid.Parse("b8e4b6d1-c9ea-5f3d-af6c-9d8e7f6b5c4d"),
                Email = "jane.smith@example.com",
                FirstName = "Jane",
                LastName = "Smith",
                PasswordHash = passwordHasher.Hash("Password123!")
            }
        ];

        await context.Users.AddRangeAsync(users);
    }

    private static async Task SeedTodoItemsAsync(ApplicationDbContext context)
    {
        if (await context.TodoItems.AnyAsync())
        {
            return;
        }

        var userId1 = Guid.Parse("a7f3a5c0-b8d9-4e2c-9f5b-8c7d6e5a4b3c");
        var userId2 = Guid.Parse("b8e4b6d1-c9ea-5f3d-af6c-9d8e7f6b5c4d");

        TodoItem[] todoItems =
        [
            new TodoItem
            {
                Id = Guid.NewGuid(),
                UserId = userId1,
                Description = "Evi temizle",
                DueDate = new DateTime(2024, 12, 25, 0, 0, 0, DateTimeKind.Utc),
                Labels = new List<string>(),
                IsCompleted = false,
                CreatedAt = new DateTime(2024, 8, 12, 10, 0, 0, DateTimeKind.Utc),
                CompletedAt = null,
                Priority = Priority.Low
            },
            new TodoItem
            {
                Id = Guid.NewGuid(),
                UserId = userId1,
                Description = "Projeyi tamamla",
                DueDate = new DateTime(2024, 8, 20, 17, 0, 0, DateTimeKind.Utc),
                Labels = new List<string> { "urgent", "work" },
                IsCompleted = false,
                CreatedAt = new DateTime(2024, 8, 12, 10, 0, 0, DateTimeKind.Utc),
                CompletedAt = null,
                Priority = Priority.High
            },
            new TodoItem
            {
                Id = Guid.NewGuid(),
                UserId = userId1,
                Description = "Market alışverişi",
                DueDate = new DateTime(2024, 8, 15, 14, 0, 0, DateTimeKind.Utc),
                Labels = new List<string> { "shopping" },
                IsCompleted = true,
                CreatedAt = new DateTime(2024, 8, 10, 9, 0, 0, DateTimeKind.Utc),
                CompletedAt = new DateTime(2024, 8, 12, 16, 0, 0, DateTimeKind.Utc),
                Priority = Priority.Medium
            },
            new TodoItem
            {
                Id = Guid.NewGuid(),
                UserId = userId2,
                Description = "Kodu incele",
                DueDate = new DateTime(2024, 8, 18, 15, 0, 0, DateTimeKind.Utc),
                Labels = new List<string> { "work" },
                IsCompleted = false,
                CreatedAt = new DateTime(2024, 8, 11, 11, 0, 0, DateTimeKind.Utc),
                CompletedAt = null,
                Priority = Priority.High
            },
            new TodoItem
            {
                Id = Guid.NewGuid(),
                UserId = userId2,
                Description = "Egzersiz yap",
                DueDate = new DateTime(2024, 8, 13, 18, 0, 0, DateTimeKind.Utc),
                Labels = new List<string> { "health" },
                IsCompleted = false,
                CreatedAt = new DateTime(2024, 8, 12, 8, 0, 0, DateTimeKind.Utc),
                CompletedAt = null,
                Priority = Priority.Medium
            }
        ];

        await context.TodoItems.AddRangeAsync(todoItems);
    }
}
