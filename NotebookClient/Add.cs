using NotebookAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotebookClient
{
    public partial class Add : Form
    {
        const string _baseAddress = "https://localhost:44353/";

        public Add()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Adding();
            Form1 f1 = (Form1)Owner;
            f1.UpdateList();
            Close();
        }

        private void Adding()
        {
            Person newReport = new Person() { Firstname = tbFirstname.Text, Secondname = tbSecondname.Text, BirthDay = DateTime.Parse(tbBirthDay.Text) };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsJsonAsync("api/People", newReport).Result;
            }
        }

    }
}
