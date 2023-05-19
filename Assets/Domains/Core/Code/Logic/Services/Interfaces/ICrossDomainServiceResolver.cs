namespace Migs.Asteroids.Core.Logic.Services.Interfaces
{
    public interface ICrossDomainServiceResolver
    {
        TInterface ResolveService<TInterface>();
    }
}