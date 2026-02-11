using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image healthBar;

    [SerializeField] private float lerpSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeValue(float value)
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, value, lerpSpeed);
    }
}
