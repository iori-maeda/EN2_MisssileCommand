using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab_;

    private Vector3 targetPos_;
    private Vector3 direction_;
    private float speed_ = 5.0f;
    //private GameObject reticle_;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction_ * speed_ * Time.deltaTime;
    }

    public void SetTargetPos(Vector3 pos)
    {
        targetPos_ = pos;
        direction_ = (targetPos_ - transform.position).normalized;
        float angle = Mathf.Atan2(direction_.y, direction_.x) * Mathf.Rad2Deg + -90.0f;
        transform.Rotate(0.0f, 0.0f, angle, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Meteor")) { Instantiate(explosionPrefab_, transform.position, Quaternion.identity); }
        Destroy(gameObject);
    }
}
