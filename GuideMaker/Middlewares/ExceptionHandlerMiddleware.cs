using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GuideMaker.Core.Models;
using GuideMaker.Exceptions;
using GuideMaker.Repository.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace GuideMaker.Middlewares
{
    internal sealed class ExceptionHandlerMiddleware: IActionFilter, IOrderedFilter
    {
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context)
        { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is ApiException apiException)
            {
                var result = Result<object>.Error(apiException.Message);
                var resultJson = JsonConvert.SerializeObject(result);
                context.Result = new ObjectResult(resultJson)
                {
                    StatusCode = 200,
                };
                context.ExceptionHandled = true;
            }
            else if (context.Exception is EntityNotFoundException entityNotFoundException)
            {
                var result = Result<object>.Error(entityNotFoundException.Message);
                var resultJson = JsonConvert.SerializeObject(result);
                context.Result = new ObjectResult(resultJson)
                {
                    StatusCode = 200,
                };
                context.ExceptionHandled = true;

            }
            else if (context.Exception is { } e)
            {
                var result = Result<object>.Error(e.Message);
                var resultJson = JsonConvert.SerializeObject(result);
                context.Result = new ObjectResult(resultJson)
                {
                    StatusCode = 200,
                };
                context.ExceptionHandled = true;
            }
        }

        
    }
}
