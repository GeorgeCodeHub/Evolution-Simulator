using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChickenStatsUI : MonoBehaviour
{
    public static List<float> SizeList = new List<float>() { 0 };
    public static List<float> SpeedList = new List<float>() { 0 };
    public static List<float> CharacterCountList = new List<float>() { 0 };
    public static int statCounter = 0;

    private void Update()
    {
        if(LevelManager.reset == true)
        {
            SizeList = new List<float>() { 0 };
            SpeedList = new List<float>() { 0 };
            CharacterCountList = new List<float>() { 0 };
        }
    }

    void FixedUpdate()
    {
        Chicken[] chickens = FindObjectsOfType<Chicken>();
        float speedSum = 0;
        float detectionRangeSum = 0;

        foreach (Chicken chicken in chickens)
        {
            speedSum += chicken._MoveSpeed;
            detectionRangeSum += chicken._DetectionRange;
        }

        float tempSize = Mathf.Round(detectionRangeSum / chickens.Length * 10.0f) * 0.1f;
        float tempSpeed = Mathf.Round(speedSum / chickens.Length * 10.0f) * 0.1f;
        float tempCount = Mathf.Round(chickens.Length * 10.0f) * 0.1f;

        if(float.IsNaN(tempSize) == false)
        if (SizeList[SizeList.Count-1] != tempSize  || SpeedList[SpeedList.Count-1] != tempSpeed || CharacterCountList[CharacterCountList.Count-1] != tempCount)
        {
            statCounter++;

            SizeList.Add(tempSize);
            SpeedList.Add(tempSpeed);
            CharacterCountList.Add(tempCount);
        }
    }
}
