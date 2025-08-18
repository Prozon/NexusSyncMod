using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;
using VRageMath;

namespace NexusSyncMod.Data.Scripts.NexusSyncMod.Antenna
{
    public class TerminalControls
    {
        private static bool _ControlsCreated = false;
        public static void CreateControls(IMyTerminalBlock block, List<Sandbox.ModAPI.Interfaces.Terminal.IMyTerminalControl> controls)
        {
            if (!(block is IMyLaserAntenna) || _ControlsCreated) return;

            var controlList = new List<IMyTerminalControl>();
            MyAPIGateway.TerminalControls.GetControls<IMyLaserAntenna>(out controlList);

            for (int i = 0; i < controlList.Count; i++)
            {
                var control = controlList[i];
                NexusAntenna.TryShow($"Control: [{i}] {control.Id} - {i.GetType().Name}");
            }


            //controlList[29].Visible = HideControls; // Example: Hide a specific control by index
            //controlList[30].Visible = HideControls; // Example: Hide a specific control by index



     




            // This method is used to create controls for the antenna
            // Currently, it does nothing but can be extended in the future
            NexusAntenna.TryShow("Creating controls for NexusAntenna");

            _ControlsCreated = true;
        }

        public static bool HideControls(IMyTerminalBlock block)
        {
            var antenna = block as IMyLaserAntenna;
            if (antenna == null)
                return true; // hide for anything that isn’t an antenna

            // Only hide on blocks that are NOT your subtype
            return !antenna.BlockDefinition.SubtypeName.Contains("NexusLaserAntenna");
        }

        public static bool ShowControls(IMyTerminalBlock block)
        {
            var antenna = block as IMyLaserAntenna;
            if (antenna == null)
                return false; // disable for anything that isn’t an antenna
            // Only show on blocks that are your subtype
            return antenna.BlockDefinition.SubtypeName.Contains("NexusLaserAntenna");
        }

        internal static bool EnableControl(IMyTerminalBlock block)
        {
            var antenna = block as IMyLaserAntenna;
            if (antenna == null)
                return false; // disable for anything that isn’t an antenna
            // Only enable on blocks that are your subtype

            return true; // Enable for all antennas for now, can be refined later
        }
    }
}
