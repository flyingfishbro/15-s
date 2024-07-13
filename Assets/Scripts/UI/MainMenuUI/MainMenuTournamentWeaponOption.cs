using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무기 옵션을 나타내기 위한 컴포넌트입니다.
/// </summary>
public class MainMenuTournamentWeaponOption : MainMenuTournamentOptionBase
{
    /// <summary>
    /// 무기 코드를 반환합니다.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.Exception">인덱스 범위 관련 예외</exception>
    public string GetWeaponOptionValue()
    {
        // 현재 표시중인 옵션 인덱스에 따라 분기합니다.
        switch (optionIndex)
        {
            case 0: return "w00001";
            case 1: return "w00002";
            case 2: return "w00003";
            case 3: return "w00004";
        }

        throw new System.Exception("인덱스가 잘못 되었습니다.");
    }

}
