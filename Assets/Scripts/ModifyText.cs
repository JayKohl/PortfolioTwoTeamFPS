using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModifyText : MonoBehaviour
{

    public TMP_Text hintText;
    public string hint1 = "This is a hint.";
    public string hint2 = "This is a hint as well, how are you today?";
    public string hint3 = "What can you do?";
    public string hint4 = "Aliens wil try to kill you.";
    public string hint5 = "Trust some.";
    public List<string> allHints;
    public List<string> usedHints;
    public string selectedHint;
    public bool hintUp;
    int position;

    void Start()
    {
        allHints.Add(hint1);
        allHints.Add(hint2);
        allHints.Add(hint3);
        allHints.Add(hint4);
        allHints.Add(hint5);
        HintSelect();
    }

    private void Update()
    {
        //if (!hintUp)
        //{
        //    hintUp = true;
        //    HintSelect();
        //}
    }


    public void HintSelect()
    {

        if (allHints.Count <= 0)
        {
            ResetHintLists();
        }

        if (allHints.Count > 0)
        {
            position = Random.Range(0, allHints.Count);
            selectedHint = allHints[position];
            hintText.text = selectedHint;
            allHints.RemoveAt(position);
            usedHints.Add(selectedHint);
        }
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
