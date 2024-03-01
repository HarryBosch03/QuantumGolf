using GolfGame.Runtime.Player;
using UnityEditor;
using UnityEngine;

namespace GolfGame.Editor.Inspector
{
    [CustomEditor(typeof(GolfBall))]
    public sealed class GolfBallEditor : BetterEditor<GolfBall>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Foldout("Info"))
            {
                if (EditorApplication.isPlaying)
                {
                    using (new EditorGUI.DisabledScope(true))
                    {
                        var body = Target.Body ? Target.Body : Target.GetComponent<Rigidbody>();
                        var speed = body.velocity.magnitude;
                        EditorGUILayout.FloatField("Speed [m/s]", speed);
                        EditorGUILayout.FloatField("Speed [Km/h]", speed * 3.6f);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Info is not available in edit mode", MessageType.Info);
                }
            }
        }
    }
}