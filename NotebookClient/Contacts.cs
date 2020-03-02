using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NotebookAPI.Models;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;

namespace NotebookClient
{
    public partial class Contacts : Form
    {
        const string _baseAddress = "https://localhost:44353/";

        public Contacts()
        {
            InitializeComponent();
        }

        private void Contacts_Load(object sender, EventArgs e)
        {
            listView.Items.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //получаем контакты
                HttpResponseMessage response = client.GetAsync("api/Values").Result;
                Contact[] contacts = response.Content.ReadAsAsync<Contact[]>().Result;

                //получаем людей
                response = client.GetAsync("api/People").Result;
                Person[] people = response.Content.ReadAsAsync<Person[]>().Result;

                //получаем типы контактов
                response = client.GetAsync("api/ContactTypes").Result;
                ContactType[] cType = response.Content.ReadAsAsync<ContactType[]>().Result;

                foreach (var c in contacts)
                {
                    var item = new ListViewItem(new[] { 
                        people.Where(p => p.Id == c.PersonId).FirstOrDefault().Firstname,   //имя
                        people.Where(p => p.Id == c.PersonId).FirstOrDefault().Secondname,  //фамилия
                        cType.Where(ct => ct.Id == c.ContactTypeId).FirstOrDefault().Title, //тип контакта
                        c.Value });                                                         //значение
                    listView.Items.Add(item);
                }
            }
        }
    }
}
