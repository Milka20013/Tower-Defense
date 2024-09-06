using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner instance;
    [HideInInspector] public int currentRound;
    [SerializeField] private int roundsToWin = 20;
    [SerializeField] private int endOfRoundMoney = 35;
    [SerializeField] private bool autostart = true;
    [SerializeField] private int startRound = 0;
    private int enemyCount = 0;
    private bool roundOver = true;

    [SerializeField] private WavesBuilder wavesBuilder;
    private int waveIndex = 0;

    [SerializeField] GameEventContainer eventContainer;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        currentRound = startRound;
        waveIndex = startRound;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (currentRound == startRound)
            {
                BeginRound();
                return;
            }
            if (autostart)
            {
                return;
            }
            if (roundOver)
            {
                BeginRound();
            }
        }
    }
    private void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, Path.instance.GetStartPoint(), Path.instance.GetStartRotation());
    }

    public void OnEnemyDeath(object _)
    {
        enemyCount--;
        if (enemyCount <= 0)
        {
            EndRound();
        }
    }
    private void BeginRound()
    {
        if (currentRound >= roundsToWin)
        {
            return;
        }
        roundOver = false;
        if (currentRound == startRound)
        {
            eventContainer.onGameStart.RaiseEvent();
        }
        enemyCount = 0;
        currentRound++;
        eventContainer.onRoundStarted.RaiseEvent(currentRound);
        SpawnRound();
    }

    private void EndRound()
    {
        roundOver = true;
        eventContainer.onRoundEnded.RaiseEvent(currentRound);
        PlayerStats.instance.AddMoney(endOfRoundMoney);
        if (currentRound == roundsToWin)
        {
            eventContainer.onGamePseudoWon.RaiseEvent(currentRound);
            return;
        }
        if (autostart)
        {
            BeginRound();
        }
    }
    private void SpawnRound()
    {
        var waves = wavesBuilder.GetWavesToSpawnAtOnce(ref waveIndex);
        if (waves == null || waves.Count == 0)
        {
            eventContainer.onGamePseudoWon.RaiseEvent(currentRound);
            return;
        }
        SetSpawningDuration(waves);
        for (int i = 0; i < waves.Count; i++)
        {
            StartCoroutine(SpawnWave(waves[i]));
        }
    }

    private void SetSpawningDuration(List<Wave> waves)
    {
        var spawningDuration = waves.Max(x => x.subWaves.Sum(y => y.delayStart) + x.subWaves.Sum(y => y.spacing * y.numberOfEnemies));
        if (spawningDuration <= 1f)
        {
            spawningDuration = 1.5f;
        }
        eventContainer.onSpawningStarted.RaiseEvent(spawningDuration);
    }
    IEnumerator SpawnWave(Wave wave)
    {
        enemyCount += wave.subWaves.Sum(x => x.numberOfEnemies);
        foreach (var subWave in wave.subWaves)
        {

            yield return new WaitForSeconds(subWave.delayStart);
            for (int j = 0; j < subWave.numberOfEnemies; j++)
            {
                SpawnEnemy(subWave.enemyBp.prefab);
                yield return new WaitForSeconds(subWave.spacing);
            }
        }
    }

    public void SkipRound()
    {
        StopAllCoroutines();
        var colliders = Physics.OverlapSphere(transform.position, 100f);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.OnHit(9999f);
            }
        }
        if (enemyCount > 0)
        {
            EndRound();
        }
    }

    public void RetryRound()
    {
        waveIndex--;
        SkipRound();
        currentRound--;
    }

    public void PreviousRound()
    {
        waveIndex -= 2;
        SkipRound();
        currentRound -= 2;
    }
}

