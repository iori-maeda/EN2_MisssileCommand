using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int chainNum = 0;

    [SerializeField] private float maxLifeTime_ = 1;
    [SerializeField] private float time_ = 0;
    [SerializeField] private Vector3 maxScale_ = new Vector3(3.0f, 3.0f, 3.0f);

    protected float taimeRatio_ { get { return time_ / maxLifeTime_; } }

    // Start is called before the first frame update
    void Start()
    {
        time_ = maxLifeTime_;
    }

    // Update is called once per frame
    void Update()
    {
        time_ -= Time.deltaTime;
        ScaleUp();
        if (time_ > 0.0f) { return; }
        Destroy(gameObject);
    }

    protected virtual void ScaleUp()
    {
        transform.localScale = maxScale_ * (1.0f - time_ / maxLifeTime_);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Item") || collision.CompareTag("Meteor"))
        {
            chainNum++;
        }
    }
}
