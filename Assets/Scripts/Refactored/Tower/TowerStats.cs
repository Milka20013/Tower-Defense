using System.Linq;
public class TowerStats : Stats<TowerAttribute>
{
    private void Awake()
    {
        var blueprint = GetComponent<Tower>().blueprint;

        attributes.AddRange(blueprint.attributes);

        amplifierSystem = new(attributes.Select(x => x.attribute), attributes.Select(x => x.value));

    }
}
