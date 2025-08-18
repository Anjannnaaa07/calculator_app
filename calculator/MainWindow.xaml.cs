using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace calculator
{
    public partial class MainWindow : Window
    {
        private string eqn = string.Empty;

        private readonly IArithematicOperations<int> intOps;
        private readonly IArithematicOperations<decimal> decOps;

        private Dictionary<char, Func<int, int, int>> intOperationMap;
        private Dictionary<char, Func<decimal, decimal, decimal>> decOperationMap;

        public MainWindow()
        {
            InitializeComponent();
            intOps = new IntArithmeticOperations();
            decOps = new DecimalArithmeticOperations();
            InitializeOpMapping();
        }

        private void InitializeOpMapping()
        {
            intOperationMap = new Dictionary<char, Func<int, int, int>>()
            {
                { '+', intOps.Add },
                { '-', intOps.Subtract },
                { '*', intOps.Multiply },
                { '/', intOps.Divide },
            };

            decOperationMap = new Dictionary<char, Func<decimal, decimal, decimal>>()
            {
                { '+', decOps.Add },
                { '-', decOps.Subtract },
                { '*', decOps.Multiply },
                { '/', decOps.Divide },
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            eqn += btn.Content.ToString();
            TextRes.Text = eqn;
        }

        private void DivideButton_Click(object sender, RoutedEventArgs e) => AppendOperator('/');
        private void MultiplyButton_Click(object sender, RoutedEventArgs e) => AppendOperator('*');
        private void SubButton_Click(object sender, RoutedEventArgs e) => AppendOperator('-');
        private void AddButton_Click(object sender, RoutedEventArgs e) => AppendOperator('+');
        private void DecimalButton_Click(object sender, RoutedEventArgs e) => AppendOperator('.');

        private void AppendOperator(char op)
        {
            eqn += op;
            TextRes.Text = eqn;
        }

        private void EqualsTo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string result = EvaluateEquation(eqn);
                TextRes.Text = eqn + "=" + result;
                eqn = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            eqn = String.Empty;
            TextRes.Clear();
        }

        private string EvaluateEquation(string eqn)
        {
            string[] parts = eqn.Split(new char[] { '+', '-', '*', '/' });

            // Detect decimal mode
            bool isDecimal = eqn.Contains(".");

            char op = eqn[parts[0].Length];

            if (isDecimal)
            {
                decimal first = Decimal.Parse(parts[0]);
                decimal second = Decimal.Parse(parts[1]);

                if (op == '/' && second == 0)
                    throw new DivideByZeroException("Division by zero not possible.");

                if (decOperationMap.TryGetValue(op, out var operation))
                    return operation(first, second).ToString();
            }
            else
            {
                int first = Int32.Parse(parts[0]);
                int second = Int32.Parse(parts[1]);

                if (op == '/' && second == 0)
                    throw new DivideByZeroException("Division by zero not possible.");

                if (intOperationMap.TryGetValue(op, out var operation))
                    return operation(first, second).ToString();
            }

            throw new InvalidOperationException("Invalid operation.");
        }
    }
}
