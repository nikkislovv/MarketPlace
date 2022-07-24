using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace Server.ActionFilters
{
    public class ValidateDeliveryPointExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        public ValidateDeliveryPointExistsAttribute(IRepositoryManager repository,
       ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
       ActionExecutionDelegate next)//делегат который возвращает ActionAxecutedContext послезавершения работы фильтра
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT");
            var id = (Guid)context.ActionArguments["id"];
            var deliveryPoint = await _repository.DeliveryPoint.GetDeliveryPointByIdAsync(id, trackChanges);
            if (deliveryPoint == null)
            {
                _logger.LogInfo($"DeliveryPoint with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("deliveryPoint", deliveryPoint);
                await next();
            }
        }
    }
}
