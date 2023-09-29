using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace MultiThreading_2._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int arrayLength = Convert.ToInt32(textBox1.Text);
            int threadsCount = Convert.ToInt32(textBox2.Text);
            int difficultyParameter = Convert.ToInt32(textBox3.Text);
            int deltaThreads = Convert.ToInt32(textBox4.Text);
            int deltaK = Convert.ToInt32(textBox5.Text);

            var objChart = chart1.ChartAreas[0];
            var objChart1 = chart2.ChartAreas[0];

            objChart.AxisX.Interval = 5;
            objChart.AxisY.Interval = 40;

            objChart.AxisX.Minimum = 1;
            objChart.AxisX.Maximum = threadsCount;

            objChart.AxisY.Minimum = 1;
            objChart.AxisY.Maximum = 600;

            chart1.Series.Clear();

            objChart1.AxisX.Interval = deltaK;
            objChart1.AxisY.Interval = 40;

            objChart1.AxisX.Minimum = 1;
            objChart1.AxisX.Maximum = difficultyParameter;

            objChart1.AxisY.Minimum = 1;
            objChart1.AxisY.Maximum = 600;

            chart2.Series.Clear();


            double[] a = new double[arrayLength];
            double[] b = new double[arrayLength];

            //Point[] points = new Point[threadsCount / deltaThreads];

            //Point[] points1 = new Point[difficultyParameter / deltaK];

            InitiateArrays(arrayLength, a, b);

            chart1.Series.Add("seria1");
            chart1.Series.Add("seria2");
            chart1.Series["seria2"].Color = Color.Coral;
            chart1.Series["seria2"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            chart2.Series.Add("seria2");
            chart1.Series["seria1"].Color = Color.BlueViolet;
            chart1.Series["seria1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart2.Series["seria2"].Color = Color.Coral;
            chart2.Series["seria2"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;



            for (int i = 0; i < difficultyParameter / deltaK; i++)
            {
                DateTime start = DateTime.Now;
                CalculateWithoutMultiThreading(0, arrayLength, i * deltaK + deltaK, a, b);
                //points1[i] = new Point(i + deltaK, (int)((DateTime.Now.Subtract(start)).Milliseconds));
                chart2.Series[0].Points.AddXY(i * deltaK + deltaK, (int)((DateTime.Now.Subtract(start)).Milliseconds));

                start = DateTime.Now;
                CalculateWithoutMultiThreading(0, arrayLength, difficultyParameter, a, b);
                //points1[i] = new Point(i + deltaK, (int)((DateTime.Now.Subtract(start)).Milliseconds));
                chart1.Series[1].Points.AddXY(i, (int)((DateTime.Now.Subtract(start)).Milliseconds));
            }


            for (int i = 0; i < threadsCount / deltaThreads; i++)
            {
                DateTime start = DateTime.Now;
                CalculateWithinMultiThreading(arrayLength, difficultyParameter, a, b, i * deltaThreads + deltaThreads);
                //points[i] = new Point(i * deltaThreads + deltaThreads, (int)((DateTime.Now.Subtract(start)).Milliseconds));
                chart1.Series[0].Points.AddXY(i * deltaThreads + deltaThreads, (int)((DateTime.Now.Subtract(start)).Milliseconds));
            }

            
        }

        public static void InitiateArrays(int arrayLength, double[] a, double[] b)
        {
            Random random = new Random();

            for (int i = 0; i < arrayLength; i++)
            {
                double element = random.NextDouble();
                a[i] = element;
                b[i] = 0;
            }
        }

        public static void CalculateWithoutMultiThreading(int start, int end, int difficultyParameter, double[] a, double[] b)
        {
            for (int i = start; i < end; i++)
            {
                for (int j = 0; j < difficultyParameter; j++)
                {
                    b[i] += Math.Pow(a[i], 1.789);
                }
            }
        }

        public static void CalculateWithinMultiThreading(int arrayLength, int difficultyParameter, double[] a, double[] b, int threadsCount)
        {
            Thread[] threads = new Thread[threadsCount];
            int partSize = (arrayLength + threadsCount - 1) / threadsCount;
            for (int i = 0; i < threadsCount; i++)
            {
                //int start = i * partSize;
                //int end = Math.Min(start + partSize, arrayLength);
                int start = i * arrayLength / threadsCount;
                int end = (i + 1) * arrayLength / threadsCount;
                threads[i] = new Thread(() =>
                {
                    CalculateWithoutMultiThreading(start, end, difficultyParameter, a, b);
                });
                threads[i].Start();
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }
    }
}
