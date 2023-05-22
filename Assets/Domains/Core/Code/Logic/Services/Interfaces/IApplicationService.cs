using Cysharp.Threading.Tasks;
using Migs.Asteroids.Core.Logic.Enums;
using Migs.Asteroids.Core.Logic.Utils;

namespace Migs.Asteroids.Core.Logic.Services.Interfaces
{
    public interface IApplicationService
    {
        ApplicationState State { get; }
        
        event ApplicationStateChangedDelegate ApplicationStateChanged;

        UniTask LoadDomain(SceneAssetReference domainReference);
    }
}