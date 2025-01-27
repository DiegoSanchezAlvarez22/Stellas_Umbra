using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExpSystem : MonoBehaviour
{
    public int _currentExp = 100;

    public void AddExp(int _newExp)
    {
        _currentExp += _newExp;
        Debug.Log("Exp aumentada a: " + _currentExp);
    }

    public void SubtractExp(int _expLost)
    {
        _currentExp -= _expLost;
        Debug.Log("Exp diasminuida a: " + _currentExp);
    }

    [System.Obsolete]
    public void GetSkills()
    {
        SkillTree_Slot[] skillsSlots = FindObjectsOfType<SkillTree_Slot>();

        //Check LearnSkill
        foreach (SkillTree_Slot slot in skillsSlots)
        {
            if (slot.isLearned)
            {
                //Debug.Log("Aprendiste " + slot.skillScript.name + " lvl:" + slot.skillLevel);
                Debug.Log("Aprende skill");
            }
        }

        gameObject.SetActive(false);
    }
}
