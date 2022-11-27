using LinearRegressionConstructor.models;
using LinearRegressionConstructor.viewmodels;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LinearRegressionConstructor.views
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainVM();
        }

        private void choseParamList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string ParamName = choseParamList.SelectedValue.ToString();
            List<Factor> Model = MainVM.Model;
            Factor Bl2 = Model[0];
            for (int i = 0; i < Model.Count; i++)
            {
                if (ParamName.Contains(Model[i].Name))
                {
                    Bl2 = Model[i];
                    break;
                }
            }
            double emin = Bl2.Observations.Min() - (Bl2.Observations.Max() - Bl2.Observations.Min()) * 0.5;
            double emax = Bl2.Observations.Max() + (Bl2.Observations.Max() - Bl2.Observations.Min()) * 0.5;
            double tmin = Bl2.Observations.Average() - (3 * Bl2.Observations.StandardDeviation());
            double tmax = Bl2.Observations.Average() + (3 * Bl2.Observations.StandardDeviation());
            double min = (emin + tmin) / 2;
            double max = (emax + tmax) / 2;
            MySlider.Minimum = min;
            MySlider.Maximum = max;
            MySlider.Value = min + ((Math.Abs(max) + Math.Abs(min)) / 2);
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ValPar.Content = (e.NewValue).ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string temp = Res.Text;
            string [] str = temp.Split('\n');
            for(var i=0; i<str.Length-1; i++)
            {
                choseParamList.Items.Add(str[i]);
            }
            Functions.Text = "";
            List<string> functions = MainVM.Functions;
            for(int i = 0; i < functions.Count; i++)
            {
                if (i == 0)
                    Functions.Text += functions[i];
                else
                    Functions.Text += " => " + functions[i];
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            double val = double.Parse(ValPar.Content.ToString());
            string ParamName = choseParamList.SelectedValue.ToString();
            List<Factor> Model = MainVM.Model;
            Factor Y = MainVM.Y;
            Factor Bl2 = Model[0];
            for (int i = 0; i < Model.Count; i++)
            {
                if (ParamName.Contains(Model[i].Name))
                {
                    Bl2 = Model[i];
                    break;
                }
            }
            List<double> temp = new List<double>();
            List<double> XI = new List<double>();
            List<Factor> y_new = new List<Factor>();
            List<Factor> strong = new List<Factor>();
            List<int> strongIndex = new List<int>();
            List<Factor> mid = new List<Factor>();
            List<int> midIndex = new List<int>();
            List<Factor> easy = new List<Factor>();
            List<int> easyIndex = new List<int>();

            double emin = Bl2.Observations.Min() - (Bl2.Observations.Max() - Bl2.Observations.Min()) * 0.5;
            double emax = Bl2.Observations.Max() + (Bl2.Observations.Max() - Bl2.Observations.Min()) * 0.5;
            double tmin = Bl2.Observations.Average() - (3 * Bl2.Observations.StandardDeviation());
            double tmax = Bl2.Observations.Average() + (3 * Bl2.Observations.StandardDeviation());
            double min = (emin + tmin) / 2;
            double max = (emax + tmax) / 2;

            for (int i = 0; i < Model.Count(); i++)
            {
                temp.Add(0);
            }

            for (int i = 0; i < Model.Count(); i++)
            {
                if (Bl2 != Model[i])
                {
                    if (Math.Abs(Correlation.Pearson(Bl2.Observations, Model[i].Observations)) > 0.7)
                    {
                        strong.Add(Model[i]);
                        strongIndex.Add(i);
                    }
                    else if (Math.Abs(Correlation.Pearson(Bl2.Observations, Model[i].Observations)) <= 0.7 && Math.Abs(Correlation.Pearson(Bl2.Observations, Model[i].Observations)) > 0.3)
                    {
                        mid.Add(Model[i]);
                        midIndex.Add(i);
                    }
                    else
                    {
                        easy.Add(Model[i]);
                        easyIndex.Add(i);
                    }
                }
            }
            //
            if (easy.Count > 0)
            {
                if (mid.Count > 0)
                {
                    if (strong.Count() == 0)
                    {
                        strong = mid; strongIndex = midIndex;
                        mid = easy; midIndex = easyIndex;
                        easy = new List<Factor>(); easyIndex = new List<int>();
                    }
                }
                else
                {
                    if (strong.Count() > 0)
                    {
                        mid = easy; midIndex = easyIndex;
                        easy = new List<Factor>(); easyIndex = new List<int>();
                    }
                    else
                    {
                        strong = easy; strongIndex = easyIndex;
                        easy = new List<Factor>(); easyIndex = new List<int>();
                    }
                }
            }
            else
            {
                if (mid.Count > 0)
                {
                    if (strong.Count() == 0)
                    {
                        strong = mid; strongIndex = midIndex;
                        mid = new List<Factor>(); midIndex = new List<int>();
                    }
                }
            }
            //
            for (int i = 0; i < strong.Count(); i++)
            {
                temp[strongIndex[i]] = Math.Abs(Correlation.Pearson(Bl2.Observations, strong[i].Observations));
            }
            if (mid.Count() > 0)
            {
                for (int i = 0; i < mid.Count(); i++)
                {
                    double sum = 0;
                    for (int j = 0; j < strong.Count(); j++)
                    {
                        sum += Math.Abs(Correlation.Pearson(Bl2.Observations, strong[j].Observations)) *
                            Math.Abs(Correlation.Pearson(mid[i].Observations, strong[j].Observations));
                    }
                    temp[midIndex[i]] = sum;
                }
            }
            if (easy.Count() > 0)
            {
                for (int i = 0; i < easy.Count(); i++)
                {
                    double sum = 0;
                    for (int j = 0; j < strong.Count(); j++)
                    {
                        for (int k = 0; k < mid.Count(); k++)
                        {
                            sum += Math.Abs(Correlation.Pearson(Bl2.Observations, strong[j].Observations)) *
                                Math.Abs(Correlation.Pearson(strong[j].Observations, mid[k].Observations)) *
                                Math.Abs(Correlation.Pearson(mid[k].Observations, easy[i].Observations));
                        }
                    }
                    temp[easyIndex[i]] = sum;
                }
            }
            int tempPar = int.Parse(ParamName.Split(' ')[0].Substring(1));
            Res.Text = "";
            Res.Foreground = Brushes.Black;
            for (int i = 0; i < temp.Count(); i++)
            {
                if (temp[i] != 0)
                {
                    double b = temp[i] * (Y.Observations.StandardDeviation()) / Model[i].Observations.StandardDeviation();
                    double a = Y.Observations.Average() - b * Model[i].Observations.Average(); 
                    Res.Text += "X" + (i + 1).ToString() + " = " + a + "+" + b + "·X" + tempPar + "=" + (a + b * val).ToString();
                    if ((a + b * val) < min || (a + b * val) > max)
                    {
                        Res.Foreground = Brushes.Red;
                        Res.Text += " - !НЕ ВХОДИТ В ИНТЕРВАЛ!";
                    }
                    Res.Text += '\n';
                    XI.Add(a + b * val);
                }
                else
                {
                    XI.Add(val);
                }
            }
            double resY = MainVM.ModelB[0];
            for (int i = 0; i < temp.Count(); i++)
            {
                resY += MainVM.ModelB[i + 1] * XI[i];
            }
            Res.Text += "\n" + "y = " + resY + ", при X" + tempPar + " = " + val;
        }
    }
}
