using UnityEngine;
using System.Linq;
using System.Collections;
using KModkit;
using System;


public class literallyBallingScript : MonoBehaviour
{
    public GameObject Module;
    public KMSelectable PlayButton;
    public KMAudio audio;
    public KMNeedyModule Needy;

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
        Activated = 54;
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
            audio.PlaySoundAtTransform("baller", transform);
            Activated = 54;
            Invoke("SillyGoofy", 2.52f);
        }
        else
        {
            GetComponent<KMSelectable>().AddInteractionPunch();
        }
    }

    private void SillyGoofy()
    {
        StartCoroutine(SpinMe());
    }

    IEnumerator SpinMe()
    {
        for(int i = 0; i < 30; i++)
        {
            Module.transform.localPosition = new Vector3(Module.transform.localPosition.x, Module.transform.localPosition.y+0.005f, Module.transform.localPosition.z);
            Module.transform.localEulerAngles = new Vector3(0f, 0f, Module.transform.localEulerAngles.z - 60f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield return new WaitForSecondsRealtime(0.25f);
        for (int i = 0; i < 30; i++)
        {
            Module.transform.localPosition = new Vector3(Module.transform.localPosition.x, Module.transform.localPosition.y - 0.005f, Module.transform.localPosition.z);
            Module.transform.localEulerAngles = new Vector3(0f, 0f, Module.transform.localEulerAngles.z - 60f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    protected void OnNeedyActivation()
    {
        _isSolved = false;
        Activated = 0;
    }

    protected void OnNeedyDeactivation()
    {
        Activated = 54;
    }

    protected void OnTimerExpired()
    {
        GetComponent<KMNeedyModule>().OnStrike();
        Needy.HandlePass();
        _isSolved = true;

    }
#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use !{0} dodgeball to press the dodgeball";
#pragma warning disable 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] Tears = command.Trim().ToLowerInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (Tears[0] == "dodgeball")
        {
            PlayButton.OnInteract();
            yield return null;
        }
        else
        {
            yield return null;
        }
    }

}