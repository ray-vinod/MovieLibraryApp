using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MovieLibraryApp.MovieLibrary.Shared.Models;

namespace MovieLibraryApp.MovieLibrary.Api.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplySoftDeleteQueryFilter(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IAuditable).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(GetIsDeletedFilter(entityType.ClrType));
            }
        }
    }

    // Create a filter expression for the IsDeleted property
    private static LambdaExpression GetIsDeletedFilter(Type entityType)
    {
        var parameter = Expression.Parameter(entityType, "e");
        var property = Expression.Property(parameter, nameof(IAuditable.IsDeleted));
        var condition = Expression.Equal(property, Expression.Constant(false));
        var lambda = Expression.Lambda(condition, parameter);
        return lambda;
    }
}