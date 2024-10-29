using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreEffect : MonoBehaviour
{
    [SerializeField] private float aliveTime_ = 1.0f;

    float aliveTimer_ = 0.0f;

    private void Update()
    {
        aliveTimer_ += Time.deltaTime;
        if (aliveTimer_ >= aliveTime_) { Destroy(gameObject); }
    }

    public void SetScore(int score)
    {
        GetComponent<TMP_Text>().text = score.ToString();
    }
}
