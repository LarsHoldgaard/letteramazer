using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Factory;
using LetterAmazer.Business.Services.Services;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Tools.CountryNameAdder
{
    public partial class Form1 : Form
    {
        private List<Country> countries; 
        private ICountryService countryService;

        public Form1()
        {
            InitializeComponent();

            this.countries = new List<Country>();
            this.countryService = new CountryService(new LetterAmazerEntities(), new CountryFactory());
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.countries = countryService.GetCountryBySpecificaiton(new CountrySpecification()
            {
                Take = 999
            });

            StringBuilder bld = new StringBuilder();
            foreach (var country in this.countries)
            {
                bld.AppendLine(string.Format("{0}\n", country.Name));
            }

            sourceTextBox.Text = bld.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var language = languageTextBox.Text;
            var countryText = translatedTextBox.Text;
            var countryStr = Regex.Split(countryText, "\r\n\r\n");

            if (countryStr.Length != countries.Count)
            {
                var msgBox = MessageBox.Show("Amount of countries not same as translation");

            }

            int index = 0;
            foreach (var s in countryStr)
            {
                var relevantCountry = countries[index];

                countryService.Create(new CountryName()
                {
                    CountryId = relevantCountry.Id,
                    Language = language,
                    Name = s
                });

                index++;
            }
        }
    }
}
