using System;
using UnityEngine;

namespace GolfGame.Player
{
    public class PlayerBallCam : MonoBehaviour
    {
        public Vector2 distanceClamp;
        public Vector2 pitchClamp;
        public float collisionCheckRadius = 0.05f;
        
        [Space]
        public float mouseSensitivity;
        public float gamepadSensitivity;
        public float zoomSensitivity;
        [Range(10.0f, 90.0f)]
        public float fieldOfView;
        public float maxVerticalOffset;

        private PlayerController player;
        private Camera cam;

        private Vector2 rotation;
        private float distancePercent;
        
        public Rigidbody GolfBall => player.golfBall;
        
        private void Awake()
        {
            player = GetComponentInParent<PlayerController>();
            cam = Camera.main;
        }

        private void Update()
        {
            if (!GolfBall) return;
            
            rotation += player.CameraRotation * gamepadSensitivity;
            rotation.y = Mathf.Clamp(rotation.y, pitchClamp.x, pitchClamp.y);

            distancePercent = Mathf.Clamp01(distancePercent - player.ZoomInput * zoomSensitivity * Time.deltaTime);

            var orientation = Quaternion.Euler(rotation.y, -rotation.x, 0.0f);
            var distance = Mathf.Lerp(distanceClamp.x, distanceClamp.y, distancePercent);

            var offset = Vector3.up * Mathf.Abs(Mathf.Cos(rotation.y * Mathf.Deg2Rad)) * maxVerticalOffset;

            cam.transform.position = GolfBall.position + offset + orientation * -Vector3.forward * distance;
            cam.transform.rotation = orientation;
            cam.fieldOfView = fieldOfView;

            CollideCamera();
        }

        private void CollideCamera()
        {
            var start = GolfBall.position;
            var end = cam.transform.position;
            var distance = (end - start).magnitude;
            var direction = (end - start) / distance;

            if (Physics.SphereCast(new Ray(start, direction), collisionCheckRadius, out var hit, distance))
            {
                cam.transform.position = hit.point + hit.normal * collisionCheckRadius;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green; 
            Gizmos.DrawWireSphere(Camera.main.transform.position, collisionCheckRadius);
        }
    }
}