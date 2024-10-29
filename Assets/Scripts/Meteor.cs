using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Meteor : MonoBehaviour
{
    /// <summary>
    /// 最低落下速度
    /// </summary>
    [SerializeField] private float fallSpeedMin_ = 1.0f;

    /// <summary>
    /// 最高落下速度
    /// </summary>
    [SerializeField] private float fallSpeedMax_ = 3.0f;

    [SerializeField] private ScoreEffect scoreEffect_;

    private Explosion explosionPrefab_;

    private BoxCollider2D groundCollider_;
    private Rigidbody2D rb_;
    private GameManageraScript gameManager_;



    // Start is called before the first frame update
    void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();
        SetupVelosity();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Setup(BoxCollider2D ground, GameManageraScript gameManager, Explosion explosionPrefeb)
    {
        groundCollider_ = ground;
        gameManager_ = gameManager;
        explosionPrefab_ = explosionPrefeb;
    }

    private void SetupVelosity()
    {
        // 衝突しに行く物体の大きさを取得
        float left = groundCollider_.bounds.center.x - groundCollider_.bounds.size.x / 2.0f;
        float right = groundCollider_.bounds.center.x + groundCollider_.bounds.size.x / 2.0f;
        float top = groundCollider_.bounds.center.y + groundCollider_.bounds.size.y / 2.0f;
        float bottom = groundCollider_.bounds.center.y - groundCollider_.bounds.size.y / 2.0f;

        // 範囲内を乱数と線形補間を使いランダムに移動
        float targetX = Mathf.Lerp(left, right, Random.Range(0.0f, 1.0f));

        // 衝突先のベクトルと生成場所からベクトルを生成し方向を決定
        Vector3 target = new Vector3(targetX, top, 0.0f);
        Vector3 direction = (target - transform.position).normalized;
        // 方向ベクトルと乱数から生成した落下速度を乗算しrigitBodyのvelosityへ反映
        float fallSpeed = Random.Range(fallSpeedMin_, fallSpeedMax_);
        rb_.velocity = direction * fallSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Explosion explosion;

        if (collision.gameObject.CompareTag("Explosion"))
        {
            collision.gameObject.TryGetComponent<Explosion>(out explosion);
            Explosion(explosion);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            Fall();
        }
    }

    private void Explosion(Explosion explosion)
    {
        int chainNum = explosion.chainNum;

        int score = 100 * chainNum;

        // スコア加算
        if (gameManager_) { gameManager_.AddScore(score); }
        // 爆発生成
        Explosion exp = Instantiate(explosionPrefab_, transform.position, Quaternion.identity);
        exp.chainNum = chainNum;
        // スコア生成
        ScoreEffect scoreEffect = Instantiate(scoreEffect_, transform.position, Quaternion.identity);
        scoreEffect.SetScore(score);
        // 消滅
        Destroy(gameObject);
    }

    private void Fall()
    {
        // ダメージ加算
        gameManager_.Damage(1);
        //　消滅
        Destroy(gameObject);
    }
}
