using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Entities.Models;

namespace ApiApplication.Repository.RepositoryPhoneExtensions
{
    public static class RepositoryProductExtensions
    {
        public static IQueryable<Product> FilterProducts(this IQueryable<Product> products, uint minPrice, uint maxPrice) =>
        products.Where(e => (e.Price >= minPrice && e.Price <= maxPrice));

        public static IQueryable<Product> Search(this IQueryable<Product> products, string searchByField, string searchTerm)//только для warehouseid userid
        {
            if (string.IsNullOrWhiteSpace(searchTerm)|| string.IsNullOrWhiteSpace(searchByField))
                return products;
            var propertyInfos = typeof(Product).GetProperties(BindingFlags.Public | BindingFlags.Instance);//получаем все поля через рефлексию
            var objectProperty = propertyInfos.FirstOrDefault(pi =>//сверяем searchByField со всеми полями
                pi.Name.Equals(searchByField, StringComparison.InvariantCultureIgnoreCase));
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            var loverCaseObjectProperty = objectProperty.Name.Trim().ToLower();//получаем objectProperty.Name(это наше поле для поиска)
            if (string.IsNullOrWhiteSpace(loverCaseObjectProperty))
                return products;
            else if (loverCaseObjectProperty == "warehouseid")
            {
                return products.Where(e => e.WarehouseId.ToString().ToLower().Contains(lowerCaseTerm));
            }
            else if (loverCaseObjectProperty=="userid")
            {
                return products.Where(e => e.UserId.ToLower().Contains(lowerCaseTerm));
            }
            return products;
        }
    }
}
