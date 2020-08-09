using System;
using UnityEngine;
using KSP;
using KSP.UI.Screens;
using System.Collections;

namespace AnimateWithResources
{

    internal class Common
    {
        internal const float WAITTIME = 0.25f;

        static public int? GetResourceID(string res)
        {
            if (res != "")
            {
                PartResourceDefinition prd = PartResourceLibrary.Instance.GetDefinition(res);
                if (prd != null)
                    return prd.id;
            }
            return null;
        }
    }
}
