ModuleAnimGenericResourceUsage

This small module simply makes sure that the needed specified resources are available before opening or closing the animation.  When the animation is triggered, the resources which needed to be available will be used (ie:  if 10 EC needed to be able to open a door and it has 15, then opening the door will use 10 ec, eaving 5)

This inherits all fields from ModuleAnimateGeneric, and the animations, etc, work exactly as they do in ModuleAnimateGeneric.

An example module entry is:

	MODULE
	{
		name = ModuleAnimGenericResourceUsage

		ResourceUsed = ElectricCharge
		ResourceAmtNeededToStart = 10
		ResourceAmtNeededToEnd = 5

		// Following is from a test example which was using the deployable docking port
		// as a test.  Replace the values with what's needed for your mod.

		animationName = dockingring
		actionGUIName = #autoLOC_502070 //#autoLOC_502070 = Toggle Shield
		startEventGUIName = #autoLOC_502071 //#autoLOC_502071 = Open Shield
		endEventGUIName = #autoLOC_502072 //#autoLOC_502072 = Close Shield
		allowAnimationWhileShielded = False
	}


	name						Name of the mod, required

	ResourceUsed      			Name of the resource which has to be present before the startEvent can occur
	ResourceAmtNeededToStart	Amount of the resource needed in order for the startEvent to occur
	ResourceAmtNeededToEnd		Amount of the resource needed in order for the endEvent to occur


	If no valid resource is specified everything will be as normal

	The following are inherited from ModuleAnimateGeneric, and the rules regard them are the same in the that module

	animationName
	actionGUIName
	startEventGUIName
	endEventGUIName
	allowAnimationWhileShielded

===================================================================================================

ModuleAnimGroupResourceUsage

This small module simply makes sure that the needed specified resources are available before starting the animation.  When the animation is triggered, the resources which needed to be available will be used (ie:  if 10 EC needed to be able to open a door and it has 15, then opening the door will use 10 ec, eaving 5).  If/when the available resources drop blow the minimum needed, the animation will stop

This inherits all fields from ModuleAnimationGroup, and the animations, etc, work exactly as they do in ModuleAnimationGroup.

The following is an example:

	@PART[SurveyScanner]:HAS[@MODULE[ModuleAnimationGroup]]
	{
		@MODULE[ModuleAnimationGroup]
		{
			%name = ModuleAnimGroupResourceUsage

			ResourceUsed = ElectricCharge
			ResourceAmtUsedForDeploy = 5
			MinimumResAmt = 5
			PowerConsumption = 1.5

		}
	}

	name						Name of the mod, required
	ResourceUsed				Name of the needed resource
	ResourceAmtUsedForDeploy	The amount of resources used to start the animation
	MinimumResAmt				The minimum amount of resources needed to continue operation
	PowerConsumption			How much of the resource is used per second

===================================================================================================

ModuleSciExpResourceUsage

This small module simply makes sure that the needed rsources are available before starting the science experiment.  When triggered, the resources which needed to be available will be used (ie:  if 10 EC needed to be able to open a door and it has 15, then opening the door will use 10 ec, eaving 5).  If/when the available resources drop blow the minimum needed, the animation will stop

This inherits all fields from ModuleScienceExperiment, and the animations, etc, work exactly as they do in ModuleScienceExperiment.

The following is an example:

	@PART[sensorAtmosphere]:HAS[@MODULE[ModuleScienceExperiment]]
	{
		@MODULE[ModuleScienceExperiment]
		{
			%name = ModuleSciExpResourceUsage

			neededResourceToStart = ElectricCharge
			ResourceAmtUsedForDeploy = 5
			ResourceAmtUsedForReset = 10
		}
	}
