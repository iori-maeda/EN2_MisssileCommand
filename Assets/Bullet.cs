using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Explosion exprosionPrefab_;
    [SerializeField] float speed_ = 0.0f;
    private Vector3 direction_ = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction_ * speed_ * Time.deltaTime;
    }

    public void SetUp(Vector3 target)
    {
        direction_ = (target - transform.position).normalized;
        float angle = Mathf.Atan2(direction_.y, direction_.x)
            * Mathf.Rad2Deg - 90.0f;
        transform.Rotate(0.0f, 0.0f, angle, Space.Self);
    }

}
