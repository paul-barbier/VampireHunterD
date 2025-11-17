using UnityEngine;

public class GroundType : MonoBehaviour
{
    public void CallEvent(string s)
    {
        AkUnitySoundEngine.PostEvent(s, gameObject);
    }
}
