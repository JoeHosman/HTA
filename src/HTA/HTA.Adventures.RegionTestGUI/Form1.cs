using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DreamSongs.MongoRepository;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.RegionTestGUI
{
    public partial class Form1 : Form
    {
        private readonly IAdventureLocationRepository _adventureLocationRepository;
        public Form1()
        {
            InitializeComponent();
            _adventureLocationRepository = new AdventureLocationRepository();
        }

        private void btnAddLocation_Click(object sender, EventArgs e)
        {
            double lat = 0.0;
            double.TryParse(txtLat.Text, out lat);
            double lon = 0.0;
            double.TryParse(txtLon.Text, out lon);
            Geo geoLocation = new Geo { Location = new Location() { Lat = lat, Lon = lon } };
            string name = txtName.Text;
            string address = txtAddress.Text;
            var location = new AdventureLocation(geoLocation, name, address);

            _adventureLocationRepository.Add(location);

            MessageBox.Show("added: " + location.Id);
        }
    }
}
