using Microsoft.AspNetCore.Mvc;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.API.Extensions;
public static class ResultExtensions
{
    public static ActionResult ToActionResult(this Result result)
    {
        if (result is null)
        {
            return new ObjectResult(new { success = false, message = "Null result returned" })
            {
                StatusCode = 500
            };
        }

        if (result.IsSuccess)
        {
            return new ObjectResult(new
            {
                success = true,
                message = result.Message
            })
            {
                StatusCode = result.StatusCode
            };
        }

        return new ObjectResult(new
        {
            success = false,
            message = result.Message,
            statusCode = result.StatusCode
        })
        {
            StatusCode = result.StatusCode
        };
    }

    public static ActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result is null)
        {
            return new ObjectResult(new { success = false, message = "Null result returned" })
            {
                StatusCode = 500
            };
        }

        if (result.IsSuccess)
        {
            return new ObjectResult(new
            {
                success = true,
                message = result.Message,
                data = result.Value
            })
            {
                StatusCode = result.StatusCode
            };
        }

        return new ObjectResult(new
        {
            success = false,
            message = result.Message,
            statusCode = result.StatusCode
        })
        {
            StatusCode = result.StatusCode
        };
    }
}