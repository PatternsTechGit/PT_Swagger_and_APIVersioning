using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Responses.V2
{
    public class LineGraphDataAverage
    {
        public decimal TotalBalance { get; set; }
        public ICollection<string> Labels { get; set; }
        public ICollection<decimal> Figures { get; set; }
        public decimal Average { get; set; }
        public LineGraphDataAverage()
        {
            Labels = new List<string>();
            Figures = new List<decimal>();
        }
    }
}
