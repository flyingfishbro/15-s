using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchInputManager : ManagerBase<TouchInputManager>
{


    private GraphicRaycaster raycaster;
    private PointerEventData eventData;

    private List<RaycastResult> rayResults = new List<RaycastResult>();




    private bool isBattleMode => BattleManager.instance.isBattle;

    public List<TouchInputSystem> battleInputSystemList = new List<TouchInputSystem>();
    public List<TouchInputSystem> progressInputSystemList = new List<TouchInputSystem>();

    public void RegisterInputSystem(TouchInputSystem inputSystem)
    {
        if (inputSystem.inputType == TouchInputSystem.TouchInputType.BATTLE)        battleInputSystemList.Add(inputSystem);
        else if (inputSystem.inputType == TouchInputSystem.TouchInputType.PROGRESS) progressInputSystemList.Add(inputSystem);


    }

    protected virtual void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        if (raycaster == null) raycaster = transform.AddComponent<GraphicRaycaster>();

        eventData = new PointerEventData(EventSystem.current);
    }


    private void Update()
    {
        List<TouchInputSystem> targetInputList;
        List<TouchInputSystem> NonTargetList;

        if (isBattleMode)
        {
            targetInputList = battleInputSystemList;
            NonTargetList = progressInputSystemList;
        }
        else
        {
            targetInputList = progressInputSystemList;
            NonTargetList = battleInputSystemList;
        }


        bool[] touchResult = ObservingTouchInput(targetInputList, out Vector2[] inputPos);
        
        for (int i = 0; i < targetInputList.Count; ++i)
        {
            targetInputList[i].SetInput(touchResult[i], inputPos[i]);
        }

        //isBattleMode�� �������� �Է°��� ���ٰ� isBattleMode�� �ٲ�鼭 false�� �Էµ��� �ʰ� �ٷ� �Ѿ�⶧���� ���� false�� �������־����. ex) aimInput���׻���
        foreach (var inputSystem in NonTargetList)
        {
            inputSystem.SetInput(false);
        }


    }


    /// <summary>
    /// �Ѱܹ��� ��ǲ���� ���� ����ڰ� ��ġ�ϴ� ��ġ�� �ִ��� üũ�ϰ� ���� ��ġ�ϰ� �ִٸ� out �Ű������� ���� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="targetInputList">üũ ����� �Ǵ� ��ǲ ����Ʈ</param>
    /// <param name="inputPos">��ǲ ��ġ ������ ����Ʈ</param>
    /// <returns></returns>
    private bool[] ObservingTouchInput(List<TouchInputSystem> targetInputList, out Vector2[] inputPos)
    {
        bool[] touchResult = new bool[targetInputList.Count];
        inputPos = new Vector2[targetInputList.Count];


        rayResults.Clear();

        foreach (var touch in Input.touches)
        {
            eventData.position = touch.position;
            raycaster.Raycast(eventData, rayResults);
        }


        for (int i = 0; i < targetInputList.Count; ++i)
        {
            foreach (var rayResult in rayResults)
            {
                if (rayResult.gameObject == targetInputList[i].gameObject)
                {
                    touchResult[i] = true;
                    inputPos[i] = rayResult.screenPosition;
                }
            }

        }

        return touchResult;
    }


}
