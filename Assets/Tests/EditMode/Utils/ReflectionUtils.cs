using System.Reflection;

namespace Tests.EditMode.Utils
{
    public static class ReflectionUtils
    {
        public static TValue GetPrivateFieldValue<TObject, TValue>(string fieldName, TObject instance)
        {
            var field = typeof(TObject).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (TValue)field?.GetValue(instance);
        } 
        
        public static void SetPrivateFieldValue<TObject, TValue>(string fieldName, TValue value, TObject instance)
        {
            var field = typeof(TObject).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(instance, value);
        } 
    }
}