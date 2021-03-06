﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ArdupilotMega.Controls;

namespace ArdupilotMega.Wizard
{
    public partial class _5AccelCalib : MyUserControl, IWizard
    {
        byte count = 0;

        public _5AccelCalib()
        {
            InitializeComponent();
        }

        public int WizardValidate()
        {
                return 1;
        }


        private void BUT_start_Click(object sender, EventArgs e)
        {
            ((MyButton)sender).Enabled = false;
            BUT_continue.Enabled = true;

            // start the process off
            MainV2.comPort.doCommand(MAVLink.MAV_CMD.PREFLIGHT_CALIBRATION, 0, 0, 0, 0, 1, 0, 0);
            MainV2.comPort.giveComport = true;

            // start thread to update display
            System.Threading.ThreadPool.QueueUserWorkItem(readmessage, this);

            BUT_continue.Focus();
        }


        static void readmessage(object item)
        {
            _5AccelCalib local = (_5AccelCalib)item;

            // clean up history
            MainV2.comPort.MAV.cs.messages.Clear();

            while (!(MainV2.comPort.MAV.cs.message.ToLower().Contains("calibration successful") || MainV2.comPort.MAV.cs.message.ToLower().Contains("calibration failed")))
            {
                try
                {
                    System.Threading.Thread.Sleep(10);
                    // read the message
                    MainV2.comPort.readPacket();
                    // update cs with the message
                    MainV2.comPort.MAV.cs.UpdateCurrentSettings(null);
                    // update user display
                    local.UpdateUserMessage();
                }
                catch { break; }
            }

            MainV2.comPort.giveComport = false;

            try
            {
                local.Invoke((MethodInvoker)delegate()
                {
                    //local.imageLabel1.Text = "Done";
                    local.BUT_continue.Enabled = false;
                });
            }
            catch { }
        }

        public void UpdateUserMessage()
        {
            this.Invoke((MethodInvoker)delegate()
            {
                if (MainV2.comPort.MAV.cs.message.ToLower().Contains("initi"))
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.apmp1;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                if (MainV2.comPort.MAV.cs.message.ToLower().Contains("level"))
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.apmp1;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                else if (MainV2.comPort.MAV.cs.message.ToLower().Contains("left"))
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.apmp3;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                else if (MainV2.comPort.MAV.cs.message.ToLower().Contains("right"))
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.apmp2;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                else if (MainV2.comPort.MAV.cs.message.ToLower().Contains("down"))
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.apmp4;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                else if (MainV2.comPort.MAV.cs.message.ToLower().Contains("up"))
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.apmp5;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                else if (MainV2.comPort.MAV.cs.message.ToLower().Contains("back"))
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.apmp6;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                else if (MainV2.comPort.MAV.cs.message.ToLower().Contains("calibration"))
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.apmp1;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
            });
        }

        private void BUT_continue_Click(object sender, EventArgs e)
        {
            count++;

            MainV2.comPort.sendPacket(new MAVLink.mavlink_command_ack_t() { command = 1, result = count });
        }
    }
}
