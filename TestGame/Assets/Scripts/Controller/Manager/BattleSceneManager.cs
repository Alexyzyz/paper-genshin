using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleSceneManager : MonoBehaviour
{
    
    public static BattleSceneManager Instance;

    private BattleScene Model = new();

    public PlayerController player_controller { get; private set; }
    public List<EnemyController> enemy_controller_list { get; } = new();
    private Transform scene_object_parent;

    [SerializeField]
    private IntroCameraController intro_camera;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        scene_object_parent = transform.Find("SceneObjects");
        BuildScene();
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        HandleInputStartGame();
        HandleInputQuitGame();

        HandleDebugCommands();
    }

    public float game_delta_time => Time.deltaTime * Model.game_speed;

    public float game_speed => Model.game_speed;

    public void HandleEnemyDeath(EnemyController enemy) {
        enemy_controller_list.Remove(enemy);
    }

    public void SetGameSpeed(float game_speed) => Model.game_speed = game_speed;

    public void IncreaseGameSpeed(float delta_game_speed) {
        Model.game_speed += delta_game_speed;
        Model.game_speed = Mathf.Max(0, Model.game_speed);
    }

    public void StartGame() {
        player_controller.gameObject.SetActive(true);
        foreach(EnemyController enemy_controller in enemy_controller_list) {
            enemy_controller.gameObject.SetActive(true);
        }
        intro_camera.gameObject.SetActive(false);
    }

    private void HandleInputStartGame() {
        if (!Input.GetKeyDown(GameKey.START)) return;
        intro_camera.HandleInputStartGame();
        TutorialManager.Instance.HideTutorial();
    }

    private void HandleInputQuitGame() {
        if (!Input.GetKeyDown(GameKey.QUIT)) return;
        Application.Quit();
    }

    private void HandleDebugCommands() {
        if (!Input.GetKey(KeyCode.P)) return;
        // DebugIncreaseGameSpeed();
        // DebugSaveScene();
    }

    private void DebugIncreaseGameSpeed() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            IncreaseGameSpeed(0.1f);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            IncreaseGameSpeed(-0.1f);
        }
    }

    private void DebugSaveScene() {
        if (!Input.GetKeyDown(KeyCode.L)) return;

        List<Transform> scene_object_list = new();
        foreach (Transform child in scene_object_parent) {
            scene_object_list.Add(child);
        }
        Model.SaveScene(scene_object_list);
    }

    private void BuildScene() {
        SceneData scene_data = Model.LoadScene();
        if (scene_data == null) {
            GameObject player_prefab = (GameObject)Resources.Load("Prefabs/Player");
            player_controller = Instantiate(player_prefab).GetComponent<PlayerController>();
            return;
        }

        List<SceneObjectData> object_data_list = scene_data.assets.objects;
        foreach (SceneObjectData object_data in object_data_list) {
            GameObject object_prefab = (GameObject)Resources.Load(object_data.sourcePath);
            GameObject placed_object = Instantiate(object_prefab, object_data.position, Quaternion.identity, scene_object_parent);
            placed_object.transform.eulerAngles = object_data.rotation;
            placed_object.transform.localScale = object_data.scale;

            if (object_prefab.CompareTag("Player")) {
                player_controller = placed_object.GetComponent<PlayerController>();
                placed_object.gameObject.SetActive(false);
            }
            if (object_prefab.CompareTag("Enemy")) {
                enemy_controller_list.Add(placed_object.GetComponent<EnemyController>());
                placed_object.gameObject.SetActive(false);
            }
        }
    }

}
