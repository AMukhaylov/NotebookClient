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
    public partial class Edit : Form
    {
        const string _baseAddress = "https://localhost:44353/";
        public int id;

        public Edit()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Editing();
            Form1 f1 = (Form1)Owner;
            f1.UpdateList();
            Close();
        }

        private void Editing()
        {
            Person person = new Person();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("api/People?Id=" + id).Result;
                person = response.Content.ReadAsAsync<Person>().Result;

                person.Firstname = tbFirstname.Text;
                person.Secondname = tbSecondname.Text;
                person.BirthDay = DateTime.Parse(tbBirthDay.Text);

                response = client.PutAsJsonAsync("api/People/" + id, person).Result;
                response.EnsureSuccessStatusCode();
            }
        }

        private void GetPerson()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("api/People?Id=" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    Person report = response.Content.ReadAsAsync<Person>().Result;

                    tbFirstname.Text = report.Firstname;
                    tbSecondname.Text = report.Secondname;
                    tbBirthDay.Text = report.BirthDay.ToShortDateString();
                }
            }
        }

        private void Edit_Load(object sender, EventArgs e)
        {
            GetPerson();
        }
    }
}
