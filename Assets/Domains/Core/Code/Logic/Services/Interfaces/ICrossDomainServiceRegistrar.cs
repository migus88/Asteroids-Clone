using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Migs.Asteroids.Core.Logic.Services.Interfaces
{
    public interface ICrossDomainServiceRegistrar
    {
        void RegisterService<TInterface, TType>(IContainerBuilder builder, LifetimeScope scope) where TType : TInterface;
        void RegisterComponentService<TInterface, TType>(IContainerBuilder builder, LifetimeScope scope,
            TType monoBehaviour) where TType : MonoBehaviour, TInterface;
        void UnRegisterService<TInterface>();
    }
}