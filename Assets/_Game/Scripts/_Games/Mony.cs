using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mony : MonoBehaviour
{
    [SerializeField] float speed;
    public int goldPerStack;
    [SerializeField] AnimationCurve animCurv;
    public IEnumerator GoToPlayer(Transform target, float time)
    {       
        while (Vector3.Distance(transform.position, target.position) > 0.1f)
        {       
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            yield return null;
        }
        Root.GameManager.CurrentPlayerGold += goldPerStack;
        Destroy(gameObject);
    }
}
