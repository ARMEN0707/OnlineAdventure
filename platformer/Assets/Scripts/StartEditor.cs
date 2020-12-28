using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StartEditor : MonoBehaviour
{
    public GameObject scrollView;
    public GameObject character;
    public GameObject fruitsCount;
    public MoveCamera moveCameraScript;
    public PlayerCamera playerCameraScript;

    public enum dir { left = 1, right, top, bottom }

    private Text textButton;
    private bool start = false;
    public bool IsStart
    {
        get
        {
            return start;
        }
    }


    private void Start()
    {
        textButton = gameObject.GetComponentInChildren<Text>();
        character.SetActive(false);
    }

    //Создание границы при которой обьекты удаляются
    public void CreateClearObject(float x,float y, float l)
    {
        float delta = 1.0f;
        GameObject clearObject = new GameObject("ClearObject");
        EdgeCollider2D collider = clearObject.AddComponent<EdgeCollider2D>();
        Vector2[] pointCollider = new Vector2[2];
        pointCollider[0] = new Vector2(x, y - delta);
        pointCollider[1] = new Vector2(x+l, y - delta);
        collider.points = pointCollider;
        clearObject.AddComponent<ClearObject>();

    }
    //создает коллайдеры для ограничения движения камеры
    public void CreateBorder(GameObject x, GameObject y, GameObject l, GameObject obj, dir dir)
    {
        float size = 0.5f;
        BoxCollider2D col = obj.AddComponent<BoxCollider2D>();
        float w = x.GetComponent<SpriteRenderer>().size.x / 2.0f;
        float h = y.GetComponent<SpriteRenderer>().size.y / 2.0f;
        float b = l.GetComponent<SpriteRenderer>().size.y / 2.0f;
        float delta = (y.transform.position.y + h) - (l.transform.position.y - b);
        col.size = new Vector2(size, Math.Abs(delta));
        if(dir == dir.right)
        {
            obj.transform.position = new Vector2(x.transform.position.x + w, y.transform.position.y + h);
            col.offset = new Vector2(size / 2.0f, -delta / 2.0f);
            return;
        }
        if(dir == dir.left)
        {
            obj.transform.position = new Vector2(x.transform.position.x - w, y.transform.position.y + h);
            col.offset = new Vector2(-size / 2.0f, -delta / 2.0f);
            return;
        }
        delta = (l.transform.position.x + b) - (x.transform.position.x - w);
        col.size = new Vector2(Math.Abs(delta), size);
        if (dir == dir.top)
        {
            obj.transform.position = new Vector2(x.transform.position.x - w, y.transform.position.y + h);
            col.offset = new Vector2(delta / 2.0f, size / 2.0f);
            return;
        }
        if(dir == dir.bottom)
        {
            obj.transform.position = new Vector2(x.transform.position.x - w, y.transform.position.y - h);
            col.offset = new Vector2(delta / 2.0f, -size / 2.0f);
            CreateClearObject(obj.transform.position.x, obj.transform.position.y,delta);
            return;
        }
        
    }
    public void DeleteBorder()
    {
        Destroy(GameObject.Find("LeftBorder"));
        Destroy(GameObject.Find("RightBorder"));
        Destroy(GameObject.Find("TopBorder"));
        Destroy(GameObject.Find("BottomBorder"));
        Destroy(GameObject.Find("ClearObject"));
    }
    //проверка на слой
    public bool checkLayer(int layer) =>
        layer == LayerMask.NameToLayer("Ground") ||
        layer == LayerMask.NameToLayer("Enemies") ||
        layer == LayerMask.NameToLayer("Object");
    //создает коллайдеры для ограничения движения камеры
    public void CreateBordersForCamera()
    {
        GameObject[] gameObjectInScenes = FindObjectsOfType<GameObject>();
        int i = 0;
        while(true)
        {
            try
            {
                if (checkLayer(gameObjectInScenes[i].layer) && gameObjectInScenes[i].GetComponent<SpriteRenderer>() != null)
                {
                    break;
                }
                else
                {
                    i++;
                }
            }
            catch
            {
                return;
            }
        }
        GameObject left = gameObjectInScenes[i];
        GameObject right = gameObjectInScenes[i];
        GameObject top = gameObjectInScenes[i];
        GameObject bottom = gameObjectInScenes[i];
        foreach (GameObject item in gameObjectInScenes)
        {
            if (checkLayer(item.layer) && item.GetComponent<SpriteRenderer>() != null )
            {
                if (item.transform.position.x < left.transform.position.x)
                {
                    left = item;
                }
                if (item.transform.position.x > right.transform.position.x)
                {
                    right = item;
                }
                if (item.transform.position.y > top.transform.position.y)
                {
                    top = item;
                }
                if (item.transform.position.y < bottom.transform.position.y)
                {
                    bottom = item;
                }
            }
        }

        int borderLayer = LayerMask.NameToLayer("Border");
        GameObject leftBorder = new GameObject("LeftBorder");
        leftBorder.layer = borderLayer;
        CreateBorder(left,top,bottom,leftBorder,dir.left);
        GameObject rightBorder = new GameObject("RightBorder");
        CreateBorder(right, top, bottom, rightBorder,dir.right);
        rightBorder.layer = borderLayer;
        GameObject topBorder = new GameObject("TopBorder");
        CreateBorder(left, top, right, topBorder, dir.top);
        topBorder.layer = borderLayer;
        GameObject bottomBorder = new GameObject("BottomBorder");
        CreateBorder(left, bottom, right, bottomBorder, dir.bottom);
        bottomBorder.layer = borderLayer;
    }

    public void ActivateAll()
    {
        ActivateBird();
        ActivateChicken();
        ActivateMovePlatform();
        ActivateKillEnemies();
    }

    //активирование скриптов
    private void ActivateBird()
    {
        Bird[] birdScripts = FindObjectsOfType<Bird>();
        foreach (Bird item in birdScripts)
        {
            item.enabled = !item.enabled;
            item.transform.position = item.startPoint;
        }
    }
    private void ActivateKillEnemies()
    {
        KillEnemies[] killEnemiesScript = FindObjectsOfType<KillEnemies>();
        foreach(KillEnemies item in killEnemiesScript)
        {
            item.colliderEnemies.isTrigger = false;
            item.rbEnemies.bodyType = RigidbodyType2D.Kinematic;
        }
    }
    private void ActivateChicken()
    {
        Chicken[] chickenScripts = FindObjectsOfType<Chicken>();
        foreach (Chicken item in chickenScripts)
        {
            item.enabled = !item.enabled;
            item.transform.position = item.startPoint;
        }
    }
    private void ActivateMovePlatform()
    {
        MovePlatform[] platformScript = FindObjectsOfType<MovePlatform>();
        foreach(MovePlatform item in platformScript)
        {
            item.enabled = !item.enabled;
            item.transform.position = new Vector3(
                item.pathElements[0].transform.position.x,
                item.pathElements[0].transform.position.y,
                item.transform.position.z);
        }
    }

    //запуск или остановка воспроизведения карты
    public void PressButton()
    {
        if(!start)
        {
            scrollView.SetActive(false);
            fruitsCount.SetActive(true);
            DataScenes.collectedFruits = 0;
            textButton.text = "Stop";
            start = true;
            if(GameObject.Find("PlayerSpawn")!=null)
            {
                character.transform.position = GameObject.Find("PlayerSpawn").GetComponent<Transform>().position;
                character.SetActive(true);
                moveCameraScript.enabled = false;
                playerCameraScript.enabled = true;
            }
            CreateBordersForCamera();
        }
        else
        {
            scrollView.SetActive(true);
            textButton.text = "Start";
            character.SetActive(false);
            fruitsCount.SetActive(false);
            start = false;
            moveCameraScript.enabled = true;
            playerCameraScript.enabled = false;
            DeleteBorder();
        }
        SelectMouseItem selectItem = Camera.main.GetComponent<SelectMouseItem>();
        selectItem.enabled = !selectItem.enabled;
        //активирование скриптов объектов
        ActivateAll();

    }
}
