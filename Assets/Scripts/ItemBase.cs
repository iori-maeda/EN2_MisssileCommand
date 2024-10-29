using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
abstract public class ItemBase : MonoBehaviour
{
    [SerializeField] protected float speed_ = 3.0f;
    private Vector3 direction_;

    protected Camera mainCamera_ = null;
    protected Collider2D collider_ = null;

    private void Awake()
    {
        mainCamera_ = Camera.main;
        collider_ = GetComponent<Collider2D>();
        direction_ = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (direction_ == Vector3.zero)
        {
            direction_ = Vector3.right;
        }
        // 移動
        transform.Translate(direction_ * speed_ * Time.deltaTime);

        // ワールド座標上からカメラの右端を計算
        float worldScreenRight = mainCamera_.orthographicSize * mainCamera_.aspect;

        float boundsSizeX = collider_.bounds.size.x;
        if (transform.position.x - boundsSizeX > worldScreenRight)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion"))
        {
            Get();
        }
    }

    public abstract void Get();
    public virtual void SetDir(Vector3 dir)
    {
        direction_ = dir;
    }
}
