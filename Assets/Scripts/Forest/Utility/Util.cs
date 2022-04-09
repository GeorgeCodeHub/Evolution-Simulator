using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening.Core;
using DG.Tweening;

public class Util
{

    public static bool isInRange(Vector3 origin, Vector3 target, float range)
    {
        return Vector3.Distance(origin, target) <= range;
    }
    public static T FindClosestObj<T>(Transform origin, float range) where T : MonoBehaviour
    {
        Collider[] collidersInRange = Physics.OverlapSphere(origin.position, range);
        List<T> objList = new List<T>();
        for (int i = 0; i < collidersInRange.Length; i++)
        {
            T obj = collidersInRange[i].GetComponent<T>();
            if (obj != null)
                objList.Add(obj);
        }

        objList.Sort((a, b) =>
        {
            float distA = Vector3.SqrMagnitude(origin.position - a.transform.position);
            float distB = Vector3.SqrMagnitude(origin.position - b.transform.position);
            if (distA < distB)
                return -1;
            else if (distA == distB)
                return 0;
            else return 1;
        }
        );

        if (objList.Count == 0)
            return null;
        else
            return objList[0];
    }

    public static Transform FindClosestObj(Transform origin, float range, string tag)
    {
        Collider[] collidersInRange = Physics.OverlapSphere(origin.position, range);
        List<Transform> objList = new List<Transform>();
        for (int i = 0; i < collidersInRange.Length; i++)
        {
            Transform obj = collidersInRange[i].transform;
            if (obj.tag.Equals(tag) && obj.transform != origin.transform)
                objList.Add(obj);
        }

        objList.Sort((a, b) =>
        {
            float distA = Vector3.SqrMagnitude(origin.position - a.transform.position);
            float distB = Vector3.SqrMagnitude(origin.position - b.transform.position);
            if (distA < distB)
                return -1;
            else if (distA == distB)
                return 0;
            else return 1;
        }
        );

        if (objList.Count == 0)
            return null;
        else
            return objList[0];
    }

    public static void PrintMessageToScreen(string msg)
    {
        Text txt = GameObject.Find("ErrorMessageText").GetComponent<Text>();
        DOTween.Kill("MessageID");
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);
        txt.text = msg;
        Sequence sq = DOTween.Sequence();
        sq.SetId<Sequence>("MessageID");
        sq.Append(DOTween.ToAlpha(() => txt.color, x => txt.color = x, 1, 0.5f));
        sq.AppendInterval(1);
        sq.Append(DOTween.ToAlpha(() => txt.color, x => txt.color = x, 0, 0.5f));
    }
}
