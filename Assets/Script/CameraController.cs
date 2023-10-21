using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController PlayerController;
    public float OffesetZPos = -6.50f;
    public float OffesetXPos = 1f;
    public float MoveSpeed = 2f;
    private float TargetXPos;
    private BaseData _baseData;
  
    private void Awake()
    {
        _baseData = Resources.Load("BaseData") as BaseData;
    }

    private void Start()
    {
        PlayerController = PlayerController.Instance;
    }

    private void Update()
    {
        MoveToPlayer();
    }

    private void LateUpdate()
    {
        //Vector3 newVector3 = new Vector3(TargetXPos,  transform.position.y, PlayerController.transform.position.z + OffesetZPos);
        //transform.position = Vector3.MoveTowards(transform.position, newVector3, MoveSpeed * Time.deltaTime);
        transform.position  = new Vector3(PlayerController.transform.position.x,  transform.position.y, PlayerController.transform.position.z + OffesetZPos);
    }

    public void MoveToPlayer()
    {
         TargetXPos =  GetPositionInCurrentState.GetPosBaseData(_baseData, PlayerController.SwipeState, OffesetXPos).x;
    }
}