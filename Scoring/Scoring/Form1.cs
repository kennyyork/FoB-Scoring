using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scoring
{
    public partial class Form1 : Form
    {
        BindingList<Person> people = new BindingList<Person>();
        Person activePerson = null;
        int active = 0;
        public Form1()
        {
            InitializeComponent();
            //people.Add(new Person { FirstName = "Kenny", LastName = "York" });
            //people.Add(new Person { FirstName = "Kacey", LastName = "York" });
            //people.Add(new Person { FirstName = "Doug", LastName = "Porter" });
            //people.Add(new Person { FirstName = "Casey", LastName = "Kuluz" });
            
            
            //people.ListChanged += new ListChangedEventHandler(people_ListChanged);
        }

        void people_ListChanged(object sender, ListChangedEventArgs e)
        {
            
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //load existing

            //txtFirst.DataBindings.Add("Text", people[0], "FirstName");
            //txtLast.DataBindings.Add("Text", people[0], "LastName");
            //people.AllowEdit = false;    
            activePerson = new Person();
            txtFirst.DataBindings.Add("Text", activePerson, "FirstName");
            txtLast.DataBindings.Add("Text", activePerson, "LastName");

            Update();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (btnEdit.Text == "Add")
            {
                AddNew();
            }
            else
            {
                //EditCurrent();
            }
        }

        private void AddNew()
        {
            people.Add(activePerson);
            ++active;
            Update();
        }

        private void Update()
        {
            btnPrev.Enabled = (active > 0);
            btnNext.Enabled = (active < (people.Count-1));

            if (active >= 0 && active < (people.Count - 1))
            {
                activePerson = new Person { FirstName = people[active].FirstName, LastName = people[active].LastName };
            }
            else
            {
                activePerson = new Person();
            }

            ((PropertyManager)this.BindingContext[activePerson]).ResumeBinding();

            if (active == people.Count)
            {
                btnEdit.Text = "Add";
            }
            else
            {
                btnEdit.Text = "Edit";
            }
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
