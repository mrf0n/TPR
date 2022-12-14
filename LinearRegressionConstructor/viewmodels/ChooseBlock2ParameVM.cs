using LinearRegressionConstructor.cmd;
using LinearRegressionConstructor.models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace LinearRegressionConstructor.viewmodels
{
    internal class ChooseBlock2ParameVM : BaseViewModel
    {
        public List<Factor> FactorsCollection { get; set; }
        public ICollectionView FactorsCollectionView => CollectionViewSource.GetDefaultView(FactorsCollection);
        private Factor selectedItem;
        public Factor SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(IsButtonEnabled));
            }
        }
        public bool IsButtonEnabled => SelectedItem != null;
        private RelayCommand? apply = null;
        public RelayCommand Apply => apply ??= new(obj =>
        {
            Window window2 = obj as Window;
            MainVM.Instance.CalculationConfig.Block2Factor = SelectedItem;
            MainVM.Bl2 = selectedItem;
            window2.Close();
        });
    }
}
