using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace Server.ActionFilters
{
    public class ValidateOrderExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        public ValidateOrderExistsAttribute(IRepositoryManager repository,
       ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
       ActionExecutionDelegate next)//делегат который возвращает ActionAxecutedContext послезавершения работы фильтра
        {
            var trackChanges = true;//тк много где требует lazy loading
            var id = (Guid)context.ActionArguments["id"];
            var order = await _repository.Order.GetOrderByIdAsync(id, trackChanges);
            if (order == null)
            {
                _logger.LogInfo($"Order with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("order", order);
                await next();
            }
        }
    }
}
