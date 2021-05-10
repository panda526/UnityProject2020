using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct skillGroup
{
	public string skillName;//技能名稱
	public Sprite skillIcon;//技能圖片
}
[CreateAssetMenu(fileName = "PlayerSkillDB", menuName = "DB/PlayerSkillDB", order = 6)]
public class PlayerSkillDB : ScriptableObject
{
    public List<skillGroup> skillList;
    public Sprite GetSkillImg(string name)
    {
		for(int i=0; i<skillList.Count;i++)
			{
				if(skillList[i].skillName==name)
				{
					return skillList[i].skillIcon;
				}
			}
        return null;
    }
}
