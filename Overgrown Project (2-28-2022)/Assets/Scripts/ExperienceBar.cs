using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceBar : MonoBehaviour
{

    public Slider expSlider;
    public TMP_Text expText;

    public void ResetEXPBar(int milestoneEXP, int currentLevel)
    {
        expSlider.maxValue = milestoneEXP;
        expSlider.value = 0;
        TextChangeCurrent(currentLevel);
    }

    public void SetCurrentEXP(int EXP)
    {
        expSlider.value = EXP;
        //(currentHealth, maxHealth);
    }

    public void TextChangeCurrent(int currentLevel)
    {
        expText.text = ("Lv." + currentLevel);
    }

}
