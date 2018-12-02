using Assets;
using UnityEngine;
using UnityEngine.UI;

public class HudFoodDisplay : MonoBehaviour
{
    Image foodBarImage;

    // Use this for initialization
    void Start()
    {
        foodBarImage = GameObject.Find("hud-food-bar").GetComponent<Image>();
        foodBarImage.type = Image.Type.Filled;
    }

    // Update is called once per frame
    void Update()
    {
        foodBarImage.fillAmount = GlobalGameState.Food;
    }
}
