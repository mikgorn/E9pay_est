using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace E9pay_est
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double start_value = Convert.ToDouble(textBox1.Text);
            double end_value = Convert.ToDouble(textBox2.Text);

            DateTime start = monthCalendar1.SelectionStart;
            DateTime end = monthCalendar2.SelectionEnd;
            

            int period = Convert.ToInt32((end-start).TotalDays)-1;

            DateTime finish = end.AddDays(period);

            double k ;
            if (period < 21)
            {
               k = Math.Log(end_value / start_value) / period;
            }
            else
            {
                k = Math.Log(end_value / start_value) / (period / 7);
            }

            var xValues = new List<String>();
            var yValues = new List<int>();

            int i = 0;

            DataTable table = new DataTable();
            table.Columns.Add("Date");
            table.Columns.Add("Value");

            foreach (DateTime date in EachDay(start, finish))
            {
                DataRow dr = table.NewRow();

                if (period < 21)
                {
                    double value = start_value * Math.Exp(k * i);

                    xValues.Add(date.ToShortDateString());
                    yValues.Add(Convert.ToInt32(value));

                    dr["Date"] = date.ToShortDateString();
                    dr["Value"] = Convert.ToInt32(value);

                    table.Rows.Add(dr);
                }
                else if (i % 7 == 0)
                {
                    double value = start_value * Math.Exp(k * (i/7));

                    xValues.Add(date.ToShortDateString());
                    yValues.Add(Convert.ToInt32(value));

                    dr["Date"] = date.ToShortDateString();
                    dr["Value"] = Convert.ToInt32(value);

                    table.Rows.Add(dr);
                }
                i++;
            }
            var series = new Series("Data");
            series.Points.DataBindXY(xValues,yValues);
            series.ChartType = SeriesChartType.Line;
            chart1.Series.Clear();
            chart1.Series.Add(series);

            

            BindingSource bind_source = new BindingSource();
            bind_source.DataSource = table;

            dataGridView1.DataSource = bind_source;


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
