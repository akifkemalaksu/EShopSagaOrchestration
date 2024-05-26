﻿namespace Shared.Interfaces.Queries
{
    public interface IQueryHandler<in TQuery, TQueryResult>
    {
        Task<TQueryResult> Handle(TQuery query, CancellationToken cancellationToken);
    }
}
