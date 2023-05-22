using Cysharp.Threading.Tasks;

namespace Migs.Asteroids.Game.Logic.Interfaces
{
    public interface IAsyncInitializable
    {
        UniTask Init();
    }
}