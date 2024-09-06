using UnityEngine;

public class ShadowTower : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    private bool doMove = true;

    [SerializeField] private GameEventContainer eventContainer;
    [SerializeField] LayerMask gameTopLayer;

    private void Update()
    {
        if (!doMove)
        {
            return;
        }
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 125f, gameTopLayer))
        {
            transform.position = hitInfo.point;
        }
    }

    public void OnNodeHovered(object nodeObj)
    {
        Node node = (Node)nodeObj;
        if (node.placedObject != null)
        {
            return;
        }
        doMove = false;
        transform.position = node.GetBuildPosition();
    }

    public void OnNodeHoverExit(object _)
    {
        doMove = true;
    }

    public void OnTowerToBuildSelect(object towerBlueprint)
    {
        gameObject.SetActive(true);
        var towerBP = (TowerBlueprint)towerBlueprint;
        var meshFilter = towerBP.prefab.GetComponentInChildren<MeshFilter>();
        Mesh mesh = null;
        if (meshFilter != null)
        {
            mesh = meshFilter.sharedMesh;
        }
        if (mesh == null)
        {
            mesh = towerBP.prefab.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
        }
        eventContainer.onTowerMove.RaiseEvent(gameObject);
        SetMesh(mesh);
    }

    public void Deactivate(object _)
    {
        gameObject.SetActive(false);
    }
    private void SetMesh(Mesh mesh)
    {
        meshFilter.mesh = mesh;
    }



}
