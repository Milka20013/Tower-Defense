using System.Collections.Generic;
using System.Linq;


public class AmplifierSystem<T> where T : Attribute
{
    private readonly Dictionary<T, AmplifierValues<T>> attributeDatas = new();
    private readonly Dictionary<string, Amplifier<T>> amplifiers = new();
    private readonly Dictionary<Amplifier<T>, int> amplifierStacking = new();

    public AmplifierSystem(IEnumerable<T> attributes, IEnumerable<float> values)
    {
        for (int i = 0; i < attributes.Count(); i++)
        {
            attributeDatas.Add(attributes.ElementAt(i), new(values.ElementAt(i)));
        }
        RegisterAmplifiers(GlobalEffects.GetAmplifiersOfType<T>());
    }

    public void RegisterAmplifiers(Amplifier<T> amplifier)
    {
        if (amplifier == null)
        {
            return;
        }
        if (!attributeDatas.TryGetValue(amplifier.attribute, out _))
        {
            return;
        }
        if (amplifiers.TryAdd(amplifier.uniqueTag, amplifier))
        {
            amplifierStacking.TryAdd(amplifier, 0);
        }
        int currentStacking = amplifierStacking[amplifier];
        if (currentStacking < amplifier.stackCount)
        {
            amplifierStacking[amplifier] = amplifierStacking[amplifier] + 1;
        }
        CalculateAmplifierValues(amplifier.attribute);
    }
    public void RegisterAmplifiers(ICollection<Amplifier<T>> amplifiers)
    {
        if (amplifiers == null)
        {
            return;
        }
        foreach (var item in amplifiers)
        {
            RegisterAmplifiers(item);
        }
    }

    public void UnRegisterAmplifiers(Amplifier<T> amplifier)
    {
        if (amplifier == null)
        {
            return;
        }

        if (amplifiers.TryGetValue(amplifier.uniqueTag, out Amplifier<T> foundAmplifier))
        {
            amplifiers.Remove(amplifier.uniqueTag);
            int stacking = amplifierStacking[foundAmplifier];
            if (stacking > 0)
            {
                amplifierStacking[foundAmplifier] = amplifierStacking[foundAmplifier] - 1;
            }
            else
            {
                amplifierStacking.Remove(foundAmplifier);
            }
            CalculateAmplifierValues(amplifier.attribute);
        }
    }
    public void UnRegisterAmplifiers(ICollection<Amplifier<T>> amplifiers)
    {
        if (amplifiers == null)
        {
            return;
        }
        foreach (var item in amplifiers)
        {
            UnRegisterAmplifiers(item);
        }
    }

    private void CalculateAmplifierValues(T attribute)
    {
        attributeDatas[attribute].Reset();
        var amplifiersInContext = amplifiers.Where(x => x.Value.attribute == attribute).Select(x => x.Value);
        foreach (var amplifier in amplifiersInContext)
        {
            int stacking = amplifierStacking[amplifier];
            for (int i = 0; i < stacking + 1; i++)
            {
                attributeDatas[attribute].RegisterAmplifier(amplifier);
            }
        }
    }

    public bool TryGetBuffedAttributeValue(T attribute, out float value)
    {
        if (attributeDatas.TryGetValue(attribute, out AmplifierValues<T> values))
        {
            value = values.GetBuffedValue();
            return true;
        }
        value = -1;
        return false;
    }
}

public class AmplifierValues<T> where T : Attribute
{
    public float baseValue;
    public float additiveValue;
    public float additiveMultiplier;
    public float trueMuliplier;

    public AmplifierValues(float baseValue)
    {
        this.baseValue = baseValue;
        Reset();
    }

    public void Reset()
    {
        additiveValue = 0;
        additiveMultiplier = 1;
        trueMuliplier = 1;
    }
    public void RegisterAmplifier(Amplifier<T> amplifier)
    {
        switch (amplifier.amplifierType)
        {
            case AmplifierType.Plus:
                additiveValue += amplifier.value;
                break;
            case AmplifierType.AdditivePercentage:
                additiveMultiplier += amplifier.value / 100;
                break;
            case AmplifierType.TruePercentage:
                trueMuliplier *= 1 + amplifier.value / 100;
                break;
            default:
                break;
        }
    }
    public float GetBuffedValue()
    {
        return (baseValue + additiveValue) * additiveMultiplier * trueMuliplier;
    }
}
