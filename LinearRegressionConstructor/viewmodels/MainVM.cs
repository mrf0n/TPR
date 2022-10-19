using LinearRegressionConstructor.cmd;
using LinearRegressionConstructor.models;
using LinearRegressionConstructor.views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using MathNet.Numerics.Statistics;
using System.Threading.Tasks;

namespace LinearRegressionConstructor.viewmodels
{
    internal class MainVM : BaseViewModel
    {
        readonly string DSReady = "Dataset is ready";
        readonly List<double> TableF = new()
        {
            3.8415,
            2.9957,
            2.6049,
            2.3719,
            2.2141,
            2.0986,
            2.0096,
            1.9384,
            1.8799,
            1.8307
        };
        readonly double StudentCo = 1.961891;
        public static MainVM Instance { get; set; }
        private static Thread Compute;
        public MainVM()
        {
            CalculationConfig = new(null, 0.5, 0.05, false, false);
            X = new();
            ModelB = new();
            Model = new();
            Models = new();
            Diseases = new();
            Instance = this;
        }

        #region Brushes
        private Brush st1Clr = Brushes.Black;
        private Brush st2Clr = Brushes.Black;
        private Brush st3Clr = Brushes.Black;
        private Brush st4Clr = Brushes.Black;
        private Brush st5Clr = Brushes.Black;
        private Brush st6Clr = Brushes.Black;
        private Brush st7Clr = Brushes.Black;

        public Brush St1Clr
        {
            get
            {
                return st1Clr;
            }
            set
            {
                this.st1Clr = value;
                OnPropertyChanged(nameof(St1Clr));
            }
        }
        public Brush St2Clr
        {
            get
            {
                return st2Clr;
            }
            set
            {
                this.st2Clr = value;
                OnPropertyChanged(nameof(St2Clr));
            }
        }
        public Brush St3Clr
        {
            get
            {
                return st3Clr;
            }
            set
            {
                this.st3Clr = value;
                OnPropertyChanged(nameof(St3Clr));
            }
        }
        public Brush St4Clr
        {
            get
            {
                return st4Clr;
            }
            set
            {
                this.st4Clr = value;
                OnPropertyChanged(nameof(St4Clr));
            }
        }
        public Brush St5Clr
        {
            get
            {
                return st5Clr;
            }
            set
            {
                this.st5Clr = value;
                OnPropertyChanged(nameof(St5Clr));
            }
        }
        public Brush St6Clr
        {
            get
            {
                return st6Clr;
            }
            set
            {
                this.st6Clr = value;
                OnPropertyChanged(nameof(St6Clr));
            }
        }
        public Brush St7Clr
        {
            get
            {
                return st7Clr;
            }
            set
            {
                this.st7Clr = value;
                OnPropertyChanged(nameof(St7Clr));
            }
        } 
        #endregion

        #region Data
        public Config CalculationConfig { get; set; }
        public static List<Factor> Diseases { get; set; }
        public static List<Factor>? X { get; set; }
        public static Factor? Y { get; set; }
        public static List<double> ModelB { get; set; }
        public static List<Factor> Model { get; set; }
        private static List<List<Factor>> Models;
        #endregion

        #region Display
        private bool IsExecute = false;
        public string Status { get; set; }
        public bool IsCalcReady => Status == DSReady && !IsExecute;
        public string Equation
        {
            get
            {
                if (Model.Count == 0)
                    return "Модели нет";
                var res = $"y = {ModelB[0]:N7} ";
                for (int i = 1; i < ModelB.Count; i++)
                {
                    if (ModelB[i] == 0)
                        continue;
                    string sign = string.Empty;
                    if (ModelB[i] > 0)
                        sign = "+";
                    res += $"{sign} {ModelB[i]:N7}X{i}";
                }
                return res;
            }
            set
            {

            }
        }
        public string CurrentCount
        {
            get => $"Количество моделей: {Models.Count}";
            set { }
        }
        public string ModelInfo
        {
            get
            {
                string res = string.Empty;
                for (int i = 0; i < Model.Count; i++)
                {
                    if (ModelB[i+1] == 0)
                        continue;
                    res += $"X{i+1} = {Model[i].Name}\n";
                }
                return res;
            }
            set { }
        }
        #endregion

        #region Commands
        private RelayCommand open;
        private RelayCommand calc;
        private RelayCommand cancel;
        private RelayCommand settings;
        private RelayCommand exit;

