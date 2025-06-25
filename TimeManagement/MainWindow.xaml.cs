using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CalcLibrary;


namespace TimeManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
//Declarations 
        public String code="", name = "";
        public double credits, hours, weeks, hrsS;
        List<Calculate> modules = new List<Calculate>();

        private void SearchInformation()
        {
            var moduleQuery = from module in modules
                              where module.MCode.Equals(comboSearch.Text, StringComparison.OrdinalIgnoreCase)
                              select module;
            rec.Items.Clear();
            if (!moduleQuery.Any())
            {
                MessageBox.Show("Please select a module!");
            }
            foreach (Calculate cl in moduleQuery)
            {
                /*Sharpcorner (accessed on: 19 September 2022) https://www.c-sharpcorner.com/UploadFile/mahesh/working-with-listview-in-C-Sharp/ */
                rec.Items.Add("Number of weeks: " + cl.NumWeeks);
                rec.Items.Add("Module code: " + cl.MCode);
                rec.Items.Add("Module name: " + cl.MName);
                rec.Items.Add("Class hours: " + cl.CHours);
                rec.Items.Add("Credits: " + cl.NumCred);
                for (int i = 0; i < cl.Weeks.Count; i++)
                {
                    rec.Items.Add("Week: " + (i + 1));
                    rec.Items.Add("Week study self hours: " + cl.Weeks[i].studyWeekHours);
                }

            }
        }

        private void SaveHours(object sender, RoutedEventArgs e)
        {
            if(dpCertainDate.SelectedDate != null || !string.IsNullOrEmpty(txtHoursSpent.Text))
            {
                double hrsSpent;
                if (Double.TryParse(txtHoursSpent.Text, out hrsSpent))
                {
                    hrsS = hrsSpent;
                    var moduleQuery = from module in modules
                                      where module.MCode.Equals(comboSearch.Text, StringComparison.OrdinalIgnoreCase)
                                      select module;
                    if (moduleQuery.Any())
                    {
                        bool hasFound = false;
                        foreach (Calculate c in modules)
                        {
                            foreach (Week we in c.Weeks)
                            {
                                foreach (DateTime d in we.Days)
                                {
                                    if (d == dpCertainDate.SelectedDate.Value)
                                    {
                                        hasFound = true;
                                        if(we.studyWeekHours - Convert.ToDouble(txtHoursSpent.Text) < 0)
                                        {
                                            MessageBox.Show("Invalid hours spent deduction!");
                                        }
                                        else
                                        {
                                            we.studyWeekHours = we.studyWeekHours - Convert.ToDouble(txtHoursSpent.Text);
                                        }
                                        break;
                                    }
                                }
                                if (hasFound)
                                {
                                    break;
                                }

                            }
                            if (hasFound)
                            {
                                break;
                            }
                        }
                        SearchInformation();
                    }
                    else
                    {
                        MessageBox.Show("Please select a module!");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid hours spent");
                }
                
            }
            else
            {
                MessageBox.Show("Please enter all data!");
            }
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            SearchInformation();
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        
//Clear button that holds the removal of the user's input in the text boxs
        private void Clear(object sender, RoutedEventArgs e)
        {
            txtcode.Clear();
            txtname.Clear();
            txtnumberofcredits.Clear();
            txtclasshoursperweek.Clear();
            txtnumberofweeks.Clear();
            
        }

//Continue button that holds the storage of the user's information, through the use of if statemants and else statements 
        private void Continue(object sender, RoutedEventArgs e)
        {
            String mCode = txtcode.Text;
            if (String.IsNullOrEmpty(mCode))
            {
                MessageBox.Show("Please enter a valid student number");
                mCode = txtcode.Text;
            }
            else
            {
                code = mCode;
            }
          

            String nme = txtname.Text;
            if (String.IsNullOrEmpty(nme))
            {
                MessageBox.Show("Please enter a valid student number");
                nme = txtname.Text;
            }
            else
            {
                name = nme;
            }
            double numCred;
            if (Double.TryParse(txtnumberofcredits.Text, out numCred ))
            {
                credits = numCred;
            }
            else
            {
                MessageBox.Show("Please enter a valid student number");
            }
            double cHours;
            if (Double.TryParse(txtclasshoursperweek.Text, out cHours))
            {
                hours = cHours;
            }
            else
            {
                MessageBox.Show("Please enter a valid student number");
                
            }
            double numWeeks;
            if (Double.TryParse(txtnumberofweeks.Text, out numWeeks))
            {
                weeks = numWeeks;
            }
            else
            {
                MessageBox.Show("Please enter a valid student number");
                
            }
            
            if (dp.SelectedDate == null)
            {
                MessageBox.Show("Please pick a date");

            }
            else
            {
                MessageBox.Show("date entered");
            }

            Calculate cl = new Calculate (code, nme, numCred, cHours,numWeeks);
            modules.Add(cl);
            DateTime startDate = dp.SelectedDate.Value;
            dpCertainDate.DisplayDateStart = startDate;
            
            DateTime endDate = startDate.AddDays((numWeeks * 7)-1);
            dpCertainDate.DisplayDateEnd = endDate;
            /*Sharpcorner (accessed on: 19 September 2022)  https://www.c-sharpcorner.com/article/datetime-in-c-sharp/ */
            
            Week w = new Week();
            int counter = 0;
            for(DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                w.Days.Add(date);
                if(counter == 6)
                {
                    w.studyWeekHours = cl.calculatehours();
                    cl.Weeks.Add(w);
                    w = new Week();
                    counter = 0;
                }
                else
                {
                    counter++;
                }
            }

            comboSearch.Items.Clear();
            foreach(Calculate mod in modules)
            {
                comboSearch.Items.Add(mod.MCode);
            }
            
        }

    }
}
