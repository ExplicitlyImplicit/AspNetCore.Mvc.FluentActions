using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.Utils
{
    public class NoOpBinder : IModelBinder
    {
        public static readonly IModelBinder Instance = new NoOpBinder();

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            return Task.CompletedTask;
        }
    }
}
