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
        public int shift = 120;
        public struct Country
        {
            public TextBox name;
            public TextBox total;
            public TextBox part;
        }

        LinkedList<Country> countries = new LinkedList<Country>();

        public Form1()
        {
            InitializeComponent();
        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        private void button1_Click_1(object sender, EventArgs e)
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

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            TextBox text1 = new TextBox();
            text1.SetBounds(10, shift, 100, 10);

            TextBox text2 = new TextBox();
            text2.SetBounds(120, shift, 50, 10);

            TextBox text3 = new TextBox();
            text3.SetBounds(180, shift, 50, 10);

            tabPage2.Controls.Add(text1);
            tabPage2.Controls.Add(text2);
            tabPage2.Controls.Add(text3);

            text1.Text = "Country name";
            text2.Text = "0";
            text3.Text = "0";

            Country country = new Country {
                name = text1,
                total = text2,
                part = text3
            };
            countries.AddLast(country);

            shift += 30;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Country");
            table.Columns.Add("Total");
            table.Columns.Add("Part");
            table.Columns.Add("%");

            double sum_total = 0;
            double sum_part = 0;

            int n = 0;
            foreach (Country country in countries)
            {
                double total = Convert.ToDouble(country.total.Text);
                double part = Convert.ToDouble(country.part.Text);
                string name = country.name.Text;

                sum_total += total;
                sum_part += part;

                double percent = Math.Round(part*100 / total,2);

                DataRow dr = table.NewRow();
                dr[0] = name;
                dr[1] = total;
                dr[2] = part;
                dr[3] = percent;

                table.Rows.Add(dr);

                n++;
            }

            double total_percent = Math.Round(sum_part * 100 / sum_total, 2);

            DataRow total_dr = table.NewRow();
            total_dr[0] = "Total";
            total_dr[1] = sum_total;
            total_dr[2] = sum_part;
            total_dr[3] = total_percent;

            table.Rows.Add(total_dr);

            

            BindingSource bind_source = new BindingSource();
            bind_source.DataSource = table;

            dataGridView2.DataSource = bind_source;


            for (int i = 0; i < 4; i++)
            {
                dataGridView2.Rows[n].Cells[i].Style.BackColor = Color.Green;
            }

            
        }
    }
}
