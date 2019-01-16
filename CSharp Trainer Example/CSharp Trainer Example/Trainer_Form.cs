using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharp_Trainer_Example
{
    public partial class Trainer_Form : Form
    {
        Memory Memory = new Memory();

        string Game = "Tutorial-i386";

        public Trainer_Form() => InitializeComponent();



        ////               ////
        ////    BUTTONS    ////
        ////               ////

        private void label1_Click(object sender, EventArgs e) => SelectGameProcess();

        private void Step2_CheckedChanged(object sender, EventArgs e) => AOBSWAP("Step 2");

        private void Step3_CheckedChanged(object sender, EventArgs e) => AOBSWAP("Step 3");

        private void Step4_CheckedChanged(object sender, EventArgs e) => AOBSWAP("Step 4");

        private void Step5_CheckedChanged(object sender, EventArgs e) => AOBSWAP("Step 5");



        ////                 ////
        ////    FUNCTIONS    ////
        ////                 ////

        private void SelectGameProcess ()
        {
            switch (Memory.OpenProcess(Game))
            {
                case false:
                    switch (label1.Text)
                    {
                        case "Is the game running?":
                            label1.Text = $"{label1.Text}?";
                            break;
                        default:
                            label1.Text = "Is the game running?";
                            break;
                    }
                    break;
                default:
                    label1.Text = $"Attached to '{Game}' with PID of '{Memory.theProc.Id}'";
                    break;
            }
        }

        private async void AOBSWAP(string step)
        {
            switch (step)
            {
                case "Step 2":
                    long s2oaddress = (await Memory.AoBScan("75 2C 8B 83 68 04 00 00")).FirstOrDefault();
                    long s2eaddress = (await Memory.AoBScan("90 90 8B 83 68 04 00 00")).FirstOrDefault();

                    if (checkBox2.Checked) {
                        if (s2oaddress != 0)
                            Memory.writeMemory(s2oaddress.ToString("X"), "bytes", "0x90 0x90");
                    }

                    else {
                        if (s2eaddress != 0)
                            Memory.writeMemory(s2eaddress.ToString("X"), "bytes", "0x75 0x2C");
                    }
                    break;

                case "Step 3":
                    long s3oaddress = (await Memory.AoBScan("75 2C 8B 83 74 04 00 00")).FirstOrDefault();
                    long s3eaddress = (await Memory.AoBScan("90 90 8B 83 74 04 00 00")).FirstOrDefault();

                    if (checkBox3.Checked) {
                        if (s3oaddress != 0)
                            Memory.writeMemory(s3oaddress.ToString("X"), "bytes", "0x90 0x90");
                    }

                    else {
                        if (s3eaddress != 0)
                            Memory.writeMemory(s3eaddress.ToString("X"), "bytes", "0x75 0x2C");
                    }
                    break;

                case "Step 4":
                    long s4oaddress = (await Memory.AoBScan("?? ?? ?? ?? dd 05 ?? ?? ?? ?? dd 83")).FirstOrDefault();
                    long s4eaddress = (await Memory.AoBScan("90 90 90 90 dd 05 ?? ?? ?? ?? dd 83")).FirstOrDefault();

                    if (checkBox4.Checked) {
                        if (s4oaddress != 0)
                        {
                            Memory.writeMemory(s4oaddress.ToString("X"), "bytes", "0x90 0x90 0x90 0x90");
                            Memory.writeMemory((s4oaddress + 0x15).ToString("X"), "bytes", "0x90 0x90 0x90 0x90");
                        }
                    }

                    else {
                        if (s4eaddress != 0)
                        {
                            Memory.writeMemory(s4eaddress.ToString("X"), "bytes", "0x7A 0x43 0x72 0x41");
                            Memory.writeMemory((s4eaddress + 0x15).ToString("X"), "bytes", "0x7A 0x2E 0x72 0x2C");
                        }
                    }
                    break;

                case "Step 5":
                    long s5oaddress = (await Memory.AoBScan("00 00 8B 00 3B 45 F4 74 02 EB 1C 8B 45 F8")).FirstOrDefault();
                    long s5eaddress = (await Memory.AoBScan("00 00 8B 00 3B 45 F4 74 02 90 90 8B 45 F8")).FirstOrDefault();

                    if (checkBox5.Checked)
                    {
                        if (s5oaddress != 0)
                            Memory.writeMemory((s5oaddress + 0x9).ToString("X"), "bytes", "0x90 0x90");
                    }

                    else
                    {
                        if (s5eaddress != 0)
                            Memory.writeMemory((s5eaddress + 0x9).ToString("X"), "bytes", "0xEB 0x1C");
                    }
                    break;

                case "Step 6":
                    break;

                default:
                    break;
            }
        }
    }
}
