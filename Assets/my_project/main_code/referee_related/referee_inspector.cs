using UnityEditor;
using UnityEngine;

namespace RefereeRelated
{
    [CustomEditor(typeof(Referee))]
    public class RefereeInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Referee referee = (Referee)target;

            if (referee != null)
            {
                foreach (var entry in referee.Robots)
                {
                    EditorGUILayout.LabelField($"ID: {entry.Key} Count: {entry.Value.Count}");
                }
            }
        }
    }
}