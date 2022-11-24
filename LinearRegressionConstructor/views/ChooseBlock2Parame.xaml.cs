using System;
using System.Collections.Generic;
using System.Linq;
using LinearRegressionConstructor.viewmodels;
using System.Windows;

namespace LinearRegressionConstructor.views
{
    /// <summary>
    /// Логика взаимодействия для ChooseBlock2Parame.xaml
    /// </summary>
    public partial class ChooseBlock2Parame : Window
    {
        public ChooseBlock2Parame()
        {
            InitializeComponent();
            DataContext = new ChooseBlock2ParameVM();
        }
    }
}
