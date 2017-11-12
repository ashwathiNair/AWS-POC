using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebAPIDemo.Sample.Model;

namespace WebAPIDemo.Sample.Controllers
{
    public class ProductsController: ApiController
    {
        public List<Product> _products = new List<Product>() {
                new Product {Id=1, Name="Product1", Category="Electronics", Price=25.50M },
                new Product {Id=2, Name="Product2", Category="Food", Price=25.00M },
                new Product {Id=3, Name="Product3", Category="Cosm", Price=24.50M },
                new Product {Id=4, Name="Product4", Category="Decor", Price=28.50M },
                new Product {Id=5, Name="Product5", Category="Stationary", Price=15.50M }
            };
        
        public List<Product> GetAllProducts()
        {
            return _products;
        }

        public IHttpActionResult GetProductById(int id)
        {
            var product = _products.FirstOrDefault(prod => prod.Id == id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

    }
}