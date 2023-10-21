using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isDebug = true;
    public bool AndroidController = false;
    
    public enum Swipe
    {
        Middle, Left, Right, Abroad    
    }
    
    public float MoveSpeed = 10.0f;
    public float JumpForce = 350.0f;
   
    public bool isPermissionToSwipe = true;
    public bool isSliding = false;

    public Vector2 LeftPoint = new Vector2(-3, 0);
    public Vector2 MiddlePoint = new Vector2(0, 0);
    public Vector2 RightPoint = new Vector2(3, 0);
    
    private Vector3 _targetPosition;
    private bool _isGrounded = true;
    private bool _isSwiping = false;
    private Swipe CurrentSwipe;
    //inside class
    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;
  
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
        _isGrounded = CheckGrounded();
        SwipeInCurrentPlatform();
        CheckInSwipePosition();
        if (CheckInCritPoint())
        {
            float newPosX = Vector3.MoveTowards(transform.position, _targetPosition, MoveSpeed * Time.deltaTime).x;
            transform.position = new Vector3(newPosX, transform.position.y);
        }
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
        Swipe swipe = CurrentSwipe;
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
        if (CurrentSwipe == Swipe.Left)
        {
            _targetPosition = LeftPoint;
        }
        else if (CurrentSwipe == Swipe.Right)
        {
            _targetPosition = RightPoint;
        }
        else if (CurrentSwipe == Swipe.Middle)
        {
            _targetPosition = MiddlePoint;
        }
        else if (CurrentSwipe == Swipe.Abroad)
        {
            if (transform.position.x >= RightPoint.x)
            {
                _targetPosition = RightPoint;
                CurrentSwipe = Swipe.Right;
            }
            else if (transform.position.x <= LeftPoint.x)
            {
                _targetPosition = LeftPoint;
                CurrentSwipe = Swipe.Left;
            }
            Debug.LogError("Added Collision Point");
        }
    }
    
  private void PerformJump()
    {
        if (_isGrounded)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
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
            if (CurrentSwipe == Swipe.Middle)
                CurrentSwipe = Swipe.Left;
            else if (CurrentSwipe == Swipe.Left)
                CurrentSwipe = Swipe.Abroad;
            else
                CurrentSwipe = Swipe.Middle;
        }
        else
        {
            if (CurrentSwipe == Swipe.Middle)
                CurrentSwipe = Swipe.Right;
            else if (CurrentSwipe == Swipe.Right)
                CurrentSwipe = Swipe.Abroad;
            else
                CurrentSwipe = Swipe.Middle;
        }
        if (isDebug) Debug.Log("MovePlayer Swipe");        
    }
}
