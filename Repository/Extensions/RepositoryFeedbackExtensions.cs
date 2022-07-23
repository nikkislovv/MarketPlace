using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions
{
    public static class RepositoryFeedbackExtensions
    {
        public static IQueryable<Feedback> Search(this IQueryable<Feedback> feedbacks, string searchByField, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || string.IsNullOrWhiteSpace(searchByField))
                return feedbacks;
            var propertyInfos = typeof(Feedback).GetProperties(BindingFlags.Public | BindingFlags.Instance);//получаем все поля через рефлексию
            var objectProperty = propertyInfos.FirstOrDefault(pi =>//сверяем searchByField со всеми полями
                pi.Name.Equals(searchByField, StringComparison.InvariantCultureIgnoreCase));
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            var loverCaseObjectProperty = objectProperty.Name.Trim().ToLower();//получаем objectProperty.Name(это наше поле для поиска)
            if (string.IsNullOrWhiteSpace(loverCaseObjectProperty))
                return feedbacks;
            else if (loverCaseObjectProperty == "userid")
            {
                return feedbacks.Where(e => e.UserId.ToLower().Contains(lowerCaseTerm));
            }
            return feedbacks;
        }
    }
}
