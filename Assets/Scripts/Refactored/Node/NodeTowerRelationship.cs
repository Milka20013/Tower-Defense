using System;
using System.Linq;


public enum Relation
{
    Must_be, Can_be, Cant_be
}
[Serializable]
public class NodeTowerRelationship
{
    public NodeAttribute attribute;
    public Relation relation;

    public static bool IsAcceptableTower(NodeTowerRelationship[] relations, NodeAttribute[] nodeAttributes)
    {
        var mustBeAttributes = relations.Where(x => x.relation == Relation.Must_be).Select(x => x.attribute).ToArray();
        var cantBeAttributes = relations.Where(x => x.relation == Relation.Cant_be).Select(x => x.attribute).ToArray();
        var canBeAttributes = relations.Where(x => x.relation == Relation.Can_be).Select(x => x.attribute).ToArray();
        foreach (var item in mustBeAttributes)
        {
            if (!nodeAttributes.Contains(item))
            {
                return false;
            }
        }
        foreach (var item in cantBeAttributes)
        {
            if (nodeAttributes.Contains(item))
            {
                return false;
            }
        }
        //if canBe attributes length is 0, we shouldn't change the result
        //if it wasn't 0, the foreach returns true if a match is found
        //else it didn't happen, so return false
        bool foundMatchingCanBe = canBeAttributes.Length == 0;
        foreach (var item in canBeAttributes)
        {
            if (nodeAttributes.Contains(item))
            {
                return true;
            }
        }
        return foundMatchingCanBe;

    }
}
