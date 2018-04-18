using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Store.data.Model;
using Store.data.dto;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace store_back_end.api
{
    [Route("[controller]/[action]")]
    [EnableCors("AllowAnyOrigin")]
    [AllowAnonymous]
    public class CategoryController : Controller
    {
        private StoreContext db;
        public CategoryController()
        {
            db = new StoreContext();
        }

        // Category/get
        [HttpGet]
        public async Task<IEnumerable<ProductCategories>> Get()
        {
            return await Task.Run(() => db.ProductCategories.ToList());
        }

        // Category/1
        [HttpGet("{id}")]
        public async Task<ActionResult> getById(int id)
        {
            try
            {

                if (id <= 0)
                    return BadRequest();

                var category = db.ProductCategories.Find(id);

                if (category == null)
                    return BadRequest();

                return await Task.Run(() => new ObjectResult(category));
            }
            catch 
            {
                 return await Task.Run(() => StatusCode(500));
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductCategorydto categorydto)
        {
            try
            {
                if (categorydto == null)
                    return BadRequest();
                var category = Mapper.Map(categorydto, new ProductCategories());
                db.ProductCategories.Add(category);
                db.SaveChanges();
                return await Task.Run(() => new ObjectResult(category));
            }
            catch
            {
                return await Task.Run(() => StatusCode(500));
            }
        }

        // Category/put/1
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProductCategorydto categorydto)
        {
            try
            {
                if (categorydto == null || id <= 0)
                    return BadRequest();

                var cat = db.ProductCategories.Find(id);
                var category = Mapper.Map(categorydto, cat);
                db.SaveChanges();
                return await Task.Run(() => new ObjectResult(category));
            }
            catch
            {
                return await Task.Run(() => StatusCode(500));
            }
        }

        // Category/Delete/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest();
                var cat = db.ProductCategories.Find(id);
                db.ProductCategories.Remove(cat);
                db.SaveChanges();
                return await Task.Run(() => new ObjectResult(cat));
            }
            catch
            {
                return await Task.Run(() => StatusCode(500));
            }
        }
    }
}
