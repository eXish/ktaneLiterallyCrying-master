using UnityEngine;
using System.Linq;
using System.Collections;
using KModkit;
using System;

public class literallyDeagingScript : MonoBehaviour
{

    public KMSelectable CakeYum;
    public Material[] Emoji;
    public Renderer EmojiShow;
    public KMAudio audio;
    public KMNeedyModule Needy;

    private static string[] _emojis = new[] { "baby", "grown", "dead" };

    int emojiSprite = 0;

    static int moduleIdCounter = 1;
    int moduleId;
    int Activated;
    private bool _isSolved;
    private bool played;

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
            CakeYum.OnInteract += delegate () { Cake(); return false; };
        }
        else
        {
            CakeYum.OnInteract += delegate () { Cake(); return true; };
        }
    }

    private void Update()
    {
        int needyTimer = (int)GetComponent<KMNeedyModule>().GetNeedyTimeRemaining();
        if(needyTimer == 5 && !played)
        {
            played = true;
            OldGeezer();
        }
    }

    private void Cake()
    {
        if (Activated <= 1)
        {
            Needy.HandlePass();
            _isSolved = true;
            GetComponent<KMSelectable>().AddInteractionPunch();
            Activated = 54;
            audio.PlaySoundAtTransform("happy birthday", transform);
            Invoke("OnNeedyDeactivation", 0.7f);
        }
        else
        {
            GetComponent<KMSelectable>().AddInteractionPunch();
        }
    }

    private void OldGeezer()
    {
        audio.PlaySoundAtTransform("wtf", transform);
        emojiSprite = 2;
        EmojiShow.GetComponent<MeshRenderer>().material = Emoji[emojiSprite];
    }

    protected void OnNeedyActivation()
    {
        played = false;
        _isSolved = false;
        audio.PlaySoundAtTransform("sneeze", transform);
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
        audio.PlaySoundAtTransform("steam", transform);
        Needy.HandlePass();
        _isSolved = true;

    }
#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use !{0} cake to give the emoji a cake.";
#pragma warning disable 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] Tears = command.Trim().ToLowerInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (Tears[0] == "cake")
        {
            yield return null;
            CakeYum.OnInteract();
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
                CakeYum.OnInteract();
            yield return null;
        }
    }
}