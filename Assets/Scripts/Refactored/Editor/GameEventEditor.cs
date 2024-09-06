using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GameEvent gameEvent = (GameEvent)target;
        if (GUILayout.Button("Show Listeners"))
        {
            var listeners = gameEvent.GetListenersList();
            foreach (var item in listeners)
            {
                Debug.Log(item.name);
            }
        }
    }
}
