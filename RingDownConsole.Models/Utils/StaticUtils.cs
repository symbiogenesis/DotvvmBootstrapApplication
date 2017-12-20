using RingDownConsole.Interfaces;
using RingDownConsole.Utils.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace RingDownConsole.Models.Utils
{
    public static class StaticUtils<T> where T : class, IIdentifiable
    {
        public static DbSet<T> GetItems(DbContext context)
        {
            return context.GetTable<T>();
        }

        public static Task<T> GetItem(Guid id, DbContext context)
        {
            var table = context.GetTable<T>();

            return table.FirstOrDefaultAsync(t => t.Id == id);
        }

        public static async Task PutItem(T record, Guid id, DbContext context)
        {
            var table = context.GetTable<T>();

            T recordToUpdate = null;

            if (id != Guid.Empty)
                recordToUpdate = table.SingleOrDefault(t => t.Id == id);

            if (recordToUpdate == null)
            {
                table.Add(record);
            }
            else
            {
                context.Entry(recordToUpdate).CurrentValues.SetValues(record);
            }

            await context.SaveChangesAsync();
        }

        public async static Task PostItem(T newOrUpdatedRecord, DbContext context)
        {
            var table = context.GetTable<T>();

            var recordToUpdate = table.SingleOrDefault(t => t.Id == newOrUpdatedRecord.Id);
            if (recordToUpdate != null)
                return;

            table.Add(newOrUpdatedRecord);
            await context.SaveChangesAsync();
        }

        public async static Task DeleteItem(Guid id, DbContext context)
        {
            var records = context.GetTable<T>();

            var recordToDelete = records.SingleOrDefault(t => t.Id == id);
            if (recordToDelete == null)
                return;
            records.Remove(recordToDelete);
            await context.SaveChangesAsync();
        }
    }
}
