using UnityEngine;
using System.Linq;
using System.Collections;
using KModkit;
using System;
using Rnd = UnityEngine.Random;

public class literallyLyingScript : MonoBehaviour
{
    public KMSelectable BottomButton;
    public KMSelectable MiddleButton;
    public KMSelectable TopButton;
    public Material[] Emoji;
    public Renderer EmojiShow;
    public KMAudio audio;
    public KMNeedyModule Needy;

    private static string[] _emojis = new[] { "sleep", "stare", "cry","trollface" };

    int emojiSprite = 0;

    static int moduleIdCounter = 1;
    int moduleId;
    int Activated;
    private bool[] rightButton = { false, false, false }; //bottom, middle, top
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
            BottomButton.OnInteract += delegate () { PressBottom(); return false; };
            MiddleButton.OnInteract += delegate () { PressMiddle(); return false; };
            TopButton.OnInteract += delegate () { PressTop(); return false; };
        }
        else
        {
            BottomButton.OnInteract += delegate () { PressBottom(); return true; };
            MiddleButton.OnInteract += delegate () { PressMiddle(); return true; };
            TopButton.OnInteract += delegate () { PressTop(); return true; };
        }
    }

    private void PressBottom()
    {
        if (Activated <= 1)
        {
            if (rightButton[0])
            {
                Needy.HandlePass();
                rightButton[0] = false;
                _isSolved = true;
                GetComponent<KMSelectable>().AddInteractionPunch();
                audio.PlaySoundAtTransform("liar", transform);
                Activated = 54;
                Invoke("Play", 2.0f);
            }
            else
            {
                OnTimerExpired();
            }
        }
        else
        {
            GetComponent<KMSelectable>().AddInteractionPunch();
        }
    }

    private void PressMiddle()
    {
        if (Activated <= 1)
        {
            if (rightButton[1])
            {
                Needy.HandlePass();
                rightButton[1] = false;
                _isSolved = true;
                GetComponent<KMSelectable>().AddInteractionPunch();
                audio.PlaySoundAtTransform("liar", transform);
                Activated = 54;
                Invoke("Play", 2.0f);
            }
            else
            {
                OnTimerExpired();
            }
        }
        else
        {
            GetComponent<KMSelectable>().AddInteractionPunch();
        }
    }

    private void PressTop()
    {
        if (Activated <= 1)
        {
            if (rightButton[2])
            {
                Needy.HandlePass();
                rightButton[2] = false;
                _isSolved = true;
                GetComponent<KMSelectable>().AddInteractionPunch();
                audio.PlaySoundAtTransform("liar", transform);
                Activated = 54;
                Invoke("Play", 2.0f);
            }
            else
            {
                OnTimerExpired();
            }
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
        audio.PlaySoundAtTransform("baby_cry", transform);
        Invoke("OnNeedyDeactivation", 2.5f);
    }

    protected void OnNeedyActivation()
    {
        int i = Rnd.Range(0, 3);
        for (int j = 0; j < 3; j++)
        {
            rightButton[j] = false;
        }
        rightButton[i] = true;
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
        for (int j = 0; j < 3; j++)
        {
            rightButton[j] = false;
        }
    }

    protected void OnTimerExpired()
    {
        GetComponent<KMNeedyModule>().OnStrike();
        for (int j = 0; j < 3; j++)
        {
            rightButton[j] = false;
        }
        audio.PlaySoundAtTransform("liar", transform);
        Invoke("NuhUh", 1.9f);
        emojiSprite = 3;
        EmojiShow.GetComponent<MeshRenderer>().material = Emoji[emojiSprite];
        Needy.HandlePass();
        _isSolved = true;

    }

    private void NuhUh()
    {
        audio.PlaySoundAtTransform("nuh_uh", transform);
    }

    void Update()
    {
        TopButton.gameObject.transform.localEulerAngles = new Vector3(0f,180f,0f);
        MiddleButton.gameObject.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        BottomButton.gameObject.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        if (!rightButton[0]) BottomButton.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        if (!rightButton[1]) MiddleButton.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        if (!rightButton[2]) TopButton.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
    }
#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use !{0} top/middle/bottom to press the respected insult button";
#pragma warning disable 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] Tears = command.Trim().ToLowerInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (Tears[0] == "top")
        {
            TopButton.OnInteract();
            yield return null;
        }
        else if (Tears[0] == "middle")
        {
            MiddleButton.OnInteract();
            yield return null;
        }
        else if (Tears[0] == "bottom")
        {
            BottomButton.OnInteract();
            yield return null;
        }
        else
        {
            yield return null;
        }
    }
}