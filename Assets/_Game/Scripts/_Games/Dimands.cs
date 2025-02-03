using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dimands : MonoBehaviour
{
    public ParticleSystem explosion;

    [SerializeField] Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] float lenght;
    [SerializeField] float m_forceExplode;
    public MeshRenderer mesh;
    bool isMoving;
    Vector3 direction;
    public IEnumerator MoveToward(PlayerController target)
    {
        transform.parent = null;
        //float distance = 999;
        //rb.isKinematic = false;
        //rb.AddForce(Vector3.up * m_forceExplode, ForceMode.Impulse);
        //rb.AddTorque(Vector3.right * m_forceExplode * 10, ForceMode.Impulse);
        yield return new WaitForSeconds(0.1f);
        //isMoving = true;
        //while (distance > 0.1f)
        //{
        //    Vector3 playerTarget = target.transform.position + Vector3.up * 0.5f;
        //    distance = Vector3.Distance(transform.position, playerTarget);
        //    Vector3 dir = playerTarget - transform.position;
        //    direction = Vector3.Slerp(transform.forward, dir, 0.75f);
        //    yield return null;
        //}
        target.TextPupUp();
        Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        //if (isMoving)
        //    rb.velocity = direction * speed;
    }

}
