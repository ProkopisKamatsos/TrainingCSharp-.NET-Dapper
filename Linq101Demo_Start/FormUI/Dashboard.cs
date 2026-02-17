using LinqLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormUI
{
  public partial class Dashboaard : Form
  {
    List<Person> people = ListManager.LoadSampleData();

    public Dashboaard()
    {
      InitializeComponent();

      InitializeBindings();
    }

    private void InitializeBindings()
    {
      allPeopleDropDown.DataSource = people;
      allPeopleDropDown.DisplayMember = "FullName";
    }

    private void UpdateBindings()
    {

    }

    private void allPeopleDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
      Person selectedPerson = (Person)allPeopleDropDown.SelectedItem;

      yearsExperiencePicker.Value = selectedPerson.YearsExperience;
    }

    private void updatePersonButton_Click(object sender, EventArgs e)
    {
      Person selectedPerson = (Person)allPeopleDropDown.SelectedItem;

      selectedPerson.YearsExperience = Convert.ToInt32(yearsExperiencePicker.Value);

      UpdateBindings();
    }
  }
}
