﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ArdupilotMega.Controls;
using ArdupilotMega.Utilities;

namespace ArdupilotMega.GCSViews.ConfigurationView
{
    public partial class ConfigAC_Fence : UserControl, IActivate
    {
        public ConfigAC_Fence()
        {
            InitializeComponent();
        }

        public void Activate()
        {
            mavlinkCheckBox1.setup(1, 0, "FENCE_ENABLE", MainV2.comPort.MAV.param);

            Utilities.ParameterMetaDataRepository repo = new Utilities.ParameterMetaDataRepository();


            mavlinkComboBox1.setup(repo.GetParameterOptionsInt("FENCE_TYPE"), "FENCE_TYPE", MainV2.comPort.MAV.param);


            mavlinkComboBox2.setup(repo.GetParameterOptionsInt("FENCE_ACTION"), "FENCE_ACTION", MainV2.comPort.MAV.param);
  

            // 3
            mavlinkNumericUpDown1.setup(10, 1000, 1, 1, "FENCE_ALT_MAX", MainV2.comPort.MAV.param);

            mavlinkNumericUpDown2.setup(0, 65536, 1, 1, "FENCE_RADIUS", MainV2.comPort.MAV.param);
        }
    }
}
