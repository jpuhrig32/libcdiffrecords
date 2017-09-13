using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;


namespace libcdiffrecords.Testing
{
    public class DummyRecords
    {
        string[] units = new string[10];
        double cdRate = 10.0f;
        double toxRate = 30.0f;
        double percentFemale = 50.0f;
        double meanAge = 65;
        double ageStdDev = 10;
         
        

        int maxSamplesPerAdmission = 18;
        int maxAdmissions = 10;
        int minTimeBetweenAdmissions = 5;
        int maxAdmissionLength = 365;
        DateTime startDate = new DateTime(2017, 1, 1);



        public DummyRecords()
        {
            ConstructUnitTable();

        }

        public DataPoint[] GenerateDummyRecords(int patientCount, int seed)
        {
            return GenerateDummyRecords(patientCount, seed, 10, 30);
        }

        /// <summary>
        /// Generates a set of dummy c diff records for testing
        /// </summary>
        /// <param name="patientCount">The number of patients</param>
        /// <param name="seed">The seed for the random number generator</param>
        /// <param name="cdiffrate">The percentage rate of positives - 10.0 = 10 percent. Only 1 decimal place considered</param>
        /// <param name="toxposrate">The percentage rate of the positives that are positive, 30.0 = 30 percent.</param>
        /// <returns></returns>
        public DataPoint[] GenerateDummyRecords(int patientCount, int seed, double cdiffrate, double toxposrate)
        {
            List<DataPoint> data = new List<DataPoint>();

            Random rand = new Random(seed);
            Dictionary<int, bool> mrnTable = new Dictionary<int, bool>();

            for (int i = 0; i < patientCount; i++)
            {

            }


            return data.ToArray();
        }

        private void ConstructUnitTable()
        {
            units[0] = "Laboratory";
            units[1] = "ICU";
            units[2] = "Neonatal";
            units[3] = "Bathroom";
            units[4] = "Parking Garage";
            units[5] = "Library";
            units[6] = "Definitely not the Hospital";
            units[7] = "Office";
            units[8] = "Coffee Shop";
            units[9] = "Cafeteria";
        }

        private double GenerateNormalDistributionRand(Random rand, double mean, double standardDev)
        {
            double u1 = 1.0 - rand.NextDouble();
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            return mean + standardDev * randStdNormal;
        }
    }
}
