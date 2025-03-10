using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MJS_Feed_In_Stand : MonoBehaviour
{
    public VRToggleButton FeedInLine1, FeedInLine2, FeedInLine3, FeedInLine4, FeedInLine5, FeedInLine6, FeedInLineA, FeedInLineB;
    public VRSnapLeverButton LeverAFeedIn, LeverBFeedOut;
    public VRToggleButton EmergencyStop, Activate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(EmergencyStop.GetCurrentState())
        {
            FeedInLine1.Reset();
            FeedInLine2.Reset();
            FeedInLine3.Reset();
            FeedInLine4.Reset();
            LeverAFeedIn.Reset();
            LeverAFeedIn.Reset();
            LeverBFeedOut.Reset();
        }
    }
}
