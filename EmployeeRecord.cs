using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Part1;

namespace Part2
{
    public class EmployeeRecord
    {
        // create an employee Record with public properties
        //    ID                        a number to identify an employee
        public int ID { get; set; }
        //    Name                      the employee name
        public string Name { get; set; }
        //    StateCode                 the state collecting taxes for this employee
        public string stateCode { get; set; }
        //    HoursWorkedInTheYear      the total number of hours worked in the entire year (including fractions of an hour)
        decimal HoursWorkedInTheYear { get; set; }
        //    HourlyRate                the rate the employee is paid for each hour worked
        public decimal HourlyRate { get; set; }
        //                                  assume no changes to the rate throughout the year.

        //    provide a constructor that takes a csv and initializes the employeerecord
        public EmployeeRecord(string csv)
        {
            //       do all error checking and throw appropriate exceptions
            var myitem = csv.Split(",");

            this.Name = myitem[1];
            this.stateCode = myitem[2];

            if (!int.TryParse(myitem[0], out int id))
            {
                throw new Exception($"Expected a integer for element 1: {csv}");
            }
            this.ID = id;
            if (!decimal.TryParse(myitem[3], out decimal hoursWorked))
            {
                throw new Exception($"Expected a DateTime for element 4: {csv}");
            }
            this.HoursWorkedInTheYear = hoursWorked;

            if (!decimal.TryParse(myitem[4], out decimal hourlyRate))
            {
                throw new Exception($"Expected a DateTime for element 5: {csv}");
            }
            this.HourlyRate = hourlyRate;
        }

        //    provide an additional READ ONLY property called  YearlyPay that will compute the total income for the employee
        //        by multiplying their hours worked by their hourly rate
        public decimal YearlyPay
        {
            get
            {
                return HourlyRate * HoursWorkedInTheYear;
            }
        }
        //    provide an additional READONLY property that will compute the total tax due by:
        //        calling into the taxcalculator providing the statecode and the yearly income computed in the YearlyPay property
        public decimal TotalTax
        {
            get
            {
                return TaxCalculator.ComputeTaxFor(stateCode, YearlyPay);

            }
        }
        //    provide an override of toString to output the record : including the YearlyPay and the TaxDue
        public override string ToString()
        {
            //Employee list with name, hourly rate, emp ID etc
            return ($"Employee with ID: {ID}, Name : {Name}, State Code: {stateCode}, " +
                $"Hours worked in the year: {HoursWorkedInTheYear}, Hourly rate: {HourlyRate}, will have a Yearly Pay of" +
                $": ${YearlyPay} and a Due Tax of : ${TotalTax}");
        }
    }


    public static class EmployeesList
    {

        // create an EmployeeList class that will read all the employees from the Employees.csv file
        // the logic is similar to the way the taxcalculator read its taxrecords

        // Create a List of employee records.  The employees are arranged into a LIST not a DICTIONARY
        //   because we are not accessing the employees by state,  we are accessing the employees sequentially as a list
        public static List<EmployeeRecord> Emplist;
        // create a static constructor to load the list from the file
        //   be sure to include try/catch to display messages
        static EmployeesList()
        {
            StreamReader reader;
            try
            {

                Emplist = new List<EmployeeRecord>();

                using (reader = File.OpenText("employees.csv"))
                {
                    string line;
                    while (!string.IsNullOrWhiteSpace(line = reader.ReadLine()))
                    {
                        try
                        {

                            Emplist.Add(new EmployeeRecord(line));
                        }
                        catch (Exception Ex)
                        {
                            Console.WriteLine(Ex.Message);
                        }
                        if (reader.EndOfStream) { break; }
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
        }

    }

    class Program
    {

        // loop over all the employees in the EmployeeList and print them
        // If the ToString() in the employee record is correct, all the data will print out.

        public static void Main()
        {
            try
            {
                // write your logic here

                foreach (EmployeeRecord record in EmployeesList.Emplist)
                {
                    Console.WriteLine(record.ToString());
                }

            }



            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
