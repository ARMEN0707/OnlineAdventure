using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectMouseItem : MonoBehaviour
{
    public List<GameObject> arrayTerrain;
    private Camera mainCamera;
    private RaycastHit2D rayMouse;
    private Vector2 mouse;
    private int indexTerrain;
    private GameObject selectTerrain;
    private GameObject createTerrain;

    private int groundLayer;
    private int terrainLayer;
    private bool selectItem;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        groundLayer = LayerMask.NameToLayer("Ground");
        terrainLayer = LayerMask.NameToLayer("UI");
    }

    void DrawSelectItem (GameObject item)
    {
        LineRenderer lineRenderer = item.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = item.AddComponent<LineRenderer>();
        }
        Bounds tempBounds = item.GetComponent<Collider2D>().bounds;
        Vector2 size = tempBounds.size;
        Vector2 center = tempBounds.center;
        float x = center.x;
        float y = center.y;
        Material redToGreen = new Material(Shader.Find("Mobile/Particles/Additive")); //Probably something                                                        wrong here.
        lineRenderer.material = redToGreen;
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        lineRenderer.widthMultiplier = 0.01f;
        lineRenderer.positionCount = 5;
        lineRenderer.loop = true;
        Vector3[] position = new Vector3[5];
        position[0] = new Vector3(x - (size.x / 2.0f) - 0.005f, y + (size.y / 2.0f) + 0.005f, 0);
        position[1] = new Vector3(x + (size.x / 2.0f) + 0.005f, y + (size.y / 2.0f) + 0.005f, 0);
        position[2] = new Vector3(x + (size.x / 2.0f) + 0.005f, y - (size.y / 2.0f) - 0.005f, 0);
        position[3] = new Vector3(x - (size.x / 2.0f) - 0.005f, y - (size.y / 2.0f) - 0.005f, 0);
        position[4] = new Vector3(x - (size.x / 2.0f) - 0.005f, y + (size.y / 2.0f) + 0.005f, 0);
        lineRenderer.SetPositions(position);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(rayMouse.collider);
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("dsf");
        }
        mouse = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        mouse = mainCamera.ScreenToWorldPoint(mouse);
        if ((rayMouse.collider!=null) && (rayMouse.collider.gameObject.layer == terrainLayer) && Input.GetMouseButtonDown(0))
        {
            if(createTerrain != null && createTerrain.GetComponent<LineRenderer>()!=null)
            {
                Destroy(createTerrain.GetComponent<LineRenderer>());
            }
            indexTerrain = Convert.ToInt32(rayMouse.collider.name);
            selectTerrain = arrayTerrain[indexTerrain];
            createTerrain =  Instantiate(selectTerrain, new Vector3(mouse.x,mouse.y,selectTerrain.transform.position.z), Quaternion.identity);
            createTerrain.name = createTerrain.name.Replace("(Clone)", "");
            selectItem = true;
            DrawSelectItem(createTerrain);
        }
        else if ((rayMouse.collider != null) && (rayMouse.collider.gameObject.layer == groundLayer) && Input.GetMouseButtonDown(0))
        {
            if (createTerrain != null && createTerrain.GetComponent<LineRenderer>() != null)
            {
                Destroy(createTerrain.GetComponent<LineRenderer>());
            }
            createTerrain = rayMouse.collider.gameObject;
            DrawSelectItem(createTerrain);
            selectItem = true;
        }
        if (Input.GetMouseButton(0) && selectItem)
        {
            createTerrain.transform.position = Vector3.MoveTowards(createTerrain.transform.position, new Vector3(mouse.x, mouse.y, selectTerrain.transform.position.z), 0.5f);
            DrawSelectItem(createTerrain);
        }
        if(selectItem && Input.GetMouseButtonUp(0))
        {
            selectItem = false;
        }
        if(Input.GetMouseButtonDown(0) && createTerrain != null && createTerrain.GetComponent<LineRenderer>() != null)
        {
            Destroy(createTerrain.GetComponent<LineRenderer>());
        }

    }
    private void FixedUpdate()
    {
        rayMouse = Physics2D.Raycast(mouse, Vector3.forward, 10.0f);
        Debug.DrawRay(mouse, Vector3.forward * 10.0f, Color.black);
    }
}
