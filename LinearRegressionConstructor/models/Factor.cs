using System.Collections.Generic;

namespace LinearRegressionConstructor.models
{
    internal class Factor
    {
        public int Num { get; set; }
        public string Name { get; set; }
        public List<double> Observations { get; set; }
        public Factor(int num, string name, List<double> obs)
        {
            Num = num;
            Name = name;
            Observations = new List<double>(obs);
        }
    }
}
