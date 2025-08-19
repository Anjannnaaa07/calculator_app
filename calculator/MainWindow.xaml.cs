using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace calculator
{
    public partial class MainWindow : Window
    {
        private string Equation = string.Empty;

        private Dictionary<char, Func<int, int, int>> IntegerOperationMap;
        private Dictionary<char, Func<decimal, decimal, decimal>> DecimalOperationMap;

        private readonly IArithematicOperations<int> IntegerOperations;
        private readonly IArithematicOperations<decimal> DecimalOperations;
        public MainWindow()
        {
            InitializeComponent();
            IntegerOperations = new IntArithmeticOperations();
            DecimalOperations = new DecimalArithmeticOperations();
            InitializeOpMapping();
        }

        private void InitializeOpMapping()
        {
            IntegerOperationMap = new Dictionary<char, Func<int, int, int>>()
            {
                { '+', IntegerOperations.Add },
                { '-', IntegerOperations.Subtract },
                { '*', IntegerOperations.Multiply },
                { '/', IntegerOperations.Divide },
            };

            DecimalOperationMap = new Dictionary<char, Func<decimal, decimal, decimal>>()
            {
                { '+', DecimalOperations.Add },
                { '-', DecimalOperations.Subtract },
                { '*', DecimalOperations.Multiply },
                { '/', DecimalOperations.Divide },
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button Button = (Button)sender;
            Equation += Button.Content.ToString();
            TextRes.Text = Equation;
        }

        private void DivideButton_Click(object sender, RoutedEventArgs e) => AppendOperator('/');
        private void MultiplyButton_Click(object sender, RoutedEventArgs e) => AppendOperator('*');
        private void SubButton_Click(object sender, RoutedEventArgs e) => AppendOperator('-');
        private void AddButton_Click(object sender, RoutedEventArgs e) => AppendOperator('+');
        private void DecimalButton_Click(object sender, RoutedEventArgs e) => AppendOperator('.');

        private void AppendOperator(char Operator)
        {
            Equation += Operator;
            TextRes.Text = Equation;
        }

        private void EqualsTo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string result = EvaluateEquation(Equation);
                TextRes.Text = Equation + "=" + result;
                Equation = result;
            }
            catch (Exception ex)
            {
                 TextRes.Text = ex.Message;
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Equation = String.Empty;
            TextRes.Clear();
        }

        private string EvaluateEquation(string Equation)
        {
            string[] parts = Equation.Split(new char[] { '+', '-', '*', '/' });

            bool isDecimal = Equation.Contains(".");

            char Operator = Equation[parts[0].Length];

            if (isDecimal)
            {
                decimal FirstOperand = Decimal.Parse(parts[0]);
                decimal SecondOperand = Decimal.Parse(parts[1]);


                if (DecimalOperationMap.TryGetValue(Operator, out var operation))
                    return operation(FirstOperand, SecondOperand).ToString();
            }
            else
            {
                int FirstOperand = Int32.Parse(parts[0]);
                int SecondOperand = Int32.Parse(parts[1]);

                if (IntegerOperationMap.TryGetValue(Operator, out var operation))
                    return operation(FirstOperand, SecondOperand).ToString();
            }

            throw new InvalidOperationException("Invalid operation.");
        }
    }
}
