using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModifyText : MonoBehaviour
{

    public TMP_Text hintText;
    public string hint1 = "This is a hint.";
    public string hint2 = "This is a hint as well, how are you today?";
    public List<string> allHints;
    public List<string> usedHints;
    public string selectedHint;
    public bool hintUp;
    
    void Start()
    {
        allHints.Add(hint1);
        allHints.Add(hint2);
    }


    void Update()
    {
        if (gameManager.instance.isPaused)
        {
            if (!hintUp)
            {
                hintUp = true;
                HintSelect();
            }   
        }
        else
        {
            hintUp = false;
        }
    }

    public void HintSelect()
    {
        if (allHints.Count <= 0)
        {
            ResetHintLists();
        }

        int position;
        position = Random.Range(0, allHints.Count);
        selectedHint = allHints[position];
        hintText.text = selectedHint;
        allHints.RemoveAt(position);
        usedHints.Add(selectedHint);

    }

    private void ResetHintLists()
    {
        string temp;

        for (int i = 0; i < usedHints.Count; i++)
        {
            temp = usedHints[i];
            allHints.Add(temp);
        }
        usedHints.Clear();
    }
}
