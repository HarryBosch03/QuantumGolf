using UnityEngine;

namespace GolfGame.Runtime.Player
{
    public class PlayerBallCam : MonoBehaviour
    {
        public Vector2 distanceClamp;
        public Vector2 pitchClamp;

        [Space]
        public float mouseSensitivity;
        public float gamepadSensitivity;
        public float zoomSensitivity;
        [Range(10.0f, 90.0f)]
        public float fieldOfView;
        public float maxVerticalOffset;

        private Camera cam;

        private Vector2 rotation;
        private float distancePercent;
        private Collider[] camColliderList;

        private void Awake()
        {
            cam = Camera.main;
            camColliderList = cam.GetComponentsInChildren<Collider>();
        }

        public void MoveCamera(Vector3 position, Vector2 panDelta, float zoomDelta)
        {
            rotation -= panDelta * gamepadSensitivity;
            rotation.y = Mathf.Clamp(rotation.y, pitchClamp.x, pitchClamp.y);

            distancePercent = Mathf.Clamp01(distancePercent - zoomDelta * zoomSensitivity * Time.deltaTime);

            transform.position = position;
            transform.rotation = Quaternion.Euler(0.0f, -rotation.x, 0.0f);
        }

        private void Update()
        {
            var orientation = Quaternion.Euler(rotation.y, -rotation.x, 0.0f);
            var distance = Mathf.Lerp(distanceClamp.x, distanceClamp.y, distancePercent);

            var offset = Vector3.up * Mathf.Abs(Mathf.Cos(rotation.y * Mathf.Deg2Rad)) * maxVerticalOffset;

            cam.transform.position = transform.position + offset + orientation * -Vector3.forward * distance;
            cam.transform.rotation = orientation;
            cam.fieldOfView = fieldOfView;

            CollideCamera();
        }

        private void CollideCamera()
        {
            foreach (var collider in camColliderList)
            {
                var bounds = collider.bounds;
                var others = Physics.OverlapBox(bounds.center, bounds.extents, Quaternion.identity, 0b1);
                foreach (var other in others)
                {
                    if (other.transform.IsChildOf(cam.transform)) continue;
                    
                    if (!Physics.ComputePenetration
                        (
                            collider,
                            collider.transform.position,
                            collider.transform.rotation,
                            other,
                            other.transform.position,
                            other.transform.rotation,
                            out var normal,
                            out var distance
                        )) continue;

                    cam.transform.position += normal * distance;
                }
            }
        }
    }
}