using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{

    public Text nameText;
    public Slider hpSlider;


    public void SetHUD(BattleActor actor)
    {
        nameText.text = actor.displayName;
        hpSlider.maxValue = actor.maxHP;
        hpSlider.value = actor.currentHP;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }
}
