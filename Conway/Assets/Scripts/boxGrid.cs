using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum cubeState
    {
        off,
        on
    };
public class CubeClass
{
    public GameObject cube;
    private MeshRenderer renderer;
    private int neighbors;
    private cubeState state;
    private (int, int, int) pos;
    private bool transparent;

    public BoxCollider box;

    public CubeClass(GameObject cube, int y, int x, int z, cubeState state)
    {
        this.cube = cube;
        neighbors = 0;
        pos = (y, x, z);
        transparent = false;
        this.state = state;
        box = cube.AddComponent<BoxCollider>();

        renderer = cube.GetComponent<MeshRenderer>();

        transparency();
    }

    public void UpdateState(CubeClass[,,] grid)
    {
        neighbors = 0;
        if (pos.Item2 > 0) // not at left bound
        {
            if (grid[pos.Item1, pos.Item2 - 1, pos.Item3].state == cubeState.on) neighbors++; // left neighbor

            if (pos.Item1 > 0)
            {
                if (grid[pos.Item1 - 1, pos.Item2 - 1, pos.Item3].state == cubeState.on) neighbors++;   // topleft neighbor
            }  
            if (pos.Item1 < grid.GetLength(0) - 1)
            {
                if (grid[pos.Item1 + 1, pos.Item2 - 1, pos.Item3].state == cubeState.on) neighbors++; // bottom left neighbor
            }
        }
        if (pos.Item2 < grid.GetLength(1) - 1) // not at right bound
        {
            if (grid[pos.Item1, pos.Item2 + 1, pos.Item3].state == cubeState.on) neighbors++; // right neighbor

            if (pos.Item1 > 0)
            {
                if (grid[pos.Item1 - 1, pos.Item2 + 1, pos.Item3].state == cubeState.on) neighbors++; // top right neighbor
            }
            if (pos.Item1 < grid.GetLength(0) - 1)
            {
                if (grid[pos.Item1 + 1, pos.Item2 + 1, pos.Item3].state == cubeState.on) neighbors++; // bottom right neighbor
            }
        }
        if (pos.Item1 > 0) // not at top bound
        {
            if (grid[pos.Item1 - 1, pos.Item2, pos.Item3].state == cubeState.on) neighbors++; // top neighbor
        }
        if (pos.Item1 < grid.GetLength(0) - 1) // not at bottom bound
        {
            if (grid[pos.Item1 + 1, pos.Item2, pos.Item3].state == cubeState.on) neighbors++; // bottom neighbor
        }
        if (pos.Item3 > 0)
        {
            if (grid[pos.Item1, pos.Item2, pos.Item3 - 1].state == cubeState.on) neighbors++;
        }
        if (pos.Item3 < grid.GetLength(2) - 1)
        {
            if (grid[pos.Item1, pos.Item2, pos.Item3 + 1].state == cubeState.on) neighbors++;
        }
    }

    public void update()
    {
        if (neighbors == 3 && state == cubeState.off)
        {
            state = cubeState.on;
            transparency();
        }
        else if ((neighbors < 2 || neighbors > 3) && state == cubeState.on)
        {
            state = cubeState.off;
            transparency();
        }
    }

    public void flip()
    {
        state = state == cubeState.on ? cubeState.off : cubeState.on;
        transparency();
    }

    private void transparency()
    {
        if (state == cubeState.on)
        {
            //var material = cube.GetComponent<Renderer>().material;
            //material.color = new Color(material.color.r, material.color.g, material.color.b, 1.0f);
            //cube.SetActive(true);
            renderer.enabled = true;
            transparent = true;
        }
        else if(state == cubeState.off)
        {
            //var material = cube.GetComponent<Renderer>().material;
            //material.color = new Color(material.color.r, material.color.g, material.color.b, 0.0f);
            //cube.SetActive(false);
            renderer.enabled = false;
            transparent = false;
        }
    }
}

public class boxGrid : MonoBehaviour
{
    public GameObject cubeTemplate;
    public CubeClass[,,] grid = new CubeClass[40, 40, 40];
    GameObject empty;

    float currTime = 0;
    bool pause;

    // Start is called before the first frame update
    void Start()
    {
        pause = false;
        empty = GameObject.Find("empty");

        Vector3 scale = cubeTemplate.transform.localScale;

        Vector3 boxsize = cubeTemplate.GetComponent<BoxCollider>().size;

        Vector3 size = new Vector3(boxsize.x * scale.x, boxsize.y * scale.y, boxsize.z * scale.z);

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for(int j = 0; j < grid.GetLength(1); j++)
            {
                for (int z = 0; z < grid.GetLength(2); z++)
                {
                    
                    grid[i, j, z] = new CubeClass(Instantiate(cubeTemplate, new Vector3(i * size.x + (i/2f), z*size.z + (z/2f), j * size.z + (j/2f)), Quaternion.identity), i, j, z, (cubeState)Random.Range(0, 2));
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pause = !pause;
        }

        if (!pause)
        {
            currTime += Time.deltaTime;
            if (currTime >= .11f)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                    {
                        for (int z = 0; z < grid.GetLength(2); z++)
                        {
                            grid[i, j, z].UpdateState(grid);
                        }
                    }
                }

                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                    {
                        for (int z = 0; z < grid.GetLength(2); z++)
                        {
                            grid[i, j, z].update();
                        }
                    }
                }
                currTime = 0;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitinfo);

            if (hit)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                    {
                        for (int z = 0; z < grid.GetLength(2); z++)
                        {
                            if (hitinfo.transform.gameObject == grid[i, j, z].cube)
                            {
                                grid[i, j, z].flip();
                                goto aferLoop;
                            }
                        }
                    }
                }
            aferLoop:;
            }
        }
    }
}
