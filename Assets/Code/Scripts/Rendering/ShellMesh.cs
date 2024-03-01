using UnityEngine;

namespace GolfGame.Runtime.Rendering
{
    [ExecuteAlways]
    public sealed class ShellMesh : MonoBehaviour
    {
        public Mesh sharedMesh;
        public Material material;

        private void Update()
        {
            if (sharedMesh)
            {
                Graphics.DrawMesh(sharedMesh, transform.localToWorldMatrix, material, gameObject.layer);
            }
        }
    }
}