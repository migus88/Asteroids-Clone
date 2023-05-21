using UnityEngine;

namespace Migs.Asteroids.Core.Logic.Utils
{
    public static class Extensions
    {
        /// <summary>
        /// This will only return the first layer set in the LayerMask.
        /// If the LayerMask includes multiple layers, this method might not behave as you expect.
        /// </summary>
        public static int ToLayer(this LayerMask layerMask)
        {
            var layerNumber = 0;
            var layer = layerMask.value;
            while(layer > 0)
            {
                layer >>= 1;
                layerNumber++;
            }
            return layerNumber - 1;
        }
    }
}