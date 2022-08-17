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
        FaceHair,
        Cloth,
        Pant,
        Helmet,
        Armor,
        Back,
        Weapon,
        Shield
    }

    private void Start()
    {
        _playerApperance = transform.GetComponent<SPUM_Prefabs>()._spriteOBj;
    }

    public void SwapApprearance(Part part, int equipNum = -1, string hexColor = "#FFFFFF")
    {
        // equipNum == -1 -> Part 장비 벗기
        // hexColor == #FFFFFF -> 기본 색상
        // Hair 와 겹쳐 사용 가능한 Helmet(악세사리류) 미구현
        List<SpriteRenderer> objList = null; // 스프라이트 오브젝트 리스트
        List<string> pathList = null; // 파일 경로 리스트
        string partPath = null; // 파일 경로
        var numOfParts = 0; // 함수 실행시 변환되는 스프라이트 수
        var partIdx = -1; // 변환되는 장비의 리스트 인덱스
        ColorUtility.TryParseHtmlString(hexColor, out Color color); // hexColor -> RGB color 변환

        switch (part)
        {
            // List Index Info
            // Hair -> Hair[0]
            case Part.Hair:
                objList = _playerApperance._hairList;
                pathList = _playerApperance._hairListString;
                partIdx = 0;
                numOfParts = 1;
                partPath = $"Items/0_Hair/Hair_{equipNum}";
                break;
            // Face_Hair -> Hair[3]
            case Part.FaceHair:
                objList = _playerApperance._hairList;
                pathList = _playerApperance._hairListString;
                partIdx = 3;
                numOfParts = 1;
                partPath = $"Items/1_FaceHair/FaceHair_{equipNum}";
                break;
            // Body_Cloth, Left_Arm, Right_Arm
            case Part.Cloth:
                objList = _playerApperance._clothList;
                pathList = _playerApperance._clothListString;
                numOfParts = 3;
                partPath = $"Items/2_Cloth/Cloth_{equipNum}";
                break;
            // Left_Cloth, Right_Cloth
            case Part.Pant:
                objList = _playerApperance._pantList;
                pathList = _playerApperance._pantListString;
                numOfParts = 2;
                partPath = $"Items/3_Pant/Pant_{equipNum}";
                break;
            // Helmet -> Hair[1]
            case Part.Helmet:
                objList = _playerApperance._hairList;
                pathList = _playerApperance._hairListString;
                partIdx = 1;
                numOfParts = 1;
                partPath = $"Items/4_Helmet/Helmet_{equipNum}";
                break;
            // Body_Armor, Left_Shoulder, Right_Shoulder
            case Part.Armor:
                objList = _playerApperance._armorList;
                pathList = _playerApperance._armorListString;
                numOfParts = 3;
                partPath = $"Items/5_Armor/Armor_{equipNum}";
                break;
            // Horse_Sprite, Horse_string, Body_Texture, Body_string
            case Part.Back:
                objList = _playerApperance._backList;
                pathList = _playerApperance._backListString;
                // Back 의 각 인덱스의 용도를 일부 파악하지 못해서 partIdx 를 할당하지 않음
                numOfParts = 1;
                partPath = $"Items/6_Back/Back_{equipNum}";
                break;
            // Right_Weapon -> Weapon[0]
            case Part.Weapon:
                objList = _playerApperance._weaponList;
                pathList = _playerApperance._weaponListString;
                partIdx = 0;
                numOfParts = 1;
                partPath = $"Items/7_Weapon/Weapon_{equipNum}";
                break;
            // Left_Shield -> Weapon[3]
            case Part.Shield:
                objList = _playerApperance._weaponList;
                pathList = _playerApperance._weaponListString;
                partIdx = 3;
                numOfParts = 1;
                partPath = $"Items/8_Shield/Shield_{equipNum}";
                break;
        }
        
        if(objList != null && pathList != null)
        {
            Sprite[] tmpSprites = Resources.LoadAll<Sprite>(partPath);

            if (equipNum == -1) // 장비 벗기
            {
                TakeOff(part, objList, pathList, partIdx);
                return;
            }
            
            for (var i = 0; i < numOfParts; i++)
            {
                if (numOfParts == 1 && partIdx == -1)
                {
                    // 에러
                    // 적용되는 장비의 수가 1개라면 반드시 인덱스를 명시하여 사용
                    Debug.Log("PlayerAppearance: partIdx Error");
                    return;
                }
                if (numOfParts == 1 && partIdx > -1)
                {
                    if (part == Part.Helmet) // Helmet 착용시 Hair 꺼짐
                    {
                        objList[0].sprite = null; // Hair 복구시에는 _hairListString[0] 경로 이용
                    }
                    else if (part == Part.Hair && objList[1]) // Hair 변경 중 헬멧이 착용되어 있다면
                    {
                        objList[partIdx].sprite = null; // 헬멧 착용 유지
                        pathList[partIdx] = partPath; // Hair 경로만 저장
                    }
                    else
                    {
                        objList[partIdx].sprite = tmpSprites[0];
                        objList[partIdx].color = color;
                        pathList[partIdx] = partPath;
                    }

                }
                else
                {
                    objList[i].sprite = tmpSprites[i];
                    objList[i].color = color;
                    pathList[i] = partPath;
                }
            }
        }
        else
        {
            // 에러
            Debug.Log("PlayerAppearance: null List Error");
        }
    }

    private void TakeOff(Part part, List<SpriteRenderer> objList, List<string> pathList, int partIdx)
    {
        if (partIdx > -1) // Helmet, Weapon, Shield
        {
            objList[partIdx].sprite = null;
            pathList[partIdx] = null;

            if (part == Part.Helmet) // Helmet off -> Hair on
            {
                Sprite tmpSprite = Resources.Load<Sprite>(pathList[0]);
                objList[0].sprite = tmpSprite;
            }
        }
        else // Armor
        {
            for (var i = 0; i < objList.Count; i++)
            {
                objList[i].sprite = null;
                pathList[i] = null;
            }
        }
    }
}
