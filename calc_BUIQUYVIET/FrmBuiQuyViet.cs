using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace calc_BUIQUYVIET
{
    public partial class FrmBuiQuyViet : Form
    {
        public FrmBuiQuyViet()
        {
            InitializeComponent();

        }
        private double previousResult = 0; 
        private bool isNewCalculation = true;
        private int clickCount = 0;
        List<string> calculationHistory = new List<string>();

        private object EvaluateExpression(string expression)
        {
            DataTable table = new DataTable();
            return table.Compute(expression, string.Empty);
        }


        private void MakeButtonRound(Button btn)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, btn.Width, btn.Height);
            btn.Region = new Region(path);
            btn.BackColor = Color.FromArgb(0, 0, 0);
            btn.ForeColor = Color.AliceBlue;

            btn.Font = new Font("Arial", 12, FontStyle.Bold);
            btn.FlatStyle = FlatStyle.Flat; // Hoặc FlatStyle.Popup
            btn.FlatAppearance.BorderSize = 0;
        }

        private void txtDisplay_TextChanged(object sender, EventArgs e)
        {
            isNewCalculation = false;

        }

        private void FrmBuiQuyViet_Load_1(object sender, EventArgs e)
        {

            MakeButtonRound(button0);
            MakeButtonRound(button1);
            MakeButtonRound(button2);
            MakeButtonRound(button3);
            MakeButtonRound(button4);
            MakeButtonRound(button5);
            MakeButtonRound(button6);
            MakeButtonRound(button7);
            MakeButtonRound(button8);
            MakeButtonRound(button9);
            MakeButtonRound(buttonminus);
            MakeButtonRound(buttonequal);
            MakeButtonRound(buttonplus);
            MakeButtonRound(buttondiv);
            MakeButtonRound(buttonmutilple);
            MakeButtonRound(buttonC);
            MakeButtonRound(buttonpercent);
            MakeButtonRound(buttonsquare);
            MakeButtonRound(buttoncomma);
            MakeButtonRound(buttonMulti);

        }

        private void buttonsquare_Click(object sender, EventArgs e)
        {
            
                clickCount++;

            if (clickCount % 2 == 1)
            {
                txtDisplay.Text += "(";
            }
            else
            {
                txtDisplay.Text += ")";
            }
        }

        private void buttonpercent_Click(object sender, EventArgs e)
        {
            try
            {
                decimal value = Convert.ToDecimal(txtDisplay.Text);
                value /= 100;
                txtDisplay.Text = value.ToString();
            }
            catch
            {
                MessageBox.Show("Invalid operation");
            }
        }


        private void button20_Click(object sender, EventArgs e)
        {

        }

        private void lblResult_Click(object sender, EventArgs e)
        {
            lblResult.Text = sender.ToString();
        }

        private void buttonC_Click(object sender, EventArgs e)
        {
            txtDisplay.Clear();
            txtDisplay1.Clear();
            //lblResult.Text = string.Empty;
        }

        private string InfixToPostfix(string expression)
        {
            Stack<char> operators = new Stack<char>();
            StringBuilder output = new StringBuilder();
            Dictionary<char, int> precedence = new Dictionary<char, int>
    {
        { '+', 1 },
        { '-', 1 },
        { '*', 2 },
        { '/', 2 }
    };

            foreach (char token in expression)
            {
                if (char.IsDigit(token) || token == '.') // Nếu là số hoặc dấu chấm (phân số)
                {
                    output.Append(token);
                }
                else if (token == '(')
                {
                    operators.Push(token);
                }
                else if (token == ')')
                {
                    while (operators.Count > 0 && operators.Peek() != '(')
                    {
                        output.Append(' ').Append(operators.Pop());
                    }
                    operators.Pop(); // Loại bỏ dấu '(' khỏi ngăn xếp
                }
                else if (precedence.ContainsKey(token))
                {
                    output.Append(' '); // Thêm dấu cách trước khi thêm toán tử
                    while (operators.Count > 0 && precedence.ContainsKey(operators.Peek()) && precedence[operators.Peek()] >= precedence[token])
                    {
                        output.Append(operators.Pop()).Append(' ');
                    }
                    operators.Push(token);
                }
            }

            while (operators.Count > 0)
            {
                output.Append(' ').Append(operators.Pop());
            }

            return output.ToString();
        }

        private double EvaluatePostfix(string postfixExpression)
        {
            Stack<double> stack = new Stack<double>();
            string[] tokens = postfixExpression.Split(' ');

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out double number))
                {
                    stack.Push(number);
                }
                else
                {
                    double operand2 = stack.Pop();
                    double operand1 = stack.Pop();

                    switch (token)
                    {
                        case "+":
                            stack.Push(operand1 + operand2);
                            break;
                        case "-":
                            stack.Push(operand1 - operand2);
                            break;
                        case "*":
                            stack.Push(operand1 * operand2);
                            break;
                        case "/":
                            stack.Push(operand1 / operand2);
                            break;
                    }
                }
            }

            return stack.Pop();
        }

        private void buttonequal_Click(object sender, EventArgs e)
        {
            try
            {
                string expression;

                if (isNewCalculation)
                {
                    expression = txtDisplay.Text;
                }
                else
                {
                    expression = previousResult.ToString() + txtDisplay.Text;
                }

                string postfixExpression = InfixToPostfix(expression);

                double result = EvaluatePostfix(postfixExpression);

                txtDisplay1.Text = FormatNumber((decimal)result);

                txtDisplay.Text = result.ToString();

                previousResult = result;

                calculationHistory.Add($"{expression} = {result}");

                isNewCalculation = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Biểu thức không hợp lệ: " + ex.Message);
            }
        }

       

        private string FormatNumber(decimal number)
        {
            return number.ToString("#,0.##"); // Định dạng số với dấu phẩy
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (isNewCalculation)
            {
                txtDisplay.Clear(); 
                isNewCalculation = false;
            }

            txtDisplay.Text += button.Text;
        }


        private void buttoncomma_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDisplay.Text))
            {
                txtDisplay.Text = "0.";
            }
            else
            {
                string currentExpression = txtDisplay.Text;

                string[] parts = currentExpression.Split(new char[] { '+', '-', '*', '/' }, StringSplitOptions.RemoveEmptyEntries);
                string lastPart = parts.Last();

                if (!lastPart.Contains(",") && !lastPart.Contains("."))
                {
                    txtDisplay.Text += ".";
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnMenu_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            string history = string.Join(Environment.NewLine, calculationHistory);
            MessageBox.Show(history, "Calculation History");

        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnMaxnimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtDisplay1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtDisplay.Text.Length > 0)
            {
                txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.Text.Length - 1);
            }

        }

        private void buttonpercent_Click_1(object sender, EventArgs e)
        {
            try
            {
                decimal value = Convert.ToDecimal(txtDisplay.Text);
                value /= 100;
                txtDisplay.Text = value.ToString();
            }
            catch
            {
                MessageBox.Show("Invalid operation");
            }
        }


       

        private void buttonplus_Click(object sender, EventArgs e)
        {

        }


        private void buttonminus_Click(object sender, EventArgs e)
        {

        }

        private void buttonMulti_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDisplay.Text))
            {
               if(txtDisplay.Text.StartsWith("-"))
                {
                    txtDisplay.Text = txtDisplay.Text.Substring(1);
                }
                else
                {
                    txtDisplay.Text = "-" + txtDisplay.Text;
                }
            }
        }
    }
}
