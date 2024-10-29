using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreText : MonoBehaviour
{
    private int score_ = 0;

    private TMP_Text socreText_;

    // Start is called before the first frame update
    void Start()
    {
        score_ = 0;
        socreText_ = GetComponent<TMP_Text>();
    }

    public void SetScore(int socre)
    {
        score_ = socre;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        socreText_.text = $"SCORE : {score_:00000000}";
    }
}
