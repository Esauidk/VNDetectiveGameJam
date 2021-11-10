using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doublsb.Dialog;

public class MainDialogue : MonoBehaviour
{
    public DialogManager DialogManager;
    public GameObject Background;

    // Start is called before the first frame update
    void Start()
    {
        var introText = new List<DialogData>();

        DialogManager.Set_Trust("Sa", 10);
        
        introText.Add(new DialogData("Hello, My Name is Sa", "Sa"));

        if(DialogManager.getCharacter("Sa").Trust > 3) {
            introText.Add(new DialogData("My Current Trust is high enough for this action", "Sa"));
        }

        introText.Add(new DialogData("Bye Bye", "Sa"));

        DialogManager.Show(introText);
    }

    public void changeBackground(string file)
    {
        Background.GetComponent<Image>().sprite= Resources.Load<Sprite>(file);
    }
}
