using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public static class TeleportAnchorHelper
{ 
    public static bool GenerateManualTeleportRequest(this TeleportationAnchor anchor, IXRInteractor interactor, RaycastHit raycastHit, ref TeleportRequest teleportRequest)
    {
        teleportRequest.matchOrientation = MatchOrientation.TargetUpAndForward;
        if (anchor.teleportAnchorTransform == null)
            return false;

        teleportRequest.destinationPosition = anchor.teleportAnchorTransform.position;
        teleportRequest.destinationRotation = anchor.teleportAnchorTransform.rotation;
        return true;
    }

    public static void OnTeleportActivated(this XRInteractorLineVisual line)
    {
        if (line.reticle != null)
            line.reticle.SetActive(false);
    }
}
