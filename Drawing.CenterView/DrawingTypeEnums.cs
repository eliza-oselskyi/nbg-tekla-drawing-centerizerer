using System;
using System.Collections.Generic;

namespace Drawing.CenterView;
    
    public partial class PluginForm
    {

        public static ViewType GetViewTypeEnum(Dictionary<string, string> viewType)
        {
            try
            {
                viewType.TryGetValue("ViewType", out var vt);
                return vt switch
                {
                    "Cover Sheet" => ViewType.CoverSheet,
                    "Building Sheet 1" => ViewType.BuildingSheet1,
                    "Building Sheet 2" => ViewType.BuildingSheet2,
                    "Plane Identification Plan" => ViewType.PlaneIdentificationPlan,
                    "Anchor Rod Plan" => ViewType.AnchorRodPlan,
                    "Base Plate Details" => ViewType.BasePlateDetails,
                    "Reactions" => ViewType.Reactions,
                    "Shakeout Plan" => ViewType.ShakeoutPlan,
                    "Roof Framed Opening Plan" => ViewType.RoofFramedOpeningPlan,
                    "Crane Beam Plan" => ViewType.CraneBeamPlan,
                    "Cross Section" => ViewType.CrossSection,
                    "Portal Cross Section" => ViewType.PortalCrossSection,
                    "Roof Framing Plan" => ViewType.RoofFramingPlan,
                    "Roof Framing Plan - Secondary" => ViewType.RoofFramingPlanSecondary,
                    "Roof Framing Plan - Openings" => ViewType.RoofFramingPlanOpenings,
                    "Roof Framing Plan - Purlin Bracing" => ViewType.RoofFramingPlanPurlinBracing,
                    "Endwall Framing" => ViewType.EndwallFraming,
                    "Endwall Partition Framing" => ViewType.EndwallPartitionFraming,
                    "Sidewall Framing" => ViewType.SidewallFraming,
                    "Sidewall Partition Framing" => ViewType.SidewallPartitionFraming,
                    "Wall Sheeting" => ViewType.WallSheeting,
                    "Partition Wall Sheeting" => ViewType.PartitionWallSheeting,
                    "Roof Sheeting" => ViewType.RoofSheeting,
                    "Wall Liner" => ViewType.WallLiner,
                    "Partition Wall Liner" => ViewType.PartitionWallLiner,
                    "Roof Liner" => ViewType.RoofLiner,
                    "Roof Panel Clip Layout" => ViewType.RoofPanelClipLayout,
                    "Mezzanine Plan - Framing Only" => ViewType.MezzaninePlanFramingOnly,
                    "Mezzanine Plan - Joists Only" => ViewType.MezzaninePlanJoistsOnly,
                    "Mezzanine Plan - Decking" => ViewType.MezzaninePlanDecking,
                    _ => ViewType.None
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return ViewType.None;
            }
        }
    }
