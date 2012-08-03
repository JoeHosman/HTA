using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HTA.Adventures.Models.Types;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using ServiceStack.Text;


namespace HTA.Adventure.Types.TestGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            AdventureType type = new AdventureType() { Name = txtName.Text };

            ServiceClientBase client = new JsonServiceClient("http://localhost:10768");
            
            var response = client.Post<AdventureType>("/Adventure/Types/", (AdventureType)type);
        }
    }
}
