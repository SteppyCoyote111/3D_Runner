using UnityEngine;

public static class GetPositionInCurrentState
{
    public static Vector3 GetPosBaseData(BaseData baseData, PlayerController.Swipe curerntState, float offestX = 0)
    {
        Vector3 currentVector3 = Vector3.zero;
        if (curerntState == PlayerController.Swipe.Left)
        {
            currentVector3 = baseData.LeftPoint + new Vector2(offestX, 0);
        }
        else if (curerntState == PlayerController.Swipe.Right)
        {
            currentVector3 = baseData.RightPoint - new Vector2(offestX, 0);
        }
        else if (curerntState == PlayerController.Swipe.Middle)
        {
            currentVector3 = baseData.MiddlePoint;
        }
        return currentVector3;
    }
}