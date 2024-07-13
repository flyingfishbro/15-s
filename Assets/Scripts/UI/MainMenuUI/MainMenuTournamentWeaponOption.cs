using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �ɼ��� ��Ÿ���� ���� ������Ʈ�Դϴ�.
/// </summary>
public class MainMenuTournamentWeaponOption : MainMenuTournamentOptionBase
{
    /// <summary>
    /// ���� �ڵ带 ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.Exception">�ε��� ���� ���� ����</exception>
    public string GetWeaponOptionValue()
    {
        // ���� ǥ������ �ɼ� �ε����� ���� �б��մϴ�.
        switch (optionIndex)
        {
            case 0: return "w00001";
            case 1: return "w00002";
            case 2: return "w00003";
            case 3: return "w00004";
        }

        throw new System.Exception("�ε����� �߸� �Ǿ����ϴ�.");
    }

}
