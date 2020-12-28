using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class SaveLoadMap : MonoBehaviour
{
    struct JsonObject
    {
        public string name;
        public Vector3 position;
    }
    struct JsonArray
    {
        public string name;
        public Vector3[] position;
    }

    public Text notificationText;
    private StartEditor editor;
    private string nameFile;
    private int groundLayer;
    private int enemiesLayer;
    private int objectLayer;


    // Start is called before the first frame update
    void Start()
    {
        nameFile = "map";
        if(DataScenes.isEditor)
        {
            editor = GameObject.Find("StartStop").GetComponent<StartEditor>();
        }        
        groundLayer = LayerMask.NameToLayer("Ground");
        enemiesLayer = LayerMask.NameToLayer("Enemies");
        objectLayer = LayerMask.NameToLayer("Object");
    }

    //запущена ли карта? останавливает её
    private void checkEditor()
    {
        if(DataScenes.isEditor)
        {
            if (editor.IsStart == true)
            {
                Debug.Log("Остановлено");
                editor.PressButton();
            }
        }        
    }

    //возвращает в главное меню
    public void BackMainMenu()
    {
        DataScenes.isEditor = false;
        SceneManager.LoadScene(0);
    }

    //запукает кастомную карту
    public void CreateCustomGame(string nameMap)
    {
        DataScenes.nameMap = nameMap;
        DataScenes.client = false;
        SceneManager.LoadScene("CustomMap");
    }

    //получает имя файла для сохранения
    public void GetFileName(InputField str)
    {
        if(str.text == "" && str.text.Length>10)
        {
            return;
        }else
        {
            nameFile = str.text.ToLower();
        }
        SaveToFile();
    }
    //открывает или закрывает окно для ввода имя файла и отключает выбор элементов из меню
    public void SetCanvas(GameObject canvas)
    {
        canvas.SetActive(!canvas.activeInHierarchy);
        notificationText.text = "";
        GameObject camera = GameObject.Find("Main Camera");
        if(editor.IsStart)
        {
            editor.PressButton();
        }
        SelectMouseItem selectItem = camera.GetComponent<SelectMouseItem>();
        selectItem.enabled = !selectItem.enabled;
        MoveCamera moveCamera = camera.GetComponent<MoveCamera>();
        moveCamera.enabled = !moveCamera.enabled;
    }

    //получаем список имеющихся карт
    public void GetExistingMap(GameObject scrollView)
    {
        try
        {
            string[] files = Directory.GetFiles("Map");
            RectTransform rect = scrollView.GetComponent<RectTransform>();
            GameObject prefabButton = Resources.Load("Prefabs/MapButton") as GameObject;
            float y = prefabButton.transform.position.y;
            float d = 80;

            rect.sizeDelta = new Vector2(0,files.Length * d + 100);
            foreach (string file in files)
            {
                GameObject button = Instantiate(
                    prefabButton,
                    new Vector3(prefabButton.transform.position.x,y,prefabButton.transform.position.z),
                    Quaternion.identity
                    );
                button.transform.SetParent(scrollView.transform,false);
                
                Text nameMap = button.GetComponentInChildren<Text>();
                nameMap.text = file.Replace(".json", "").Replace("Map\\","");


                Button buttonComponent = button.GetComponent<Button>();
                if(DataScenes.isEditor)
                {
                    buttonComponent.onClick.AddListener(delegate { LoadFromFile((string)nameMap.text); });
                }else
                {
                    buttonComponent.onClick.AddListener(() => CreateCustomGame(nameMap.text));
                }                
                y -= d;
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }
     
    private void WriteToFileObject(StreamWriter sw, GameObject item)
    {
        JsonObject temp = new JsonObject
        {
            name = item.name,
            position = item.transform.position
        };
        string jsonString = JsonUtility.ToJson(temp);
        sw.WriteLine(jsonString);
        sw.Flush();
    }
    private void WriteToFileArray(StreamWriter sw, GameObject[] arrayObject,string nameObject)
    {
        Vector3[] arrayVector = new Vector3[arrayObject.Length];
        for(int i =0;i<arrayObject.Length;i++)
        {
            arrayVector[i] = arrayObject[i].transform.position;
        }
        JsonArray temp = new JsonArray
        {
            name = nameObject,
            position = arrayVector
        };
        string jsonString = JsonUtility.ToJson(temp);
        sw.WriteLine(jsonString);
        sw.Flush();
    }

    private void ReadFromFile()
    {

    }

    public void SaveToFile()
    {
        checkEditor();
        try
        {
            GameObject[] finish = GameObject.FindGameObjectsWithTag("Finish");
            GameObject[] respawn = GameObject.FindGameObjectsWithTag("Respawn");

            if (respawn.Length != 1)
            {
                notificationText.text = "The map is not saved.\nAdd start";
                Debug.Log("Должен быть один старт");
                return;
            }
            if (finish.Length != 1)
            {
                notificationText.text = "The map is not saved.\nAdd finish";
                Debug.Log("Должен быть один финиш");
                return;
            }
            

            GameObject[] gameObjectsInScenes = FindObjectsOfType<GameObject>();
            FileStream fs = new FileStream("Map/" + nameFile + ".json", FileMode.OpenOrCreate,FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            foreach (GameObject item in gameObjectsInScenes)
            {
                if (item.tag == "movePlatform")
                {
                    MovePlatform platformScript = item.GetComponentInChildren<MovePlatform>();
                    WriteToFileArray(sw, platformScript.pathElements, "Chain");
                    continue;
                }
                if (item.layer == groundLayer || item.layer == enemiesLayer || item.layer == objectLayer || item.tag == "Finish" || item.tag == "Respawn")
                {
                    if (item.transform.parent == null)
                    {
                        WriteToFileObject(sw, item);
                    }
                }

            }
            sw.Close();
            fs.Close();
            notificationText.text = "Map saved successfully";
            Debug.Log("Карта успешно сохранена");
        }catch(IOException e)
        {
            Debug.Log(e.Message);
        }
    }

    public void LoadFromFile(string nameMap)
    {
        checkEditor();
        if (DataScenes.isEditor)
        {
            GameObject[] gameObjectsInScenes = FindObjectsOfType<GameObject>();
            foreach (GameObject item in gameObjectsInScenes)
            {
                if (item.layer == groundLayer || item.layer == enemiesLayer || item.layer== objectLayer)
                {
                    Destroy(item);
                }
            }
        }

        try
        {
            FileStream fs = new FileStream("Map/" + nameMap + ".json", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            while (!sr.EndOfStream)
            {
                string jsonString = sr.ReadLine();
                JsonArray tempArray=new JsonArray();
                GameObject prefab;
                JsonObject tempObject = JsonUtility.FromJson<JsonObject>(jsonString);
                if(tempObject.position==default)
                {
                    tempArray = JsonUtility.FromJson<JsonArray>(jsonString);
                    prefab = Resources.Load("Prefabs/" + tempObject.name) as GameObject;
                }
                else
                {
                    if ((prefab = Resources.Load("Prefabs/" + tempObject.name) as GameObject) != null)
                    {
                        prefab = Instantiate(prefab, tempObject.position, Quaternion.identity);
                    }
                    else if ((prefab = Resources.Load("Prefabs/PrefabsTerrain/" + tempObject.name) as GameObject) != null)
                    {
                        prefab = Instantiate(prefab, tempObject.position, Quaternion.identity);
                    }
                    prefab.name = prefab.name.Replace("(Clone)", "");
                }                
                if (prefab.tag == "BlueBird")
                {
                    SelectMouseItem.InitObjectBird(prefab);
                }
                if(prefab.tag == "Chicken")
                {
                    SelectMouseItem.InitObjectChicken(prefab);
                }
                if(prefab.tag == "Chain")
                {
                    GameObject platform;
                    List<GameObject> pointLine = new List<GameObject>();
                    GameObject emptyObject = SelectMouseItem.CreateMovePlatform(
                        tempArray.position[0].x,
                        tempArray.position[0].y,
                        objectLayer,
                        out platform);
                    List<GameObject> pointPlatform = new List<GameObject>();
                    for (int i = 0;i<tempArray.position.Length-1;i++)
                    {
                        pointLine.Clear();
                        SelectMouseItem.PaintLine(
                            tempArray.position[i].x,
                            tempArray.position[i].y,
                            tempArray.position[i+1].x,
                            tempArray.position[i+1].y,
                            0.05f,
                            emptyObject,
                            prefab,
                            pointLine);
                        pointPlatform.Add(pointLine[0]);                       
                    }
                    pointPlatform.Add(pointLine[pointLine.Count - 1]);
                    MovePlatform platformScript = platform.GetComponent<MovePlatform>();
                    platformScript.pathElements = pointPlatform.ToArray();
                }
            }
            fs.Close();
            sr.Close();
        }catch(IOException e)
        {
            Debug.Log(e.Message);
        }
    }

}
