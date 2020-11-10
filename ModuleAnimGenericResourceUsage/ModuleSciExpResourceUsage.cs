using System;
using UnityEngine;
using KSP;
using KSP.UI.Screens;
using System.Collections;
//using KSP_Log;
//

namespace AnimateWithResources
{
    public class ModuleSciExpResourceUsage : ModuleScienceExperiment
    {
        //public static KSP_Log.Log Log = null;

        int? resourceId;

        [KSPField]
        public string ResourceUsed = "";

        [KSPField]
        public float ResourceAmtUsedForDeploy = 0;


        //[KSPField]
        //public float PowerConsumption = 0;

        public override string GetInfo()
        {
            moduleName = "Science Experiment Resource Usage";
            string st = "\nResource Used: " + ResourceUsed +
                "\nAmt Needed To Deploy: " + ResourceAmtUsedForDeploy +
                "\nAmt Needed For Reset: " + ResourceAmtUsedForDeploy;
            return st;
        }

        bool UseResources(double amt)
        {
            if (resourceId != null)
            {
                var r = this.part.RequestResource((int)resourceId, amt);
                if (r == ResourceAmtUsedForDeploy)
                    return true;
                this.part.RequestResource((int)resourceId, -r);
                return false;
            }
            return true;
        }


        #region KSPAction
        public new void DeployAction(KSPActionParam actParams)
        {
            if (UseResources(ResourceAmtUsedForDeploy))
                base.DeployAction(actParams);
        }

        #endregion

        #region KSPEvent
        public new void DeployExperiment()
        {
            if (UseResources(ResourceAmtUsedForDeploy))
                base.DeployExperiment();
        }

        public new void DeployExperimentExternal()
        {
            if (UseResources(ResourceAmtUsedForDeploy))
                base.DeployExperimentExternal();
        }

        #endregion

        public new void Awake()
        {
            //Logg = new Log("ModuleSciExpResourceUsage");
            base.Awake();
        }
        public void Start()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                //Logg.Info("Start");
                resourceId = Common.GetResourceID(ResourceUsed);
                if (resourceId != null)
                {
                    //resPerTic = PowerConsumption * Planetarium.fetch.fixedDeltaTime;
                    //Actions["DeployAction"].active = false;
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
                this.part.GetConnectedResourceTotals((int)resourceId, out resourceAmtAvailable, out maxAmount);
                if (!Deployed)
                {
                    if (ScienceUtil.RequiredUsageInternalAvailable(base.vessel, base.part, (ExperimentUsageReqs)usageReqMaskInternal, experiment, ref usageReqMessage))
                    {
                        Actions["DeployAction"].active = Events["DeployExperiment"].guiActive = (ResourceAmtUsedForDeploy <= resourceAmtAvailable);
                        //Logg.Info("Setting DeployAction & DeployExperiment");
                    }
                    if (ScienceUtil.RequiredUsageExternalAvailable(base.vessel, FlightGlobals.ActiveVessel, (ExperimentUsageReqs)usageReqMaskExternal, experiment, ref usageReqMessage))
                    {
                        Events["DeployExperimentExternal"].guiActive = (ResourceAmtUsedForDeploy <= resourceAmtAvailable);
                        //Logg.Info("Setting DeployExperimentExternal");
                    }
                }
                yield return wait;
            }
        }
    }
}
