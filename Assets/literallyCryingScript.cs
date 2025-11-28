using UnityEngine;
using System.Linq;
using System.Collections;
using KModkit;
using System;

public class literallyCryingScript : MonoBehaviour
{
    public KMSelectable PlayButton;
    public Material[] Emoji;
    public Renderer EmojiShow;
    public KMAudio audio;
    public KMNeedyModule Needy;

    private static string[] _emojis = new[] { "sleep", "stare", "cry"};

    int emojiSprite = 0;

    static int moduleIdCounter = 1;
    int moduleId;
    int Activated;
    private bool _isSolved;

    private void Start()
    {
        audio = GetComponent<KMAudio>();
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
            PlayButton.OnInteract += delegate () { PressPlay(); return false; };
        }
        else
        {
            PlayButton.OnInteract += delegate () { PressPlay(); return true; };
        }
    }

    private void PressPlay()
    {
        if (Activated <= 1)
        {
            Needy.HandlePass();
            _isSolved = true;
            GetComponent<KMSelectable>().AddInteractionPunch();
            audio.PlaySoundAtTransform("insult", transform);
            Activated = 54;
            Invoke("Play", 1.7f);
        }
        else
        {
            GetComponent<KMSelectable>().AddInteractionPunch();
        }
    }

    private void Play()
    {
        emojiSprite = 2;
        EmojiShow.GetComponent<MeshRenderer>().material = Emoji[emojiSprite];
        audio.PlaySoundAtTransform("crybaby", transform);
        Invoke("OnNeedyDeactivation", 0.7f);
    }

    protected void OnNeedyActivation()
    {
        _isSolved = false;
        emojiSprite = 1;
        EmojiShow.GetComponent<MeshRenderer>().material = Emoji[emojiSprite];
        Activated = 0;
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
        Activated = 54;
        EmojiShow.GetComponent<MeshRenderer>().material = Emoji[emojiSprite];
        Needy.HandlePass();
        _isSolved = true;

    }
#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use !{0} play to press the insult button";
#pragma warning disable 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] Tears = command.Trim().ToLowerInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (Tears[0] == "play")
        {
            yield return null;
            PlayButton.OnInteract();
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
            if (Activated <= 1)
                PlayButton.OnInteract();
            yield return null;
        }
    }
}