using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// •K‚¸SpriteRenderer‚ðŽg—p‚·‚é
[RequireComponent(typeof(SpriteRenderer))]
public class Cannon : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] Missile missile_;

    [Header("ShotCoolTimeSrttings")]
    [SerializeField] private float shootCoolTimer_ = 5.0f;
    private float time_ = 0.0f;


    private SpriteRenderer spriteRenderer_;
    private Color normalColor_ = Color.white;
    private Color coolTimeColor_ = Color.gray;

    // Start is called before the first frame update
    void Start()
    {
        time_ = 0.0f;
        spriteRenderer_ = GetComponent<SpriteRenderer>();
        normalColor_ = spriteRenderer_.color;
    }

    // Update is called once per frame
    void Update()
    {
        time_ -= Time.deltaTime;
        time_ = time_ <= 0.0f ? 0.0f : time_;
        Color coolTimeColor = normalColor_ * (1.0f - time_ / shootCoolTimer_);
        coolTimeColor.a = 1.0f;
        spriteRenderer_.color = coolTimeColor;
    }

    public void Shoot(Vector3 target)
    {
        if (time_ > 0.0f) { return; }
        Missile missile = Instantiate(missile_, transform.position, Quaternion.identity);
        missile.SetTargetPos(target);
        CoolTimeReset();
        spriteRenderer_.color = coolTimeColor_;
    }

    void CoolTimeReset()
    {
        time_ = shootCoolTimer_;
    }

    public bool IsShootRady()
    {
        return time_ <= 0.0f;
    }
}
