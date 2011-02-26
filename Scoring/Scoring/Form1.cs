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
        List<Person> people = new List<Person>();
        Person activePerson = null;
        int active = 0;
        public Form1()
        {
            InitializeComponent();
            people.Add(new Person { FirstName = "Kenny", LastName = "York" });
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
            ++active;
            UpdateUI();
        }

        private void Form1_Load(object sender, EventArgs e)
        {            
            //load existing            
            activePerson = new Person();
            active = 0;
            txtFirst.DataBindings.Add("Text", activePerson, "FirstName");
            txtLast.DataBindings.Add("Text", activePerson, "LastName");

            UpdateUI();

            Score s1 = new Score { TeamName = "s1" };
            Score s2 = new Score { TeamName = "s2" };
            Score s3 = new Score { TeamName = "s3" };
            Score s4 = new Score { TeamName = "s4" };

            scoringInput1.SetScores(s1,s2,s3,s4);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            --active;
            UpdateUI();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (btnEdit.Text == "Add")
            {
                AddNew();
            }
            else
            {
                EditCurrent();
            }
        }

        private void AddNew()
        {
            //validate input

            //make a copy
            Person p = new Person(activePerson);
            people.Add(p);            
            ++active;

            UpdateUI();
        }

        private void EditCurrent()
        {
            people[active].Update(activePerson);
            UpdateUI();
        }

        private void UpdateUI()
        {
            btnPrev.Enabled = (active > 0);
            btnNext.Enabled = (active < (people.Count));

            if (active >= 0 && active < (people.Count))
            {
                activePerson.Update(people[active]);
            }
            else
            {
                activePerson.Default();
            }            

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

    public class Person : INotifyPropertyChanged
    {
        private string fName;
        private string lName;

        public string FirstName 
        {
            get { return fName; }
            set
            {
                fName = value;
                NotifyPropertyChanged("FirstName");
            }
        }
        public string LastName
        {
            get { return lName; }
            set
            {
                lName = value;
                NotifyPropertyChanged("LastName");
            }
        }

        public Person()
        {
            Default();
        }

        public Person(Person source)
        {
            Update(source);
        }

        public void Update(Person source)
        {
            this.FirstName = source.FirstName;
            this.LastName = source.LastName;
        }

        public void Default()
        {
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string name)
        {
            PropertyChangedEventHandler evt = PropertyChanged;
            if( evt != null )
            {
                evt(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}
