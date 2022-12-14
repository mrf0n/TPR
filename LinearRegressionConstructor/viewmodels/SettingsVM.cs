using LinearRegressionConstructor.cmd;
using LinearRegressionConstructor.views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearRegressionConstructor.viewmodels
{
    internal class SettingsVM:BaseViewModel
    {
        public string ThresholdPair
        {
            get
            {
                return $"{MainVM.Instance.CalculationConfig.ThresholdForCouples}";
            }
            set
            {
                double val;
                try
                {
                    val = Convert.ToDouble(value);
                }
                catch (Exception)
                {
                    val = MainVM.Instance.CalculationConfig.ThresholdForCouples;
                }
                MainVM.Instance.CalculationConfig.ThresholdForCouples = val;
                OnPropertyChanged(nameof(ThresholdPair));
            }
        }
        public string ThresholdLinear
        {
            get
            {
                return $"{MainVM.Instance.CalculationConfig.ThresholdForManaged}";
            }
            set
            {
                double val;
                try
                {
                    val = Convert.ToDouble(value);
                }
                catch (Exception)
                {
                    val = MainVM.Instance.CalculationConfig.ThresholdForManaged;
                }
                MainVM.Instance.CalculationConfig.ThresholdForManaged = val;
                OnPropertyChanged(nameof(ThresholdLinear));
            }
        }
        public bool IsSignChecked
        {
            get
            {
                return MainVM.Instance.CalculationConfig.IsSignChecked;
            }
            set
            {
                MainVM.Instance.CalculationConfig.IsSignChecked = value;
                OnPropertyChanged(nameof(IsSignChecked));
            }
        }
        public bool IsSignCheckedBlock1
        {
            get
            {
                return MainVM.Instance.CalculationConfig.IsSignCheckedBlock1;
            }
            set
            {
                MainVM.Instance.CalculationConfig.IsSignCheckedBlock1 = value;
                OnPropertyChanged(nameof(IsSignCheckedBlock1));
            }
        }
        public bool IsSignCheckedBlock2
        {
            get
            {
                return MainVM.Instance.CalculationConfig.IsSignCheckedBlock2;
            }
            set
            {
                MainVM.Instance.CalculationConfig.IsSignCheckedBlock2 = value;
                OnPropertyChanged(nameof(IsSignCheckedBlock2));
            }
        }
        public string CurrentParam
        {
            get
            {
                if (MainVM.Y == null)
                    return "Empty";
                return MainVM.Y.Name.Substring(0, Math.Min(30, MainVM.Y.Name.Count()));
            }
        }
        public string CurrentParamb
        {
            get
            {
                if (MainVM.Bl2 == null)
                    return "Empty";
                return MainVM.Bl2.Name.Substring(0, Math.Min(30, MainVM.Bl2.Name.Count()));
            }
        }
        private RelayCommand chooseParam;
        public RelayCommand ChooseParam => chooseParam ??= new(obj =>
        {
            ChoseeManagedParam wnd = new();
            ((ChooseManagedFactorVM)wnd.DataContext).FactorsCollection = MainVM.Diseases;
            wnd.ShowDialog();
            OnPropertyChanged(nameof(CurrentParam));
        });

        //private RelayCommand chooseParambl2;
        //public RelayCommand ChooseParamBlock2 => chooseParambl2 ??= new(obj =>
        //{
        //    ChooseBlock2Parame wnd2 = new();
        //    ((ChooseBlock2ParameVM)wnd2.DataContext).FactorsCollection = MainVM.listafterbl2;
        //    wnd2.ShowDialog();
        //    OnPropertyChanged(nameof(CurrentParamb));
        //});
    }
}
