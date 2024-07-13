using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 중력 옵션을 나타내기 위한 컴포넌트입니다.
/// </summary>
public class MainMenuTournamentGravityOption : MainMenuTournamentOptionBase
{

    /// <summary>
    /// 중력 옵션값을 반환합니다.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.Exception">인덱스 범위 관련 예외</exception>
    public GameManager.GravityType GetGravityOptionValue()
    {
        // 현재 표시중인 옵션 인덱스에 따라 분기합니다.
        switch (optionIndex)
        {
            case 0: return GameManager.GravityType.LV1;
            case 1: return GameManager.GravityType.LV2;
            case 2: return GameManager.GravityType.LV3;
        }

        throw new System.Exception("인덱스가 잘못 되었습니다.");
    }

}
