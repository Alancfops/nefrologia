using UnityEngine;
using Unity.XR.CoreUtils;  

public class SetCameraSpawnPosition : MonoBehaviour {
    public Transform spawnPoint; 
    private XROrigin xrOrigin;

    void Start() {
        xrOrigin = GetComponent<XROrigin>();

        if (xrOrigin != null && spawnPoint != null) {
            // Move o XR Origin para o Spawn Point
            xrOrigin.MoveCameraToWorldLocation(spawnPoint.position);
            xrOrigin.MatchOriginUpCameraForward(spawnPoint.up, spawnPoint.forward);
        }
    }
}
