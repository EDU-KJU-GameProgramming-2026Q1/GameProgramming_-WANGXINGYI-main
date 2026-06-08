using UnityEngine;
using UnityEngine.InputSystem;

public class EX_Trigger_Mouse : MonoBehaviour
{
    [Header("Settings")]
    public Transform CameraTransform;
    public Vector3 Offset = new Vector3(-0.5f, -0.3f, 0.5f);
    public float maxDistance = 20f;
    public LayerMask InteractableLayer;

    [Header("Visuals")]
    public LineRenderer LineRenderer;
    public GameObject HitPointer;

    private IInterface LastHitTarget;
    private Transform GrabableObject;

    [Header("Throw")]
    private Vector3 PreviousPosition;
    private Vector3 CurrentVelocity;
    public float throwSpeed = 2f;

    [Header("Input Settings")]
    // 인스펙터에서 버튼을 선택
    public InputAction MouseAction;

    [Header("Grab Settings")]
    private Vector3 hitOffset;

    Transform DefaultParent;
    private void OnEnable()
    {
        // InputAction은 사용 전에 반드시 활성화해야 합니다.
        MouseAction.Enable();
    }

    private void OnDisable()
    {
        MouseAction.Disable();
    }

    void Update()
    {
        UpdateControllerPosition();

        // 1. Grab 해제: 마우스 버튼을 떼면 조건 없이 해제 시도
        if (MouseAction.WasReleasedThisFrame() && GrabableObject != null)
        {
            ReleaseObject();
            return;
        }

        // 2. 무언가를 잡고 있을 때의 라인 처리
        if (GrabableObject != null)
        {
            // 매 프레임 컨트롤러의 속도 계산 (던지기용)
            CalculateVelocity();
            UpdateGrabLine(); // 잡고 있는 동안 라인을 물체에 고정
            return;
        }

        // 3. 인터랙션 시도
        //PerformRaycast();
        if (MouseAction.IsPressed())
        {
            PerformRaycast();
        }
        else
        {
            ResetInteraction();
        }
    }

    void UpdateControllerPosition()
    {
        Vector3 targetPos = CameraTransform.TransformPoint(Offset);
        transform.position = targetPos;
        transform.rotation = CameraTransform.rotation;
    }

    void PerformRaycast()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        LineRenderer.enabled = true;
        LineRenderer.SetPosition(0, transform.position);

        if (Physics.Raycast(ray, out hit, maxDistance, InteractableLayer))
        {
            LineRenderer.SetPosition(1, hit.point);
            HitPointer.SetActive(true);
            HitPointer.transform.position = hit.point + (hit.normal * 0.01f);
            HitPointer.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            IInterface targetInterface = hit.collider.GetComponent<IInterface>();

            if (targetInterface != null)
            {
                // Hover 처리 (Enter)
                if (LastHitTarget != targetInterface)
                {
                    LastHitTarget?.OnExit();
                    LastHitTarget = targetInterface;
                    LastHitTarget.OnEnter();
                }

                // --- [핵심 로직] 클릭한 순간의 처리 ---
                if (MouseAction.WasPressedThisFrame())
                {
                    // 대상에게 Grab Actor가 있는가?
                    if (hit.collider.GetComponent<EX_Actor_Mouse>() != null)
                    {
                        //print("grab");
                        // Actor가 있으면 '잡기'
                        GrabObject(hit.collider.transform, targetInterface, hit.point);
                    }
                    else
                    {
                        //print("action");
                        // Actor가 없으면 '일반 실행'
                        targetInterface.OnAction();
                    }
                }
            }
            else
            {
                ResetInteractionState();
            }
        }
        else
        {
            LineRenderer.SetPosition(1, transform.position + transform.forward * maxDistance);
            HitPointer.SetActive(false);
            ResetInteractionState();
        }
    }

    void UpdateGrabLine()
    {
        LineRenderer.enabled = true;
        LineRenderer.SetPosition(0, transform.position); // 시작점: 내 손
        // 물체의 현재 위치 + (회전을 고려한) 오프셋 지점을 끝점으로 설정
        Vector3 worldHitPoint = GrabableObject.TransformPoint(hitOffset);
        LineRenderer.SetPosition(1, worldHitPoint);
        //LineRenderer.SetPosition(1, GrabableObject.position); // 끝점: 물체 중심 (또는 피벗)

        // 잡고 있는 동안에는 히트 포인터가 필요 없으므로 숨김
        HitPointer.SetActive(false);
    }

    void GrabObject(Transform target, IInterface targetInterface, Vector3 hitPoint) // hitPoint 매개변수 추가    
    { 
        GrabableObject = target;
        DefaultParent = GrabableObject.parent;
        // 잡은 지점이 물체의 중심으로부터 얼마나 떨어져 있는지 로컬 좌표로 저장
        // InverseTransformPoint를 써야 물체가 회전해도 정확한 지점을 따라옵니다.
        hitOffset = target.InverseTransformPoint(hitPoint);
        PreviousPosition = GrabableObject.position;

        Rigidbody rb = GrabableObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 1. 속도를 먼저 0으로 초기화 (물리 상태일 때)
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // 2. 그 다음에 키네마틱으로 전환
            rb.isKinematic = true;
        }

        targetInterface?.OnAction();
        GrabableObject.SetParent(this.transform);

        //LineRenderer.enabled = false;
        HitPointer.SetActive(false);
    }

    // 컨트롤러의 실시간 속도를 추적하는 함수
    void CalculateVelocity()
    {
        // 속도 = (현재 위치 - 이전 위치) / 프레임 시간
        CurrentVelocity = (GrabableObject.position - PreviousPosition) / Time.deltaTime;
        PreviousPosition = GrabableObject.position;
    }

    void ReleaseObject()
    {
        IInterface targetInterface = GrabableObject.GetComponent<IInterface>();
        targetInterface?.OnExit();

        // 4. 놓을 때 다시 물리 연산 활성화
        Rigidbody rb = GrabableObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;

            // 컨트롤러가 움직이던 속도를 물체에 전달
            rb.velocity = CurrentVelocity * throwSpeed;

            // 약간의 회전 랜덤성을 주어 더 자연스럽게 던져지게 함
            rb.angularVelocity = Random.insideUnitSphere * 5f;
        }

        GrabableObject.SetParent(DefaultParent );
        GrabableObject = null;
    }

    void ResetInteraction()
    {
        if (GrabableObject != null) return;
        LineRenderer.enabled = false;
        HitPointer.SetActive(false);
        ResetInteractionState();
    }

    void ResetInteractionState()
    {
        if (LastHitTarget != null)
        {
            LastHitTarget.OnExit();
            LastHitTarget = null;
        }
    }
}