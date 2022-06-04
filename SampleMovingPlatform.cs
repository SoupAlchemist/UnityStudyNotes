using UnityEngine;
public class SampleMovingPlatform : MonoBehaviour
{
    private GameObject player;
    
    private enum MovingPlatformStates
    {
        Stop,
        Moving,
        Wait,
    }
    private MovingPlatformStates States;
    private enum MovingPlatformModel
    {
        circulate,
        reciprocate,
    }
    private MovingPlatformModel Model;
    
    public Transform[] TargetPos;
    private int i;
    private int iLength;
    private float dir;
    
    public bool NeedTrigger = false;
    private float mTimer;
    public float MovingStopTime;
    public float MovingWaitTime;
    public float MovingPlatformSpeed;
    private bool OnPlatform;
    [Range(0, 2f)] public float PlatformWeight = 1f;
    
    private Vector3 offSet;
    private Vector3 oriPos;
    private Vector3 nowPos;
    
    private void Awake()
    {
        i = 0;
        dir = 1;
        iLength = TargetPos.Length - 1;
        mTimer = Time.time;
        States = NeedTrigger ? MovingPlatformStates.Wait : MovingPlatformStates.Stop;
        player = GameObject.FindWithTag("Player");
    }
    private void Update()
    {
        
        if (States == MovingPlatformStates.Stop)
        {
            if (Time.time - mTimer > MovingStopTime)
            {
                Geti();
            }
        }else if (States == MovingPlatformStates.Moving) 
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPos[i].position, MovingPlatformSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, TargetPos[i].position) < 0.01f)
            {
                mTimer = Time.time;
                States = NeedTrigger ? MovingPlatformStates.Wait : MovingPlatformStates.Stop;
            }
        }else if (States == MovingPlatformStates.Wait)
        {
            if (OnPlatform && Time.time - mTimer > MovingWaitTime)
            {
                Geti();
            }
        }
        if (OnPlatform)
        {
            nowPos = transform.position;                            
            offSet = transform.position - oriPos;                   
            player.transform.position += offSet * PlatformWeight;   
            oriPos = nowPos;                                        
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            oriPos = transform.position; 
            OnPlatform = true;
            player = col.gameObject;
            if (States == MovingPlatformStates.Wait) mTimer = Time.time;
            //col.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnPlatform = false;
            //other.transform.SetParent(null);
        }
    }

    void Geti()
    {
        if (Model == MovingPlatformModel.circulate)
        {
            if (i < iLength)  i++; else  i = 0;
            States = MovingPlatformStates.Moving;
        }
        else if(Model == MovingPlatformModel.reciprocate)
        {
            if (dir == 1)
            {
                if (i < iLength)
                {
                    i++;
                    States = MovingPlatformStates.Moving;
                }
                else
                {
                    dir = -1;
                }
            }else if (dir == -1)
            {
                if (i > 0)
                {
                    i--;
                    States = MovingPlatformStates.Moving;
                }
                else
                {
                    dir = 1;
                }
            }
        }
    }
}