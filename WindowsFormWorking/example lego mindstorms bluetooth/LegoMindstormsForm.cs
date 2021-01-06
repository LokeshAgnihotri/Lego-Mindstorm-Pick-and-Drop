using EV3MessengerLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Example_Lego_Mindstorms_Bluetooth
{
    public partial class LegoMindstormsForm : Form
    {
        private EV3Messenger messenger;

        public LegoMindstormsForm()
        {
            InitializeComponent();
            // Init application
            messenger = new EV3Messenger();
            fillSerialPortSelectionBoxWithAvailablePorts();
            updateFormGUI();
        }
        
        #region Connection form
        private void refreshButton_Click(object sender, EventArgs e)
        {
            fillSerialPortSelectionBoxWithAvailablePorts();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            // Check if a port has been selected
            if (portListBox.SelectedIndex > -1)
            {
                // Get the selected port from the ListBox
                string port = portListBox.SelectedItem.ToString().ToUpper();
                // Try to connect with the Brick via the selected port
                if (messenger.Connect(port))
                {
                    updateFormGUI();
                }
                else
                {
                    MessageBox.Show("Failed to connect to serial port '" + port + "'.\n"
                        + "Make sure your robot is connected to that serial port and try again.");
                }
            }
            else
            {
                MessageBox.Show("Please select a port for the bluetooth connection");
            }
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            // Disconnect from the Brick
            messenger.Disconnect();

            updateFormGUI();
        }

        private void fillSerialPortSelectionBoxWithAvailablePorts()
        {
            String[] ports = SerialPort.GetPortNames();
            Array.Sort(ports);

            portListBox.Items.Clear();
            foreach (String port in ports)
            {
                portListBox.Items.Add(port);
            }
        }

        #endregion

        #region Input & output form

        

        //private void outputButton_Click(object sender, EventArgs e)
        //{
        //    // Try to get a message
        //    EV3Message message = messenger.ReadMessage();
        //    // Check if there is a message received from the Brick
        //    if (message != null)
        //    {
        //        outputlistBox.Items.Add("Message: " + message.ValueAsText);
        //        // Auto scroll the listbox
        //        outputlistBox.TopIndex = outputlistBox.Items.Count - 1;
        //    }
        //    else
        //    {
        //        MessageBox.Show("No message recieved from the Brick");
        //    }
        //}

        #endregion

        #region GUI

        private void updateFormGUI()
        {
            if (messenger.IsConnected)
            {
                refreshButton.Enabled = false;
                connectButton.Enabled = false;

                //inputGroupBox.Enabled = true;
                //outputGroupBox.Enabled = true;
                disconnectButton.Enabled = true;

                //outputlistBox.Items.Clear();
            }
            else
            {
                refreshButton.Enabled = true;
                connectButton.Enabled = true;

                //inputGroupBox.Enabled = false;
                //outputGroupBox.Enabled = false;
                disconnectButton.Enabled = false;
            }
        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (connectButton.Enabled == false)
            {
                EV3Message message = messenger.ReadMessage();
                if (message != null)
                {
                    switch (message.ValueAsText)
                    {
                        case "black":
                            pictureBox1.BackColor = Color.Black;
                            break;
                        case "red":
                            pictureBox1.BackColor = Color.Red;
                            break;
                        case "yellow":
                            pictureBox1.BackColor = Color.Yellow;
                            break;
                        case "white":
                            pictureBox1.BackColor = Color.White;
                            break;
                        default:
                            pictureBox1.BackColor = SystemColors.Control;
                            break;
                    }

                }

                EV3Message message2 = messenger.ReadMessage();
                if (message != null)
                {
                    switch (message.ValueAsNumber)
                    {
                        case 25:
                            progressBar1.Value = 25;
                            break;
                        case 50:
                            progressBar1.Value = 50;
                            break;
                        case 75:
                            progressBar1.Value = 75;
                            break;
                        case 100:
                            progressBar1.Value = 100;
                            break;
                        default:
                            progressBar1.Value = 0;
                            break;
                    }

                }

            }
            else if (connectButton.Enabled == true)
            {
                progressBar1.Value = 0;
                pictureBox1.BackColor = SystemColors.Control;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            messenger.SendMessage("STATUS", "start");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            messenger.SendMessage("STATUS", "pause");
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            messenger.SendMessage("STATUS", "stop");
        }
    }
}
