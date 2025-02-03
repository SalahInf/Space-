using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StuckMony : MonoBehaviour
{
    public List<Mony> mony = new List<Mony>();
    public List<Mony> monyColectibles = new List<Mony>();
    [SerializeField] Transform parent;
    [SerializeField] Transform startPos;
    //[SerializeField] float offset;

    [SerializeField] Mony monyObj;

    [SerializeField] int testCount;

    [SerializeField] Vector3 offset;

    [SerializeField] Vector3 Target;
    public int index;
    public int count;
    public bool isStacked;
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.M))
        //    StartCoroutine(Stack(testCount));
    }
    public IEnumerator Stack(int MonyCount)
    {
        if (MonyCount > 0)
        {
            for (int i = 0; i < MonyCount; i++)
            {
                Mony m = Instantiate(monyObj, startPos.position, Quaternion.identity, parent);
                mony.Add(m);
            }
            isStacked = true;
            for (int i = 0; i < MonyCount; i++)
            {
                if (i == 0)
                {
                    Target.x = offset.x;
                    Target.z = offset.z;
                }
                else
                {
                    if (index % 3 == 0 && i != 0)
                    {
                        Target.x -= offset.x;
                        Target.z = offset.z;
                    }
                    else
                    {
                        Target.z -= offset.z;
                    }
                }
                if (index == 9)
                {
                    Target.x = offset.x;
                    Target.z = offset.z;
                    Target.y += offset.y;
                    index = 0;
                }
                if (isColecting)
                    break;

                mony[i].transform.DOLocalRotate(Vector3.zero, 0.15f).SetEase(Ease.Flash).SetId(i);
                mony[i].transform.DOLocalJump(Target, 1.5f, 1, 0.3f).SetEase(Ease.Flash).SetId(i);
                index++;
                if (count++ > (MonyCount))
                    break;
                yield return new WaitForSeconds(0.01f);
            }
            //monyToWin += MonyCount;
        }
    }

    public void SMony(int ctr)
    {
        if (isColecting)
            return;

        StartCoroutine(Stack(ctr));
    }
    //public IEnumerator AddToStuck(int MonyCount)
    //{
    //    int index = 0;
    //    monyToWin += MonyCount;
    //    int count = mony[mony.Count - 1].Count;
    //    for (int i = 0; i < MonyCount; i++)
    //    {
    //        List<GameObject> g = new List<GameObject>();
    //        index = mony.Count - 1;

    //        if (count < 9)
    //        {
    //            if ((mony[index].Count - 1) % 3 == 0)
    //            {
    //                Target.x -= offset.x;
    //                Target.z = offset.z * 2;
    //            }
    //            else
    //            {
    //                Target.z -= offset.z;
    //            }
    //            count++;
    //            AddMony();
    //            yield return new WaitForSeconds(0.08f);
    //        }
    //        else
    //        {
    //            count = 0;
    //            Target.x = offset.x * 2;
    //            Target.z = offset.z * 2;
    //            Target.y += offset.y;
    //            mony.Add(g);
    //            AddMony();
    //        }
    //    }

    //    void AddMony()
    //    {
    //        GameObject m = Instantiate(monyObj, startPos.position, Quaternion.identity, parent);
    //        mony[mony.Count - 1].Add(m);
    //        m.transform.DOLocalRotate(Vector3.zero, 0.15f).SetEase(Ease.Flash);
    //        m.transform.DOLocalJump(Target, 1.5f, 1, 0.3f).SetEase(Ease.Flash);

    //    }

    //}
    public bool isColecting;
    public IEnumerator ColectMony(PlayerController player)
    {
        isColecting = true;
        Target = Vector3.zero;
        index = 0;
        count = 0;
        for (int i = mony.Count - 1; i >= 0; i--)
        {
            DOTween.Kill(i);
            StartCoroutine(mony[i].GoToPlayer(Root.Upgrademanager.playerController.m_startLine, 0));
            yield return new WaitForSeconds(0.05f);
            //yield return null;
        }
        mony.Clear();
        isStacked = false;
        player.isColecting = false;
        isColecting = false;
        Root.GameManager.EndTaksTutorial(2);
    }
}
