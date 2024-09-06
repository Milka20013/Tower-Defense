using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave/Wave")]
public class Wave : ScriptableObject
{
    [Serializable]
    public struct SubWave
    {
        public float delayStart;
        public int numberOfEnemies;
        public float spacing;
        public EnemyBlueprint enemyBp;
    }
    public Wave simulWave;
    public SubWave[] subWaves;

    public static List<Wave> GetAllSimulWaves(Wave wave)
    {
        HashSet<Wave> simulWaves = new();
        Wave currentWave = wave;
        while (currentWave.HasSimulWave())
        {
            if (!simulWaves.Add(currentWave.simulWave))
            {
                throw new Exception("Waves had a circular reference problem. " + wave.name + " references " + wave.simulWave.name);
            }
            currentWave = currentWave.simulWave;
        }
        return simulWaves.ToList();
    }

    public bool HasSimulWave()
    {
        return simulWave != null;
    }
}
