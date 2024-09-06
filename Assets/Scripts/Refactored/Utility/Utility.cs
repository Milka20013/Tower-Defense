
using System.Collections.Generic;
using System.Linq;

public class Utility
{
    public static System.Random rnd = new();
    public static int Mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }

    public static T RandomElement<T>(ICollection<T> collection)
    {
        int number = rnd.Next(collection.Count);
        return collection.ElementAt(number);
    }

    public static bool RandomTrue(decimal chance, bool percentage = false)
    {
        if (percentage)
        {
            chance /= 100;
        }
        if (chance <= 0m)
        {
            return false;
        }
        if (chance >= 1m)
        {
            return true;
        }
        byte[] bytes = new byte[8];
        rnd.NextBytes(bytes);
        ulong number = System.BitConverter.ToUInt64(bytes, 0);
        ulong shiftedChance = (ulong)(chance * ulong.MaxValue);
        return number <= shiftedChance;
    }
    public static bool RandomTrue(float chance, bool percentage = false)
    {
        return RandomTrue((decimal)chance, percentage);
    }
}
