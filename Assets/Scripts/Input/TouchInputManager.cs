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

        //isBattleMode를 기준으로 입력값이 들어가다가 isBattleMode가 바뀌면서 false가 입력되지 않고 바로 넘어가기때문에 직접 false를 전달해주어야함. ex) aimInput버그생김
        foreach (var inputSystem in NonTargetList)
        {
            inputSystem.SetInput(false);
        }


    }


    /// <summary>
    /// 넘겨받은 인풋들이 현재 사용자가 터치하는 위치에 있는지 체크하고 만일 터치하고 있다면 out 매개변수를 통해 반환합니다.
    /// </summary>
    /// <param name="targetInputList">체크 대상이 되는 인풋 리스트</param>
    /// <param name="inputPos">인풋 터치 포지션 리스트</param>
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
