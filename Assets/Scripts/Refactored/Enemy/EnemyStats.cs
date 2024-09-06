using System.Linq;

public class EnemyStats : Stats<EnemyAttribute>
{
    private void Awake()
    {
        var blueprint = GetComponent<Enemy>().blueprint;

        attributes.AddRange(blueprint.attributes);

        amplifierSystem = new(attributes.Select(x => x.attribute), attributes.Select(x => x.value));

    }
}
