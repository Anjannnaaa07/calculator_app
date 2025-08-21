using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace calculator
{
    public partial class MainWindow : Window
    {
        private string Equation = string.Empty;
        private Dictionary<char, Func<decimal, decimal, decimal>> DecimalOperationMap;

        private readonly IArithematicOperations<decimal> DecimalOperations;
        public MainWindow()
        {
            InitializeComponent();
            DecimalOperations = new DecimalArithmeticOperations();
            InitializeOperationMapping();
        }

        private void InitializeOperationMapping()
        {
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
        private string EvaluateEquation(string equation)
        {
            if (string.IsNullOrWhiteSpace(equation))
                throw new ArgumentException("Equation cannot be empty.");

            try
            {
                // Convert to postfix (RPN)
                var postfix = InfixToPostfix(equation);

                // Evaluate postfix
                decimal result = EvaluatePostfix(postfix);

                return result % 1 == 0 ? ((int)result).ToString() : result.ToString();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Invalid equation: {ex.Message}");
            }
        }
        private List<string> InfixToPostfix(string infix)
        {
            var output = new List<string>();
            var operators = new Stack<char>();
            string numberBuffer = "";

            int Precedence(char op) => (op == '+' || op == '-') ? 1 : 2;

            foreach (char c in infix)
            {
                if (char.IsDigit(c) || c == '.')
                {
                    numberBuffer += c; // build number
                }
                else if ("+-*/".Contains(c))
                {
                    if (numberBuffer.Length > 0)
                    {
                        output.Add(numberBuffer);
                        numberBuffer = "";
                    }

                    while (operators.Count > 0 && Precedence(operators.Peek()) >= Precedence(c))
                    {
                        output.Add(operators.Pop().ToString());
                    }

                    operators.Push(c);
                }
            }

            if (numberBuffer.Length > 0)
                output.Add(numberBuffer);

            while (operators.Count > 0)
                output.Add(operators.Pop().ToString());

            return output;
        }
        private decimal EvaluatePostfix(List<string> postfix)
        {
            var stack = new Stack<decimal>();

            foreach (var token in postfix)
            {
                if (decimal.TryParse(token, out decimal number))
                {
                    stack.Push(number);
                }
                else if (token.Length == 1 && "+-*/".Contains(token))
                {
                    decimal b = stack.Pop();
                    decimal a = stack.Pop();

                    if (!DecimalOperationMap.TryGetValue(token[0], out var op))
                        throw new InvalidOperationException("Invalid operator.");

                    stack.Push(op(a, b));
                }
                else
                {
                    throw new InvalidOperationException($"Invalid token '{token}' in expression.");
                }
            }

            return stack.Pop();
        }
    }
}
