using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Meteor : MonoBehaviour
{
    /// <summary>
    /// �Œᗎ�����x
    /// </summary>
    [SerializeField] private float fallSpeedMin_ = 1.0f;

    /// <summary>
    /// �ō��������x
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
        // �Փ˂��ɍs�����̂̑傫�����擾
        float left = groundCollider_.bounds.center.x - groundCollider_.bounds.size.x / 2.0f;
        float right = groundCollider_.bounds.center.x + groundCollider_.bounds.size.x / 2.0f;
        float top = groundCollider_.bounds.center.y + groundCollider_.bounds.size.y / 2.0f;
        float bottom = groundCollider_.bounds.center.y - groundCollider_.bounds.size.y / 2.0f;

        // �͈͓��𗐐��Ɛ��`��Ԃ��g�������_���Ɉړ�
        float targetX = Mathf.Lerp(left, right, Random.Range(0.0f, 1.0f));

        // �Փː�̃x�N�g���Ɛ����ꏊ����x�N�g���𐶐�������������
        Vector3 target = new Vector3(targetX, top, 0.0f);
        Vector3 direction = (target - transform.position).normalized;
        // �����x�N�g���Ɨ������琶�������������x����Z��rigitBody��velosity�֔��f
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

        // �X�R�A���Z
        if (gameManager_) { gameManager_.AddScore(score); }
        // ��������
        Explosion exp = Instantiate(explosionPrefab_, transform.position, Quaternion.identity);
        exp.chainNum = chainNum;
        // �X�R�A����
        ScoreEffect scoreEffect = Instantiate(scoreEffect_, transform.position, Quaternion.identity);
        scoreEffect.SetScore(score);
        // ����
        Destroy(gameObject);
    }

    private void Fall()
    {
        // �_���[�W���Z
        gameManager_.Damage(1);
        //�@����
        Destroy(gameObject);
    }
}
