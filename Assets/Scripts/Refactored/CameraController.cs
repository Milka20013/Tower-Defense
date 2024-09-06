using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool doMovement = false;
    public float panSpeed = 30f;
    public float panBorderThickness = 20f;
    public float scrollSpeed = 20f;
    public float maxY = 80f;
    public float minY = 10f;
    public float[] positionRestrictions = new float[4];
    public float offset = 0.002f;
    void Update()
    {
        /*if (GameMaster.gameIsOver)
        {
            this.enabled = false;
            return;
        }*/
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            doMovement = !doMovement;
        }
        if (!doMovement)
        {
            return;
        }
        if ((Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panBorderThickness) && transform.position.z <= positionRestrictions[0])
        {
            transform.Translate(offset * panSpeed * Vector3.forward, Space.World);
        }
        if ((Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorderThickness) && transform.position.z >= positionRestrictions[1])
        {
            transform.Translate(offset * panSpeed * Vector3.back, Space.World);
        }
        if ((Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorderThickness) && transform.position.x <= positionRestrictions[2])
        {
            transform.Translate(offset * panSpeed * Vector3.right, Space.World);
        }
        if ((Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorderThickness) && transform.position.x >= positionRestrictions[3])
        {
            transform.Translate(offset * panSpeed * Vector3.left, Space.World);
        }
        float Scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 Pos = transform.position;
        Pos.y -= Scroll * 500 * scrollSpeed * offset;
        Pos.y = Mathf.Clamp(Pos.y, minY, maxY);
        transform.position = Pos;

    }
}
