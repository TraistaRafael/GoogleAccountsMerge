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

            DownloadButton.Enabled = true;
            MergeButton.Enabled = true;
            ExecuteButton.Enabled = true;
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

            DownloadButton.Enabled = true;
            MergeButton.Enabled = true;
            ExecuteButton.Enabled = true;
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

            DownloadButton.Enabled = true;
            MergeButton.Enabled = true;
            ExecuteButton.Enabled = true;
        }
        private void OnReadContactsReady(string connectionName, List<Contact> contacts)
        {
            if (connectionName.Equals(SOURCE_NAME))
            {
                SourceListBox.Items.Clear();

                foreach (Contact item in contacts)
                {
                    if (item.Phonenumbers.Count == 0) continue;
                    string boxItem = item.Name.FullName + " | " + item.Phonenumbers[0].Value;
                    SourceListBox.Items.Add(boxItem);
                }
            }
            else if (connectionName.Equals(TARGET_NAME))
            {
                OldTargetListBox.Items.Clear();

                foreach (Contact item in contacts)
                {
                    //if (item.Phonenumbers.Count == 0) continue;
                    string boxItem = item.Name.AdditionalName + "|" + item.Name.FullName + " | " + item.Name.GivenName + " | " + item.Name.FamilyName + "|" + item.Phonenumbers[0].Value;
                    OldTargetListBox.Items.Add(boxItem);
                }
            }

            DownloadButton.Enabled = true;
            MergeButton.Enabled = true;
            ExecuteButton.Enabled = true;
        }
        private void OnContactsMergingSuccess(string connectionName)
        {
            TargetLog.Text = "Contacts updated successfully!";
        }
        private void OnContactsMergingFail(string connectionName)
        {
            TargetLog.Text = "Failed to update contacts.";
        }

        private void OnTaskFail(string connectionName)
        {
            if (connectionName.Equals(SOURCE_NAME))
            {
                SourceLog.Text = "Action failed, check connection first";     
            }
            else if (connectionName.Equals(TARGET_NAME))
            {
                TargetLog.Text = "Action failed, check connection first";
            }
            DownloadButton.Enabled = true;
            MergeButton.Enabled = true;
            ExecuteButton.Enabled = true;
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
                this.OnContactsMergingFail,
                this.OnConnectionFail
            );

            TargetConnection = new GoogleConnection(
                TARGET_NAME,
                this.OnConnectionSuccess,
                this.OnConnectionFail,
                this.OnDisconnected,
                this.OnReadContactsReady,
                this.OnContactsMergingSuccess,
                this.OnContactsMergingFail,
                this.OnConnectionFail
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

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            if (TargetConnection.IsConnected())
            {
                TargetConnection.InitReadContacts();
            }
            
            if (SourceConnection.IsConnected())
            { 
                SourceConnection.InitReadContacts();
            }    
        }
        private void MergeButton_Click(object sender, EventArgs e)
        {
            if (!TargetConnection.IsConnected() || !SourceConnection.IsConnected())
            {
                return;
            }

            List<Contact> mergedContacts = Utils.MergeContactsPreview(SourceConnection.LastContactsList, TargetConnection.LastContactsList);
            NewTargetListBox.Items.Clear();
            foreach (Contact item in mergedContacts)
            {
                if (item.Phonenumbers.Count == 0) continue;
                string boxItem = item.Name.FullName + " | " + item.Phonenumbers[0].Value;
                NewTargetListBox.Items.Add(boxItem);
            } 
        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            if (TargetConnection.IsConnected() && SourceConnection.IsConnected())
            {
                TargetConnection.InitMergeContacts(SourceConnection.LastContactsList);
            }
        }

    }
}
