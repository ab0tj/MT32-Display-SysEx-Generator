using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MT32_Display_SysEx_Generator
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            comboFormat.Items.Add("Hex");
            comboFormat.Items.Add("Byte Array");
            comboFormat.Items.Add("Escaped String");
            comboFormat.SelectedIndex = 0;

            textText_TextChanged(null, null);
        }

        private void textText_TextChanged(object sender, EventArgs e)
        {
            byte[] sysex = { 0xf0, 0x41, 0x10, 0x16, 0x12, 0x20, 0x00, 0x00,
                             0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
                             0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20,
                             0x20, 0x20, 0x20, 0x20, 0x00, 0xf7 };

            Encoding.ASCII.GetBytes(textText.Text).CopyTo(sysex, 8);

            int total = 0;
            for (int i = 5; i < 28; i++)
            {
                total += sysex[i];
            }
            sysex[28] = (byte)(128 - total % 128);

            StringBuilder sysexstr = new StringBuilder();
            switch (comboFormat.SelectedIndex)
            {
                case 0:
                    for (int i = 0; i < 30; i++)
                    {
                        sysexstr.Append(sysex[i].ToString("x02"));
                    }
                    break;

                case 1:
                    sysexstr.Append("{ ");
                    for (int i = 0; i < 30; i++)
                    {
                        sysexstr.Append("0x" + sysex[i].ToString("x02"));
                        if (i < 29) sysexstr.Append(", ");
                    }
                    sysexstr.Append(" }");
                    break;

                case 2:
                    for (int i = 0; i < 8; i++)
                    {
                        sysexstr.Append("\\x" + sysex[i].ToString("x02"));
                    }
                    for (int i = 8; i < 28; i++)
                    {
                        sysexstr.Append((char)sysex[i]);
                    }
                    for (int i = 28;i < 30; i++)
                    {
                        sysexstr.Append("\\x" + sysex[i].ToString("x02"));
                    }
                    break;
            }

            textSysex.Text = sysexstr.ToString();
        }

        private void comboFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            textText_TextChanged(null, null);
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textSysex.Text);
        }
    }
}
