using RingDownConsole.Models.Utils;
using RingDownConsole.Utils.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Primitives;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RingDownConsole.Models
{
    public class RingDownConsoleDbContext : IdentityDbContext
    {
        private readonly IServiceProvider _serviceProvider;

        public DbSet<Audit> Audits { get; set; }
        public DbSet<ExampleRecord> ExampleTable { get; set; }

        public RingDownConsoleDbContext(DbContextOptions<RingDownConsoleDbContext> options, IServiceProvider serviceProvider) : base(options)
        {
            _serviceProvider = serviceProvider;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            return PrivateSaveChangesAsync(CancellationToken.None).Result;
        }

        public Task<int> SaveChangesAsync()
        {
            return PrivateSaveChangesAsync(CancellationToken.None);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return PrivateSaveChangesAsync(cancellationToken);
        }

        private async Task<int> PrivateSaveChangesAsync(CancellationToken cancellationToken)
        {
#if DEBUG
            if (!DbInitializer.Initialized)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
#endif
            // If you want to have audits in transaction with records you must handle
            // transactions manually
            using (var transaction = await Database.BeginTransactionAsync())
            {
                // Now you must call your audit code but instead of adding audits to context
                // you must add them to list. 
                var audits = new List<Audit>();

                var httpContext = (IHttpContextAccessor)_serviceProvider.GetService(typeof(IHttpContextAccessor));
                var currentPrincipal = httpContext?.HttpContext?.User;

                IdentityUser user = null;

                if (currentPrincipal != null)
                {
                    var userManager = (UserManager<IdentityUser>)_serviceProvider.GetService(typeof(UserManager<IdentityUser>));
                    user = await userManager?.GetUserAsync(currentPrincipal);
                }

                foreach (var entry in ChangeTracker.Entries().Where(o => o.State != EntityState.Unchanged && o.State != EntityState.Detached).ToList())
                {
                    var changeType = entry.State.ToString();
                    Type entityType = GetEntityType(entry);

                    var tableName = entityType.Name;

                    var identityJson = GetIdentityJson(entry, entityType);

                    var audit = new Audit
                    {
                        Id = Guid.NewGuid(),
                        AuditUserId = user?.Id == null ? null : user.Id,
                        IpAddress = GetRequestIP(httpContext),
                        ChangeType = changeType,
                        ObjectType = entityType.ToString(),
                        FromJson = (entry.State == EntityState.Added ? "{  }" : GetAsJson(entry.OriginalValues)),
                        ToJson = (entry.State == EntityState.Deleted ? "{  }" : GetAsJson(entry.CurrentValues)),
                        TableName = tableName,
                        IdentityJson = identityJson,
                        DateCreated = DateTime.UtcNow,
                    };
#if DEBUG
                    if (audit.FromJson == audit.ToJson)
                    {
                        Log.Information($"Something went wrong because this {audit.ChangeType} Audit shows no changes!", audit);
                        continue;
                    }
#endif
                    audits.Add(audit);
                }

                // Now add all audits from list to context
                Audits.AddRange(audits);

                var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                // Complete the transaction
                transaction.Commit();

                return result;
            }
        }

        private string GetIdentityJson(EntityEntry entry, Type entityType)
        {
            var identityJson = string.Empty;
            foreach (var field in entityType.GetProperties().Where(o => o.CustomAttributes.FirstOrDefault(oo => oo.AttributeType == typeof(System.ComponentModel.DataAnnotations.KeyAttribute)) != null))
            {
                if (identityJson.Length > 0)
                {
                    identityJson += ", ";
                }

                identityJson += $@"""{field.Name}"":{GetFieldValue(field.Name, entry.State == EntityState.Deleted ? entry.OriginalValues : entry.CurrentValues)}";
            }
            return $"{{ {identityJson} }}";
        }

        private object GetFieldValue(string name, PropertyValues values)
        {
            var val = values[name];
            return val == null ? "null" : (IsNumber(val) ? val.ToString() : $@"""{val}""");
        }

        private static Type GetEntityType(EntityEntry entry)
        {
            Type entityType = entry.Entity.GetType();

            if (entityType.BaseType != null && entityType.Namespace == "System.Data.Entity.DynamicProxies")
                entityType = entityType.BaseType;
            return entityType;
        }

        private static string GetAsJson(PropertyValues values)
        {
            var json = string.Empty;
            if (values != null)
            {
                foreach (var property in values.Properties)
                {
                    if (json.Length > 0)
                    {
                        json += ", ";
                    }
                    var val = values[property.Name];
                    json += $@"""{property.Name}"":{(val == null ? "null" : (IsNumber(val) ? val.ToString() : $@"""{val}"""))}";
                }
            }
            return $"{{ {json} }}";
        }

        public static bool IsNumber(object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }

        public string GetRequestIP(IHttpContextAccessor httpContextAccessor, bool tryUseXForwardHeader = true)
        {
            string ip = null;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
                ip = GetHeaderValueAs<string>(httpContextAccessor, "X-Forwarded-For").SplitCsvAndGetFirst();

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (string.IsNullOrWhiteSpace(ip) && httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress != null)
                ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (string.IsNullOrWhiteSpace(ip))
                ip = GetHeaderValueAs<string>(httpContextAccessor, "REMOTE_ADDR");

            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            return ip;
        }

        public T GetHeaderValueAs<T>(IHttpContextAccessor httpContextAccessor, string headerName)
        {
            if (httpContextAccessor.HttpContext?.Request?.Headers?.TryGetValue(headerName, out StringValues values) ?? false)
            {
                var rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!string.IsNullOrEmpty(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }
    }
}