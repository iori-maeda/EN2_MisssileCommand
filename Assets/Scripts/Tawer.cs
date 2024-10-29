using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tawer : MonoBehaviour
{
    private SpriteRenderer rendrer_;

    [SerializeField] private Color normalColor_ = Color.white;
    [SerializeField] private Color coolTimeColor_ = Color.gray;
    [SerializeField] private float shootCoolTime = 5.0f;
    [SerializeField] private Bullet bullet_;

    private float timer_ = 0.0f;

    public bool isShootReady { get { return timer_ <= 0.0f; } }

    // Start is called before the first frame update
    void Start()
    {
        rendrer_ = GetComponent<SpriteRenderer>();
        rendrer_.color = normalColor_;
        timer_ = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer_ > 0.0f)
        {
            timer_ -= Time.deltaTime;
        }
        ColorTransition();
    }

    private void ColorTransition()
    {
        float ratio = 1.0f - timer_ / shootCoolTime;
        rendrer_.color = Color.Lerp(coolTimeColor_, normalColor_, ratio);
    }

    public void Shoot(Vector3 targetPos)
    {
        timer_ = shootCoolTime;
        rendrer_.color = coolTimeColor_;

        Bullet bullet = Instantiate(bullet_, transform.position, Quaternion.identity);
        bullet.SetUp(targetPos);
    }
}
