using System;
using UnityEngine;

namespace GolfGame.Runtime.Rendering
{
    [ExecuteAlways]
    public sealed class ShellMesh : MonoBehaviour
    {
        public Mesh mesh;
        public Material material;
        public int submeshIndex;
        public int shells;
        public float distance;
        public string debug;

        private void LateUpdate()
        {
            if (!mesh) return;

            var matrices = new Matrix4x4[shells];
            for (var i = 0; i < shells; i++)
            {
                matrices[i] = transform.localToWorldMatrix;
            }
            
            Shader.SetGlobalFloat("_ShellSpacing", distance / shells);
            Shader.SetGlobalInt("_Shells", shells);
            Graphics.DrawMeshInstanced(mesh, submeshIndex, material, matrices);
        }

        private void OnValidate()
        {
            shells = Mathf.Max(1, shells);
            if (mesh)
            {
                submeshIndex = Mathf.Clamp(submeshIndex, 0, mesh.subMeshCount - 1);
            }
            material.enableInstancing = true;
        }
    }
}