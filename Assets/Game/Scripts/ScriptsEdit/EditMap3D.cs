using _GAME.Scripts.Controllers;
using DenkKits.GameServices.Audio.Scripts;
using Sirenix.OdinInspector;
using System.IO;
using UnityEngine;

public class EditMap3D : MonoBehaviour
{
    public GameObject _prefGround;
    public Transform parentGround;
    public EditorMapUI editorMapUI;

    [Space(2.0f)]
    [Header("PREFABS")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject _prefPallet;
    [SerializeField] private GameObject _prefBridge;
    [SerializeField] private GameObject _prefBomb;
    [SerializeField] private GameObject _prefSaw;
    [SerializeField] private GameObject _prefSpikes;
    [SerializeField] private GameObject _prefJewel;

    [Button("Destroy Map")]
    public void Destroy()
    {
        foreach (Transform child in parentGround)
        {
            GameObject.DestroyImmediate(child.gameObject);
        }
    }

    [Button("Create 3D map")]
    public void CreateBlock3D()
    {
        CreateMap(editorMapUI._level);
    }
    [SerializeField] private MapData curentmapJson;
    public void CreateMap(int level)
    {
        LoadJsonMap(level);
        //GenerateGroundMap();
    }

    private void LoadJsonMap(int level)
    {
        string path = $"Assets/Resources/puzzles/custom_map_{level}.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            curentmapJson = JsonUtility.FromJson<MapData>(json);
        }
        else
        {
            Debug.LogError("File not found: " + path);
        }
    }

    //private void GenerateGroundMap()
    //{
    //    MeshRenderer renderer = _prefGround.GetComponent<Ground>().MeshRenderer;
    //    Bounds bounds = renderer.bounds;
    //    int index = -1;
    //    for (int i = 0; i < curentmapJson.rows; i++)
    //    {
    //        for (int j = 0; j < curentmapJson.cols; j++)
    //        //for (int j = curentmapJson.cols - 1; j >= 0; j--)
    //        {
    //            index++;

    //            //float x = (int)(i * bounds.size.x);
    //            //float z = (j * bounds.size.z);
    //            //float z = (int)(i * bounds.size.z);
    //            //float x = (j * bounds.size.x);
    //            //Vector3Int cellPos = new Vector3Int((int)x, 0, (int)z);

    //            float z = (int)(i * 2);
    //            float x = (j * 2);
    //            Vector3 cellPos = new Vector3((int)x, 0, (int)z);

    //            MapType type = (MapType)curentmapJson.map[index];
    //            if (type == MapType.None) continue;
    //            if (type != MapType.None && type != MapType.Bridge)
    //            {
    //                //Create ground
    //                var go = Instantiate(_prefGround, parentGround);
    //                go.transform.localPosition = cellPos;
    //            }
    //        }
    //    }

    //    GenerateItemsMap();
    //}

    //private void GenerateItemsMap()
    //{
    //    MeshRenderer renderer = _prefGround.GetComponent<Ground>().MeshRenderer;
    //    Bounds bounds = renderer.bounds;
    //    int index = -1;
    //    for (int i = 0; i < curentmapJson.rows; i++)
    //    {
    //        for (int j = 0; j < curentmapJson.cols; j++)
    //        {
    //            index++;
    //            float z = (int)(i * bounds.size.z);
    //            float x = (j * bounds.size.x);
    //            Vector3 cellPos = new Vector3((int)x, 0, (int)z);

    //            MapType type = (MapType)curentmapJson.map[index];
    //            if (type != MapType.None && type != MapType.Ground)
    //            {
    //                GameObject pref = null;
    //                switch (type)
    //                {
    //                    case MapType.First:
    //                        player.transform.localPosition = cellPos;
    //                        return;
    //                    case MapType.Pallet:
    //                        pref = _prefPallet; break;
    //                    case MapType.Bridge:
    //                        pref = _prefBridge; break;
    //                    case MapType.Bomb:
    //                        pref = _prefBomb; break;
    //                    case MapType.Saw:
    //                        pref = _prefSaw; break;
    //                    case MapType.Spikes:
    //                        pref = _prefSpikes; break;
    //                    case MapType.Jewel:
    //                        pref = _prefJewel; break;
    //                    default: continue;
    //                }
    //                var go = Instantiate(pref, parentGround);
    //                go.transform.localPosition = cellPos;
    //            }
    //        }
    //    }
    //}
}