        public RelayCommand Open
        {
            get
            {
                return open ??= new(obj =>
                {
                    var fd = new OpenFileDialog
                    {
                        Filter = "Excel (*.xlsx)| *.xlsx"
                    };
                    if (fd.ShowDialog() is true)
                    {
                        string path = fd.FileName;
                        try
                        {
                            ReadData(path);
                            Status = DSReady;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Data read error");
                            Status = String.Empty;
                            X.Clear();
                            Model.Clear();
                            Models.Clear();
                            ModelB.Clear();
                            ModelInfo = String.Empty;
                            Y = CalculationConfig.ManagedFactor = null;
                            Diseases.Clear();
                            ClearColors();
                        }
                    }
                    OnPropertyChanged(nameof(ModelInfo));
                    OnPropertyChanged(nameof(Status));
                    OnPropertyChanged(nameof(Equation));
                    OnPropertyChanged(nameof(CurrentCount));
                    OnPropertyChanged(nameof(IsCalcReady));
                });
            }
        }

        public RelayCommand Calculate => calc ??= new(async obj =>
        {
            ModelB.Clear();
            Model.Clear();
            IsExecute = !IsExecute;
            ModelInfo = string.Empty;
            OnPropertyChanged(nameof(ModelInfo));
            OnPropertyChanged(nameof(Equation));
            OnPropertyChanged(nameof(IsCalcReady));
            ClearColors();
            Compute = new Thread(Execute);
            Compute.IsBackground = true;
            Compute.Start();
        });

        public RelayCommand Cancel => cancel ??= new(obj =>
         {
             Compute.Interrupt();
             UpdateControls();
         });

        public RelayCommand Settings => settings ??= new(obj =>
        {
            Settings settings = new();
            settings.ShowDialog();
        });

        public RelayCommand Exit => exit ??= new(obj =>
        {
            Application.Current.Shutdown();
        });

        #endregion

        #region Help Methods
        private void ReadData(string path)
        {
            //Чтение данных из xlsx
            var connectionString = String.Format($"Provider=Microsoft.ACE.OLEDB.12.0;Extended Properties=\"EXCEL 12.0; HDR=Yes;\";Data Source={path}");
            var excelDataSet = new DataSet();
            using (OleDbConnection connection = new(connectionString))
            {
                connection.Open();
                var objDA = new OleDbDataAdapter("select * from [2005-2020$C:BC]", connection);
                objDA.Fill(excelDataSet);
            }
            var table = excelDataSet.Tables[0];

            var dis = new List<Factor>();
            //Запись заболеваемостей, управляющих факторов
            for (int i = 0; i < table.Columns.Count; i++)
            {
                var obs = new List<double>();
                for (int j = 0; j < table.Rows.Count; j++)
                    obs.Add((double)table.Rows[j][i]);

                if (i < 16)
                    dis.Add(new(i + 1, table.Columns[i].Caption, obs));
                else
                    X.Add(new(i - 15, table.Columns[i].Caption, obs));
            }
            Diseases = new List<Factor>(dis);
            //Открытие окна с выбором управляемого фактора
            ChoseeManagedParam wnd = new();
            ((ChooseManagedFactorVM)wnd.DataContext).FactorsCollection = dis;
            wnd.ShowDialog();
            //Выбор управляемого фактора
            if (CalculationConfig.ManagedFactor == null)
            {
                CalculationConfig.ManagedFactor = Diseases.First();
            }
            Y = CalculationConfig.ManagedFactor;
            Y.Num = 0;
        }
        public void UpdateControls()
        {
            Models.Clear();
            Models.Add(Model);
            IsExecute = !IsExecute;
            OnPropertyChanged(nameof(Equation));
            OnPropertyChanged(nameof(CurrentCount));
            OnPropertyChanged(nameof(ModelInfo));
            OnPropertyChanged(nameof(IsCalcReady));
        }
        public void SetWarningStage(ref Brush brush, Brush color) => brush = color;
        private void Execute()
        {
            #region Stage 0
            if (CalculationConfig.IsSignCheckedBlock1)
                {
                    int test = 0;
                    List<Factor> tempo = new List<Factor>(X.Count);
                    for (int i = 0; i < X.Count; i++)
                    {
                        FuncPreprocessingOfStatData(i, X, Y, ref tempo);
                        X[i] = tempo[i];
                    }
                    var t = 0;
                }
            #endregion

            #region Stage 1
            var r = CalculatePearson(X);
            var gr = CreateGroups(r, CalculationConfig.ThresholdForCouples);
            GetModels(gr);
            if (gr.Count == 0)
            {
                UpdateControls();
                return;
            }
            if (Models.Count == 1)
                SetWarningStage(ref st1Clr, Brushes.Gold);
            else
                SetWarningStage(ref st1Clr, Brushes.Green);
            OnPropertyChanged(nameof(St1Clr));
            #endregion

            #region Stage 2
            var linearPearson = GetLinearMultipliers(X, Models, Y);
            if (CalculationConfig.IsSignChecked)
                ImportanceMethod(ref Models, ref linearPearson);
            else
                EmpiricalMethod(ref Models, ref linearPearson, CalculationConfig.ThresholdForManaged);
            if (Models.Count == 1)
                SetWarningStage(ref st2Clr, Brushes.Gold);
            else
                SetWarningStage(ref st2Clr, Brushes.Green);
            OnPropertyChanged(nameof(St2Clr));
            #endregion

            #region Stage 3
            var BE = GetRegressionMult(Models, Y);
            St3Clr = Brushes.Green;
            #endregion

            #region Stage 4
            CheckAdequacyOfModels(ref BE);
            if (Models.Count == 1)
                SetWarningStage(ref st4Clr, Brushes.Gold);
            else
                SetWarningStage(ref st4Clr, Brushes.Green);
            OnPropertyChanged(nameof(St4Clr));
            #endregion

            #region Stage 5
            GetDeterminationMult(ref BE, Y);
            if (Models.Count == 1)
                SetWarningStage(ref st5Clr, Brushes.Gold);
            else
                SetWarningStage(ref st5Clr, Brushes.Green);
            OnPropertyChanged(nameof(St5Clr));
            #endregion

            int pos = 0;
            if (Models.Count > 1)
            {
                #region Stage 7
                var variations = GetVariations(Models, BE, Y);
                var minv = Queryable.Min(variations.AsQueryable());
                for (var i = 0; i < variations.Count; i++)
                    if (variations[i] == minv)
                        pos = i;
                #endregion
            }
            if (Models.Count == 1)
                SetWarningStage(ref st6Clr, Brushes.Gold);
            else
                SetWarningStage(ref st6Clr, Brushes.Green);
            OnPropertyChanged(nameof(St6Clr));
            St7Clr = Brushes.Green;

            ModelB = new ();
            foreach (var item in BE.Item1[pos])
                ModelB.Add(item);

            Model = new ();
            foreach (var item in Models[pos])
                Model.Add(item);

            UpdateControls();
        }
        private void ClearColors()
        {
            St1Clr = St2Clr = St3Clr = St4Clr = St5Clr = St6Clr = St7Clr = Brushes.Black;
        }
        #endregion

