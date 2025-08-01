﻿using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ExpenseTracker.Data;
using Volo.Abp.DependencyInjection;

namespace ExpenseTracker.EntityFrameworkCore;

public class EntityFrameworkCoreExpenseTrackerDbSchemaMigrator
    : IExpenseTrackerDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreExpenseTrackerDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the ExpenseTrackerDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<ExpenseTrackerDbContext>()
            .Database
            .MigrateAsync();
    }
}
