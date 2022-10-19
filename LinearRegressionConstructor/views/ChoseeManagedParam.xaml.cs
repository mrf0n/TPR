using LinearRegressionConstructor.viewmodels;
using System.Windows;

namespace LinearRegressionConstructor.views
{
    /// <summary>
    /// Логика взаимодействия для ChoseeManagedParam.xaml
    /// </summary>
    public partial class ChoseeManagedParam : Window
    {
        public ChoseeManagedParam()
        {
            InitializeComponent();
            DataContext = new ChooseManagedFactorVM();
        }
    }
}
