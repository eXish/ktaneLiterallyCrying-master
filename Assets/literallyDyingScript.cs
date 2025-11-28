using UnityEngine;
using System.Linq;
using System.Collections;
using KModkit;
using System;

public class literallyDyingScript : MonoBehaviour
{
    public KMSelectable Bandage;
    public Material[] Emoji;
    public Renderer EmojiShow;
    public KMAudio audio;
    public KMNeedyModule Needy;
    public GameObject customImage;

    private static string[] _emojis = new[] { "stare", "fever", "ghost"};

    int emojiSprite = 0;
    int scare = 0;

    static int moduleIdCounter = 1;
    int moduleId;
    int Activated;
    private bool _isSolved;
    private bool _isScare = false;
    private void Start()
    {
        audio = GetComponent<KMAudio>();
    }

    private void Update()
    {
        if (_isScare == true)
        {
            customImage.gameObject.SetActive(true);
        }
        else
        {
            customImage.gameObject.SetActive(false);
        }
    }

    void Awake()
    {
        moduleId = moduleIdCounter++;
        Needy = GetComponent<KMNeedyModule>();
        Needy.OnNeedyActivation += OnNeedyActivation;
        Needy.OnNeedyDeactivation += OnNeedyDeactivation;
        Needy.OnTimerExpired += OnTimerExpired;
        emojiSprite = 0;
        Activated = 54;
        EmojiShow.GetComponent<MeshRenderer>().material = Emoji[emojiSprite];
        if (Activated <= 1)
        {
            Bandage.OnInteract += delegate () { PressPlay(); return false; };
        }
        else
        {
            Bandage.OnInteract += delegate () { PressPlay(); return true; };
        }
    }

    private void PressPlay()
    {
        if(scare == 0)
        {
            if (Activated <= 1)
            {
                Needy.HandlePass();
                _isSolved = true;
                GetComponent<KMSelectable>().AddInteractionPunch();
                Activated = 54;
                emojiSprite = 0;
                EmojiShow.GetComponent<MeshRenderer>().material = Emoji[emojiSprite];
            }
            else
            {
                GetComponent<KMSelectable>().AddInteractionPunch();
            }
        }
        else
        {
            emojiSprite = 2;
            EmojiShow.GetComponent<MeshRenderer>().material = Emoji[emojiSprite];
            _isSolved = true;
            GetComponent<KMSelectable>().AddInteractionPunch();
        }
        
    }


    protected void OnNeedyActivation()
    {
        if(scare == 0)
        {
            _isSolved = false;
            emojiSprite = 1;
            EmojiShow.GetComponent<MeshRenderer>().material = Emoji[emojiSprite];
            Activated = 0;
        }
        else
        {
            emojiSprite = 2;
            EmojiShow.GetComponent<MeshRenderer>().material = Emoji[emojiSprite];
            Needy.HandlePass();
            _isSolved = true;
            _isScare = true;
            Invoke("Play", 1.7f);
            audio.PlaySoundAtTransform("insult", transform);
        }

    }

    private void Play()
    {
        _isScare = false;

    }

    protected void OnNeedyDeactivation()
    {
        emojiSprite = 0;
        Activated = 54;
        EmojiShow.GetComponent<MeshRenderer>().material = Emoji[emojiSprite];
    }

    protected void OnTimerExpired()
    {
        GetComponent<KMNeedyModule>().OnStrike();
        emojiSprite = 2;
        scare = 1;
        EmojiShow.GetComponent<MeshRenderer>().material = Emoji[emojiSprite];
        Needy.HandlePass();
        _isSolved = true;

    }
#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use !{0} heal to heal with bandages";
#pragma warning disable 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] Tears = command.Trim().ToLowerInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (Tears[0] == "heal")
        {
            yield return null;
            Bandage.OnInteract();
        }
        else
        {
            yield return null;
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
            if (scare == 0 && Activated <= 1)
                Bandage.OnInteract();
            yield return null;
        }
    }
}