public enum AmplifierType
{
    Plus, AdditivePercentage, TruePercentage
}
public class Amplifier<T> where T : Attribute
{
    public string uniqueTag;
    public T attribute;
    public AmplifierType amplifierType;
    public float value;
    public int stackCount;
}
