using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BattleScene
{

    public float game_speed { get; set; } = 1f;

    public void SaveScene(List<Transform> scene_object_list) {
        SceneData scene_data = new();
        SceneAssetsData assets_data = new();

        foreach (Transform child in scene_object_list) {
            string prefab_name = child.name.Split(' ')[0];

            SceneObjectData test_object_data = new(
                child.name,
                child.position,
                child.rotation.eulerAngles,
                child.localScale,
                "Prefabs/" + prefab_name
            );
            assets_data.objects.Add(test_object_data);
        }
        scene_data.assets = assets_data;

        string json_string = JsonUtility.ToJson(scene_data);
        System.IO.File.WriteAllText(Application.dataPath + "/GameAssets/Resources/SceneData/BattleSceneData.json", json_string);
        Debug.Log("JSON saved to " + Application.persistentDataPath);
    }

    public SceneData LoadScene() {
        TextAsset json_file = Resources.Load<TextAsset>("SceneData/BattleSceneData");
        if (json_file != null) {
            return JsonUtility.FromJson<SceneData>(json_file.text);
        }
        return null;
    }

}

[System.Serializable]
public class SceneObjectData {
    public string name;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public string sourcePath;

    public SceneObjectData(string _name, Vector3 _position, Vector3 _rotation, Vector3 _scale, string _sourcePath) {
        name = _name;
        position = _position;
        rotation = _rotation;
        scale = _scale;
        sourcePath = _sourcePath;
    }
}

[System.Serializable]
public class SceneAssetsData
{
    public List<SceneObjectData> objects = new();
}

[System.Serializable]
public class SceneData
{
    public SceneAssetsData assets = new();
}