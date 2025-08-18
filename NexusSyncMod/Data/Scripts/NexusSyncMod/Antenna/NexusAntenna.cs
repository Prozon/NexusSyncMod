using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace NexusSyncMod.Data.Scripts.NexusSyncMod.Antenna
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_LaserAntenna), false, "NexusLaserAntenna")]
    public class NexusAntenna : MyGameLogicComponent
    {
        private static bool _isDebug = true; // Set to false in production

        private bool _isInitialized = false;
   
    

        private IMyLaserAntenna _antenna;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            base.Init(objectBuilder);
            NeedsUpdate = MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME;

            _antenna = Entity as IMyLaserAntenna;
            if (_antenna == null)
            {
                TryShow("NexusAntenna: Failed to get IMyRadioAntenna instance.");
                return;
            }

            
		}

        private void _antenna_AppendingCustomInfo(IMyTerminalBlock arg1, StringBuilder arg2)
        {
            //TryShow("Setting Custom Info");
            //arg2.Clear(); // Clear existing info
            //TryShow("Setting Custom Info");
            if (_antenna == null)
            {
                //TryShow("NexusAntenna: _antenna_AppendingCustomInfo - Failed to get IMyRadioAntenna instance.");
                return;
            }
            // Append custom info to the antenna's info
            arg2.AppendLine("NexusMod Antenna Control");
            arg2.AppendLine($"Antenna Name: {_antenna.CustomName}");
            arg2.AppendLine($"Subtype: {_antenna.BlockDefinition.SubtypeName}");
            arg2.AppendLine($"Controlled by NexusMod");

            _antenna.SetDetailedInfoDirty();
        }

        public override void Close()
        {
            _antenna.AppendingCustomInfo -= _antenna_AppendingCustomInfo;
            MyAPIGateway.TerminalControls.CustomControlGetter -= TerminalControls_CustomControlGetter;
            base.Close();
        }


        public override void UpdateAfterSimulation100()
        {
         

            if (!_isInitialized)
            {
                // Initialize the antenna
                TryShow("Block Init!");
                CreateControls();

                

                _antenna.AppendingCustomInfo += _antenna_AppendingCustomInfo;

                _isInitialized = true;
            }
                
        }

        public override void UpdateAfterSimulation()
        {
            _antenna.RefreshCustomInfo();
        }



        private bool CreateControls()
        {
            MyAPIGateway.TerminalControls.CustomControlGetter += TerminalControls_CustomControlGetter;
            return true;
        }

        private void TerminalControls_CustomControlGetter(IMyTerminalBlock block, List<IMyTerminalControl> controls)
        {
            TerminalControls.CreateControls(block, controls);
        }

        public static void TryShow(string message)
        {
            if (MyAPIGateway.Session.LocalHumanPlayer != null && _isDebug)
                MyAPIGateway.Utilities?.ShowMessage("NexusAntenna", message);

            MyLog.Default?.WriteLineAndConsole($"NexusAntenna: {message}");
        }

        public static void TryShow(Exception ex, string message)
        {
            if (MyAPIGateway.Session.LocalHumanPlayer != null && _isDebug)
                MyAPIGateway.Utilities?.ShowMessage("NexusAntenna", message + $"\n {ex.ToString()}");

            MyLog.Default?.WriteLineAndConsole($"NexusAntenna: {message} \n {ex.ToString()}");
        }

        public static void UpdateGui(IMyTerminalBlock block)
        {

            if (MyAPIGateway.Gui.GetCurrentScreen == MyTerminalPageEnum.ControlPanel)
            {
                var myCubeBlock = block as MyCubeBlock;

                if (myCubeBlock.IDModule != null)
                {

                    var share = myCubeBlock.IDModule.ShareMode;
                    var owner = myCubeBlock.IDModule.Owner;
                    myCubeBlock.ChangeOwner(owner, share == MyOwnershipShareModeEnum.None ? MyOwnershipShareModeEnum.Faction : MyOwnershipShareModeEnum.None);
                    myCubeBlock.ChangeOwner(owner, share);
                }
            }
        }
    }
}
