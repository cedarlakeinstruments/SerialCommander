using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace SerialCommander
{
    /// <summary>
    /// Controller for DC 5V 12V 2-Channel RS232 Serial Control Relay Switch Board SCM PC SController
    /// </summary>
    public partial class RelayControllerForm : Form
    {
        SerialPort _port;
        public RelayControllerForm()
        {
            InitializeComponent();
            string[] ports = SerialPort.GetPortNames();
            if (ports.Length != 0)
            {
                listBoxPorts.Items.AddRange(ports);
            }
            else
            {
                MessageBox.Show("No serial ports found");
                buttonSend.Enabled = false;
            }
        }

        /// <summary>
        /// Open serial port
        /// </summary>
        /// <param name="portName">COMn</param>
        /// <returns>true if connected successfully</returns>
        bool connect(string portName)
        {
            bool connected = false;
            try
            {
                if (_port != null && _port.IsOpen)
                {
                    _port.Close();
                }
                _port = new SerialPort(portName, 9600);
                _port.Handshake = Handshake.None;
                _port.DataBits = 8;
                _port.StopBits = StopBits.One;
                _port.Parity = Parity.None;
                _port.Open();
                connected = true;
                Thread.Sleep(10);
            }
            catch (System.IO.IOException )
            {
            }
            return connected;
        }

        /// <summary>
        /// Handles the relay control button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSend_Click(object sender, EventArgs e)
        {
            int ch1 = checkBox1.Checked ? 1 : 0;
            int ch2 = checkBox2.Checked ? 2 : 0;
            int ch3 = checkBox3.Checked ? 4 : 0;
            int ch4 = checkBox4.Checked ? 8 : 0;

            byte cmd = (byte)(ch1+ch2+ch3+ch4);
            _port.Write(new byte[] { cmd }, 0, 1);
        }

        /// <summary>
        /// Select a COM port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = sender as ListBox;
            string portName = (string)lb.SelectedItem;
            if (portName != null)
            {
                if (connect(portName) == true)
                {
                    labelStatus.Text = "Connected to " + portName;
                }
                else
                {
                    labelStatus.Text = "Could not connect to " + portName;
                }
            }
        }
    }
}
