    using UnityEngine;

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BaseData", order = 1)]
    public class BaseData : ScriptableObject
    {
        public Vector2 LeftPoint = new Vector2(-3, 0);
        public Vector2 MiddlePoint =  new Vector2(0, 0);
        public Vector2 RightPoint = new Vector2(3, 0);
    }
