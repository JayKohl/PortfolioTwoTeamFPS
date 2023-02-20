using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilitiesColdown : MonoBehaviour
{
    [SerializeField] private Image cooldownImage;
    [SerializeField] private TMP_Text cooldownText;

    private bool used = false;
    public float cooldownTime = 10.0f;
    public float cooldownTimer = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        cooldownText.gameObject.SetActive(false);
        cooldownImage.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    { 
        if(used)
        {
            coolDownAbility();
        }
            
    }

    public void coolDownAbility()
    {
        cooldownTimer -= Time.deltaTime;
        if(cooldownTimer <= 0.0f)
        {
            used = false;
            cooldownText.gameObject.SetActive(false);
            cooldownImage.fillAmount = 0.0f;
        }
        else
        {
            cooldownText.text = Mathf.RoundToInt(cooldownTimer).ToString();
            cooldownImage.fillAmount = cooldownTimer / cooldownTime;
        }
    }

    public bool wasSpellUsed()
    {      
        if(used)
        {
            return false;
        }
        else
        {
            used = true;
            cooldownText.gameObject.SetActive(true);
            cooldownTimer = cooldownTime;
            return true;
        }
          
    }
}
