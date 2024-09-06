
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(WaveSpawner))]
public class WaveSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        WaveSpawner waveSpawner = (WaveSpawner)target;
        if (GUILayout.Button("Skip Round"))
        {
            waveSpawner.SkipRound();
        }
        if (GUILayout.Button("Retry Round"))
        {
            waveSpawner.RetryRound();
        }
        if (GUILayout.Button("Previous Round"))
        {
            waveSpawner.PreviousRound();
        }
    }
}