        #region CalcMethods
        #region Stage 1
        /// <summary>
        /// Расчет парных коэффициентов Пирсона
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        static List<List<double>> CalculatePearson(List<Factor> parameters)
        {
            var pairs = new List<List<double>>();

            for (int i = 0; i < parameters.Count - 1; i++)
            {
                pairs.Add(new());
                for (int j = i + 1; j < parameters.Count; j++)
                {
                    double up = 0, down;
                    double iavg = Queryable.Average(parameters[i].Observations.AsQueryable());
                    double javg = Queryable.Average(parameters[j].Observations.AsQueryable());

                    double isum = 0, jsum = 0;
                    for (int l = 0; l < parameters[i].Observations.Count; l++)
                    {
                        up += (parameters[i].Observations[l] - iavg) * (parameters[j].Observations[l] - javg);
                        isum += Math.Pow(parameters[i].Observations[l] - iavg, 2);
                        jsum += Math.Pow(parameters[j].Observations[l] - javg, 2);
                    }
                    down = Math.Pow(isum * jsum, 0.5);
                    pairs.Last().Add(up / down);
                }
            }
            return pairs;
        }

        /// <summary>
        /// Создание групп управляющих факторов.
        /// </summary>
        /// <param name="r">Таблица парных коэффициентов Пирсона.</param>
        /// <param name="c">Пороговое значение.</param>
        /// <returns>Возвращает список групп.</returns>
        static List<List<int>> CreateGroups(List<List<double>> r, double c)
        {
            var groups = new List<List<int>>();
            var graph = new List<Vertex>();
            for (int i = 0; i <= r.Count; i++)
            {
                graph.Add(new(i));
            }

            for (int i = 0; i < r.Count; i++)
            {

                for (int j = 0; j < r[i].Count; j++)
                {
                    if (Math.Abs(r[i][j]) > c)
                    {
                        graph[i].AddVertex(i + j + 1);
                    }
                }
            }

            Bka(new List<Vertex>(), graph, new List<Vertex>(), ref groups);

            for (int i = 0; i < graph.Count; i++)
            {
                var section = groups.Where(x => x[0] == i).ToList();
                section.Remove(section[0]);
                foreach (var item in section)
                {
                    groups.Remove(item);
                }
            }
            groups = groups.OrderBy(x => x.Count).Reverse().ToList();
            for (int i = 0; i < groups.Count - 1; i++)
            {
                for (int j = 0; j < groups[i].Count; j++)
                {
                    for (int iO = i + 1; iO < groups.Count; iO++)
                    {
                        groups[iO].Remove(groups[i][j]);
                    }
                }
            }

            var independentLists = groups.Where(x => x.Count == 1).ToList();
            var independentGroup = new List<int>();
            foreach (var item in independentLists)
            {
                independentGroup.Add(item[0]);
            }
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups[i].Count < 2)
                {
                    groups.RemoveAt(i);
                    i--;
                }
            }
            groups.Add(independentGroup);
            return groups;
        }

        /// <summary>
        /// Получение моделей.
        /// </summary>
        /// <param name="gr"></param>
        /// <returns></returns>
        void GetModels(List<List<int>> gr)
        {
            var models = new List<List<int>>();
            GenerateModels(new List<int>(), gr, ref models);
            foreach (var model in models)
            {
                var modelF = new List<Factor>();
                foreach (var param in model)
                {
                    modelF.Add(X.Where(x => x.Num == param + 1).First());
                }
                Models.Add(modelF);
                OnPropertyChanged(nameof(CurrentCount));
            }
        }

        /// <summary>
        /// Рекурсивный проход по группам.
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="grps"></param>
        /// <param name="models"></param>
        static void GenerateModels(List<int> cur, List<List<int>> grps, ref List<List<int>> models)
        {
            if (grps.Count == 0)
            {
                models.Add(cur);
                return;
            }

            foreach (var item in grps.First())
            {
                var new_cur = new List<int>(cur)
                {
                    item
                };
                var new_grps = grps.Where(x => x != grps.First()).ToList();
                GenerateModels(new_cur, new_grps, ref models);
            }
        }

        /// <summary>
        /// Находжение клик в графе.
        /// </summary>
        /// <param name="R"></param>
        /// <param name="P"></param>
        /// <param name="X"></param>
        /// <param name="groups"></param>
        static void Bka(List<Vertex> R, List<Vertex> P, List<Vertex> X, ref List<List<int>> groups)
        {
            if (P.Count == 0 && X.Count == 0)
            {
                groups.Add(R.Select(x => x.Num).ToList());
                return;
            }
            foreach (var item in P)
            {
                var newR = new List<Vertex>(R)
                {
                    item
                };
                Bka(newR, P.Where(x => item.IsRelated(x.Num)).ToList(), X.Where(x => item.IsRelated(x.Num)).ToList(), ref groups);
                P = P.Where(x => x != item).ToList();
                X.Add(item);
            }
        }
        #endregion

        #region Stage 2
        /// <summary>
        /// Вычисление линейной корреляции Пирсона.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="models"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static List<List<double>> GetLinearMultipliers(List<Factor> parameters, List<List<Factor>> models, Factor y)
        {
            var mults = new List<List<double>>();
            foreach (var model in models)
            {
                var multsParam = new List<double>();
                foreach (var param in model)
                {
                    var xavr = Queryable.Average(parameters[param.Num - 1].Observations.AsQueryable());
                    var yavg = Queryable.Average(y.Observations.AsQueryable());
                    double r, up, down, fsum, ssum;
                    up = fsum = ssum = 0;
                    for (int l = 0; l < parameters[0].Observations.Count; l++)
                    {
                        up += (parameters[param.Num - 1].Observations[l] - xavr) * (y.Observations[l] - yavg);
                        fsum += Math.Pow(parameters[param.Num - 1].Observations[l] - xavr, 2);
                        ssum += Math.Pow(y.Observations[l] - yavg, 2);
                    }
                    down = Math.Pow(fsum * ssum, 0.5);
                    r = up / down;
                    multsParam.Add(r);
                }
                mults.Add(multsParam);
            }
            return mults;
        }

        /// <summary>
        /// Выявление значимых моделей эмпирическим подходом.
        /// </summary>
        /// <param name="models"></param>
        /// <param name="linearPearson"></param>
        /// <param name="threshold"></param>

        List<Factor> SegmentTransform(List<Factor> x)
        {
            var k = 0.0;
            for(int i=0; i < x.Count; i++)
            {
                k = 100.0 / (x[i].Observations.Max() - x[i].Observations.Min());
                for(int j = 0; j < x[i].Observations.Count;j++)
                {
                    x[i].Observations[j] = k * (x[i].Observations[j] - x[i].Observations.Min()) + 2;
                }
            }
            return x;
        }

        void FuncPreprocessingOfStatData(int index, List<Factor> _x, Factor y, ref List<Factor> tempo)
        {
            var x = SegmentTransform(_x);
            var corCoeff = 0.0;
            int funcCount = 16;
            
            List<double> TableAlfa = new()
            {
                0.001,
                -0.001,
                0.01,
                -0.01,
                0.1,
                -0.1,
                0.5,
                -0.5,
                1.0,
                -1.0,
                1.5,
                -1.5,
                2.0,
                -2.0,
                2.5,
                -2.5,
                3.0,
                -3.0,
                3.5,
                -3.5,
                4.0,
                -4.0,
                4.5,
                -4.5,
                5.0,
                -5.0,
                6.0,
                -6.0,
                7.0,
                -7.0,
                8.0,
                -8.0,
                9.0,
                -9.0,
                10.0,
                -10.0
            };

            List<Factor> tempFuncMax = _x;

            List<List<double>> funcResult = new List<List<double>>();

            List<string> funcResultName = new List<string>();

            corCoeff = Correlation.Pearson(x[index].Observations, y.Observations);

            for (int funcIndex = 1; funcIndex <= funcCount; funcIndex++)
            {
                if (funcIndex < 9)
                {
                    List<double> func = new List<double>();
                    for (int j = 0; j < x[index].Observations.Count; j++)
                    {
                        switch (funcIndex)
                        {
                            case 1:
                                func.Add(Math.Pow(x[index].Observations[j], 2));
                                break;
                            case 2:
                                func.Add(Math.Pow(x[index].Observations[j], 3));
                                break;
                            case 3:
                                func.Add(Math.Pow(x[index].Observations[j], -1));
                                break;
                            case 4:
                                func.Add(Math.Pow(x[index].Observations[j], -2));
                                break;
                            case 5:
                                func.Add(Math.Pow(x[index].Observations[j], -3));
                                break;
                            case 6:
                                func.Add(Math.Pow(x[index].Observations[j], 1 / 3));
                                break;
                            case 7:
                                func.Add(Math.Log(x[index].Observations[j]));
                                break;
                            case 8:
                                func.Add(Math.Pow(x[index].Observations[j], 1 / 2));
                                break;
                        }
                    }
                    funcResult.Add(func);
                    switch (funcIndex)
                    {
                        case 1:
                            funcResultName.Add("x^2");
                            break;
                        case 2:
                            funcResultName.Add("x^3");
                            break;
                        case 3:
                            funcResultName.Add("x^-1");
                            break;
                        case 4:
                            funcResultName.Add("x^-2");
                            break;
                        case 5:
                            funcResultName.Add("x^-3");
                            break;
                        case 6:
                            funcResultName.Add("x^1/3");
                            break;
                        case 7:
                            funcResultName.Add("lnx");
                            break;
                        case 8:
                            funcResultName.Add("x^1/2");
                            break;
                    }
                    //corCoeffFunc = Correlation.Pearson(func, y.Observations);
                    //if (Math.Abs(corCoeffFunc) > Math.Abs(maxCorCoeffFunc))
                    //{
                    //    maxCorCoeffFunc = Math.Abs(corCoeffFunc);
                    //    tempFuncMax[index].Observations = func;
                    //}
                }
                else
                {
                    for (int alfaIndex = 0; alfaIndex < TableAlfa.Count; alfaIndex++)
                    {
                        List<double> func = new List<double>();
                        for (int j = 0; j < x[index].Observations.Count; j++)
                        {
                            switch (funcIndex)
                            {
                                case 9:
                                    func.Add(Math.Sin(x[index].Observations[j] * TableAlfa[alfaIndex]));
                                    break;
                                case 10:
                                    func.Add(Math.Tan(x[index].Observations[j] * TableAlfa[alfaIndex]));
                                    break;
                                case 11:
                                    func.Add(Math.Atan(x[index].Observations[j] * TableAlfa[alfaIndex]));
                                    break;
                                case 12:
                                    func.Add(Math.Exp(x[index].Observations[j] * TableAlfa[alfaIndex]));
                                    break;
                                case 13:
                                    func.Add(Math.Exp(x[index].Observations[j] * Math.Pow(TableAlfa[alfaIndex], 2)));
                                    break;
                                case 14:
                                    func.Add(1.0 / (1 - (Math.Exp(x[index].Observations[j] * Math.Pow(TableAlfa[alfaIndex], 1)))));
                                    break;
                                case 15:
                                    func.Add((Math.Exp(x[index].Observations[j] * TableAlfa[alfaIndex]) - 1) / (Math.Exp(x[index].Observations[j] * TableAlfa[alfaIndex]) + 1));
                                    break;
                                case 16:
                                    func.Add((Math.Exp(x[index].Observations[j] * TableAlfa[alfaIndex]) - Math.Exp(x[index].Observations[j] * (-TableAlfa[alfaIndex]))) / 2.0);
                                    break;
                            }
                        }
                        funcResult.Add(func);
                        switch (funcIndex)
                        {
                            case 9:
                                funcResultName.Add("sin"+ TableAlfa[alfaIndex].ToString() + "x");
                                break;
                            case 10:
                                funcResultName.Add("tg" + TableAlfa[alfaIndex].ToString() + "x");
                                break;
                            case 11:
                                funcResultName.Add("arctg" + TableAlfa[alfaIndex].ToString() + "x");
                                break;
                            case 12:
                                funcResultName.Add("e^" + TableAlfa[alfaIndex].ToString() + "x");
                                break;
                            case 13:
                                funcResultName.Add("e^" + TableAlfa[alfaIndex].ToString() + "(x^2)");
                                break;
                            case 14:
                                funcResultName.Add("1/(1+e^" + TableAlfa[alfaIndex].ToString() + "x)");
                                break;
                            case 15:
                                funcResultName.Add("(e^" + TableAlfa[alfaIndex].ToString() + "x-1)/(e^" + TableAlfa[alfaIndex].ToString() + "x+1)");
                                break;
                            case 16:
                                funcResultName.Add("(e^" + TableAlfa[alfaIndex].ToString() + "x-e^-" + TableAlfa[alfaIndex].ToString() + "x)/2");
                                break;
                        }
                    }
                }
            }

            List<double> pearsonLst = new List<double>();
            for(int i = 0; i < funcResult.Count; i++)
            {
                pearsonLst.Add(Correlation.Pearson(funcResult[i], y.Observations));
            }
            var max = pearsonLst.Max();
            var maxName = funcResultName[pearsonLst.LastIndexOf(max)];

            List<Factor> temp = _x;
            temp[index].Name += " " + maxName;
            temp[index].Observations = funcResult[pearsonLst.LastIndexOf(max)];
            if (Math.Abs(max) > (Math.Abs(corCoeff) + 0.01))
            {
                FuncPreprocessingOfStatData(index, temp, y, ref tempo);
            }
            else
            {
                tempo.Add(temp[index]);
            }
        }

        void EmpiricalMethod(ref List<List<Factor>> models, ref List<List<double>> linearPearson, double threshold)
        {
            var max = double.MinValue;
            var pos = 0;
            for (int i = 0; i < linearPearson.Count; i++)
            {
                var sum = Queryable.Sum(linearPearson[i].AsQueryable());
                if (sum > max)
                {
                    max = sum;
                    pos = i;
                }
            }
            var modelOne = models[pos];
            var linearPearsonOne = linearPearson[pos];
            for (int i = 0; i < models.Count; i++)
            {
                for (int j = 0; j < linearPearson[i].Count; j++)
                {
                    if (Math.Abs(linearPearson[i][j]) < threshold)
                    {
                        models[i].RemoveAt(j);
                        linearPearson[i].RemoveAt(j);
                        j--;
                    }
                }
                if (models[i].Count == 0)
                {
                    models.RemoveAt(i);
                    linearPearson.RemoveAt(i);
                    i--;
                    OnPropertyChanged(nameof(CurrentCount));
                }
            }
            if (models.Count == 0)
            {
                models.Add(modelOne);
                linearPearson.Add(linearPearsonOne);
            }
        }
        private void ImportanceMethod(ref List<List<Factor>> models, ref List<List<double>> linearPearson)
        {
            var min = double.MaxValue;
            List<Factor> modelOne = models[0];
            List<double> linearPearsonOne = linearPearson[0];

            for (int i = 0; i < models.Count; i++)
            {
                var sum = 0.0;
                var model = models[i];
                var linPearson = linearPearson[i];
                for (int j = 0; j < linearPearson[i].Count; j++)
                {
                    var Tob = linearPearson[i][j] * Math.Sqrt((models[i][j].Observations.Count - 2) / (1 - Math.Pow(linearPearson[i][j], 2)));
                    var p = 2 * (1 - Tob * StudentCo);
                    sum += p;
                    if (p >= 0.05)
                    {
                        models[i].RemoveAt(j);
                        linearPearson[i].RemoveAt(j);
                        j--;
                    }
                }
                if (sum < min)
                {
                    modelOne = model;
                    linearPearsonOne = linPearson;
                }
                if (models[i].Count == 0)
                {
                    models.RemoveAt(i);
                    linearPearson.RemoveAt(i);
                    i--;
                    OnPropertyChanged(nameof(CurrentCount));
                }
            }
            if (models.Count == 0)
            {
                models.Add(modelOne);
                linearPearson.Add(linearPearsonOne);
            }
        }
        #endregion

        #region Stage 3
        /// <summary>
        /// Получение векторов-столбцов (B) коэффициентов для моделей.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="models"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static (List<List<double>>, List<List<double>>) GetRegressionMult(List<List<Factor>> models, Factor y)
        {
            var bs = new List<List<double>>();
            var es = new List<List<double>>();
            for (int i = 0; i < models.Count; i++)
            {
                var B = new List<double>();
                var ZT = new List<List<double>>();
                var single = Enumerable.Repeat(1.0, models[0][0].Observations.Count).ToList();
                ZT.Add(single);
                for (int j = 0; j < models[i].Count; j++)
                {
                    ZT.Add(models[i][j].Observations);
                }

                var Z = new List<List<double>>();
                for (int j = 0; j < ZT[0].Count; j++)
                {
                    var ZTcol = new List<double>();
                    for (int k = 0; k < ZT.Count; k++)
                    {
                        ZTcol.Add(ZT[k][j]);
                    }
                    Z.Add(ZTcol);
                }

                var yMatrix = new List<List<double>>();
                foreach (var item in y.Observations)
                {
                    var m = new List<double>
                    {
                        item
                    };
                    yMatrix.Add(m);
                }
                var temp = MultMatrix(MultMatrix(GetReverseMatrix(MultMatrix(ZT, Z)), ZT), yMatrix);
                foreach (var item in temp)
                {
                    B.Add(item[0]);
                }

                es.Add(CalculateError(temp, Z, yMatrix));
                bs.Add(B);
            }
            return (bs, es);
        }
        /// <summary>
        /// Расчет матрицы-вектора e.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="z"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static List<double> CalculateError(List<List<double>> b, List<List<double>> z, List<List<double>> y)
        {
            var e = new List<double>();
            var zb = MultMatrix(z, b);
            for (int i = 0; i < y.Count; i++)
                e.Add(y[i][0] - zb[i][0]);
            return e;
        }
        /// <summary>
        /// Умножение матрицы m1 на m2.
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns>Произведение матриц m1 и m2.</returns>
        static List<List<double>> MultMatrix(List<List<double>> m1, List<List<double>> m2)
        {
            var m3 = new List<List<double>>();
            for (int i = 0; i < m1.Count; i++)
            {
                var list = new List<double>();
                for (int j = 0; j < m2[0].Count; j++)
                {
                    list.Add(0.0);
                }
                m3.Add(list);
            }

            for (int i = 0; i < m1.Count; i++)
            {
                for (int j = 0; j < m2[0].Count; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < m2.Count; k++)
                        sum += m1[i][k] * m2[k][j];
                    m3[i][j] = sum;
                }
            }
            return m3;
        }
        /// <summary>
        /// Получение обратной матрицы.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        static List<List<double>> GetReverseMatrix(List<List<double>> m)
        {
            double temp;

            var E = new List<List<double>>();

            for (int i = 0; i < m.Count; i++)
            {
                E.Add(new List<double>());
                for (int j = 0; j < m.Count; j++)
                {
                    if (i == j)
                        E[i].Add(1.0);
                    else
                        E[i].Add(0.0);
                }
            }


            for (int k = 0; k < m.Count; k++)
            {
                temp = m[k][k];

                for (int j = 0; j < m.Count; j++)
                {
                    m[k][j] /= temp;
                    E[k][j] /= temp;
                }

                for (int i = k + 1; i < m.Count; i++)
                {
                    temp = m[i][k];

                    for (int j = 0; j < m.Count; j++)
                    {
                        m[i][j] -= m[k][j] * temp;
                        E[i][j] -= E[k][j] * temp;
                    }
                }
            }

            for (int k = m.Count - 1; k > 0; k--)
            {
                for (int i = k - 1; i >= 0; i--)
                {
                    temp = m[i][k];

                    for (int j = 0; j < m.Count; j++)
                    {
                        m[i][j] -= m[k][j] * temp;
                        E[i][j] -= E[k][j] * temp;
                    }
                }
            }

            for (int i = 0; i < m.Count; i++)
                for (int j = 0; j < m.Count; j++)
                    m[i][j] = E[i][j];

            return m;
        }
        #endregion

        #region Stage 4
        private void CheckAdequacyOfModels(ref (List<List<double>>, List<List<double>>) bE)
        {
            var min = double.MaxValue;
            var modelOne = Models[0];
            var bOne = bE.Item1[0];
            var eOne = bE.Item2[0];

            var obs = (int)(bE.Item2[0].Count * 0.0027);

            for (int i = 0; i < Models.Count; i++)
            {
                //Второе условие
                var check = true;
                var avgErr = Queryable.Average(bE.Item2[i].AsQueryable());
                var dev = Math.Sqrt(Queryable.Sum(bE.Item2[i].AsQueryable(), x => Math.Pow(avgErr - x, 2)) / (bE.Item2.Count - 1));
                var As = GetU(bE.Item2[i], 3, avgErr) / Math.Pow(dev, 3);
                var Ee = GetU(bE.Item2[i], 4, avgErr) / Math.Pow(dev, 4) - 3;
                if (Math.Abs(As) > 1 || Math.Abs(Ee) > 1)
                    check = false;

                //Третье условие
                var low = avgErr - 3 * dev;
                var high = avgErr + 3 * dev;
                var outer = bE.Item2[i].Where(x => x <= low && x >= high).Count();
                if (outer > obs)
                    check = false;

                var sum = As + Ee + outer;
                if (sum < min)
                {
                    min = sum;
                    modelOne = Models[i];
                    bOne = bE.Item1[i];
                    eOne = bE.Item2[i];
                }

                if (!check)
                {
                    Models.RemoveAt(i);
                    bE.Item1.RemoveAt(i);
                    bE.Item2.RemoveAt(i);
                    OnPropertyChanged(nameof(CurrentCount));
                    i--;
                }
            }
            if (Models.Count == 0)
            {
                Models.Add(modelOne);
                bE.Item1.Add(bOne);
                bE.Item2.Add(eOne);
            }
        }
        static double GetU(List<double> e, int f, double avg)
        {
            return Queryable.Average(e.AsQueryable(), x => Math.Pow(x - avg, f));
        }
        #endregion

        #region Stage 5
        private List<double> GetDeterminationMult(ref (List<List<double>>, List<List<double>>) bE, Factor y)
        {
            var modelOne = Models[0];
            var bOne = bE.Item1[0];
            var eOne = bE.Item2[0];
            var max = double.MinValue;

            var r = new List<double>();
            for (int i = 0; i < Models.Count; i++)
            {
                var yAvg = Queryable.Average(y.Observations.AsQueryable());
                var rSqr = 1 - Queryable.Sum(bE.Item2[i].AsQueryable(), x => Math.Pow(x, 2)) /
                    Queryable.Sum(y.Observations.AsQueryable(), x => Math.Pow(x - yAvg, 2));
                var F = rSqr / (1 - rSqr) * (Models[i][0].Observations.Count - Models[i].Count - 1) / Models[i].Count;
                if (F > max)
                {
                    modelOne = Models[i];
                    bOne = bE.Item1[i];
                    eOne = bE.Item2[i];
                    max = F;
                }
                if (F <= TableF[Models[i].Count - 1])
                {
                    Models.RemoveAt(i);
                    bE.Item1.RemoveAt(i);
                    bE.Item2.RemoveAt(i);
                    i--;
                    OnPropertyChanged(nameof(CurrentCount));
                    continue;
                }
                r.Add(rSqr);
            }
            if (Models.Count == 0)
            {
                Models.Add(modelOne);
                bE.Item1.Add(bOne);
                bE.Item2.Add(eOne);
            }
            return r;
        }
        #endregion

        #region Stage 7
        private static List<double> GetVariations(List<List<Factor>> models, (List<List<double>>, List<List<double>>) bE, Factor y)
        {
            var vs = new List<double>();
            for (int i = 0; i < models.Count; i++)
            {
                var ds = new List<double>();
                for (int k = 77; k <= models[0][0].Observations.Count; k += 77)
                {
                    var yAvg = Queryable.Average(y.Observations.AsQueryable());
                    var rSqr = 1 - Queryable.Sum(bE.Item2[i].Take(k).AsQueryable(), x => x * x) /
                        Queryable.Sum(y.Observations.AsQueryable(), x => Math.Pow(x - yAvg, 2));
                    var rRSqr = 1 - (k - 1) / (k - models[i].Count * (1 - rSqr));
                    ds.Add(rRSqr);
                }
                var dAvg = Queryable.Average(ds.AsQueryable());
                var div = Math.Sqrt(Queryable.Sum(ds.AsQueryable(), x => Math.Pow(dAvg - x, 2)) / (models[i].Count - 1));
                var v = div / dAvg;
                vs.Add(v);
            }
            return vs;
        }
        #endregion
        #endregion
    }
}
