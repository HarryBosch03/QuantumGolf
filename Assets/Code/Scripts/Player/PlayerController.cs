using UnityEngine;
using UnityEngine.InputSystem;

namespace GolfGame.Player
{
    public class PlayerController : MonoBehaviour
    {
        public InputActionAsset inputAsset;
        
        [Space]
        public Rigidbody golfBall;
        
        public Vector2 CameraRotation { get; private set; }
        public float ZoomInput { get; private set; }

        private void Awake()
        {
            inputAsset = Instantiate(inputAsset);
        }

        private void OnEnable()
        {
            inputAsset.Enable();
        }

        private void OnDisable()
        {
            inputAsset.Disable();
        }

        private void OnDestroy()
        {
            Destroy(inputAsset);
        }

        private void Update()
        {
            CameraRotation = inputAsset.FindAction("RotateCamera").ReadValue<Vector2>() * Time.deltaTime;
            ZoomInput = inputAsset.FindAction("Zoom").ReadValue<float>();
        }
    }
}
