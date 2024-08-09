using LibrarySystem.Domain.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibrarySystem.Presentation.Middlewares.Filters;

/// <summary>
/// An action filter that is executed after an endpoint response, if the data returned is a IDtoSerializable implementation we call the ToDto() to serialize the data
/// </summary>
public class CustomDtoSerializationFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var objectResult = context.Result as ObjectResult;

        if (objectResult?.Value is IDtoSerializable serializableObject) // one entity
        {
            objectResult.Value = serializableObject.ToDto();
        }
        else if (objectResult?.Value is IEnumerable<IDtoSerializable> serializableObjectCollection) // an list, array, ... of entities
        {
            objectResult.Value = serializableObjectCollection.Select(item => item.ToDto());
        }
        
        await next();
    }
}
