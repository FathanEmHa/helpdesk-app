using Helpdesk.Models;
using Helpdesk.Dtos.Ticket;
using Helpdesk.Dtos.User;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyPagination<T>(
        this IQueryable<T> query,
        int page,
        int pageSize)
    {
        return query
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
    }

    // =========================
    // Ticket
    // =========================

    public static IQueryable<Ticket> ApplyFilters(
        this IQueryable<Ticket> query,
        TicketQueryRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var keyword = request.Search.Trim();

            query = query.Where(t =>
                EF.Functions.ILike(
                    t.Title,
                    $"%{keyword}%") ||

                EF.Functions.ILike(
                    t.Description,
                    $"%{keyword}%"));
        }

        if (request.Status.HasValue)
        {
            query = query.Where(t =>
                t.Status == request.Status.Value);
        }

        if (request.Priority.HasValue)
        {
            query = query.Where(t =>
                t.Priority == request.Priority.Value);
        }

        return query;
    }

    public static IQueryable<Ticket> ApplySorting(
        this IQueryable<Ticket> query,
        TicketQueryRequest request)
    {
        return request.SortBy switch
        {
            TicketSortBy.Title => request.Descending
                ? query.OrderByDescending(t => t.Title)
                : query.OrderBy(t => t.Title),

            TicketSortBy.Priority => request.Descending
                ? query.OrderByDescending(t => t.Priority)
                : query.OrderBy(t => t.Priority),

            TicketSortBy.Status => request.Descending
                ? query.OrderByDescending(t => t.Status)
                : query.OrderBy(t => t.Status),

            TicketSortBy.CreatedAt => request.Descending
                ? query.OrderByDescending(t => t.CreatedAt)
                : query.OrderBy(t => t.CreatedAt),

            _ => request.Descending
                ? query.OrderByDescending(t => t.CreatedAt)
                : query.OrderBy(t => t.CreatedAt)
        };
    }

    // =========================
    // User
    // =========================

    public static IQueryable<User> ApplyFilters(
        this IQueryable<User> query,
        UserQueryRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var keyword = request.Search.Trim();

            query = query.Where(u =>
                EF.Functions.ILike(
                    u.Name,
                    $"%{keyword}%") ||

                EF.Functions.ILike(
                    u.Email,
                    $"%{keyword}%"));
        }

        if (request.Role.HasValue)
        {
            query = query.Where(u =>
                u.Role == request.Role.Value);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(u =>
                u.Status == request.Status.Value);
        }

        return query;
    }

    public static IQueryable<User> ApplySorting(
        this IQueryable<User> query,
        UserQueryRequest request)
    {
        return request.SortBy switch
        {
            UserSortBy.Name => request.Descending
                ? query.OrderByDescending(u => u.Name)
                : query.OrderBy(u => u.Name),

            UserSortBy.Email => request.Descending
                ? query.OrderByDescending(u => u.Email)
                : query.OrderBy(u => u.Email),

            UserSortBy.Role => request.Descending
                ? query.OrderByDescending(u => u.Role)
                : query.OrderBy(u => u.Role),

            UserSortBy.Status => request.Descending
                ? query.OrderByDescending(u => u.Status)
                : query.OrderBy(u => u.Status),

            UserSortBy.CreatedAt => request.Descending
                ? query.OrderByDescending(u => u.CreatedAt)
                : query.OrderBy(u => u.CreatedAt),

            _ => request.Descending
                ? query.OrderByDescending(u => u.CreatedAt)
                : query.OrderBy(u => u.CreatedAt)
        };
    }
}