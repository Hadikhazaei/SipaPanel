using System.Collections.Generic;

namespace ZyPanel.Helper.Vmodel {
    public class PlanningInfoVm {
        public string Info { get; set; }
    }

    // 
    // Line chart vm
    // 
    public class LineChartVm {
        public string SeriesTitle { get; set; }

        public List<LineChartItemVm> Series { get; set; }
    }

    public class LineChartItemVm {
        public long DataNumber { get; set; }

        public string DataTitle { get; set; }

        public string DataValue { get; set; }
    }
}