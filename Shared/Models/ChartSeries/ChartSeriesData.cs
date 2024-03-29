using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.ChartSeries
{
    public class ChartSeriesData<X, Y>
    {
        public X XValue { get; set; } = default(X)!;
        public Y YValue { get; set; } = default(Y)!;


    }
    public class ChartSeries<X, Y>
    {
        public string Name { get; set; } = string.Empty;

        public List<ChartSeriesData<X, Y>> DataCollection { get; set; } = new List<ChartSeriesData<X, Y>>();


    }
}
