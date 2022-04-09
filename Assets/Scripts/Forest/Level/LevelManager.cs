using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Vector2 _levelSize;
    public GameObject _bushPrefab;
    public GameObject _chickenPrefab;
    public LayerMask _blockingLayerMask;
    //UI info
    public InputField characterCountText;
    public InputField foodCountText;
    public Slider startSpeedText;
    public Slider startSizeText;

    public Button startButton;
    public Button resetButton;

    public Image navPanel;
    public Image blackPanel;
    public Image setupPanel;

    public int _maxBushes = 80;

    private List<Bush> _bushList;

    private float _timer = 60;

    private int foodCount = 40; //Starting food count, but used whenever new food created

    //Character info
    private GameObject[] gameObjects;
    private int characterCount = 100; //Starting character count, however, not reused again

    //Initial speed and Size
    public static float startingSpeed;
    public static float startingSize;

    //Life cycle info
    public static bool reset;
    private bool createFood;

    //Camera Functions
    public CameraController cameraStats;

    void Awake()
    {
        _bushList = new List<Bush>();
        cameraStats = FindObjectOfType<CameraController>();
    }

    void Start()
    {
        startButton.onClick.AddListener(StartClick);
        resetButton.onClick.AddListener(ResetClick);

        reset = false;
        createFood = false;

        cameraStats.FreezeCamera();
    }

    void Update()
    {
        if(reset == true)
        {
            for (int i = 0; i < characterCount; i++)
            {
                SpawnAtRandomLocation(_chickenPrefab);
            }

            for (int i = 0; i < foodCount; i++)
            {
                if (_bushList.Count < foodCount)
                {
                    Bush newBush = SpawnAtRandomLocation(_bushPrefab).GetComponent<Bush>();
                    newBush.onDie += OnBushDied;
                    _bushList.Add(newBush);
                }
            }

            reset = false;
            createFood = true;
        }

        if (createFood == true)
        {
            _timer += Time.deltaTime;
            if (_timer >= 30)
            {
                _timer = 0;

                for (int i = 0; i < foodCount; i++)
                {
                    if (_bushList.Count < foodCount)
                    {
                        Bush newBush = SpawnAtRandomLocation(_bushPrefab).GetComponent<Bush>();
                        newBush.onDie += OnBushDied;

                        _bushList.Add(newBush);
                    }
                }
            }
        }
    }

    private void StartClick()
    {
        characterCount = int.Parse(characterCountText.text);
        foodCount = int.Parse(foodCountText.text);

        startingSpeed = startSpeedText.value;
        startingSize = startSizeText.value;

        reset = true;
        startButton.gameObject.SetActive(false);
        navPanel.gameObject.SetActive(true);
        blackPanel.gameObject.SetActive(false);
        setupPanel.gameObject.SetActive(false);

        cameraStats.UnFreezeCamera();
    }

    private void ResetClick()
    {
        gameObjects = GameObject.FindGameObjectsWithTag("Chicken");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }

        gameObjects = GameObject.FindGameObjectsWithTag("Bush");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }

        //Ensure all time and spawn related stats are also reset
        reset = false;
        createFood = false;

        startButton.gameObject.SetActive(true);
        navPanel.gameObject.SetActive(false);
        blackPanel.gameObject.SetActive(true);
        setupPanel.gameObject.SetActive(true);

        cameraStats.FreezeCamera();
    }

    private GameObject SpawnAtRandomLocation(GameObject prefab)
    {
        Collider coll = prefab.GetComponent<Collider>();
        float radius = coll.bounds.extents.x > coll.bounds.extents.z ? coll.bounds.extents.x : coll.bounds.extents.z;

        for (int i = 0; i < 20; i++)
        {
            Vector3 rndPos = new Vector3(Random.Range(-_levelSize.x+1, _levelSize.x-1) / 2f, 0, Random.Range(-_levelSize.y+1, _levelSize.y-1) / 2f);
            Collider[] colliders = Physics.OverlapSphere(rndPos + coll.bounds.center, radius, _blockingLayerMask);
            if (colliders.Length == 0)
            {
                return Instantiate(prefab, rndPos, Quaternion.identity);
            }
        }
        Debug.LogWarning("Failed to spawn " + prefab.name);
        return null;
    }

    private void OnBushDied(Bush sender)
    {
        sender.onDie -= OnBushDied;
        _bushList.Remove(sender);
    }
}
