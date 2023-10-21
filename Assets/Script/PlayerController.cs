using System;
using UnityEngine;

public class PlayerController : MonoBehaviourSingleton<PlayerController>
{
    public bool isDebug = true;
    public bool AndroidController = false;
   [field: SerializeField] public Swipe SwipeState { get; private set; }

    public enum Swipe
    {
        Middle, Left, Right, Abroad    
    }
    
    public float MoveSpeed = 10.0f;
    public float JumpForce = 350.0f;
   
    public bool isPermissionToSwipe = true;
    public bool isSliding = false;
    public float MovePosZ;
    
    private Vector3 _targetPosition;
    private bool _isGrounded = true;
    private bool _isSwiping = false;
   
    //inside class
    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;
    private BaseData _baseData;
    private Rigidbody _rb;

    
    private void Awake()
    {
        //TODO adad
        _baseData = Resources.Load("BaseData") as BaseData;
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _targetPosition = transform.position;
#if UNITY_ANDROID
AndroidController = true;
#else
        AndroidController = false;
#endif
    }

    void Update()
    {
      
    }
    
    
    

    private void ChangesMovePlayer()
    {
        float newPosX = 0;

        if (CheckInCritPoint())
        {
            newPosX = Vector3.MoveTowards(transform.position, _targetPosition, MoveSpeed).x;
            transform.position = new Vector3(newPosX, transform.position.y, transform.position.z );
        }
    }
    
    

    private void FixedUpdate()
    {
        if(!GameManager.Instance.IsPlayGame)
            return;
        
        _isGrounded = CheckGrounded();
        SwipeInCurrentPlatform();
        CheckInSwipePosition();
        ChangesMovePlayer();

        transform.position += new Vector3(0,0, MovePosZ);
    }

    private void SwipeInCurrentPlatform()
    {
        if (AndroidController)
            SwipeAndroid();
        else
            SwipeWindow();
    }

    private void SwipeWindow()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        }
        if(Input.GetMouseButtonUp(0))
        {
            //save ended touch 2d point
            secondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
       
            //create vector from the two points
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
           
            //normalize the 2d vector
            currentSwipe.Normalize();
 
            //swipe upwards
            if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                Debug.Log("up swipe");
                PerformJump();
            }
            //swipe down
            if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                Debug.Log("down swipe");
            }
            //swipe left
            if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                Debug.Log("left swipe");
                ChangesStateSwipe(true);
            }
            //swipe right
            if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                Debug.Log("right swipe");
                ChangesStateSwipe(false);
            }
        }
    }
    
    private bool CheckInCritPoint()
    {
        Swipe swipe = SwipeState;
        float posX = transform.position.x;
        float targetX = _targetPosition.x;
        if (swipe == Swipe.Left)
            return targetX < posX - 0.05f;
        else if(swipe == Swipe.Right)
            return targetX  > posX + 0.05f;
        else if (swipe == Swipe.Middle)
            return targetX > posX + 0.05f || targetX < posX - 0.05f;
        else
            return false;
    }

    private void CheckInSwipePosition()
    {
        if (SwipeState == Swipe.Abroad)
        {
            if (transform.position.x >= _baseData.RightPoint.x)
            {
                _targetPosition = _baseData.RightPoint;
                SwipeState = Swipe.Right;
            }
            else if (transform.position.x <= _baseData.LeftPoint.x)
            {
                _targetPosition = _baseData.LeftPoint;
                SwipeState = Swipe.Left;
            }
            Debug.LogError("Added Collision Point");
        }
        else
             _targetPosition =  GetPositionInCurrentState.GetPosBaseData(_baseData, SwipeState);
    }
    
  private void PerformJump()
    {
        if (_isGrounded)
        {
            _rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }

   private bool CheckGrounded()
    {
        float rayLength = 1.0f;
        Vector3 rayStart = transform.position;

        if (Physics.Raycast(rayStart, Vector3.down, rayLength))
        {
            if (isDebug) Debug.Log("Player is grounded.");
            return true;
        }
        else
        {
            if (isDebug) Debug.Log("Player is not grounded.");
            return false;
        }
    }
    
    private void SwipeAndroid()
    {
        if(Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if(t.phase == TouchPhase.Began)
            {
                //save began touch 2d point
                firstPressPos = new Vector2(t.position.x,t.position.y);
            }
            if(t.phase == TouchPhase.Ended)
            {
                //save ended touch 2d point
                secondPressPos = new Vector2(t.position.x,t.position.y);
                           
                //create vector from the two points
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
               
                //normalize the 2d vector
                currentSwipe.Normalize();
 
                //swipe upwards
                if(currentSwipe.y > 0 && currentSwipe.x > -0.5f &&currentSwipe.x < 0.5f)
                {
                    Debug.Log("up swipe");
                    PerformJump();
                }
                //swipe down
                if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    Debug.Log("down swipe");
                }
                //swipe left
                if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    Debug.Log("left swipe");
                    ChangesStateSwipe(true);
                }
                //swipe right
                if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    Debug.Log("right swipe");
                    ChangesStateSwipe(false);
                }
            }
        }
    }
    
    private void ChangesStateSwipe(bool isLeftPos)
    {
        if (isLeftPos)
        {
            if (SwipeState == Swipe.Middle)
                SwipeState = Swipe.Left;
            else if (SwipeState == Swipe.Left)
                SwipeState = Swipe.Abroad;
            else
                SwipeState = Swipe.Middle;
        }
        else
        {
            if (SwipeState == Swipe.Middle)
                SwipeState = Swipe.Right;
            else if (SwipeState == Swipe.Right)
                SwipeState = Swipe.Abroad;
            else
                SwipeState = Swipe.Middle;
        }
        if (isDebug) Debug.Log("MovePlayer Swipe");        
    }
}
