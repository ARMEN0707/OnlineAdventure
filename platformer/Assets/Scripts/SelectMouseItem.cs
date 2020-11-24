﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectMouseItem : MonoBehaviour
{
    public List<GameObject> listPrefabs;
    private Camera mainCamera;
    private RaycastHit2D rayMouse;
    private Vector2 mouse;
    private int indexTerrain;
    private GameObject selectTerrain;
    private GameObject createObject;

    private int groundLayer;
    private int terrainLayer;
    private int enemiesLayer;
    private bool selectItem;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        groundLayer = LayerMask.NameToLayer("Ground");
        terrainLayer = LayerMask.NameToLayer("UI");
        enemiesLayer = LayerMask.NameToLayer("Enemies");
    }

    public static void InitObjectBird(GameObject createObject)
    {
        Bird birdScript = createObject.GetComponent<Bird>();
        birdScript.startPoint = new Vector3(
            birdScript.transform.position.x,
            birdScript.transform.position.y,
            birdScript.transform.position.z
        );
        birdScript.tempPointRight = new Vector3(
            birdScript.transform.position.x + birdScript.distance,
            birdScript.transform.position.y,
            birdScript.transform.position.z
        );
        birdScript.tempPointLeft = new Vector3(
            birdScript.transform.position.x - birdScript.distance,
            birdScript.transform.position.y,
            birdScript.transform.position.z
        );
    }
    public static void InitObjectChicken(GameObject createObject)
    {
        Chicken chickenScript = createObject.GetComponentInChildren<Chicken>();
        chickenScript.startPoint = new Vector3(
            chickenScript.transform.position.x,
            chickenScript.transform.position.y,
            chickenScript.transform.position.z
        );
    }

    //рисует обводку, когда объект выбран
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
        Material redToGreen = new Material(Shader.Find("Mobile/Particles/Additive"));
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
        mouse = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        mouse = mainCamera.ScreenToWorldPoint(mouse);
        //создание объекта
        if ((rayMouse.collider!=null) && (rayMouse.collider.gameObject.layer == terrainLayer) && Input.GetMouseButtonDown(0))
        {
            if(createObject != null && createObject.GetComponent<LineRenderer>()!=null)
            {
                Destroy(createObject.GetComponent<LineRenderer>());
            }
            indexTerrain = Convert.ToInt32(rayMouse.collider.name);
            selectTerrain = listPrefabs[indexTerrain];
            createObject =  Instantiate(selectTerrain, new Vector3(mouse.x,mouse.y,selectTerrain.transform.position.z), Quaternion.identity);
            createObject.name = createObject.name.Replace("(Clone)", "");
            selectItem = true;
            DrawSelectItem(createObject);
        }
        //выбор созданого объекта
        else if ((rayMouse.collider != null) && Input.GetMouseButtonDown(0) && 
        ((rayMouse.collider.gameObject.layer == groundLayer) || (rayMouse.collider.gameObject.layer == enemiesLayer)))        
        {
            if (createObject != null && createObject.GetComponent<LineRenderer>() != null)
            {
                Destroy(createObject.GetComponent<LineRenderer>());
            }
            createObject = rayMouse.collider.gameObject;
            DrawSelectItem(createObject);
            selectItem = true;
        }
        //перемещение объекта
        if (Input.GetMouseButton(0) && selectItem)
        {
            createObject.transform.position = Vector3.MoveTowards(createObject.transform.position, new Vector3(mouse.x, mouse.y, selectTerrain.transform.position.z), 0.5f);
            DrawSelectItem(createObject);
        }

        //когда перестали перемещать
        if(selectItem && Input.GetMouseButtonUp(0))
        {
            if(createObject.tag == "BlueBird")
            {
                InitObjectBird(createObject);
            }
            if(createObject.tag == "Chicken")
            {
                InitObjectChicken(createObject);
            }
            selectItem = false;
        }
        //Уничтожение обводки
        if(Input.GetMouseButtonDown(0) && createObject != null && createObject.GetComponent<LineRenderer>() != null)
        {
            Destroy(createObject.GetComponent<LineRenderer>());
        }
        //быбранного объекта нет
        if(rayMouse.collider == null && Input.GetMouseButtonDown(0))
        {
            createObject = null;
        }
        //удаление объекта
        if (Input.GetKey(KeyCode.Delete))
        {
            Destroy(createObject);
        }

    }
    private void FixedUpdate()
    {
        //луч от курсора
        rayMouse = Physics2D.Raycast(mouse, Vector3.forward, 10.0f);
        Debug.DrawRay(mouse, Vector3.forward * 10.0f, Color.black);
    }
}
