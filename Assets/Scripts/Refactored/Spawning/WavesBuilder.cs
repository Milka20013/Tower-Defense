using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave/WaveBuilder")]
public class WavesBuilder : ScriptableObject
{
    public Wave[] waves;

    public List<Wave> GetWavesToSpawnAtOnce(ref int waveIndex)
    {
        if (waveIndex == waves.Length)
        {
            return null;
        }
        List<Wave> resultWaves = new()
        {
            waves[waveIndex]
        };
        var simulWaves = Wave.GetAllSimulWaves(waves[waveIndex]);
        resultWaves.AddRange(simulWaves);

        waveIndex++;
        return resultWaves;
    }
}
