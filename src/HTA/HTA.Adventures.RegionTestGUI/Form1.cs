using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
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
            string name = txtName.Text;
            string address = txtAddress.Text;
            var location = new AdventureSpot(new LocationPoint() { Lat = lat, Lon = lon }, name, address, textBox1.Text);

            MessageBox.Show("added: " + location.Id);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            double lat = 0.0;
            double.TryParse(txtLatSearch.Text, out lat);
            double lon = 0.0;
            double.TryParse(txtLonSearch.Text, out lon);
            
            int range = 5;
            int.TryParse(txtMilesSearch.Text, out range);
            GeoRange geoRange = new GeoRange() { Range = range };
            //var locations = _adventureLocationRepository.GetNearBy(new LocationPoint() { Lat = lat, Lon = lon }, geoRange);

            //StringBuilder sb = new StringBuilder();
            //int c = 0;
            //foreach (var adventureLocation in locations)
            //{
            //    sb.AppendFormat("{0}. {1} [Lon:{2}, lat:{3}]", c++, adventureLocation.Name, adventureLocation.LocationPoint.Lon, adventureLocation.LocationPoint.Lat);
            //    sb.AppendLine();
            //}

            //MessageBox.Show(sb.ToString());
        }
    }


}
