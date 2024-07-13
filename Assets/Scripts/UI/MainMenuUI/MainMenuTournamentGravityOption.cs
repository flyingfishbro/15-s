using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �߷� �ɼ��� ��Ÿ���� ���� ������Ʈ�Դϴ�.
/// </summary>
public class MainMenuTournamentGravityOption : MainMenuTournamentOptionBase
{

    /// <summary>
    /// �߷� �ɼǰ��� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.Exception">�ε��� ���� ���� ����</exception>
    public GameManager.GravityType GetGravityOptionValue()
    {
        // ���� ǥ������ �ɼ� �ε����� ���� �б��մϴ�.
        switch (optionIndex)
        {
            case 0: return GameManager.GravityType.LV1;
            case 1: return GameManager.GravityType.LV2;
            case 2: return GameManager.GravityType.LV3;
        }

        throw new System.Exception("�ε����� �߸� �Ǿ����ϴ�.");
    }

}
