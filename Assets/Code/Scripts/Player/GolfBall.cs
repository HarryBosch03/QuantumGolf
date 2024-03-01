using System;
using GolfGame.Runtime.Kinematics;
using UnityEngine;

namespace GolfGame.Runtime.Player
{
    [SelectionBase]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public sealed class GolfBall : MonoBehaviour
    {
        private const float SkinWidth = 0.02f;
        public float baseResistance;
        
        private new SphereCollider collider;
        
        public Rigidbody Body { get; private set; }

        private void Awake()
        {
            Body = GetComponent<Rigidbody>();
            collider = GetComponent<SphereCollider>();
        }

        private void FixedUpdate()
        {
            var radius = collider.radius;
            var touching = Physics.OverlapSphere(Body.position, radius + SkinWidth, ~0, QueryTriggerInteraction.Ignore);
            var resistance = baseResistance;
            
            foreach (var e in touching)
            {
                if (e.transform.IsChildOf(transform)) continue;
                var surface = e.GetComponentInParent<SurfaceProperties>();
                if (!surface) continue;

                resistance += surface.resistance;
            }

            ApplyResistanceForce(resistance);
        }

        private void ApplyResistanceForce(float resistance)
        {
            var speed = Body.velocity.magnitude;
            var squaredSpeed = speed * speed;
            var direction = Body.velocity.normalized;
            var momentum = speed * Body.mass;

            var force = -direction * squaredSpeed * resistance;
            var dv = force / Time.deltaTime;
            dv = Vector3.ClampMagnitude(dv, momentum);
            force = dv * Time.deltaTime;
            
            Body.AddForce(force);
        }
    }
}
