using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public GameObject plane;
    public int cubeSide = 2;
    public bool uClicked = false;
    public bool dClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        Camera.main.farClipPlane = 10000;
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position += transform.forward * Input.mouseScrollDelta.y * 5;

        if (Input.GetKeyDown(KeyCode.W))
        {
            uClicked = true;
        }
        if (Input.GetKeyUp(KeyCode.W) && uClicked)
        {
            plane.transform.position = new Vector3(plane.transform.position.x, plane.transform.position.y + cubeSide, plane.transform.position.z);
            uClicked = false;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            dClicked = true;
        }
        if (Input.GetKeyUp(KeyCode.S) && dClicked)
        {
            plane.transform.position = new Vector3(plane.transform.position.x, plane.transform.position.y - cubeSide, plane.transform.position.z);
            dClicked = false;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            gameObject.transform.Rotate(new Vector3(1, 0, 0), Space.Self);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            gameObject.transform.Rotate(new Vector3(-1, 0, 0), Space.Self);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            gameObject.transform.Rotate(new Vector3(0, 1, 0), Space.World);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            gameObject.transform.Rotate(new Vector3(0, -1, 0), Space.World);
        }

        //Vector3 boxV = new Vector3(GameObject.Find("Cube").transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        //gameObject.transform.position = boxV;
    }
}
