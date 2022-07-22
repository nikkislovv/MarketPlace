using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Entities.Models;

namespace ApiApplication.Repository.RepositoryPhoneExtensions
{
    public static class RepositoryOrderExtensions
    {
        public static IQueryable<Order> Search(this IQueryable<Order> orders, string searchByField, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)|| string.IsNullOrWhiteSpace(searchByField))
                return orders;
            var propertyInfos = typeof(Order).GetProperties(BindingFlags.Public | BindingFlags.Instance);//получаем все поля через рефлексию
            var objectProperty = propertyInfos.FirstOrDefault(pi =>//сверяем searchByField со всеми полями
                pi.Name.Equals(searchByField, StringComparison.InvariantCultureIgnoreCase));
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            var loverCaseObjectProperty = objectProperty.Name.Trim().ToLower();//получаем objectProperty.Name(это наше поле для поиска)
            if (string.IsNullOrWhiteSpace(loverCaseObjectProperty))
                return orders;
            else if (loverCaseObjectProperty == "deliverypointid")
            {
                return orders.Where(e => e.DeliveryPointId.ToString().ToLower().Contains(lowerCaseTerm));
            }
            else if (loverCaseObjectProperty=="userid")
            {
                return orders.Where(e => e.UserId.ToLower().Contains(lowerCaseTerm));
            }
            return orders;
        }
    }
}
