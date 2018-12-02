﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroHandlerScript : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject MainMenuLogo;
    public GameObject Sleigh;
    public GameObject HUD;
    public GameObject SleighClickInfo;

    RectTransform mainMenuTransform;
    RectTransform logoTransform;
    RectTransform hudTransform;

    Transform sleighTransform;
    Vector3 slPos;

    Camera locCamera;
    Vector3 cameraStartPosition;

    List<GameObject> huskies;
    List<Animator> huskyAnimators;

    Image helpPlayInfoImage;

    float passedTime = 0;
    bool sleighButtonActivated = false;
    float startGameTransition = 0f;
    bool disableIntroScript = false;

    readonly float hudHiddenY = -600;

    //this is used to resetup the game
    void SetupAllGameStates()
    {
        passedTime = 0;
        startGameTransition = 0;
        sleighButtonActivated = false;
        disableIntroScript = false;
    }

    // Use this for initialization
    void Start()
    {
        SetupAllGameStates();

        locCamera = Camera.main;
        cameraStartPosition = locCamera.transform.position;
        helpPlayInfoImage = SleighClickInfo.GetComponent<Image>();

        FindHuskies(Sleigh);

        mainMenuTransform = MainMenu.GetComponent<RectTransform>();
        logoTransform = MainMenuLogo.GetComponent<RectTransform>();
        hudTransform = HUD.GetComponent<RectTransform>();
        sleighTransform = Sleigh.GetComponent<Transform>();

        slPos = sleighTransform.localPosition;
        helpPlayInfoImage.color = new Color(1, 1, 1, 0);
        hudTransform.localPosition = new Vector3(0, hudHiddenY, 0);
    }

    void FindHuskies(GameObject sleigh)
    {
        huskies = new List<GameObject>();
        huskyAnimators = new List<Animator>();
        var cnt = 0;

        foreach (Transform child in Sleigh.transform)
        {
            if (child.gameObject.name.StartsWith("Husky_"))
            {
                huskies.Add(child.gameObject);
                huskyAnimators.Add(child.gameObject.GetComponent<Animator>());
                cnt++;
            }
        }

        Debug.Log("Huskies found: " + cnt);
    }

    void SetHuskeySpeed(float speed)
    {
        foreach (var ani in huskyAnimators)
        {
            var abs = Mathf.Abs(speed);
            ani.SetFloat("Speed", abs);
            ani.speed = abs * 0.1f;
        }
    }

    // 0 = center
    // 1 = screen top
    void SetMenuLogoPositionIntro(float progress)
    {
        if (progress < 0 || progress > 1)
            return;

        Vector2 ancorPos = new Vector2();
        ancorPos.x = 0.5f;
        ancorPos.y = Mathf.Lerp(0.5f, 1, progress);

        Vector3 relPos = new Vector3();
        relPos.y = Mathf.Lerp(0, -100, progress);

        mainMenuTransform.anchorMax = ancorPos;
        mainMenuTransform.anchorMin = ancorPos;

        logoTransform.localPosition = relPos;
    }

    void SetInfoIntro(float progress)
    {
        if (progress < 0 || progress > 1)
            return;

        helpPlayInfoImage.color = new Color(1, 1, 1, progress);
    }

    void SetInfoOutro(float progress)
    {
        if (progress < 0 || progress > 1)
            return;

        helpPlayInfoImage.color = new Color(1, 1, 1, 1 - progress);
    }

    void SetMenuLogoPositionOutro(float progress)
    {
        if (progress < 0 || progress > 1)
            return;

        Vector3 relPos = new Vector3();
        relPos.y = Mathf.Lerp(-100, 200, progress);

        logoTransform.localPosition = relPos;
    }

    void ProgressHudIntro(float progress)
    {
        if (progress < 0 || progress > 1)
            return;

        Vector3 relPos = new Vector3();
        relPos.y = Mathf.Lerp(hudHiddenY, 0, progress);

        hudTransform.localPosition = relPos;
    }

    float TimeToProgress(float startTime, float endTime, EasingFunction.Function easingFunction)
    {
        if (passedTime < startTime)
            return -0.0001f;

        if (passedTime > endTime)
            return 1.0001f;

        var timePassedSinceStart = passedTime - startTime;
        var animationDuration = endTime - startTime;

        return easingFunction(0, 1, timePassedSinceStart / animationDuration);
    }

    // Update is called once per frame
    void Update()
    {
        if (disableIntroScript)
            return;

        passedTime += Time.deltaTime;

        var LogoMoveUpStart = 1f;
        var LogoMoveUpEnd = 3f;

        var HelpBlendInStart = 4f;
        var HelpBlendInEnd = 5f;

        var sleighOffset = 6f;

        // if sleigh was not pressed
        // delay the transation ...
        if (!sleighButtonActivated)
        {
            startGameTransition = passedTime + 0.1f;
            if (startGameTransition < HelpBlendInEnd)
                startGameTransition = HelpBlendInEnd;
        }

        var LogoMoveOutStart = startGameTransition + 0.5f;
        var LogoMoveOutEnd = startGameTransition + 1.5f;

        var HelpBlendOutStart = startGameTransition + 0.5f;
        var HelpBlendOutEnd = startGameTransition + 1.5f;

        var SleighSlowDownStart = startGameTransition + 0.5f;
        var SleighSlowDownEnd = startGameTransition + 2f;

        var HudIntroStart = startGameTransition + 1f;
        var HudIntroEnd = HudIntroStart + 1f;

        var EndIntro = startGameTransition + 2f;

        if(passedTime >= EndIntro)
        {
            disableIntroScript = true;
            return;
        }

        var sleighSpeed = (1 - TimeToProgress(SleighSlowDownStart, SleighSlowDownEnd, EasingFunction.Linear)) * 10f;

        SetHuskeySpeed(sleighSpeed);
        slPos.x += Time.deltaTime * sleighSpeed;
        sleighTransform.localPosition = slPos;


        SetMenuLogoPositionIntro(TimeToProgress(LogoMoveUpStart, LogoMoveUpEnd, EasingFunction.EaseOutBounce));
        SetMenuLogoPositionOutro(TimeToProgress(LogoMoveOutStart, LogoMoveOutEnd, EasingFunction.EaseInQuad));

        SetInfoIntro(TimeToProgress(HelpBlendInStart, HelpBlendInEnd, EasingFunction.Linear));
        SetInfoOutro(TimeToProgress(HelpBlendOutStart, HelpBlendOutEnd, EasingFunction.Linear));

        ProgressHudIntro(TimeToProgress(HudIntroStart, HudIntroEnd, EasingFunction.EaseOutCubic));

        var sleightPosXLimit = cameraStartPosition.x - sleighOffset;

        if (slPos.x >= sleightPosXLimit)
        {
            var camPos = cameraStartPosition;
            camPos.x = slPos.x + sleighOffset;
            locCamera.transform.position = camPos;
        }


        var sleighScreenPos = locCamera.WorldToViewportPoint(sleighTransform.position);
        helpPlayInfoImage.rectTransform.position = locCamera.ViewportToScreenPoint(new Vector3(sleighScreenPos.x - 0.05f, sleighScreenPos.y + 0.15f, 0));

        if (Input.GetMouseButtonDown(0) && !sleighButtonActivated && passedTime >= HelpBlendInStart)
        {
            var mousePos = locCamera.ScreenToViewportPoint(Input.mousePosition);
            var sleighWithoutZ = sleighScreenPos;
            sleighWithoutZ.z = 0;
            var distSleigh = Vector3.Distance(sleighWithoutZ, mousePos);

            if (distSleigh <= 0.1)
            {
                sleighButtonActivated = true;

                //transition to normal game-mode
                Debug.Log("Switch to game mode " + passedTime);

            }
        }
    }
}
