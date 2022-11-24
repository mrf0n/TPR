namespace LinearRegressionConstructor.models
{
    internal class Config
    {
        private Factor managedF;
        private double thresholdC;
        private double thresholdL;
        private bool isSignC;
        private bool isSignCheckedBl1;
        private bool isSignCheckedBl2;

        public Factor ManagedFactor
        {
            get => managedF;
            set => managedF = value;
        }
        public double ThresholdForCouples
        {
            get => thresholdC;
            set
            {
                if (value < 0.5 || value > 1)
                    thresholdC = 0.5;
                else
                    thresholdC = value;
            }
        }
        public double ThresholdForManaged
        {
            get => thresholdL;
            set
            {
                if (value <= 0.01 || value >= 0.1)
                    thresholdL = 0.02;
                else
                    thresholdL = value;
            }
        }
        public bool IsSignChecked
        {
            get => isSignC;
            set => isSignC = value;
        }
        public bool IsSignCheckedBlock1
        {
            get => isSignCheckedBl1;
            set => isSignCheckedBl1 = value;
        }
        public bool IsSignCheckedBlock2
        {
            get => isSignCheckedBl2;
            set => isSignCheckedBl2 = value;
        }
        public Config(Factor manFactor, double tfc, double tfm, bool isSignCh, bool isSignCheckedBl1, bool isSignCheckedBl2)
        {
            ManagedFactor = manFactor;
            ThresholdForCouples = tfc;
            ThresholdForManaged = tfm;
            IsSignChecked = isSignCh;
            IsSignCheckedBlock1 = isSignCheckedBl1;
            IsSignCheckedBlock2 = isSignCheckedBl2;
        }
    }
}
