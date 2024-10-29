using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class LifeBar : MonoBehaviour
{

    private float ratio_ = 1.0f;
    private Slider slider_;


    // Start is called before the first frame update
    private void Awake()
    {
        slider_ = GetComponent<Slider>();
    }

    public void SetGaugeRatio(float ratio)
    {
        ratio_ = Mathf.Clamp01(ratio);

        slider_.value = ratio_;
    }
}
