using Google.Contacts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoogleAccountMerge
{
    public partial class MainForm : Form
    {

        private static string SOURCE_NAME = "source";
        private static string TARGET_NAME = "target";

        GoogleConnection SourceConnection;
        GoogleConnection TargetConnection;

        // Delegates begin
        private void OnConnectionSuccess(string connectionName, string gmail)
        {
            if(connectionName.Equals(SOURCE_NAME))
            {
                SourceLog.Text = "Source account connected";
                SourceAccount.Text = gmail;
                SourceButton.Text = "Disconnect";
            }
            else if (connectionName.Equals(TARGET_NAME))
            {
                TargetLog.Text = "Target account connected";
                TargetAccount.Text = gmail;
                TargetButton.Text = "Disconnect";
            }
        }
        private void OnConnectionFail(string connectionName)
        {
            if (connectionName.Equals(SOURCE_NAME))
            {
                SourceLog.Text = "Target account connection failed";
                SourceAccount.Text = "";
                SourceButton.Text = "Connect";
            }
            else if (connectionName.Equals(TARGET_NAME))
            {
                TargetLog.Text = "Target account connection failed";
                TargetAccount.Text = "";
                TargetButton.Text = "Connect";
            }
        }
        private void OnDisconnected(string connectionName)
        {
            if (connectionName.Equals(SOURCE_NAME))
            {
                SourceLog.Text = "Source account disconnected";
                SourceAccount.Text = "";
                SourceButton.Text = "Connect";
            }
            else if (connectionName.Equals(TARGET_NAME))
            {
                TargetLog.Text = "Target account disconnected";
                TargetAccount.Text = "";
                TargetButton.Text = "Connect";
            }
        }
        private void OnReadContactsReady(string connectionName, List<Contact> contacts)
        {

        }
        private void OnContactsMergingSuccess(string connectionName)
        {

        }
        private void OnContactsMergingFail(string connectionName)
        {

        }
        // Delegates end

        public MainForm()
        {
            InitializeComponent();
            InitializeGoogleConnections();
        }

        void InitializeGoogleConnections()
        {
            //OnConnectionSuccessDelegate d1 = this.OnConnectionSuccess;
            SourceConnection = new GoogleConnection(
                SOURCE_NAME,
                this.OnConnectionSuccess,
                this.OnConnectionFail,
                this.OnDisconnected,
                this.OnReadContactsReady,
                this.OnContactsMergingSuccess,
                this.OnContactsMergingFail
            );

            TargetConnection = new GoogleConnection(
                TARGET_NAME,
                this.OnConnectionSuccess,
                this.OnConnectionFail,
                this.OnDisconnected,
                this.OnReadContactsReady,
                this.OnContactsMergingSuccess,
                this.OnContactsMergingFail
            );
        }

        private void SourceButton_Click(object sender, EventArgs e)
        {
            if(SourceConnection.IsConnected())
            {
                SourceAccount.Text = "Disconnecting...";
                SourceConnection.Disconnect();
            }
            else
            {
                SourceAccount.Text = "Connecting...";
                SourceConnection.Connect();
            }
        }

        private void TargetButton_Click(object sender, EventArgs e)
        {
            if (TargetConnection.IsConnected())
            {
                TargetAccount.Text = "Disconnecting...";
                TargetConnection.Disconnect();
            }
            else
            {
                TargetAccount.Text = "Connecting...";
                TargetConnection.Connect();
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {

        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {

        }
    }
}
