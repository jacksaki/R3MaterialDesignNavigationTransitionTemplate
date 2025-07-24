using Microsoft.Extensions.DependencyInjection;

namespace R3MaterialDesignNavigationTransitionTemplate.Services;

public class LazyResolver<T> : Lazy<T>
    where T : class
{
    public LazyResolver(IServiceProvider provider)
        : base(() => provider.GetRequiredService<T>())
    { }
}
