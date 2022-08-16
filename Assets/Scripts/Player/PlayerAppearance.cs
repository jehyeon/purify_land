using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerAppearance : MonoBehaviour
{
    private SPUM_SpriteList _playerApperance;
    
    public enum Part
    {
        Hair,
        Cloth,
        Armor,
        Pant,
        Weapon,
        Back
    }

    private void Start()
    {
        _playerApperance = transform.GetComponent<SPUM_Prefabs>()._spriteOBj;
        SwapApprearance(Part.Armor, "SPUM/SPUM_Sprites/Items/5_Armor/Armor_1");
        SwapApprearance(Part.Weapon, "SPUM/SPUM_Sprites/Items/6_Weapons/Soon_Spear", 2);
    }

    public void SwapApprearance(Part _part, string _partPath, int partIdx = -1)
    {
        List<SpriteRenderer> objList = null; // 스프라이트 오브젝트 리스트
        List<string> pathList = null; // 파일 경로 리스트
        var numOfParts = 0; // 함수 실행시 변환되는 스프라이트 수
        
        switch (_part)
        {
            // List Index Info
            // Hair, Helmet1, Helmet2, Face_Hair
            case Part.Hair:
                objList = _playerApperance._hairList;
                pathList = _playerApperance._hairListString;
                numOfParts = 1;
                break;
            // Body_Cloth, Left_Arm, Right_Arm
            case Part.Cloth:
                objList = _playerApperance._clothList;
                pathList = _playerApperance._clothListString;
                numOfParts = 3;
                break;
            // Body_Armor, Left_Shoulder, Right_Shoulder
            case Part.Armor:
                objList = _playerApperance._armorList;
                pathList = _playerApperance._armorListString;
                numOfParts = 3;
                break;
            // Left_Cloth, Right_Cloth
            case Part.Pant:
                objList = _playerApperance._pantList;
                pathList = _playerApperance._pantListString;
                numOfParts = 2;
                break;
            // Right_Weapon, Right_Shield, Left_Weapon, Left_Shield
            case Part.Weapon:
                objList = _playerApperance._weaponList;
                pathList = _playerApperance._weaponListString;
                numOfParts = 1;
                break;
            // Horse_Sprite, Horse_string, Body_Texture, Body_string
            case Part.Back:
                objList = _playerApperance._backList;
                pathList = _playerApperance._backListString;
                numOfParts = 1;
                break;
        }
        
        if(objList != null && pathList != null)
        {
            Sprite[] tmpSprites = Resources.LoadAll<Sprite>(_partPath);
            
            for (var i = 0; i < numOfParts; i++)
            {                
                if (numOfParts == 1 && partIdx == -1)
                {
                    // 에러
                    // 적용되는 장비의 수가 1개라면 반드시 인덱스를 명시하여 사용
                    // ex) Hair, Weapons
                    return;
                }
                
                
                if (numOfParts == 1 && partIdx > -1)
                {
                    objList[partIdx].sprite = tmpSprites[0];
                    pathList[partIdx] = "Assets/Resources/" + _partPath;
                }
                else
                {
                    objList[i].sprite = tmpSprites[i];
                    pathList[i] = "Assets/Resources/" + _partPath;
                }
            }
        }
        else
        {
            // 에러
            // List == null
        }
    }
    
    
}
