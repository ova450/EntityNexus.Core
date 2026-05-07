using EntityNexus.DomainModel.Interfaces.Core;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EntityNexus.Infrastructure.AbstractClasses.Core
{
    public abstract class ADbContext(DbContextOptions options, string[]? primaryKeys = null) : DbContext(options)
    {
        private readonly string[] _primaryKeys = primaryKeys ?? ["Id"];

        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    var now = DateTimeOffset.UtcNow;

        //    foreach (var entry in ChangeTracker.Entries())
        //    {
        //        var entity = entry.Entity;

        //        if (entry.State == EntityState.Added)
        //        {
        //            if (entity is ICreated created) created.CreatedAt = now;// временно, см. ниже
        //        }

        //        if (entry.State == EntityState.Modified)
        //        {
        //            if (entity is IModified modified) modified.ModifiedAt = now;
        //        }

        //        if (entry.State == EntityState.Modified)
        //        {
        //            foreach (var prop in entry.Properties)
        //            {
        //                if (!Equals(prop.OriginalValue, prop.CurrentValue))
        //                {
        //                    // 👉 здесь ты знаешь:
        //                    // - какое поле изменилось
        //                    // - старое значение
        //                    // - новое значение
        //                }
        //            }
        //        }
        //    }

        //    BuildHistoryEntries();

        //    return await base.SaveChangesAsync(cancellationToken);
        //}

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //var historyEntries = BuildHistoryEntries();

            var result = await base.SaveChangesAsync(cancellationToken);

            //if (historyEntries.Count > 0)
            //{
            //    foreach (var h in historyEntries)
            //        Add(h);

            //    await base.SaveChangesAsync(cancellationToken);
            //}

            return result;
        }

        //private List<object> BuildHistoryEntries()
        //{
        //    var result = new List<object>();
        //    var now = DateTimeOffset.UtcNow;

        //    foreach (var entry in ChangeTracker.Entries())
        //    {
        //        if (entry.State != EntityState.Modified) continue;

        //        var clrType = entry.Entity.GetType();

        //        //var hasHistory = clrType.GetCustomAttribute<TrackHistoryAttribute>() != null;
        //        //if (!hasHistory) continue;

        //        var keyProp = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey());
        //        if (keyProp == null) continue;

        //        var entityId = keyProp.CurrentValue;

        //        foreach (var prop in entry.Properties)
        //        {
        //            if (prop.Metadata.IsPrimaryKey()) continue;
        //            if (Equals(prop.OriginalValue, prop.CurrentValue)) continue;

        //            //var historyType = typeof(HistoryEntry<>)
        //            //    .MakeGenericType(keyProp.Metadata.ClrType);

        //            var history = Activator.CreateInstance(historyType)!;

        //            historyType.GetProperty("EntityId")!.SetValue(history, entityId);
        //            historyType.GetProperty("EntityName")!.SetValue(history, clrType.Name);
        //            historyType.GetProperty("PropertyName")!.SetValue(history, prop.Metadata.Name);
        //            historyType.GetProperty("OldValue")!.SetValue(history, prop.OriginalValue?.ToString());
        //            historyType.GetProperty("NewValue")!.SetValue(history, prop.CurrentValue?.ToString());
        //            historyType.GetProperty("ChangedAt")!.SetValue(history, now);

        //            result.Add(history);
        //        }
        //    }

        //    return result;
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<HistoryEntry<int>>().ToTable("History_Int");
            //modelBuilder.Entity<HistoryEntry<Guid>>().ToTable("History_Guid");

            // Применяем все конфигурации из сборки
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                //var isAuditable = clrType.GetCustomAttribute<AuditableAttribute>() != null;

                //if (isAuditable)
                //{
                //    modelBuilder.Entity(clrType).Property<DateTimeOffset>("CreatedAt");
                //    modelBuilder.Entity(clrType).Property<DateTimeOffset?>("ModifiedAt");
                //}

                //// 2. Concurrency
                //if (typeof(IConcurrency).IsAssignableFrom(clrType))
                //    modelBuilder.Entity(clrType).Property<byte[]>("RowVersion").IsRowVersion();

                //// 3. Soft Delete
                //if (typeof(ISoftDeletable).IsAssignableFrom(clrType) ||
                //    clrType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISoftDeletable)))
                //{
                //    modelBuilder.Entity(clrType).HasQueryFilter(e => EF.Property<bool>(e, nameof(ISoftDeletable.IsDeleted)) == false);
                //}

                var hasParent = clrType.GetInterfaces().FirstOrDefault(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHasParent<,>));

                if (hasParent != null)
                {
                    var parentType = hasParent.GetGenericArguments()[0];

                    modelBuilder.Entity(clrType)
                        .HasOne(parentType)
                        .WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);
                }
            }
        }
    }
}
