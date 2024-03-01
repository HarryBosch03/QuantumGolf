using System;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GolfGame.Runtime.Player
{
    public class Putter : MonoBehaviour
    {
        public Color color = Color.white;
        
        [FormerlySerializedAs("swingAngle")] [Space]
        public float swingRadius = 1.0f;
        public float spring = 100.0f;
        public float damping = 30.0f;
        public float endOffset;
        public float heightOffset;
        
        [Space]
        public float liftDistance = 0.3f;
        public float animationSpeed = 1.0f;
        public AnimationCurve animationCurve = AnimationCurve.Linear(0, 0, 1, 1);

        private Vector3 position;
        private Vector3 velocity;
        private int zeroInput;
        
        private new MeshRenderer renderer;
        private Transform end;

        private float animationTime;

        public bool Visible { get; set; }
        public Vector2 PutterPosition { get; set; }
        public bool IsReady { get; set; }
        
        private static readonly int ColorProp = Shader.PropertyToID("_Color");

        private void Awake()
        {
            renderer = GetComponentInChildren<MeshRenderer>();
            end = transform.Find("Model/Shaft.End");
        }

        private void Update()
        {
            UpdatePutterColor();
        }

        private void FixedUpdate()
        {
            if (PutterPosition.magnitude <= float.Epsilon)
            {
                zeroInput++;
            }
            
            if (IsReady && zeroInput > 3)
            {
                IsReady = false;
            }
            
            UpdatePose();
            
            animationTime = Mathf.MoveTowards(animationTime, Visible ? 1 : 0, animationSpeed * Time.deltaTime);
            transform.localScale = Vector3.one * animationCurve.Evaluate(animationTime);
        }

        private void UpdatePose()
        {
            var target = new Vector3(PutterPosition.x, IsReady ? 0.0f : liftDistance, PutterPosition.y * swingRadius);
            
            var force = (target - position) * spring - velocity * damping;
            position += velocity * Time.deltaTime;
            velocity += force * Time.deltaTime;
            
            var end0 = end.localPosition + end.localRotation * Vector3.up * endOffset;
            var vec = (end.localRotation * Vector3.up).normalized;
            var tLength = Vector3.Dot(vec, end0);
            var a0 = Mathf.Atan(-position.z / tLength);
            var a1 = Mathf.Atan(position.x / tLength);
            var rotation = Quaternion.Euler(end.localRotation * (Vector3.right * a0 * Mathf.Rad2Deg + Vector3.forward * a1 * Mathf.Rad2Deg));
            var end1 = rotation * end0;
            var diff = end0 - end1;
            
            var twist = 
            
            transform.localRotation = Quaternion.identity;
            transform.localPosition = diff + Vector3.up * (position.y + heightOffset);
            transform.localRotation = rotation;
        }

        private void UpdatePutterColor()
        {
            var propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetColor(ColorProp, color);

            for (var i = 0; i < renderer.sharedMaterials.Length; i++)
            {
                renderer.SetPropertyBlock(i == 0 ? propertyBlock : new MaterialPropertyBlock(), i);
            }
        }

        private void OnValidate()
        {
            renderer = GetComponentInChildren<MeshRenderer>();
            UpdatePutterColor();
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            Gizmos.color = new Color(1f, 0.69f, 0.09f);
            Handles.DrawWireArc(transform.parent ? transform.parent.position : transform.position, Vector3.up, Vector3.forward, 360.0f, swingRadius);
#endif
        }
    }
}