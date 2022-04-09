using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour, IObserver
{
    //Food info
    public GameObject food;
    public Dictionary<int, GameObject> foodList = new Dictionary<int, GameObject>();
    private int foodCount = 40; //Starting food count, but used whenever new food created

    //Character related Prefabs
    public GameObject character;


    //Character info
    public List<GameObject> characterList = new List<GameObject>();
    private int characterCount = 100; //Starting character count, however, not reused again

    private float startingSpeed;
    private float startingSize;
    private int startingQuality;
    private float startingEnergy;
    private float startingRandom;


    //Life cycle info
    private int expendedChars;
    public static bool reset;
    private float timeToReset;
    

    //World info
    private readonly int radius = 30;
    private bool init;
    public static int dayCounter;

    //Chart measures
    public static List<float> SpeedList;
    public static List<float> SizeList;
    public static List<float> QualityList;

    //UI info
    public InputField characterCountText;
    public InputField foodCountText;
    public InputField StartEnergyText;
    public Slider startSpeedText;
    public Slider startSizeText;
    public Slider startQualityText;
    public Slider startRandomText;
    public TextMeshProUGUI DayCounter;

    public Button startButton;
    public Button resetButton;

    public Image navPanel;
    public Image blackPanel;
    public Image setupPanel;

    //Camera Functions
    public CameraController cameraStats;

    void Awake()
    {
        cameraStats = FindObjectOfType<CameraController>();
    }


    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartClick);
        resetButton.onClick.AddListener(ResetClick);

        //Chart measures
        SpeedList = new List<float>() { };
        SizeList = new List<float>() { };
        QualityList = new List<float>() { };
        
        dayCounter = 0;
        expendedChars = 0;
        reset = false;
        timeToReset = 0;
        init = false;

        cameraStats.FreezeCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if (reset == true)
        {
            timeToReset += Time.deltaTime;
            if(timeToReset >= 1)
            {
                reset = false;
                timeToReset = 0;

                dayCounter++;
                //This changes the day counter everytime a loop ends
                DayCounter.text = "DAY " + dayCounter.ToString();


                MakeFood(foodCount);
                if(init == false)
                {
                    //Intial values
                    SpeedList.Add(startingSpeed * 100);
                    SizeList.Add(startingSize * 100);
                    QualityList.Add(startingQuality);


                    SpawnCharacters(characterCount);
                    init = true;
                }
                else
                {
                    //Calculate average stats from current day
                    float averageSpeed = 0;
                    float averageSize = 0;
                    float averageQuality = 0;

                    for (int i=0; i< characterList.Count; i++)
                    {
                        averageSpeed += characterList[i].GetComponent<CharacterMovement>().GetSpeed();
                        averageSize += characterList[i].GetComponent<CharacterMovement>().GetSize();
                        averageQuality += characterList[i].GetComponent<CharacterMovement>().GetQuality();
                    }

                    SpeedList.Add((averageSpeed / characterList.Count) * 100);
                    SizeList.Add((averageSize / characterList.Count) * 100);
                    QualityList.Add((averageQuality / characterList.Count));

                    MutateCharacters();
                }
                
                
            }
        }
    }


    //UI Button related Functions


    private void StartClick()
    {
        characterCount = int.Parse(characterCountText.text);
        foodCount = int.Parse(foodCountText.text);

        startingSpeed = startSpeedText.value;
        startingSize = startSizeText.value;
        startingQuality = (int) startQualityText.value;
        startingEnergy = int.Parse(StartEnergyText.text);
        startingRandom = startRandomText.value;

        reset = true;
        startButton.gameObject.SetActive(false);
        navPanel.gameObject.SetActive(true);
        blackPanel.gameObject.SetActive(false);
        setupPanel.gameObject.SetActive(false);

        cameraStats.UnFreezeLab();
    }

    private void ResetClick()
    {
        //Destroy all characters
        for (int i = characterList.Count - 1; i >= 0; i--)
        {
            GameObject go = characterList[i];
            DestroyCharacter(go, i);
        }

        //Destroy all food
        DestroyFood();

        //Ensure all time and spawn related stats are also reset
        expendedChars = 0;
        reset = false;
        timeToReset = 0;
        init = false;
        dayCounter = 0;

        SpeedList = new List<float>() { };
        SizeList = new List<float>() { };
        QualityList = new List<float>() { };


        startButton.gameObject.SetActive(true);
        navPanel.gameObject.SetActive(false);
        blackPanel.gameObject.SetActive(true);
        setupPanel.gameObject.SetActive(true);

        cameraStats.FreezeCamera();
    }


    //Character related Functions


    //Method to create a new character
    //Is affected by the randomness value
    private void CreateCharacter()
    {
        GameObject prefab;
        
        prefab = Instantiate(character);
        
        float rndSpeed = startingRandom / 10;
        float rndSize = startingRandom / 25;
        int rndQuality = (int) System.Math.Round(startingRandom / 20, 0);

        
        //float speed, float size, int quality, float energy
        prefab.GetComponent<CharacterMovement>().SetStats(startingSpeed + Random.Range(-rndSpeed, rndSpeed),
                                                          startingSize + Random.Range(-rndSize, rndSize), 
                                                          startingQuality + Random.Range(-rndQuality, rndQuality), 
                                                          startingEnergy);
        prefab.GetComponent<CharacterMovement>().sub.AddObserver(this);
        RepositionCharacter(prefab);

        prefab.transform.parent = this.transform;
        characterList.Add(prefab);

        //Although this should be called in the character itself, for init purposes, I want the statistics to work properly and hacked the first time in here.
        prefab.GetComponent<CharacterMovement>().ChangeColor();

    }

    // Method to use a preset character to make a new character from
    // is not affected by the randomizer.
    private GameObject CreateCharacter(GameObject go)
    {
        GameObject newGO = Instantiate(go);
        
        newGO.GetComponent<CharacterMovement>().SetStats(
            go.GetComponent<CharacterMovement>().GetSpeed(),
            go.GetComponent<CharacterMovement>().GetSize(),
            go.GetComponent<CharacterMovement>().GetQuality(), 
            startingEnergy);
        newGO.GetComponent<CharacterMovement>().sub.AddObserver(this);
        RepositionCharacter(newGO);

        newGO.transform.parent = this.transform;
        characterList.Add(newGO);

        newGO.GetComponent<CharacterMovement>().ChangeColor();    

        return newGO;
    }

    //Method to destroy a character
    private void DestroyCharacter(GameObject go, int i)
    {
        go.GetComponent<CharacterMovement>().sub.RemoveObserver(this);
        characterList.RemoveAt(i);
        Destroy(go.GetComponent<CharacterMovement>().statter);
        Destroy(go);
    }


    //Method used in start to create the first characters
    private void SpawnCharacters(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            CreateCharacter();
        }
    }

    //Method that visits all characters food collection and mutates/reproduces/kills based on results
    //Make sure that all characters after mutations are then properly relocated
    private void MutateCharacters()
    {
        for (int i = characterList.Count - 1; i >= 0; i--)
        {
            GameObject go = characterList[i];

            //Not enough food
            if(go.GetComponent<CharacterMovement>().foodCollected < go.GetComponent<CharacterMovement>().foodNeeded/2)
            {
                DestroyCharacter(go, i);
            }

            //Best case scenario, enough food to reproduce
            else if(go.GetComponent<CharacterMovement>().foodCollected >= go.GetComponent<CharacterMovement>().foodNeeded)
            {
                RepositionCharacter(go);

                GameObject newGO = CreateCharacter(go);

                go.GetComponent<CharacterMovement>().UpdateBiases();
                newGO.GetComponent<CharacterMovement>().UpdateBiases();

                go.GetComponent<CharacterMovement>().Mutate();
                newGO.GetComponent<CharacterMovement>().Mutate();

                go.GetComponent<CharacterMovement>().ResetEnergy();
                newGO.GetComponent<CharacterMovement>().ResetEnergy();
            }

            //Midcase, enough food to survive
            else
            {
                RepositionCharacter(go);
                go.GetComponent<CharacterMovement>().ResetEnergy();
            }

        }
    }

    private void RepositionCharacter(GameObject go)
    {
        float radian = Random.Range(0, 2 * Mathf.PI);

        float sin = Mathf.Sin(radian) * (radius / 2 - 0.5f);
        float cos = Mathf.Cos(radian) * (radius / 2 - 0.5f);

        go.transform.position = new Vector3(sin, 0.5f, cos);

    }

    //Separate Observer when a character runs out of energy
    public void SubjectUpdate(object sender)
    {
        expendedChars += 1;

        //Start resetting process
        if (expendedChars == characterList.Count)
        {
            expendedChars = 0;
            reset = true;
        }
    }

    /*-------------------------------------------------------*/
    /*----------------Food related Functions-----------------*/
    /*-------------------------------------------------------*/

    //Method to clear and respawn as much food as is needed.
    private void MakeFood(int amount)
    {
        DestroyFood();
        AddFood(amount);
    }

    //Method to delete all food currently on the field
    private void DestroyFood()
    {
        //Dictionary variation
        foreach (KeyValuePair<int, GameObject> entry in foodList)
            Destroy(entry.Value);
        foodList.Clear();


    }

    //Method that, instead of resetting and creating food, only adds a smaller amount
    private void AddFood(int amount)
    {
        //Add the actual food
        for (int i = 0; i < amount; i++)
        {
            Vector2 area = Random.insideUnitCircle * (radius / 2 - 2f);
            GameObject prefab = Instantiate(food,
                                            new Vector3(area.x, 0.5f, area.y),
                                            Quaternion.identity);
            prefab.transform.parent = this.transform;
            prefab.GetComponent<Food>().SetID(i);

            foodList[i] = prefab; //Dictionary version
        }
        //DetectFood();
    }

    public void DetectFood()
    {
        //Notify all players that new food has been created
        foreach (GameObject character in characterList)
        {
            character.GetComponent<CharacterMovement>().DetectFood(false);
        }
    }

}
