using System;
using UnityEditor;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Migs.Asteroids.Core.Logic.Utils
{
    [Serializable]
    public class SceneAssetReference : AssetReference
    {
#if UNITY_EDITOR
        public new SceneAsset editorAsset => (SceneAsset)base.editorAsset;
#endif
        
        public SceneAssetReference(string guid) : base(guid) { }

        public override bool ValidateAsset(Object obj)
        {
#if UNITY_EDITOR
            return obj is SceneAsset;
#else
            return false;
#endif
        }

        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            var assetTypeAtPath = AssetDatabase.GetMainAssetTypeAtPath(path);
            return typeof(SceneAsset).IsAssignableFrom(assetTypeAtPath);
#else
            return false;
#endif
        }
    }
}