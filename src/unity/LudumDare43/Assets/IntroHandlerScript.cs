using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroHandlerScript : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject MainMenuLogo;
    public GameObject Sleigh;
    public GameObject SleighClickInfo;

    RectTransform mainMenuTransform;
    RectTransform logoTransform;

    Transform sleighTransform;
    Vector3 sleighStartPosition;

    Camera locCamera;
    Vector3 cameraStartPosition;

    List<GameObject> huskies;
    List<Animator> huskyAnimators;

    Image helpPlayInfoImage;

    float passedTime = 0;

    // Use this for initialization
    void Start()
    {
        locCamera = Camera.main;
        cameraStartPosition = locCamera.transform.position;
        helpPlayInfoImage = SleighClickInfo.GetComponent<Image>();

        FindHuskies(Sleigh);

        mainMenuTransform = MainMenu.GetComponent<RectTransform>();
        logoTransform = MainMenuLogo.GetComponent<RectTransform>();
        sleighTransform = Sleigh.GetComponent<Transform>();

        sleighStartPosition = sleighTransform.localPosition;

        SetupAllGameStates();
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
            ani.SetFloat("Speed", Mathf.Abs(speed));
        }
    }

    // 0 = center
    // 1 = screen top
    void SetMenuLogoPosition(float progress)
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

    //this is used to resetup the game
    void SetupAllGameStates()
    {

    }

    float TimeToProgress(float startTime, float endTime, EasingFunction.Function easingFunction)
    {
        if (passedTime < startTime)
            return -1;

        if (passedTime > endTime)
            return 2f;

        var timePassedSinceStart = passedTime - startTime;
        var animationDuration = endTime - startTime;

        return easingFunction(0, 1, timePassedSinceStart / animationDuration);
    }

    // Update is called once per frame
    void Update()
    {
        var sleighOffset = 6f;
        var sleighSpeed = 10f;

        passedTime += Time.deltaTime;

        var sleightPos = sleighStartPosition;
        sleightPos.x += passedTime * sleighSpeed;
        SetHuskeySpeed(1f);

        sleighTransform.localPosition = sleightPos;

        SetMenuLogoPosition(TimeToProgress(1, 3, EasingFunction.EaseOutBounce));
        helpPlayInfoImage.color = new Color(1, 1, 1, TimeToProgress(4, 5, EasingFunction.Linear));

        var sleightPosXLimit = cameraStartPosition.x - sleighOffset;

        if (sleightPos.x >= sleightPosXLimit)
        {
            var camPos = cameraStartPosition;
            camPos.x = sleightPos.x + sleighOffset;
            locCamera.transform.position = camPos;
        }
    }
}
