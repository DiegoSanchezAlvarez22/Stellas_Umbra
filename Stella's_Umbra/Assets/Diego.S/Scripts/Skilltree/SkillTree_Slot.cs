using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTree_Slot : MonoBehaviour, IPointerClickHandler
     , IPointerEnterHandler
     , IPointerExitHandler
{
    #region Variables
    private PlayerExpSystem _playerExpSystem;
    private PlayerMov _playerMov;
    private PlayerAttacks _playerAttacks;
    //public SkillDescScript skillScript;

    [SerializeField] private string _skillName;
    [SerializeField] private int skillLevel;
    [SerializeField] private int _skillExpValue;
    [SerializeField] private bool isLearned;
    
    public SkillTree_Link[] linkToThis;
    public SkillTree_Link[] linkGoOut;
    public RawImage rawImageSkill;
    private RawImage m_RawImage;
    #endregion

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        _playerExpSystem = player.GetComponent<PlayerExpSystem>();
        _playerMov = player.GetComponent<PlayerMov>();
        _playerAttacks = player.GetComponent<PlayerAttacks>();
    }

    void Start()
    {
        m_RawImage = GetComponent<RawImage>();
        //rawImageSkill.texture = skillScript.skillImage;
    }

    public void Learn()
    {
        if (!isLearned)
        {
            isLearned = true;
            m_RawImage.color = linkToThis[0].linkedColor;

            PlayerExpSystem._playerExpSystemInstance._currentExp -= _skillExpValue;
            PlayerExpSystem._playerExpSystemInstance.UpdateExp();
            _playerMov.MovSkillsActivation(_skillName, isLearned);
            _playerAttacks.AttackSkillsActivation(_skillName, isLearned);
        }
    }
    public void UnLearn()
    {
        if (isLearned)
        {
            isLearned = false;
            m_RawImage.color = Color.white;

            PlayerExpSystem._playerExpSystemInstance._currentExp += _skillExpValue;
            PlayerExpSystem._playerExpSystemInstance.UpdateExp();
            _playerMov.MovSkillsActivation(_skillName, isLearned);
            _playerAttacks.AttackSkillsActivation(_skillName, isLearned);
        }
    }

    bool CheckLinkToThis()
    {
       foreach(SkillTree_Link links in linkToThis)
       {
            if (!links.isOpen)
                return false;
       }
        return true;
    }

    void LinkedLinks()
    {
        foreach (SkillTree_Link links in linkToThis)
        {
            links.Linked();
        }
    }

    void OpenLinks(SkillTree_Link[] arr)
    {
        foreach (SkillTree_Link links in arr)
        {
            links.OpenLink();
        }
    }

    void CloseLinks(SkillTree_Link[] arr)
    {
        foreach (SkillTree_Link links in arr)
        {
            links.CloseLink();
        }
    }

    bool CheckIfAnyLinkedToThis()
    {
       foreach (SkillTree_Link links in linkGoOut)
       {
            if (links.isLinked)
                return false;
       }
        return true;
    }

    public void OnPointerClick(PointerEventData eventData) 
    {
        if (CheckLinkToThis())
        {
            if (!isLearned && _playerExpSystem._currentExp > 0)
            {
                if (linkGoOut.Length > 0)
                {
                    OpenLinks(linkGoOut);
                }
                LinkedLinks();
                Learn();
            }
            else
            {
                if (linkGoOut.Length > 0)
                {
                    if (CheckIfAnyLinkedToThis())
                    {
                        CloseLinks(linkGoOut);
                        OpenLinks(linkToThis);
                        UnLearn();
                    }
                }
                else
                {
                    OpenLinks(linkToThis);
                    UnLearn();
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //SkillTree_ToolTip.instance.ShowDesc(skillScript, skillLevel,transform.position);
        if (!isLearned)
        {
            m_RawImage.color = Color.grey;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //SkillTree_ToolTip.instance.HideDesc();
        if (!isLearned)
        {
            m_RawImage.color = Color.white;
        }
    }
}
