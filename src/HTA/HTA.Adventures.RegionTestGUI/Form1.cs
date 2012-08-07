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
using Nest;

namespace HTA.Adventures.RegionTestGUI
{
    public class AdventureLocationRepository : IAdventureLocationRepository
    {
        public AdventureLocationRepository()
        {
            var setting = new ConnectionSettings("office.mtctickets.com", 9200);
            setting.SetDefaultIndex("pins");
            client = new ElasticClient(setting);
        }

        static readonly MongoRepository<AdventureLocation> MongoAdventureLocationRepository = new MongoRepository<AdventureLocation>();
        private readonly ElasticClient client;

        public void Add(AdventureLocation location)
        {
            MongoAdventureLocationRepository.Add(location);

            var result = client.Index(location, "pins", "region", location.Id);

            //it was extra testing
            //string data = JsonConvert.SerializeObject(location);
        }

        public List<AdventureLocation> GetNearBy(Geo geoLocation, GeoRange geoRange)
        {

            var results = client.Search<AdventureLocation>(s => s
                                                                    .From(0)
                                                                    .Size(10)
                                                                    .Filter(f => f
                                                                                     .GeoDistance("geo.location", filter => filter
                                                                                                                                .Location(geoLocation.Location.Lon, geoLocation.Location.Lat)
                                                                                                                                .Distance(string.Format("{0}mi", geoRange.Range))))
                                                                    .Index("pins")
                                                                    .Type("region")
                );
            return results.Documents.ToList();
        }
    }
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
            var location = new AdventureLocation(geoLocation, name, address, textBox1.Text);

            _adventureLocationRepository.Add(location);

            MessageBox.Show("added: " + location.Id);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            double lat = 0.0;
            double.TryParse(txtLatSearch.Text, out lat);
            double lon = 0.0;
            double.TryParse(txtLonSearch.Text, out lon);
            Geo geoLocation = new Geo { Location = new Location() { Lat = lat, Lon = lon } };

            int range = 5;
            int.TryParse(txtMilesSearch.Text, out range);
            GeoRange geoRange = new GeoRange() { Range = range };
            var locations = _adventureLocationRepository.GetNearBy(geoLocation, geoRange);

            StringBuilder sb = new StringBuilder();
            int c = 0;
            foreach (var adventureLocation in locations)
            {
                sb.AppendFormat("{0}. {1} [Lon:{2}, lat:{3}]", c++, adventureLocation.Name, adventureLocation.Geo.Location.Lon, adventureLocation.Geo.Location.Lat);
                sb.AppendLine();
            }

            MessageBox.Show(sb.ToString());
        }
    }


}
