using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stats<T> : MonoBehaviour where T : Attribute
{
    protected readonly List<AttributeSelector<T>> attributes = new();
    protected AmplifierSystem<T> amplifierSystem;
    public delegate void StatValueChangeHandler();
    public StatValueChangeHandler onValueChanged;

    public bool TryGetAttributeValue(T attribute, out float value)
    {
        var result = amplifierSystem.TryGetBuffedAttributeValue(attribute, out value);
        return result;
    }

    public void RegisterAmplifiers(Amplifier<T> amplifier)
    {
        amplifierSystem.RegisterAmplifiers(amplifier);
    }
    public void RegisterAmplifiers(ICollection<Amplifier<T>> amplifiers)
    {
        amplifierSystem.RegisterAmplifiers(amplifiers);
        onValueChanged?.Invoke();
    }
    public void RegisterAmplifiers(ICollection<Amplifier<T>> amplifiers, float time)
    {
        amplifierSystem.RegisterAmplifiers(amplifiers);
        onValueChanged?.Invoke();
        StartCoroutine(UnRegisterAmplifiersByTime(amplifiers, time));
    }

    IEnumerator UnRegisterAmplifiersByTime(ICollection<Amplifier<T>> amplifiers, float time)
    {
        yield return new WaitForSeconds(time);
        UnRegisterAmplifiers(amplifiers);
    }
    public void UnRegisterAmplifiers(Amplifier<T> amplifier)
    {
        amplifierSystem.UnRegisterAmplifiers(amplifier);
    }
    public void UnRegisterAmplifiers(ICollection<Amplifier<T>> amplifiers)
    {
        amplifierSystem.UnRegisterAmplifiers(amplifiers);
        onValueChanged?.Invoke();
    }
}
