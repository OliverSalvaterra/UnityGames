using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infiniteGrid : MonoBehaviour
{
    Dictionary<Vector3, GameObject> cubes = new Dictionary<Vector3, GameObject>();
    bool pause = false;
    float currTime = 0;
    int cubeside = 2;

    public GameObject cubeTemplate;
    GameObject empty;
    public GameObject ind;
    public GameObject plane;

    public void create(Vector3 pos, Dictionary<Vector3, GameObject> cubes)
    {
        GameObject cube = Instantiate(cubeTemplate, pos, Quaternion.identity);
        cubes.Add(pos, cube);
    }
    public void destroy(Vector3 pos, Dictionary<Vector3, GameObject> cubes)
    {
        cubes.TryGetValue(pos, out GameObject cube);
        Destroy(cube);
        cubes.Remove(pos);
    }

    public int neighbors(Dictionary<Vector3, GameObject> cubes, Vector3 pos)
    {
        int rtrn = 0;

        if (cubes.ContainsKey(new Vector3(pos.x - cubeside, pos.y, pos.z))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x + cubeside, pos.y, pos.z))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x - cubeside, pos.y, pos.z - cubeside))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x + cubeside, pos.y, pos.z - cubeside))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x - cubeside, pos.y, pos.z + cubeside))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x + cubeside, pos.y, pos.z + cubeside))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x - cubeside, pos.y - cubeside, pos.z))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x + cubeside, pos.y - cubeside, pos.z))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x - cubeside, pos.y + cubeside, pos.z))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x + cubeside, pos.y + cubeside, pos.z))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x, pos.y - cubeside, pos.z))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x, pos.y + cubeside, pos.z))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x, pos.y - cubeside, pos.z - cubeside))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x, pos.y + cubeside, pos.z - cubeside))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x, pos.y - cubeside, pos.z + cubeside))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x, pos.y + cubeside, pos.z + cubeside))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x, pos.y, pos.z - cubeside))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x, pos.y, pos.z + cubeside))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x - cubeside, pos.y - cubeside, pos.z - cubeside))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x + cubeside, pos.y - cubeside, pos.z - cubeside))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x - cubeside, pos.y + cubeside, pos.z - cubeside))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x - cubeside, pos.y - cubeside, pos.z + cubeside))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x + cubeside, pos.y + cubeside, pos.z - cubeside))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x + cubeside, pos.y - cubeside, pos.z + cubeside))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x - cubeside, pos.y + cubeside, pos.z + cubeside))) rtrn++;
        if (cubes.ContainsKey(new Vector3(pos.x + cubeside, pos.y + cubeside, pos.z + cubeside))) rtrn++;

        return rtrn;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(0, 0, 0);
        bool cd = true;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            pause = !pause;
        }

        if (!pause)
        {
            currTime += Time.deltaTime;
            if (currTime >= .25f)
            {
                
                currTime = 0;
            }
        }

        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitinfo);

        if (hit)
        {
            if (hitinfo.collider.gameObject == plane)
            {
                pos = new Vector3((int)(hitinfo.collider.transform.position.x/cubeside), (int)(hitinfo.collider.transform.position.y / cubeside), (int)(hitinfo.collider.transform.position.z / cubeside));
            }
            else
            {
                pos = hitinfo.collider.transform.position;
                cd = false;
            }

            ind.transform.position = pos;

            if (Input.GetMouseButtonDown(0))
            {
                if (cd)
                {
                    create(pos, cubes);
                }
                else
                {
                    destroy(pos, cubes);
                }
            }
        }
    }
}
