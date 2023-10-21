using UnityEngine;

public static class GetPositionInCurrentState
{
    public static Vector3 GetPosBaseData(BaseData baseData, PlayerController.Swipe curerntState)
    {
        Vector3 currentVector3 = Vector3.zero;
        if (curerntState == PlayerController.Swipe.Left)
        {
            currentVector3 = baseData.LeftPoint;
        }
        else if (curerntState == PlayerController.Swipe.Right)
        {
            currentVector3 = baseData.RightPoint;
        }
        else if (curerntState == PlayerController.Swipe.Middle)
        {
            currentVector3 = baseData.MiddlePoint;
        }
        return currentVector3;
    }
}