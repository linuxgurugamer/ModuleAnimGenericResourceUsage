using System;
using UnityEngine;
using KSP;
using KSP.UI.Screens;
using System.Collections;

namespace AnimateWithResources
{
    public class ModuleAnimGroupResourceUsage : ModuleAnimationGroup
    {

        [KSPField]
        public string ResourceUsed = "";

        [KSPField]
        public float ResourceAmtUsedForDeploy = 0;

        [KSPField]
        public float MinimumResAmt = 0;

        [KSPField]
        public float PowerConsumption = 0;

        public override string GetInfo()
        {
            moduleName = "Deploy/Retract Resource Usage";
            string st = "\nResource Used: " + ResourceUsed +
                "\nAmt Needed To Deploy: " + ResourceAmtUsedForDeploy +
                "\nAmt Needed To Retract: " + MinimumResAmt +
                "\nPower Consumption: " + PowerConsumption;
            return st;
        }

        bool UseResources()
        {
            if (resourceId != null && !isDeployed)
            {
                var r = this.part.RequestResource((int)resourceId, (double)ResourceAmtUsedForDeploy);
                if (r == ResourceAmtUsedForDeploy)
                    return true;
                this.part.RequestResource((int)resourceId, -r);
                return false;
            }
            if (resourceId != null && isDeployed)
            {
                var r = this.part.RequestResource((int)resourceId, (double)MinimumResAmt);
                if (r == MinimumResAmt)
                    return true;
                this.part.RequestResource((int)resourceId, -r);
                return false;
            }
            return true;
        }

        public new void DeployModule()
        {
            if (UseResources())
                base.DeployModule();
        }

        public new void DeployModuleAction(KSPActionParam param)
        {
            if (UseResources())
                base.DeployModuleAction(param);
        }

        public new void RetractModule()
        {
            if (UseResources())
                base.RetractModule();
        }


        public new void RetractModuleAction(KSPActionParam param)
        {
            if (UseResources())
                base.RetractModuleAction(param);
        }


        public new void ToggleModuleAction(KSPActionParam param)
        {
            if (UseResources())
                base.ToggleModuleAction(param);

        }

        int? resourceId;
        double resPerTic;
        public new void Start()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                resourceId = Common.GetResourceID(ResourceUsed);
                if (resourceId != null)
                {
                    resPerTic = PowerConsumption * Common.WAITTIME;
                    Actions["ToggleModuleAction"].active = false;
                    StartCoroutine(CheckResources());
                }
            }
            base.Start();
        }

        IEnumerator CheckResources()
        {
            double resourceAmtAvailable, maxAmount;

            var wait = new WaitForSeconds(Common.WAITTIME);

            while (true)
            {
                if ( !isDeployed)
                {
                    this.part.GetConnectedResourceTotals((int)resourceId, out resourceAmtAvailable, out maxAmount);
                    Actions["DeployModuleAction"].active = Events["DeployModule"].guiActive = Actions["ToggleModuleAction"].active = (ResourceAmtUsedForDeploy <= resourceAmtAvailable);
                }
                if ( isDeployed)
                {
                    this.part.GetConnectedResourceTotals((int)resourceId, out resourceAmtAvailable, out maxAmount);
                    Actions["RetractModuleAction"].active = Events["RetractModule"].guiActive = Actions["ToggleModuleAction"].active = (MinimumResAmt <= resourceAmtAvailable);
                    if (MinimumResAmt >= resourceAmtAvailable)
                    {
                        Debug.Log("Running out of " + ResourceUsed);
                        var r = this.part.RequestResource((int)resourceId, (double)resourceAmtAvailable);

                        base.RetractModule();
                    }
                }

                yield return wait;
            }
        }
        void FixedUpdate()
        {
            if (isDeployed)
            {
                var r = this.part.RequestResource((int)resourceId, resPerTic);
            }
        }
    }
}
