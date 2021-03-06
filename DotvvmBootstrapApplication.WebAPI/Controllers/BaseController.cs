﻿using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using DotvvmBootstrapApplication.Interfaces;
using DotvvmBootstrapApplication.Models;
using DotvvmBootstrapApplication.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotvvmBootstrapApplication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class BaseController<T> : Controller where T : class, IIdentifiable
    {
        protected readonly BootstrapDbContext _context;

        public BaseController(BootstrapDbContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public virtual IEnumerable<T> Get()
        {
            return _context.GetTable<T>();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public virtual T Get(Guid id)
        {
            var table = _context.GetTable<T>();

            return table.FirstOrDefault(t => t.Id == id);
        }

        // POST api/values
        [HttpPost]
        public virtual async Task Post([FromBody]T record)
        {
            // For more information on protecting this API from Cross Site Request Forgery (CSRF) attacks, see https://go.microsoft.com/fwlink/?LinkID=717803

            var table = _context.GetTable<T>();

            var recordToUpdate = table.SingleOrDefault(t => t.Id == record.Id);
            if (recordToUpdate != null)
                return;

            table.Add(record);
            await _context.SaveChangesAsync();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task Put(Guid id, [FromBody]T record)
        {
            var table = _context.GetTable<T>();

            T recordToUpdate = null;

            if (id != Guid.Empty)
                recordToUpdate = table.SingleOrDefault(t => t.Id == id);

            if (recordToUpdate == null)
            {
                record.Id = Guid.NewGuid();
                table.Add(record);
            }
            else
            {
                _context.Entry(recordToUpdate).CurrentValues.SetValues(record);
            }

            await _context.SaveChangesAsync();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            var records = _context.GetTable<T>();

            var recordToDelete = records.SingleOrDefault(t => t.Id == id);
            if (recordToDelete == null)
                return;
            records.Remove(recordToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
