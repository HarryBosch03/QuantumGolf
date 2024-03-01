using UnityEngine;
using UnityEngine.InputSystem;

namespace GolfGame.Runtime.Player
{
    public class PlayerController : MonoBehaviour
    {
        public InputActionAsset inputAsset;
        
        [Space]
        public GolfBall golfBall;
        public Putter putter;

        private PlayerBallCam ballCam;
        
        private void Awake()
        {
            inputAsset = Instantiate(inputAsset);
            ballCam = GetComponent<PlayerBallCam>();
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
            var camInput = Input<Vector2>("RotateCamera") * Time.deltaTime;
            var zoomInput = Input<float>("Zoom");
            
            if (golfBall)
            {
                ballCam.MoveCamera(golfBall.Body.position, camInput, zoomInput);
            }

            if (putter)
            {
                var swing = Input<Vector2>("Swing");
                var ready = WasPressedThisFrame("Ready");

                putter.Visible = swing.magnitude > 0.1f || putter.IsReady;
                putter.PutterPosition = swing;
                if (ready) putter.IsReady = !putter.IsReady;
            }
        }

        public T Input<T>(string key, T fallback = default) where T : struct
        {
            var action = inputAsset.FindAction(key);
            return action?.ReadValue<T>() ?? fallback;
        }
        
        public bool IsPressed(string key, bool fallback = default)
        {
            var action = inputAsset.FindAction(key);
            return action?.IsPressed() ?? fallback;
        }
        
        public bool WasPressedThisFrame(string key, bool fallback = default)
        {
            var action = inputAsset.FindAction(key);
            return action?.WasPressedThisFrame() ?? fallback;
        }
    }
}
