using LinearRegressionConstructor.viewmodels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LinearRegressionConstructor.views
{
    /// <summary>
    /// Логика взаимодействия для ChooseBlock2Param.xaml
    /// </summary>
    public partial class ChooseBlock2Param : Page
    {
        public ChooseBlock2Param()
        {
            InitializeComponent();
            DataContext = new ChooseBlock2Param();
        }
    }
}
