using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Migs.Asteroids.Game.Logic.Interfaces.Services
{
    public interface IPoolableObjectLoader<TObject>
    {
        UniTask Preload(int amount = 0);
        TObject GetObject();
        void ReturnObject(TObject obj);
    }
}