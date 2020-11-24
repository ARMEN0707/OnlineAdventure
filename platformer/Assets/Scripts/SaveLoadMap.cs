using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Text.Json;
using System.IO;

public class SaveLoadMap : MonoBehaviour
{
    struct JsonTerrain
    {
        public string name;
        public Vector3 position;
    }

    private int groundLayer;
    private int enemiesLayer;

    // Start is called before the first frame update
    void Start()
    {
        groundLayer = LayerMask.NameToLayer("Ground");
        enemiesLayer = LayerMask.NameToLayer("Enemies");
    }

    private void WriteToFile(StreamWriter sw, GameObject item)
    {
        JsonTerrain temp = new JsonTerrain {
            name = item.name,
            position = item.transform.position
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
        try
        {
            GameObject[] finish = GameObject.FindGameObjectsWithTag("Finish");
            GameObject[] respawn = GameObject.FindGameObjectsWithTag("Respawn");

            if(finish.Length != 1)
            {
                Debug.Log("Должен быть один финиш");
                return;
            }
            if (respawn.Length != 1)
            {
                Debug.Log("Должен быть один старт");
                return;
            }



            GameObject[] gameObjectsInScenes = FindObjectsOfType<GameObject>();
            FileStream fs = new FileStream("map.json", FileMode.OpenOrCreate,FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            foreach (GameObject item in gameObjectsInScenes)
            {
                if (item.layer == groundLayer || item.layer == enemiesLayer || item.tag == "Finish" || item.tag == "Respawn")
                {
                    if (item.transform.parent == null)
                    {
                        WriteToFile(sw, item);
                    }
                }
            }
            sw.Close();
            fs.Close();
        }catch(IOException e)
        {
            Debug.Log(e.Message);
        }
    }

    public void LoadFromFile()
    {
        GameObject[] gameObjectsInScenes = FindObjectsOfType<GameObject>();
        foreach(GameObject item in gameObjectsInScenes)
        {
            if(item.layer == groundLayer || item.layer==enemiesLayer)
            {
                Destroy(item);
            }
        }

        try
        {
            FileStream fs = new FileStream("map.json", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            while (!sr.EndOfStream)
            {
                string jsonString = sr.ReadLine();
                JsonTerrain tempObject = JsonUtility.FromJson<JsonTerrain>(jsonString);
                GameObject prefab;

                if ((prefab = Resources.Load("Prefabs/" + tempObject.name) as GameObject) != null)
                {
                    prefab = Instantiate(prefab, tempObject.position, Quaternion.identity);
                }
                else if ((prefab = Resources.Load("Prefabs/PrefabsTerrain/" + tempObject.name) as GameObject) != null)
                {
                    prefab = Instantiate(prefab, tempObject.position, Quaternion.identity);
                }
                prefab.name = prefab.name.Replace("(Clone)", "");
                if (prefab.tag == "BlueBird")
                {
                    SelectMouseItem.InitObjectBird(prefab);
                }
                if(prefab.tag == "Chicken")
                {
                    SelectMouseItem.InitObjectChicken(prefab);
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
