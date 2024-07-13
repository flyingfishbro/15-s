using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 1. MainMenu Scene 의 토너먼트 옵션 하나를 나타내기 위한 컴포넌트입니다.
/// 해당 클래스를 옵션별로 상속시켜 사용합니다.
/// </summary>
public abstract class MainMenuTournamentOptionBase : MonoBehaviour
{
    [Header("# 화살표 버튼")]
    public Button m_DownButton;

    [Header("# 옵션 요소 Transform")]
    public List<Transform> m_OptionItemTransforms;

    /// <summary>
    /// 현재 표시중인 아이템 요소 인덱스를 나타냅니다.
    /// </summary>
    [Header("# 시작 인덱스")][SerializeField]
    private int _ShowItemIndex;

    /// <summary>
    /// 현재 표시중인 옵션 요소 인덱스에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public int optionIndex => _ShowItemIndex;

    protected virtual void Awake()
    {
        m_DownButton.onClick.AddListener(CALLBACK_OnNextOptionButtonClicked);
    }

    protected virtual void Start()
    {
        // 초기 옵션 설정
        SetOptionItemIndex(_ShowItemIndex);
    }

    /// <summary>
    /// 옵션 요소 인덱스 설정
    /// </summary>
    /// <param name="newItemIndex">새롭게 설정시킬 index 를 전달합니다.</param>
    private void SetOptionItemIndex(int newItemIndex)
    {
        _ShowItemIndex = newItemIndex;

        // 옵션 이미지 활/비활성화
        for(int i = 0; i < m_OptionItemTransforms.Count; ++i)
        {
            m_OptionItemTransforms[i].gameObject.SetActive(i == newItemIndex);
        }


        GameManager.instance.SetGravity((GameManager.GravityType)_ShowItemIndex);
    }

    private void NextOption()
    {
        // 설정시킬 인덱스를 다음 옵션 인덱스로 설정
        int newIndex = (_ShowItemIndex == m_OptionItemTransforms.Count - 1) ?
            0 : _ShowItemIndex + 1;

        // 옵션 인덱스 설정
        SetOptionItemIndex(newIndex);
    }

    private void CALLBACK_OnNextOptionButtonClicked() => NextOption();
}
