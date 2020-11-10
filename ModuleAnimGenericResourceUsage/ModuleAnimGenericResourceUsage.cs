using System;
using UnityEngine;
using KSP;
using KSP.UI.Screens;
using System.Collections;
using KSP_Log;

namespace AnimateWithResources
{
    public class ModuleAnimGenericResourceUsage : ModuleAnimateGeneric
    {
        #region KSPFields

        [KSPField]
        public string ResourceUsed = "";

        [KSPField]
        public float ResourceAmtNeededToStart = 0;

        [KSPField]
        public float ResourceAmtNeededToEnd = 0;
        #endregion

        public override string GetInfo()
        {
            moduleName = "Deploy/Retract Resource Usage";
            string st = "\nResource Used: " + ResourceUsed +
                "\nAmt Needed To Start: " + ResourceAmtNeededToStart +
                "\nAmt Needed To End: " + ResourceAmtNeededToEnd;
            return st;
        }

        #region KSPEventsAndActions

        bool UseResources()
        {
            if (resourceId != null && animTime == 0)
            {
                var r = this.part.RequestResource((int)resourceId, (double)ResourceAmtNeededToStart);
                if (r == ResourceAmtNeededToStart)
                    return true;
                this.part.RequestResource((int)resourceId, -r);
                return false;
            }
            if (resourceId != null && animTime == 1)
            {
                var r = this.part.RequestResource((int)resourceId, (double)ResourceAmtNeededToEnd);
                if (r == ResourceAmtNeededToEnd)
                    return true;
                this.part.RequestResource((int)resourceId, -r);
                return false;
            }
            return true;
        }
        public new void Toggle()
        {
            if (!UseResources())
                return;


            base.Toggle();
        }

        public new void ToggleAction(KSPActionParam param)
        {
            if (!UseResources())
                return;
            base.ToggleAction(param);
        }


        #endregion

        int? resourceId;

        public new void Awake()
        {
            base.Awake();
            if (HighLogic.LoadedSceneIsFlight)
                Debug.Log("ModuleAnimGenericResourceUsage.Awake");
        }
        public void Start()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                resourceId = Common.GetResourceID(ResourceUsed);

                if (resourceId != null)
                {
                    Actions["ToggleAction"].active = false;
                    StartCoroutine(CheckResources());
                }
            }
        }

        IEnumerator CheckResources()
        {
            double resourceAmtAvailable, maxAmount;
            var wait = new WaitForSeconds(Common.WAITTIME);
            while (true)
            {
                if ( animTime == 0)
                {
                    this.part.GetConnectedResourceTotals((int)resourceId, out resourceAmtAvailable, out maxAmount);
                    Actions["ToggleAction"].active = Events["Toggle"].guiActive = (ResourceAmtNeededToStart <= resourceAmtAvailable);
                }
                if ( animTime == 1)
                {
                    this.part.GetConnectedResourceTotals((int)resourceId, out resourceAmtAvailable, out maxAmount);
                    Actions["ToggleAction"].active = Events["Toggle"].guiActive = (ResourceAmtNeededToEnd <= resourceAmtAvailable);
                }

                yield return wait;
            }
        }
    }
}