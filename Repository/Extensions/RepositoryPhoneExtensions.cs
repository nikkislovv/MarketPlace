using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Entities.Models;

namespace ApiApplication.Repository.RepositoryPhoneExtensions
{
    public static class RepositoryPhoneExtensions
    {
        public static IQueryable<Product> FilterProducts(this IQueryable<Product> products, uint minPrice, uint maxPrice) =>
        products.Where(e => (e.Price >= minPrice && e.Price <= maxPrice));
    }
}
