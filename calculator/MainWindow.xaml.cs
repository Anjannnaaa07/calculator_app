using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string eqn = string.Empty;
        IArithematicOperations operations;
        Dictionary<char, Func<int, int, int>> operationMap;

        public MainWindow()
        {
            InitializeComponent();
            operations = new ArithmeticOperations();
            InitializeOpMapping();
        }

        private void InitializeOpMapping()
        {
            operationMap = new Dictionary<char, Func<int, int, int>>()
            {
                { '+', operations.Add },
                { '-', operations.Subtract },
                { '*', operations.Multiply },
                { '/', operations.Divide },
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            eqn += btn.Content.ToString();
            TextRes.Text = eqn;
        }

        private void DivideButton_Click(object sender, RoutedEventArgs e)
        {
            eqn += '/';
            TextRes.Text = eqn;
        }

        private void MultiplyButton_Click(object sender, RoutedEventArgs e)
        {
            eqn += '*';
            TextRes.Text = eqn;
        }

        private void SubButton_Click(object sender, RoutedEventArgs e)
        {
            eqn += '-';
            TextRes.Text = eqn;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            eqn += '+';
            TextRes.Text = eqn;
        }

        private void EqualsTo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int result = EvaluateEquation(eqn);
                TextRes.Text = eqn + "=" + result.ToString();
                eqn = result.ToString(); 
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
        private int EvaluateEquation(string eqn)
        {
            string[] parts = eqn.Split(new char[] { '+', '-', '*', '/' });
            int first = Int32.Parse(parts[0]);
            int second = Int32.Parse(parts[1]);
            char op = eqn[parts[0].Length];
            if (op == '/' && second == 0)
            {
                throw new DivideByZeroException("Division by zero not possible.");
            }

            if (operationMap.TryGetValue(op, out var operation))
            {
                return operation(first, second);
            }
            else
            {
                throw new InvalidOperationException("Invalid operation.");
            }
        }
    }
}
