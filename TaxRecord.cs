using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part1
{
    public class TaxRecord
    {
        // create a TaxRecord class representing a line from the file.  
        // It should have public properties of the correct type
        // for each of the columns in the file
        //  StateCode   (used as the key to the dictionary)
        public string StateCode { get; set; }
        //  State       (Full state name)
        public string State { get; set; }
        //  Floor       (lowest income for this tax bracket)
        public decimal Floor { get; set; }
        //  Ceiling     (highest income for this tax bracket )
        public decimal Ceiling { get; set; }
        //  Rate        (Rate at which income is taxed for this tax bracket)
        public decimal Rate { get; set; }
        //  Create a ctor taking a single string (a csv) and use it to load the properties in the record
        public TaxRecord(string csv)
        {
            var myitems = csv.Split(",");
            this.StateCode = myitems[0];
            this.State = myitems[1];
            if (!decimal.TryParse(myitems[2], out decimal floor))
            {
                throw new Exception($"Expected a decimal for element 3: {csv}");
            }
            this.Floor = floor;
            if (!decimal.TryParse(myitems[3], out decimal ceiling))
            {
                throw new Exception($"Expected a decimal for element 4: {csv}");
            }
            this.Ceiling = ceiling;
            if (!decimal.TryParse(myitems[4], out decimal rate))
            {
                throw new Exception($"Expected a decimal for element 5: {csv}");
            }
            this.Rate = rate;

        }
        //  Be sure to throw detailed exceptions when the data is invalid
        //
        //  Create an override of ToString to print out the tax record info nicely
        public override string ToString()
        {
            return $"State Code: {StateCode} State: {State} Floor: {Floor} Ceiling: {Ceiling} Rate: {Rate}";
        }


    }  // this is the end of the TaxRecord


    public static class TaxCalculator
    {
        // Create a static dictionary field that holds a List of TaxRecords and is keyed by a string
        static Dictionary<string, List<TaxRecord>> TaxRec;
        public static bool VerboseMode = false;
        // create a static constructor that:
        static TaxCalculator()
        {

            // declare a streamreader to read a file

            StreamReader reader;


            // enter a try/catch block for the entire static constructor to print out a message if an error occur
            try
            {
                // initialize the static dictionary to a newly create empty one
                TaxRec = new();
                // open the taxtable.csv file into the streamreader
                using (reader = File.OpenText("taxtable.csv"))
                {


                    // loop over the lines from the streamreader
                    // read a line from the file
                    string line;
                    while (!string.IsNullOrWhiteSpace(line = reader.ReadLine()))
                    {
                        try
                        {

                            // constuct a taxrecord from the (csv) line in the file
                            TaxRecord Tax = new TaxRecord(line);

                            // see if the state in the taxrecord is already in the dictionary
                            if (TaxRec.ContainsKey(Tax.StateCode))
                            {

                                //     if it is:  add the new tax record to the list of records in that state
                                TaxRec[Tax.StateCode].Add(Tax);
                                //TaxRec.Add(Tax.StateCode, new List<TaxRecord>() { Tax});

                            }
                            //     if it is not
                            else
                            {
                                //            create a new list of taxrecords

                                //            add the new taxrecord to the list
                                //            add the list to the dictionary under the state for the taxrecord
                                TaxRec.Add(Tax.StateCode, new List<TaxRecord>() { Tax });
                            }



                            //provide a way to get out of the loop when you are done with the file....
                            if (reader.EndOfStream) { break; }

                        }
                        catch (Exception Ex)
                        {
                            Console.WriteLine(Ex.Message);
                        }
                    }
                }
            }

            // catch any exceptions while processing each line in another try/catch block located INSIDE the loop
            //   this way if the line in the CSV file is incorrect, you will continue to process the next line
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message); return;
            }
            // make sure the streamreader is disposed no matter what happens.

        }






        // create a static method (ComputeTaxFor)  to return the computed tax given a state and income
        public static decimal ComputeTaxFor(string statecode, decimal income)
        {
            decimal final = 0;
            //  use the state as a key to find the list of taxrecords for that state
            if (!TaxRec.ContainsKey(statecode))
            {
                //   throw an exception if the state is not found.
                throw new Exception($" This state Code {statecode} is not in our Records");
            }
            //   otherwise use the list to compute the taxes
            else
            {
                //  Create a variable to hold the final computed tax.  set it to 0

                //  loop over the list of taxrecords for the state
                List<TaxRecord> list;

                list = TaxRec[statecode];

                foreach (TaxRecord tr in list)
                {
                    //check to see if the income is within the tax bracket using the floor and ceiling properties in the taxrecord
                    if (!(income >= tr.Floor && income <= tr.Ceiling))
                    {
                        decimal totalTax = (tr.Ceiling - tr.Floor) * tr.Rate;
                        if (VerboseMode)
                        {
                            Console.WriteLine($"for this tax bracket [{tr} ] the tax was {totalTax}, and the final was ${final}");
                        }
                        final += totalTax;
                    }
                    else
                    {
                        decimal totalTax = (income - tr.Floor) * tr.Rate;
                        if (VerboseMode)
                        {
                            Console.WriteLine($"for this FINAL tax bracket [{tr}] the tax was ${totalTax}");
                        }
                        final += totalTax;
                        if (VerboseMode)
                        {
                            Console.WriteLine($"the FINAL tax  was ${final}");
                        }
                        return final;
                    }

                }




            }


            return final;
        }
        //     
        //     if NOT:  (the income is greater than the ceiling)
        //        compute the total tax for the bracket and add it to the running total of accumulated final taxes
        //        the total tax for the bracket is the ceiling minus the floor times the tax rate for that bracket.  
        //        all this information is located in the taxrecord
        //        after adding the total tax for this bracket, continue to the next iteration of the loop
        //     IF The income is within the tax bracket (the income is higher than the floor and lower than the ceiling
        //        compute the final tax by adding the tax for this bracket to the accumulated taxes
        //        the tax for this bracket is the income minus the floor time the tax rate for this bracket
        //        this number is the total final tax, and can be returned as the final answer


        // try to figure out how to implement the Verbose option AFTER you have everything else done.
        //public static void Verbose(string statecode, decimal income)
        
    }
    class Program
    {
        public static void Main()
        {

            // create an infinite loop to:
            // prompt the user for a state and an income
            // validate the data
            // calculate the tax due and print out the total
            // loop

            // after accomplishing this, you may want to also prompt for verbose mode or not in this loop
            // wrap everythign in a try/catch INSIDE the loop.  print out any exceptions that are unhandled
            //  something like this:


            do
            {
                try
                {
                    // put your logic here
                    //TaxCalculator.TaxRec;
                    Console.Write("Do you want to try the verbose mode? Yes or No: ");
                    string v = Console.ReadLine();
                    if (v.ToUpper() == "YES")
                    {
                        TaxCalculator.VerboseMode = true;
                    }
                    else
                    {
                        TaxCalculator.VerboseMode = false;
                    }
                    Console.WriteLine("Please enter your state followed by a 'comma', then your income:");
                    string reading = Console.ReadLine();

                    string[] data = reading.Split(",");

                    if (string.IsNullOrWhiteSpace(reading)) { break; }
                    if (data.Length != 2) { throw new Exception($"Entry must be two elements separated by a comma: {reading}"); }
                    if (!decimal.TryParse(data[1], out decimal value))
                    {
                        throw new Exception($"Expected a decimal type for the second entry: {reading}");
                    }
                    string statecode = data[0];
                    decimal income = value;


                    decimal result = TaxCalculator.ComputeTaxFor(statecode, income);
                    Console.WriteLine($"Tax: ${result}");


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (true);


        }
    }


}
