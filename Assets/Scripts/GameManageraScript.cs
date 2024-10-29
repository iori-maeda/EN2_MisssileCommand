using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManageraScript : MonoBehaviour
{
    private Camera mainCamera_;
    private int score_ = 0;

    [Header("Prefabs")]
    [SerializeField] private Explosion explosionPrefab_;
    [SerializeField] private Meteor meteorPrefab_;

    [Header("Item Prefabs")]
    [SerializeField] private List<ItemBase> items_;

    [Header("Item SpawnSettings")]
    [SerializeField] Transform itemSpawnPoint_;
    [SerializeField] private float itemSpawnInterval = 10.0f;
    private float itemSpawnTimer = 0.0f;

    [Header("MeteorSpawner")]
    [SerializeField] private BoxCollider2D ground_;
    [SerializeField] private float meteorIntervel_ = 1.0f;
    private float meteorTimer_ = 0;
    [SerializeField] private List<Transform> spawnPositions_;

    [Header("SourceSetthings")]
    [SerializeField] private ScoreText scoreText_;

    [Header("LifeUISettings")]
    [SerializeField] private LifeBar lifeBar_;
    [SerializeField] private float maxLife_ = 10.0f;
    private float life_ = 0.0f;

    [Header("CannonSettings")]
    private List<Cannon> cannons_;

    private List<Tawer> towers_;

    // Start is called before the first frame update
    void Start()
    {
        // mainカメラのゲームオブジェクトの取得
        GameObject mainCameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        // 取得してるかチェック
        Assert.IsNotNull(mainCameraObj, "MainCameraが見つかりませんでした。");
        // カメラコンポーネントの取得チェック
        Assert.IsTrue(mainCameraObj.TryGetComponent(out mainCamera_), "MainCameraにCameraコンポーネントがないよ～");

        Assert.IsTrue(spawnPositions_.Count > 0, "要素無いよ～");
        foreach (Transform t in spawnPositions_)
        {
            if (t == null)
            {
                Assert.IsNotNull(t, "内容がnullのものがあったよ～");
            }
        }

        ResetLife();

        GetCannons();
        GetTawersToStage();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("mouse right button clicked\n");
            Shoot();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("mouse left button clicked\n");
            GenerateExplosion();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space key down\n");
            ShootFromTower();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("R key down\n");
            SceneManager.LoadScene("Result");
        }

        UpdateMeteorTimer();
        UpdateItemSawnTimer();
    }

    private void GenerateExplosion()
    {
        Vector3 clickPosition = mainCamera_.ScreenToWorldPoint(Input.mousePosition);
        clickPosition.z = 0.0f;

        Explosion explosion = Instantiate(explosionPrefab_, clickPosition, Quaternion.identity);
    }

    public void AddScore(int point)
    {
        score_ += point;
        scoreText_.SetScore(score_);
    }

    public void Damage(int point)
    {
        life_ -= point;
        UpdateLifeBar();
    }

    private void UpdateMeteorTimer()
    {
        meteorTimer_ -= Time.deltaTime;
        if (meteorTimer_ > 0) { return; }
        meteorTimer_ = meteorIntervel_;
        GenerateMeteor();
    }

    private void GenerateMeteor()
    {
        int max = spawnPositions_.Count;
        int posIndex = Random.Range(0, max);
        Vector3 spawnPosition = spawnPositions_[posIndex].position;

        //Vector3 spawnPosition = new Vector3(0.0f, 6.0f, 0.0f);
        Meteor meteor = Instantiate(meteorPrefab_, spawnPosition, Quaternion.identity);
        meteor.Setup(ground_, this, explosionPrefab_);
    }

    private void ResetLife()
    {
        life_ = maxLife_;
        UpdateLifeBar();
    }

    private void UpdateLifeBar()
    {
        float lifeRatio = Mathf.Clamp01(life_ / maxLife_);
        lifeBar_.SetGaugeRatio(lifeRatio);
    }

    private void Shoot()
    {
        Cannon shootableCannon = ShotableCannon();
        if (shootableCannon == null) { return; }
        Vector3 clickPos = mainCamera_.ScreenToWorldPoint(Input.mousePosition);
        clickPos.z = 0.0f;
        shootableCannon.Shoot(clickPos);
    }

    private void GetCannons()
    {
        GameObject[] cannons = GameObject.FindGameObjectsWithTag("Cannon");
        cannons_ = new List<Cannon>();

        foreach (var cannon in cannons)
        {
            Cannon preCannon;
            if (cannon == null) { continue; }
            if (!cannon.TryGetComponent(out preCannon)) { continue; }
            cannons_.Add(preCannon);
        }
    }

    private Cannon ShotableCannon()
    {
        Cannon result = null;
        foreach (var cannon in cannons_)
        {
            if(cannon == null) {  continue; }
            if (cannon.IsShootRady())
            {
                result = cannon;
                break;
            }
        }
        return result;
    }

    private void ShootFromTower()
    {
        Tawer shootableTower = GetShootableTower();
        if (shootableTower == null) { return; }

        Vector3 reticlePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        reticlePos.z = 0.0f;

        shootableTower.Shoot(reticlePos);
    }

    private Tawer GetShootableTower()
    {
        foreach (var tower in towers_)
        {
            if(tower == null) { continue; }
            if (tower.isShootReady)
            {
                return tower;
            }
        }
        return null;
    }

    private void GetTawersToStage()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Tower");
        towers_ = new List<Tawer>();
        foreach (var obj in objects)
        {
            if (obj == null) continue;
            Tawer preTower;
            if (obj.TryGetComponent<Tawer>(out preTower))
            {
                towers_.Add(preTower);
            }
        }
    }

    private ItemBase PickupItem()
    {
        Assert.IsTrue(items_.Count > 0);
        int pickupItemIndex = Random.Range(0, items_.Count);
        return items_[pickupItemIndex];
    }

    private void UpdateItemSawnTimer()
    {
        itemSpawnTimer -= Time.deltaTime;
        if (itemSpawnTimer > 0.0f) { return; }

        itemSpawnTimer = itemSpawnInterval;

        ItemBase spawnItem = PickupItem();

        int spawnPosIndex = Random.Range(0, spawnPositions_.Count);
        ItemBase item = Instantiate(spawnItem, spawnPositions_[spawnPosIndex].transform.position, Quaternion.identity);

        Vector3 dir = Vector3.zero;

        dir.x = Random.Range(ground_.bounds.min.x, ground_.bounds.max.x);
        dir.y = ground_.bounds.max.y;
        dir = dir - item.transform.position;
        dir.Normalize();

        item.SetDir(dir);
    }
};