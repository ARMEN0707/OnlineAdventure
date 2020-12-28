using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectMouseItem : MonoBehaviour
{
    public List<GameObject> listPrefabs;
    private Camera mainCamera;
    private RaycastHit2D rayMouse;
    private Vector2 mouse;
    private int indexObject;
    private GameObject selectObject;
    private GameObject createObject;
    private GameObject emptyObject;
    private GameObject platform;

    private int groundLayer;
    private int uiLayer;
    private int enemiesLayer;
    private int objectLayer;
    private bool selectItem;
    private bool paintLine;

    //Для рисования линии для платформы
    public float distanceChain;
    private float lineX;
    private float lineY;
    private List<GameObject> pointLine;
    private List<GameObject> pointPlatform;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        groundLayer = LayerMask.NameToLayer("Ground");
        uiLayer = LayerMask.NameToLayer("UI");
        enemiesLayer = LayerMask.NameToLayer("Enemies");
        objectLayer = LayerMask.NameToLayer("Object");
    }

    public bool checkLayer(int layer) =>
        layer == groundLayer || layer == enemiesLayer || layer == objectLayer;
    public GameObject GetChainParent(GameObject chain, int i) => chain.transform.parent.transform.GetChild(i).gameObject;



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
    //инициализация необходимых объектов для движущейся платформы
    public static GameObject CreateMovePlatform(float lineX, float lineY, int objectLayer, out GameObject platform)
    {
        GameObject go = new GameObject("MovePlatform");
        go.transform.position = new Vector3(lineX, lineY, 14);
        go.layer = objectLayer;
        go.tag = "movePlatform";
        GameObject goChild = new GameObject("chain");
        goChild.transform.position = new Vector3(lineX, lineY, 14);
        goChild.transform.SetParent(go.transform);
        platform = Resources.Load("Prefabs/greyPlatform") as GameObject;
        platform = Instantiate(platform, new Vector3(lineX, lineY, 0), Quaternion.identity);
        platform.name = platform.name.Replace("(Clone)", "");
        platform.transform.SetParent(go.transform);
        platform.transform.localPosition = new Vector3(
            platform.transform.localPosition.x,
            platform.transform.localPosition.y,
            -1);
        return goChild;
    }
    //создание одного звена дороги
    public static void CreateChain(GameObject emptyObject, GameObject chainItem,float x, float y, List<GameObject> pointLine)
    {
        GameObject tempObject = Instantiate(chainItem, new Vector3(x, y, chainItem.transform.position.z), Quaternion.identity);
        tempObject.name = tempObject.name.Replace("(Clone)", "");
        tempObject.transform.SetParent(emptyObject.transform);
        tempObject.transform.localPosition = new Vector3(
            tempObject.transform.localPosition.x,
            tempObject.transform.localPosition.y,
            0);
        pointLine.Add(tempObject);
    }
    //отрисовка линии движения движущейся платформы
    public static void PaintLine(float lineX, float lineY, float mouseX, float mouseY, float distanceChain, GameObject emptyObject, GameObject chain, List<GameObject> pointLine)
    {
        foreach (GameObject item in pointLine)
        {
            Destroy(item);
        }
        pointLine.Clear();
        float k = (mouseY - lineY) / (mouseX - lineX);
        float b = lineY - (k * lineX);

        if (Math.Atan(k) >= -(Math.PI / 4) && Math.Atan(k) <= (Math.PI / 4))
        {
            for (float x = lineX; x <= mouseX - distanceChain; x += distanceChain)
            {
                float y = (k * x) + b;
                CreateChain(emptyObject,chain, x, y,pointLine);
            }
        }
        if ( (Math.Atan(k) >= (Math.PI / 4) && Math.Atan(k) <= (Math.PI / 2))
            || (Math.Atan(k) >= -(Math.PI / 2) && Math.Atan(k) <= -(Math.PI / 4)) )
        {
            for (float y = lineY; y <= mouseY - distanceChain; y += distanceChain)
            {
                float x;
                if (k == 0 || float.IsNaN(k) || float.IsInfinity(k))
                {
                    x = mouseX;
                }
                else
                {
                    x = (y - b) / k;
                }
                CreateChain(emptyObject, chain, x, y, pointLine);
            }
        }
        if (Math.Atan(k) >= -(Math.PI / 4) && Math.Atan(k) <=  (Math.PI / 4))
        {
            for (float x = lineX; x >= mouseX + distanceChain; x -= distanceChain)
            {
                float y = (k * x) + b;
                CreateChain(emptyObject, chain, x, y, pointLine);
            }
        }
        if ( (Math.Atan(k) >= -(Math.PI / 2) && Math.Atan(k) <= -(Math.PI / 4))
            || (Math.Atan(k) >= (Math.PI / 4) && Math.Atan(k) <= (Math.PI / 2)))
        {
            for (float y = lineY; y >= mouseY + distanceChain; y -= distanceChain)
            {
                float x;
                if (k==0 || float.IsNaN(k) || float.IsInfinity(k))
                {
                    x = lineX;
                }else
                {
                    x = (y - b) / k;
                }
                CreateChain(emptyObject, chain, x, y, pointLine);
            }
        }
        
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
        //рисуем линию
        if(paintLine)
        {
            PaintLine(lineX, lineY, mouse.x, mouse.y,distanceChain,emptyObject, selectObject, pointLine);
            if (Input.GetMouseButtonDown(0))
            {
                pointPlatform.Add(pointLine[0]);
                lineX = mouse.x;
                lineY = mouse.y;
                pointLine.Clear();
            }else if(Input.GetMouseButtonDown(1))
            {
                MovePlatform platformScript = platform.GetComponent<MovePlatform>();
                pointPlatform.Add(pointLine[0]);
                pointPlatform.Add(pointLine[pointLine.Count-1]);
                platformScript.pathElements = pointPlatform.ToArray();
                pointPlatform.Clear();
                pointLine.Clear();
                paintLine = false;
                platform.SetActive(true);
                platform = null;
                pointPlatform = null;
                pointLine = null;

            }
        }
        //создание объекта
        if ((rayMouse.collider!=null) && (rayMouse.collider.gameObject.layer == uiLayer) && Input.GetMouseButtonDown(0))
        {
            if(createObject != null && createObject.GetComponent<LineRenderer>()!=null)
            {
                Destroy(createObject.GetComponent<LineRenderer>());
            }
            indexObject = Convert.ToInt32(rayMouse.collider.name);
            selectObject = listPrefabs[indexObject];
            createObject =  Instantiate(selectObject, new Vector3(mouse.x,mouse.y,selectObject.transform.position.z), Quaternion.identity);
            createObject.name = createObject.name.Replace("(Clone)", "");
            selectItem = true;
            DrawSelectItem(createObject);
        }
        //выбор созданого объекта
        else if ((rayMouse.collider != null) && Input.GetMouseButtonDown(0) 
                && checkLayer(rayMouse.collider.gameObject.layer))        
        {
            if (createObject != null && createObject.GetComponent<LineRenderer>() != null)
            {
                if (createObject.tag == "Chain")
                {
                    //получаем все объекты цепи и удаляем обводку
                    for (int i = 0; i < createObject.transform.parent.childCount; i++)
                    {
                        GameObject chain = GetChainParent(createObject,i);
                        Destroy(chain.GetComponent<LineRenderer>());
                    }
                }
                else
                {
                    Destroy(createObject.GetComponent<LineRenderer>());
                }
            }
            createObject = rayMouse.collider.gameObject;
            if(createObject.tag == "Chain")
            {
                //получаем все объекты цепи и выделем их
                for(int i = 0;i < createObject.transform.parent.childCount; i++)
                {
                    GameObject chain = GetChainParent(createObject, i);
                    DrawSelectItem(chain);
                }                
            }else
            {
                DrawSelectItem(createObject);
                selectItem = true;
            }            
        }
        //перемещение объекта
        if (Input.GetMouseButton(0) && selectItem)
        {
            createObject.transform.position = Vector3.MoveTowards(createObject.transform.position, new Vector3(mouse.x, mouse.y, selectObject.transform.position.z), 0.5f);
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
            if(createObject.tag == "Chain")
            {                
                paintLine = true;
                pointLine = new List<GameObject>();
                pointPlatform = new List<GameObject>();
                lineX = mouse.x;
                lineY = mouse.y;          
                emptyObject = CreateMovePlatform(lineX,lineY,objectLayer, out platform);
                platform.SetActive(false);
                Destroy(createObject);
            }
            selectItem = false;
        }
        //Уничтожение обводки и забываем про выбранный объект
        if(Input.GetMouseButtonDown(0) && createObject != null && createObject.GetComponent<LineRenderer>() != null && rayMouse.collider == null)
        {
            if (createObject.tag == "Chain")
            {
                //удаляем обводку со всех элементов цепи
                for (int i = 0; i < createObject.transform.parent.childCount; i++)
                {
                    GameObject chain = GetChainParent(createObject, i);
                    Destroy(chain.GetComponent<LineRenderer>());
                }
            }
            else
            {
                Destroy(createObject.GetComponent<LineRenderer>());
            }
            createObject = null;
        }
        //удаление объекта
        if (Input.GetKey(KeyCode.Delete) && createObject != null)
        {
            if(createObject.tag == "Chain")
            {
                //удаляем платформу
                Destroy(createObject.transform.parent.gameObject.transform.parent.gameObject);
            }else
            {
                Destroy(createObject);
            }
        }

    }
    private void FixedUpdate()
    {
        //луч от курсора
        rayMouse = Physics2D.Raycast(mouse, Vector3.forward, 10.0f);
        Debug.DrawRay(mouse, Vector3.forward * 10.0f, Color.black);
    }
}
