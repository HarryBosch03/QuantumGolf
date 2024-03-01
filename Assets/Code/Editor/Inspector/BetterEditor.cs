using UnityEditor;

namespace GolfGame.Editor.Inspector
{
    public class BetterEditor<T> : UnityEditor.Editor where T : UnityEngine.Object
    {
        public T Target => target as T;

        public bool Foldout(string name, bool defaultValue = false)
        {
            var key = $"BetterEditor.{GetType()}.{name}.foldout";
            var val = EditorPrefs.GetBool(key, defaultValue);
            val = EditorGUILayout.Foldout(val, name, true);
            EditorPrefs.SetBool(key, val);
            return val;
        }
    }
}