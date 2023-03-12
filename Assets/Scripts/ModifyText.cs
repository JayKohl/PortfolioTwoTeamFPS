using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModifyText : MonoBehaviour
{

    public TMP_Text hintText;
    public string hint1 = "This is a hint.";
    public List<string> allHints;
    public List<string> usedHints;

    void Start()
    {
        hintText.text = hint1;
    }

    
    void Update()
    {
        
    }
}
