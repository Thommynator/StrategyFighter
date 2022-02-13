using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker current;
    private Animator cameraAnimator;
    void Start()
    {
        current = this;
        cameraAnimator = GetComponent<Animator>();
    }

    public void Shake(ShakeStrength strength)
    {
        switch (strength)
        {
            case ShakeStrength.WEAK:
                cameraAnimator.SetTrigger("WeakShake");
                break;

            case ShakeStrength.MEDIUM:
                Debug.Log("NOT IMPLEMENTED");
                break;

            case ShakeStrength.STRONG:
                Debug.Log("NOT IMPLEMENTED");
                break;
        }

    }

    public enum ShakeStrength
    {
        WEAK, MEDIUM, STRONG
    }
}
