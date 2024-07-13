using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 1. MainMenu Scene �� ��ʸ�Ʈ �ɼ� �ϳ��� ��Ÿ���� ���� ������Ʈ�Դϴ�.
/// �ش� Ŭ������ �ɼǺ��� ��ӽ��� ����մϴ�.
/// </summary>
public abstract class MainMenuTournamentOptionBase : MonoBehaviour
{
    [Header("# ȭ��ǥ ��ư")]
    public Button m_DownButton;

    [Header("# �ɼ� ��� Transform")]
    public List<Transform> m_OptionItemTransforms;

    /// <summary>
    /// ���� ǥ������ ������ ��� �ε����� ��Ÿ���ϴ�.
    /// </summary>
    [Header("# ���� �ε���")][SerializeField]
    private int _ShowItemIndex;

    /// <summary>
    /// ���� ǥ������ �ɼ� ��� �ε����� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public int optionIndex => _ShowItemIndex;

    protected virtual void Awake()
    {
        m_DownButton.onClick.AddListener(CALLBACK_OnNextOptionButtonClicked);
    }

    protected virtual void Start()
    {
        // �ʱ� �ɼ� ����
        SetOptionItemIndex(_ShowItemIndex);
    }

    /// <summary>
    /// �ɼ� ��� �ε��� ����
    /// </summary>
    /// <param name="newItemIndex">���Ӱ� ������ų index �� �����մϴ�.</param>
    private void SetOptionItemIndex(int newItemIndex)
    {
        _ShowItemIndex = newItemIndex;

        // �ɼ� �̹��� Ȱ/��Ȱ��ȭ
        for(int i = 0; i < m_OptionItemTransforms.Count; ++i)
        {
            m_OptionItemTransforms[i].gameObject.SetActive(i == newItemIndex);
        }


        GameManager.instance.SetGravity((GameManager.GravityType)_ShowItemIndex);
    }

    private void NextOption()
    {
        // ������ų �ε����� ���� �ɼ� �ε����� ����
        int newIndex = (_ShowItemIndex == m_OptionItemTransforms.Count - 1) ?
            0 : _ShowItemIndex + 1;

        // �ɼ� �ε��� ����
        SetOptionItemIndex(newIndex);
    }

    private void CALLBACK_OnNextOptionButtonClicked() => NextOption();
}
