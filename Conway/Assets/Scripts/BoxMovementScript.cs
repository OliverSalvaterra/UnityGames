using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoxMovementScript : MonoBehaviour
{
    bool clicked;
    Vector3 start;
    Vector3 end;
    Vector3 move;
    float stepsPerSec;
    float currentStep;
    public GameObject c;
    bool downdown;
    bool updown;
    bool leftdown;
    bool rightdown;
    int speed;

    // Start is called before the first frame update
    void Start()
    {
        clicked = false;
        start = new Vector3(0, 100, 0);
        end = new Vector3(0, 100, 0);
        move = new Vector3(0, 0, 0);
        stepsPerSec = .5f;
        currentStep = 0;
        speed = 7;

        c = Instantiate(c, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            downdown = false;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            downdown = true;
        }
        if (downdown)
        {
            c.transform.position = new Vector3(c.transform.position.x, c.transform.position.y, c.transform.position.z - speed);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            updown = false;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            updown = true;
        }

        if (updown)
        {
            c.transform.position = new Vector3(c.transform.position.x, c.transform.position.y, c.transform.position.z + speed);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            leftdown = false;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            leftdown = true;
        }

        if (leftdown)
        {
            c.transform.position = new Vector3(c.transform.position.x - speed, c.transform.position.y, c.transform.position.z);
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            rightdown = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rightdown = true;
        }

        if (rightdown)
        {
            c.transform.position = new Vector3(c.transform.position.x + speed, c.transform.position.y, c.transform.position.z);
        }

        if (Input.GetMouseButtonDown(0)) { clicked = true; }

        if (Input.GetMouseButtonUp(0) && clicked)
        {
            bool hit;
            RaycastHit rhit = new RaycastHit();


            start = move;


            Ray pointToScreen = Camera.main.ScreenPointToRay(Input.mousePosition);
            hit = Physics.Raycast(pointToScreen, out rhit);

            if (hit)
            {
                end = new Vector3(rhit.point.x, move.y, rhit.point.z);
            }

            currentStep = 0;

            clicked = false;
        }
        if (currentStep < 1)
        {
            currentStep += stepsPerSec * Time.deltaTime;
            move = Vector3.Lerp(start, end, currentStep);
        }

        gameObject.transform.position = move;
    }
}
