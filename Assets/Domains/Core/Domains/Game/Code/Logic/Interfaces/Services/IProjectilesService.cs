using Cysharp.Threading.Tasks;
using Migs.Asteroids.Game.Logic.Interfaces.Entities;

namespace Migs.Asteroids.Game.Logic.Interfaces.Services
{
    public interface IProjectilesService : IPoolableObjectLoader<IProjectile>
    {
        IProjectile GetAvailablePlayerProjectile(int magazineSize);
        void ReturnPlayerProjectile(IProjectile projectile);
        
        IProjectile GetAvailableEnemyProjectile();
        void ReturnEnemyProjectile(IProjectile projectile);
    }
}