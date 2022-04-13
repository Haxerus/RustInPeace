using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{

    public Text nameText;
    public Slider hpSlider;

    public GameObject buffIcons;

    public void SetHUD(BattleActor actor)
    {
        nameText.text = string.Format("{0} Lv. {1}", actor.displayName, actor.level);
        hpSlider.maxValue = actor.maxHP;
        hpSlider.value = actor.currentHP;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

    public void AddBuffIcon(Buff b)
    {
        GameObject newBuff = Instantiate(Resources.Load<GameObject>("Prefabs/Icon"), buffIcons.transform);
        newBuff.GetComponent<Image>().sprite = b.icon;
    }

    public void ClearBuffIcons()
    {
        for (int i = 0; i < buffIcons.transform.childCount; i++)
        {
            Destroy(buffIcons.transform.GetChild(i).gameObject);
        }
    }
}
