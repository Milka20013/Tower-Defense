using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Wave))]
public class WaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var waveTarget = (Wave)target;
        var everyWave = Wave.GetAllSimulWaves(waveTarget);
        everyWave.Add(waveTarget);
        float result = 0f;
        foreach (var wave in everyWave)
        {
            if (wave.subWaves == null)
            {
                continue;
            }
            foreach (var subWave in wave.subWaves)
            {
                if (subWave.enemyBp == null)
                {
                    continue;
                }
                result += subWave.enemyBp.GetRelativeStrength() * subWave.numberOfEnemies / (subWave.spacing / 4.2f + 1) * (Mathf.Sqrt(everyWave.Count - 0.25f) + 0.25f);
            }
        }
        EditorGUILayout.HelpBox("Strength of the wave is : " + result.ToString(), MessageType.Info);
    }
}
