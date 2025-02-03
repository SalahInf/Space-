using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStuns : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] float distance;
    [SerializeField] LayerMask layer;
    RaycastHit hit;
    Vector3 endforwaord = Vector3.zero;
    [SerializeField] float offset;
    public bool isIn;
    [SerializeField] float time = 1.2f;
    private void Update()
    {
        RaycastCheck();
    }
    void RaycastCheck()
    {
        bool ray = Physics.Raycast(transform.position + Vector3.up * 0.2f, transform.forward, out hit, distance, layer);

        if (ray && !isIn)
        {
            isIn = true;
            Collider col = hit.collider;
            StartCoroutine(Jump((SphereCollider)col));
        }
    }

    IEnumerator Jump(SphereCollider colider)
    {
        float height = colider.transform.position.y + (colider.transform.localScale.y / 2);
        float whith = Mathf.Abs(Vector3.Dot(hit.normal, colider.transform.forward)) > 0.8f ? colider.transform.localScale.x : colider.transform.localScale.z;
        Vector3 endHeight = new Vector3(hit.point.x, height, hit.point.z);
        endforwaord = transform.position + (-hit.normal.normalized * (whith + offset));
        player.animator.SetTrigger("Jump");
        yield return new WaitForSeconds(0.15f);
        endforwaord.y = transform.position.y;
        Vector3 dir = (endHeight - transform.position).normalized;
        player._rb.AddForce(dir * time, ForceMode.Impulse);
    }
    public void RestRay() => isIn = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position + Vector3.up * 0.1f, transform.forward * distance);
        Gizmos.DrawSphere(endforwaord, 0.2f);
    }
}
