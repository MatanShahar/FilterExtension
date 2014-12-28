﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FilterExtensions.Categoriser
{
    static class PartType
    {
        internal static bool checkCustom(AvailablePart part, string value)
        {
            if (part.category == PartCategories.none)
                return false;

            bool val;
            switch (value)
            {
                case "isEngine":
                    val = isEngine(part);
                    break;
                case "isCommand":
                    val = isCommand(part);
                    break;
                case "LFLOx Engine":
                    val = isLFLOxEngine(part);
                    break;
                case "LF Engine":
                    val = isLFEngine(part);
                    break;
                case "adapter":
                    val = isAdapter(part);
                    break;
                case "Xenon Engine":
                    val = isIonEngine(part);
                    break;
                default:
                    val = false;
                    break;
            }
            return val;
        }

        internal static bool checkModuleTitle(AvailablePart part, string value)
        {
            if (part.category == PartCategories.none)
                return false;

            bool moduleCheck = part.moduleInfos.Any(m => m.moduleName == value);

            return moduleCheck;
        }

        internal static bool checkModuleName(AvailablePart part, string value)
        {
            if (part.category == PartCategories.none)
                return false;

            bool moduleCheck = part.partPrefab.Modules.Contains(value);

            return moduleCheck;
        }

        internal static bool checkCategory(AvailablePart part, string value)
        {
            if (part.category == PartCategories.none)
                return false;

            switch (value)
            {
                case "Pod":
                    if (part.category == PartCategories.Pods)
                        return true;
                    break;
                case "Engine":
                    if (part.category == PartCategories.Engine)
                        return true;
                    else if (part.category == PartCategories.Propulsion && PartType.isEngine(part))
                        return true;
                    break;
                case "Tank":
                    if (part.category == PartCategories.FuelTank)
                        return true;
                    else if (part.category == PartCategories.Propulsion && !PartType.isEngine(part))
                        return true;
                    break;
                case "Command":
                    if (part.category == PartCategories.Control)
                        return true;
                    break;
                case "Struct":
                    if (part.category == PartCategories.Structural)
                        return true;
                    break;
                case "Aero":
                    if (part.category == PartCategories.Aero)
                        return true;
                    break;
                case "Utility":
                    if (part.category == PartCategories.Utility)
                        return true;
                    break;
                case "Science":
                    if (part.category == PartCategories.Science)
                        return true;
                    break;
            }

            return false;
        }

        internal static bool checkName(AvailablePart part, string value)
        {
            if (part.category == PartCategories.none)
                return false;

            bool nameCheck = part.name == value;

            return nameCheck;
        }

        internal static bool checkTitle(AvailablePart part, string value)
        {
            if (part.category == PartCategories.none)
                return false;

            bool titleCheck = part.title.Contains(value);

            return titleCheck;
        }

        internal static bool checkResource(AvailablePart part, string value)
        {
            if (part.category == PartCategories.none)
                return false;

            bool resourceCheck = part.resourceInfos.Any(r => r.resourceName == value);

            return resourceCheck;
        }

        internal static bool checkTech(AvailablePart part, string value)
        {
            if (part.category == PartCategories.none)
                return false;

            bool techCheck = part.TechRequired == value;

            return techCheck;
        }

        internal static bool checkManufacturer(AvailablePart part, string value)
        {
            if (part.category == PartCategories.none)
                return false;

            bool manuCheck = (part.manufacturer == value);

            return manuCheck;
        }

        internal static bool checkFolder(AvailablePart part, string value)
        {
            if (part.category == PartCategories.none)
                return false;

            bool folderCheck = false;
            if (Core.partFolderDict.ContainsKey(part.name))
                folderCheck = Core.partFolderDict[part.name] == value;

            return folderCheck;
        }

        internal static bool checkFolder(AvailablePart part, string[] values)
        {
            if (part.category == PartCategories.none)
                return false;

            if (Core.partFolderDict.ContainsKey(part.name))
            {
                foreach (string s in values)
                {
                    if (Core.partFolderDict[part.name] == s.Trim())
                        return true;
                }
            }

            return false;
        }

        public static int partSize(AvailablePart part)
        {
            int size = -1;
            foreach (AttachNode node in part.partPrefab.attachNodes)
            {
                if (size < node.size)
                    size = node.size;
            }
            return size;
        }

        public static bool isCommand(AvailablePart part)
        {
            if (isMannedPod(part))
                return true;
            if (isDrone(part))
                return true;
            if (part.partPrefab.Modules.OfType<KerbalSeat>().Any())
                return true;
            return false;
        }
        
        public static bool isEngine(AvailablePart part)
        {
            if (part.partPrefab.Modules.OfType<ModuleEngines>().Any())
                return true;
            if (part.partPrefab.Modules.OfType<ModuleEnginesFX>().Any())
                return true;
            if (part.partPrefab.Modules.OfType<MultiModeEngine>().Any())
                return true;
            return false;
        }

        public static bool isResourceIntake(AvailablePart part)
        {
            if (part.partPrefab.Modules.OfType<ModuleResourceIntake>().Any())
                return true;
            return false;
        }

        public static bool isMannedPod(AvailablePart part)
        {
            if (part.partPrefab.Modules.OfType<ModuleCommand>().Any() && part.partPrefab.CrewCapacity > 0)
                return true;
            return false;
        }

        public static bool isDrone(AvailablePart part)
        {
            if (part.partPrefab.Modules.OfType<ModuleCommand>().Any() && part.partPrefab.CrewCapacity == 0)
                return true;
            return false;
        }

        public static bool isFuselage(AvailablePart part)
        {
            if (!(part.partPrefab.Modules.Count == 0 && part.partPrefab.Resources.Count == 0 && part.partPrefab.attachNodes.Count == 2 && part.category.ToString() != "Aero"))
                return false;
            if (part.partPrefab.attachNodes[0].size == part.partPrefab.attachNodes[1].size)
                return true;
            return false;
        }

        public static bool isMultiCoupler(AvailablePart part)
        {
            if (part.partPrefab.attachNodes.Count > 2)
                return true;
            return false;
        }

        public static bool isAdapter(AvailablePart part)
        {
            if (isCommand(part))
                return false;
            if (part.partPrefab.attachNodes.Count != 2)
                return false;
            if (part.partPrefab.attachNodes[0].size != part.partPrefab.attachNodes[1].size)
                return true;
            return false;
        }

        public static bool isWing(AvailablePart part)
        {
            if (part.partPrefab.GetComponent<Winglet>() != null)
                return true;
            if (part.partPrefab.Modules.Contains("FARWingAerodynamicModel"))
                return true;
            
            return false;
        }

        public static bool isParachute(AvailablePart part)
        {
            if (part.partPrefab.Modules.OfType<ModuleParachute>().Any())
                return true;
            if (part.partPrefab.Modules.Contains("RealChuteModule"))
                return true;
            return false;
        }

        public static bool isDockingPort(AvailablePart part)
        {
            if (part.partPrefab.Modules.OfType<ModuleDockingNode>().Any())
                return true;
            return false;
        }

        public static bool isLight(AvailablePart part)
        {
            if (part.partPrefab.Modules.OfType<ModuleLight>().Any() && part.partPrefab.Modules.Count == 1)
                return true;
            return false;
        }

        public static bool isLadder(AvailablePart part)
        {
            if (part.partPrefab.Modules.OfType<RetractableLadder>().Any() && part.partPrefab.Modules.Count == 1)
                return true;
            if (part.name == "ladder1") // blasted radial ladder thing.
                return true;
            return false;
        }

        public static bool isLFLOxEngine(AvailablePart part)
        {
            bool LF = false, LOx = false;
            List<Propellant> propellants = new List<Propellant>();

            if (part.partPrefab.GetModuleEngines() != null)
            {
                propellants = part.partPrefab.GetModuleEngines().propellants;
            }
            else if (part.partPrefab.GetModuleEnginesFx() != null)
            {
                propellants = part.partPrefab.GetModuleEnginesFx().propellants;
            }

            foreach (Propellant p in propellants)
            {
                if (p.name == "LiquidFuel")
                    LF = true;
                else if (p.name == "Oxidizer")
                    LOx = true;
            }
            return LF && LOx;
        }

        public static bool isLFEngine(AvailablePart part)
        {
            bool LF = false, LOx = false;
            List<Propellant> propellants = new List<Propellant>();

            if (part.partPrefab.GetModuleEngines() != null)
            {
                propellants = part.partPrefab.GetModuleEngines().propellants;
            }
            else if (part.partPrefab.GetModuleEnginesFx() != null)
            {
                propellants = part.partPrefab.GetModuleEnginesFx().propellants;
            }

            foreach (Propellant p in propellants)
            {
                if (p.name == "LiquidFuel")
                    LF = true;
                else if (p.name == "Oxidizer")
                    LOx = true;
            }
            return LF && !LOx;
        }

        public static bool isSRB(AvailablePart part)
        {
            bool Solid = false;
            List<Propellant> propellants = new List<Propellant>();

            if (part.partPrefab.GetModuleEngines() != null)
            {
                propellants = part.partPrefab.GetModuleEngines().propellants;
            }
            else if (part.partPrefab.GetModuleEnginesFx() != null)
            {
                propellants = part.partPrefab.GetModuleEnginesFx().propellants;
            }

            foreach (Propellant p in propellants)
            {
                if (p.name == "SolidFuel")
                    Solid = true;
            }
            return Solid;
        }

        public static bool isIonEngine(AvailablePart part)
        {
            bool Ec = false, Xe = false;
            List<Propellant> propellants = new List<Propellant>();
            //Engine modules
            if (part.partPrefab.GetModuleEngines() != null)
            {
                propellants = part.partPrefab.GetModuleEngines().propellants;
            }
            else if (part.partPrefab.GetModuleEnginesFx() != null)
            {
                propellants = part.partPrefab.GetModuleEnginesFx().propellants;
            }
            // get type
            foreach (Propellant p in propellants)
            {
                if (p.name == "XenonGas")
                    Xe = true;
                else if (p.name == "ElectricCharge")
                    Ec = true;
            }
            return Xe && Ec;
        }

        public static bool isNFEArgonEngine(AvailablePart part) // Near future argon EC engine
        {
            if (part.partPrefab.Modules.Contains("VariablePowerEngine"))
                return false;

            bool Ec = false, Arg = false;
            List<Propellant> propellants = new List<Propellant>();
            //Engine modules
            if (part.partPrefab.GetModuleEngines() != null)
            {
                propellants = part.partPrefab.GetModuleEngines().propellants;
            }
            else if (part.partPrefab.GetModuleEnginesFx() != null)
            {
                propellants = part.partPrefab.GetModuleEnginesFx().propellants;
            }
            // get type
            foreach (Propellant p in propellants)
            {
                if (p.name == "ArgonGas")
                    Arg = true;
                else if (p.name == "ElectricCharge")
                    Ec = true;
            }

            return Arg && Ec;
        }

        public static bool isNFEVariableEngine(AvailablePart part) // Near future variable thrust argon EC engine
        {
            if (!part.partPrefab.Modules.Contains("VariablePowerEngine"))
                return false;

            bool Ec = false, Arg = false;
            List<Propellant> propellants = new List<Propellant>();
            //Engine modules
            if (part.partPrefab.GetModuleEngines() != null)
            {
                propellants = part.partPrefab.GetModuleEngines().propellants;
            }
            else if (part.partPrefab.GetModuleEnginesFx() != null)
            {
                propellants = part.partPrefab.GetModuleEnginesFx().propellants;
            }
            // get type
            foreach (Propellant p in propellants)
            {
                if (p.name == "ArgonGas")
                    Arg = true;
                else if (p.name == "ElectricCharge")
                    Ec = true;
            }

            return Arg && Ec;
        }

        public static T GetModule<T>(this Part part) where T : PartModule
        {
            return part.Modules.OfType<T>().FirstOrDefault();
        }

        public static ModuleEngines GetModuleEngines(this Part part)
        {
            return part.GetModule<ModuleEngines>();
        }

        public static ModuleEnginesFX GetModuleEnginesFx(this Part part)
        {
            return part.GetModule<ModuleEnginesFX>();
        }
    }
}
