using System.Collections.Generic;

public static class GlobalEffects
{
    public static List<TowerAmplifier> towerAmplifiers = new();
    public static List<EnemyAmplifier> enemyAmplifiers = new();

    public static Amplifier<T>[] GetAmplifiersOfType<T>() where T : Attribute
    {
        if (typeof(T) == typeof(TowerAttribute))
        {
            return towerAmplifiers.ToArray() as Amplifier<T>[];
        }
        if (typeof(T) == typeof(EnemyAttribute))
        {
            return enemyAmplifiers.ToArray() as Amplifier<T>[];
        }
        return null;
    }
}
