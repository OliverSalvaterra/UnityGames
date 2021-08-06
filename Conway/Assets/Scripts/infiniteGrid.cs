using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infiniteGrid : MonoBehaviour
{
    public Dictionary<Vector3, GameObject> cubes = new Dictionary<Vector3, GameObject>();
    public Dictionary<Vector3, int> neighbors = new Dictionary<Vector3, int>();
    bool pause = false;
    float currTime = 0;
    int cubeside = 2;

    public GameObject cubeTemplate;
    GameObject empty;
    public GameObject ind;
    public GameObject plane;

    public void create(Vector3 pos)
    {
        if (!neighbors.ContainsKey(pos)) neighbors.Add(pos, 0);
        
        if (!cubes.ContainsKey(pos))
        {
            updateCubes(pos, true);
            GameObject cube = Instantiate(cubeTemplate, pos, Quaternion.identity);
            cubes.Add(pos, cube);
        }
    }
    public void destroy(Vector3 pos)
    {
        updateCubes(pos, false);
        cubes.TryGetValue(pos, out GameObject cube);
        cubes.Remove(pos);
        Destroy(cube);
    }

    public void updateNeighbors(Vector3 pos, bool cd)
    {
        if (cd)
        {
            if (!neighbors.ContainsKey(pos))
            {
                neighbors.Add(pos, 1);
            }
            else
            {
                neighbors[pos]++;
            }
        }
        else
        {
            //if(neighbors[pos] == 1)
            //{
                //neighbors.Remove(pos);
            //}
           // else
            //{
                neighbors[pos]--;
           // }
        }
    }

    public void updateStates()
    {
        List<(Vector3, bool)> cd = new List<(Vector3, bool)>();
        bool contains = false;
        int n = 0;

        foreach(Vector3 pos in neighbors.Keys) 
        {
            contains = cubes.ContainsKey(pos);
            n = neighbors[pos];

            if (!contains && n == 3) cd.Add((pos, true));
            else if (contains && (n > 3 || n < 2)) cd.Add((pos, false));
        }

        for(int i = 0; i < cd.Count; i++)
        {
            if (cd[i].Item2 == true) create(cd[i].Item1);
            else destroy(cd[i].Item1);
        }
    }

    public void updateCubes(Vector3 pos, bool cd)
    {
            for(int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    for(int z = -1; z <= 1; z++)
                    {
                        if (x == 0 && y == 0 && z == 0) continue;
                        updateNeighbors(new Vector3(pos.x + x*cubeside, pos.y + y*cubeside, pos.z + z*cubeside), cd);
                    }
                }
            }
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
            if (currTime >= .40f)
            {
                updateStates();

                currTime = 0;
            }
        }

        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitinfo);

        if (hit)
        {
            if (hitinfo.collider.gameObject == plane)
            {
                pos = new Vector3(((int)(hitinfo.point.x / cubeside)) * cubeside, ((int)(hitinfo.point.y / cubeside)) * cubeside, ((int)(hitinfo.point.z / cubeside)) * cubeside);
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
                    create(pos);
                }
                else
                {
                    destroy(pos);
                }
            }
        }
    }
}
