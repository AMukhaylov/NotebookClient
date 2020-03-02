using NotebookAPI.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;

namespace NotebookClient
{
    public partial class Form1 : Form
    {
        const string _baseAddress = "https://localhost:44353/";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateList();
        }

        public void UpdateList()
        {
            listView.Items.Clear();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;

                response = client.GetAsync("api/People").Result;
                if (response.IsSuccessStatusCode)
                {
                    Person[] reports = response.Content.ReadAsAsync<Person[]>().Result;
                    foreach (var p in reports)
                    {
                        var item = new ListViewItem(new[] { p.Firstname, p.Secondname, p.BirthDay.ToShortDateString() });
                        item.Tag = p.Id;
                        listView.Items.Add(item);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count != 0)
            {
                int id = (int)listView.SelectedItems[0].Tag;
                Delete(id);
                UpdateList();
            }

        }

        private void Delete(int delete)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.DeleteAsync("api/People/" + delete).Result;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Add addForm = new Add();
            addForm.Owner = this;
            addForm.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Edit editForm = new Edit();
            editForm.Owner = this;
            if (listView.SelectedItems.Count < 1)
                MessageBox.Show("Выделите запись в списке");
            else
            {
                editForm.id = (int)listView.SelectedItems[0].Tag;
                editForm.ShowDialog();
            }
        }

        private void tbContacts_Click(object sender, EventArgs e)
        {
            Contacts c = new Contacts();
            c.Owner = this;
            c.ShowDialog();
        }
    }
}
