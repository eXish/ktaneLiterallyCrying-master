using UnityEngine;
using System.Linq;
using System.Collections;
using KModkit;
using System;
using System.Text.RegularExpressions;

public class literallyTauntingScript : MonoBehaviour
{
    public KMSelectable PlayButton;
    public Material[] Emoji;
    public GameObject[] Taunts;
    public KMSelectable Taunt1;
    public KMSelectable Taunt2;
    public KMSelectable Taunt3;
    public KMSelectable Taunt4;
    public KMSelectable Taunt5;
    public Renderer EmojiShow;
    public KMAudio audio;
    public KMNeedyModule Needy;

    private static string[] _emojis = new[] { "sleep", "stare", "cry" };

    int emojiSprite = 0;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool _isSolved;
    private bool _active1;
    private bool _active2;
    private bool _active3;
    private bool _active4;
    private bool _active5;
    private int stage = 1;
    private int press = 0;
    private KMSelectable[] buttons;
    private bool isActivated;

    private void Update()
    {
        if (press == stage || press == 5)
        {
            OnNeedyDeactivation();
        }
        if (stage >= 1 && _active1)
        {
            Taunts[0].SetActive(true);
        }
        if (stage >= 2 && _active2)
        {
            Taunts[1].SetActive(true);
        }
        if (stage >= 3 && _active3)
        {
            Taunts[2].SetActive(true);
        }
        if (stage >= 4 && _active4)
        {
            Taunts[3].SetActive(true);
        }
        if (stage >= 5 && _active5)
        {
            Taunts[4].SetActive(true);
        }
    }

    private void Start()
    {
        buttons = new[] { Taunt1, Taunt2, Taunt3, Taunt4, Taunt5 };
        audio = GetComponent<KMAudio>();
        _isSolved = true;
        _active1 = false;
        _active2 = false;
        _active3 = false;
        _active4 = false;
        _active5 = false;
        Taunts[0].SetActive(false);
        Taunts[1].SetActive(false);
        Taunts[2].SetActive(false);
        Taunts[3].SetActive(false);
        Taunts[4].SetActive(false);
    }

    void Awake()
    {
        PlayButton.gameObject.SetActive(true);
        Taunt1.OnInteract += delegate () { TauntRemove1(); return false; };
        Taunt2.OnInteract += delegate () { TauntRemove2(); return false; };
        Taunt3.OnInteract += delegate () { TauntRemove3(); return false; };
        Taunt4.OnInteract += delegate () { TauntRemove4(); return false; };
        Taunt5.OnInteract += delegate () { TauntRemove5(); return false; };
        moduleId = moduleIdCounter++;
        Needy = GetComponent<KMNeedyModule>();
        Needy.OnNeedyActivation += OnNeedyActivation;
        Needy.OnNeedyDeactivation += OnNeedyDeactivation;
        Needy.OnTimerExpired += OnTimerExpired;
        emojiSprite = 0;
        EmojiShow.GetComponent<MeshRenderer>().material = Emoji[emojiSprite];
    }



    private void TauntRemove1()
    {
        press++;
        _active1 = false;
        audio.PlaySoundAtTransform("huh", transform);
        Taunts[0].SetActive(false);
        
    }

    private void TauntRemove3()
    {
        press++;
        _active3 = false;
        audio.PlaySoundAtTransform("huh", transform);
        Taunts[2].SetActive(false);
        
    }
    private void TauntRemove2()
    {
        press++;
        _active2 = false;
        audio.PlaySoundAtTransform("huh", transform);
        Taunts[1].SetActive(false);
        
    }
    private void TauntRemove4()
    {
        press++;
        _active4 = false;
        audio.PlaySoundAtTransform("huh", transform);
        Taunts[3].SetActive(false);
        
    }
    private void TauntRemove5()
    {
        press++;
        _active5 = false;
        audio.PlaySoundAtTransform("huh", transform);
        Taunts[4].SetActive(false);
        
    }

    protected void OnNeedyActivation()
    {
        isActivated = true;
        emojiSprite = 1;
        EmojiShow.GetComponent<MeshRenderer>().material = Emoji[emojiSprite];
        PlayButton.gameObject.SetActive(false);
        audio.PlaySoundAtTransform("wololo", transform);
        Invoke("activateButtons",1.5f);
    }

    protected void OnNeedyDeactivation()
    {
        Needy.HandlePass();
        PlayButton.gameObject.SetActive(true);
        press = 0;
        audio.PlaySoundAtTransform("dead", transform);
        emojiSprite = 0;
        EmojiShow.GetComponent<MeshRenderer>().material = Emoji[emojiSprite];
        stage++;
        _isSolved = true;
        isActivated = false;
    }

    protected void OnTimerExpired()
    {
        _active1 = false;
        _active2 = false;
        _active3 = false;
        _active4 = false;
        _active5 = false;
        Taunts[0].SetActive(false);
        Taunts[1].SetActive(false);
        Taunts[2].SetActive(false);
        Taunts[3].SetActive(false);
        Taunts[4].SetActive(false);
        GetComponent<KMNeedyModule>().OnStrike();
        emojiSprite = 0;
        stage = 1;
        EmojiShow.GetComponent<MeshRenderer>().material = Emoji[emojiSprite];
        Needy.HandlePass();
        _isSolved = true;
        isActivated = false;
    }

    private void activateButtons()
    {
        if (stage >= 1)
        {
            _active1 = true;
        }
        if (stage >= 2)
        {
            _active2 = true;
        }
        if (stage >= 3)
        {
            _active3 = true;
        }
        if (stage >= 4)
        {
            _active4 = true;
        }
        if (stage >= 5)
        {
            _active5 = true;
        }
        emojiSprite = 2;
        EmojiShow.GetComponent<MeshRenderer>().material = Emoji[emojiSprite];
        _isSolved = false;
    }

#pragma warning disable 0414
    private readonly string TwitchHelpMessage = @"Use !{0} taunt to taunt TracksJosh.";
#pragma warning restore 0414

    private IEnumerator ProcessTwitchCommand(string command)
    {
        var m = Regex.Match(command, @"^\s*taunt\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        if (!m.Success)
            yield break;
        yield return null;
        if (!isActivated)
            yield break;
        while (_isSolved)
            yield return null;
        buttons = new[] { Taunt1, Taunt2, Taunt3, Taunt4, Taunt5 }.Where(i => i.gameObject.activeInHierarchy).ToArray();
        foreach (var b in buttons)
        {
            b.OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
    }

    void TwitchHandleForcedSolve()
    {
        StartCoroutine(HandleSolve());
    }

    IEnumerator HandleSolve()
    {
        while (true)
        {
            if (!_isSolved)
            {
                buttons = new[] { Taunt1, Taunt2, Taunt3, Taunt4, Taunt5 }.Where(i => i.gameObject.activeInHierarchy).ToArray();
                foreach (var b in buttons)
                {
                    b.OnInteract();
                    yield return new WaitForSeconds(0.1f);
                }
            }
            yield return null;
        }
    }
}
