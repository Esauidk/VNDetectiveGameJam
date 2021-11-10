using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doublsb.Dialog;

public class MainDialogue : MonoBehaviour
{
    public DialogManager dialogManager;
    public GameObject Background;

    // Start is called before the first frame update
    void Start()
    {
        // Intro Scene
        StartCoroutine(IntroScene());


    }

    public void changeBackground(string file)
    {
        var newBackground = Resources.Load<Sprite>(file);
        if(newBackground != null) Background.GetComponent<Image>().sprite = newBackground;
    }

    public void addDI(List<DialogData> text, string person, string DItext) {
        text.Add(new DialogData(DItext, person));
    }

    public void addSelectDI(DialogData text, string[] answers) {
        for(int i = 0; i < answers.Length; i++) {
            text.SelectList.Add(i.ToString(), answers[i]);
        }
    }

    public IEnumerator OneChoice(string question, string[] answers, Action[] diags) {
        Dictionary<string, Action> ndiag = new Dictionary<string, Action>();
        for(int i=0;i<answers.Length;i++) {
            ndiag.Add(answers[i], diags[i]);
        }

        var dialogText = new List<DialogData>();
        var text = new DialogData(question);
        for(int i = 0; i < answers.Length; i++) {
            Debug.Log(answers[i]);
            text.SelectList.Add(answers[i], answers[i]);
        }

        text.Callback = () => diagCallBack(ndiag);
        dialogText.Add(text);
        yield return StartCoroutine(dialogManager.Show(dialogText));
    }

    private void diagCallBack(Dictionary<string, Action> ndiag) {
        ndiag[dialogManager.Result]();
    }

    public IEnumerator AllChoices(string question, string[] answers, Action[] diags) {
        Dictionary<string, Action> ndiag = new Dictionary<string, Action>();
        for(int i=0;i<answers.Length;i++) {
            ndiag.Add(answers[i], diags[i]);
        }
        
        HashSet<string> visited = new HashSet<string>();
        for(int j=0;j<answers.Length;j++) {
            var dialogText = new List<DialogData>();
            var text = new DialogData(question);
            for(int i = 0; i < answers.Length; i++) {
                if(!visited.Contains(answers[i])){
                    text.SelectList.Add(answers[i], answers[i]);
                }
            }
            text.Callback = () => diagCallBack(ndiag, visited);
            dialogText.Add(text);
            yield return StartCoroutine(dialogManager.Show(dialogText));
        }
    }

    private void diagCallBack(Dictionary<string, Action> ndiag, HashSet<String> visited) {
        ndiag[dialogManager.Result]();
        visited.Add(dialogManager.Result);
    }

    public IEnumerator IntroScene() {
        changeBackground("Backgrounds/restaurantbirthday");

        // ------------------- BIRTHDAY TALKING TO RESTAURANT FRIEND ----------------------------

        yield return StartCoroutine(IntroBirthdayDialogue());

        // -------------------- BIRTHDAY TALKING TO EVERYONE ELSE --------------------------------
        
        yield return IntroBirthdayPeople();

        // --------------------- PARTYING AND THEN LEAVING --------------------------------------

        yield return Partying();
    }

    public IEnumerator IntroBirthdayDialogue() {
        var introScene = new List<DialogData>();

        addDI(introScene, "rest", "Hey my friend! Welcome to The Richard Graves Restaurant!");
        addDI(introScene, "rest", "I'm glad you could make it to my birthday!");
        addDI(introScene, "rest", "By the way, we're also celebrating the 10th anniversary of my restaurant's founding.");
        addDI(introScene, "rest", "We're just getting started, so make yourself comfortable.");
        addDI(introScene, "rest", "Talk to some people!");

        yield return StartCoroutine(dialogManager.Show(introScene));
    }

    public IEnumerator IntroBirthdayPeople() {
        string[] birthdayChoices = new[] {"Ms. Sutherland - LandLord", "Jack Brewski - Bartender", "Lily Lou - Florist", "Sam O'hair - Butcher", "Dr. Phillip - Pharmacist"};

        Action birth_land = () => {
            var dialogText = new List<DialogData>();
            addDI(dialogText, "land", "Welcome to the party, my child. I'm Ms. Sutherland. I run the apartments nearby");
            addDI(dialogText, "land", "I've known Graves for 7 years now. Such a great man.");
            addDI(dialogText, "land", "His restaurant have been a boon for my apartments.");
            addDI(dialogText, "land", "I hope he accepts my present with gratitude");
            StartCoroutine(dialogManager.Show(dialogText));
        };

        Action birth_butcher = () => {
            var dialogText = new List<DialogData>();
            addDI(dialogText, "butcher", "WELCOME my friend this amazing party!");
            addDI(dialogText, "butcher", "I'm Sam O'hair if you're wondering");
            addDI(dialogText, "butcher", "It feels like it's only been 3 months since O'hair started this restaurant");
            addDI(dialogText, "butcher", "People say his meats are the best. You know why?");
            addDI(dialogText, "butcher", "CUZ HE GETS HIS MEATS FROM ME");
            addDI(dialogText, "butcher", "He better like the presents I got for him");
            StartCoroutine(dialogManager.Show(dialogText));
        };

        Action birth_bar = () => {
            var dialogText = new List<DialogData>();
            addDI(dialogText, "bar", "I'm Jack Brewski, nice to meet you.");
            addDI(dialogText, "bar", "I'm You bopping to killer music yet?");
            addDI(dialogText, "bar", "That actually how I met Dick 20 years ago. We were in a jazz band together.");
            addDI(dialogText, "bar", "We've had our ups and downs as friends, but he's made killer owning this restaurant");
            addDI(dialogText, "bar", "I hope he loves the present I got him");
            StartCoroutine(dialogManager.Show(dialogText));
        };

        Action birth_flower = () => {
            var dialogText = new List<DialogData>();
            addDI(dialogText, "flower", "HEYO MY DASHING FRIEND. What brings you to this party?");
            addDI(dialogText, "flower", "Ohhh, you're Old Man Graves's nibling. Wow, I see the resemblance.");
            addDI(dialogText, "flower", "I run the quaint flower shop nearby; You should visit");
            addDI(dialogText, "flower", "It's a bit breezy in there, but the plants keep me company and that's all I need");
            addDI(dialogText, "flower", "My presents are the always the best, you'll see");
            StartCoroutine(dialogManager.Show(dialogText));
        };

        Action birth_pharma = () => {
            var dialogText = new List<DialogData>();
            addDI(dialogText, "pharma", "Hi, I'm Dr. Phillip.");
            addDI(dialogText, "pharma", ".........................................................................");
            addDI(dialogText, "pharma", ".........................................................................");
            addDI(dialogText, "pharma", "Bye.");
            StartCoroutine(dialogManager.Show(dialogText));
        };

        Action[] birthdayDiags = new[] {birth_land, birth_bar, birth_flower, birth_butcher, birth_pharma};

        yield return AllChoices("Which person would you like to talk to?", birthdayChoices, birthdayDiags);
    }

    public IEnumerator Partying() {
        var dialogText = new List<DialogData>();
        addDI(dialogText, "", "The party continues on all night.");
        addDI(dialogText, "", "It's a lot of fun.");
        addDI(dialogText, "", "You leave the party earlier than expect. You have to do your Genshin Resin in the morning");
        addDI(dialogText, "", "But nothing prepares you for tomorrow morning.");
        yield return StartCoroutine(dialogManager.Show(dialogText));
    }
}
