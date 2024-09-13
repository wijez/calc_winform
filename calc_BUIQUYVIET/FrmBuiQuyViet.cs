using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private string previousExpression = "";
        private bool isNewCalculation = true;
        List<string> calculationHistory = new List<string>();

        private object EvaluateExpression(string expression)
        {
            DataTable table = new DataTable();
            return table.Compute(expression, string.Empty);
        }

        private void MakeButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;

        }

        private void MakeButtonRound(Button btn)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, btn.Width, btn.Height);
            btn.Region = new Region(path);
            btn.BackColor = Color.FromArgb(0, 0, 0);
            btn.ForeColor = Color.AliceBlue;

            btn.Font = new Font("Arial", 12, FontStyle.Bold);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
        }

        private string GetLastOperator(string expression, out string lastNumber)
        {
            char[] operators = { '+', '-', '*', '/' };
            lastNumber = string.Empty;

            foreach (char op in operators)
            {
                int index = expression.LastIndexOf(op);
                if (index != -1)
                {
                    lastNumber = expression.Substring(index + 1).Trim();
                    return op.ToString();
                }
            }

            lastNumber = expression.Trim();
            return "+";
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

            MakeButton(btnHistory);
            MakeButton(btnClearHistory);
            MakeButton(btnDelete);
            MakeButton(btnMaxnimize);
            MakeButton(btnMinimize);
            MakeButton(btnClose);
            MakeButton(btnMenu);

        }

        private void buttonsquare_Click(object sender, EventArgs e)
        {
            string currentExpression = txtDisplay.Text;

            int openBrackets = currentExpression.Count(c => c == '(');
            int closeBrackets = currentExpression.Count(c => c == ')');

            // Kiểm tra ký tự cuối cùng
            bool lastCharIsOperator = currentExpression.Length > 0 &&
                                      "+-*/%".Contains(currentExpression.Last());

            // Thêm dấu ngoặc mở chỉ nếu ký tự cuối là toán tử hoặc biểu thức rỗng
            if (lastCharIsOperator || currentExpression.Length == 0)
            {
                txtDisplay.Text += "(";
            }
            // Thêm dấu ngoặc đóng nếu ký tự cuối là số hoặc dấu ngoặc đóng
            else if (currentExpression.Length > 0 &&
                     (char.IsDigit(currentExpression.Last()) || currentExpression.Last() == ')'))
            {
                // Chỉ thêm dấu ngoặc đóng nếu số lượng dấu ngoặc mở lớn hơn số lượng dấu ngoặc đóng
                if (openBrackets > closeBrackets)
                {
                    txtDisplay.Text += ")";
                }
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
        }
        private void buttonequal_Click(object sender, EventArgs e)
        {
            try
            {
                string expression = txtDisplay.Text;

                if (expression.Contains("%"))
                {
                    expression = expression.Replace("%", "").Trim();
                    decimal value = Convert.ToDecimal(expression) / 100;
                    txtDisplay1.Text = FormatNumber(value);
                    txtDisplay.Text = value.ToString();
                    previousResult = Convert.ToDouble(value);
                    calculationHistory.Add($"{expression}% = {value}");
                    isNewCalculation = true;
                    return;
                }
                expression = expression.Replace(',', '.');

                if (isNewCalculation)
                {
                    // Retrieve last operator and last number
                    string lastOperator = GetLastOperator(previousExpression, out string lastNumber);

                    // Construct the new expression using previous result
                    expression = $"{previousResult} {lastOperator} {lastNumber}";
                }
                else
                {
                    // Store the current expression for future use
                    previousExpression = expression;
                }

                object result = EvaluateExpression(expression);
                if (result != null)
                {

                    txtDisplay1.Text = FormatNumber(Convert.ToDecimal(result));
                    txtDisplay.Text = result.ToString();

                    previousResult = Convert.ToDouble(result);
                    calculationHistory.Add($"{expression} = {result}");

                    if (listBoxHistory.Visible)
                    {
                        listBoxHistory.Items.Add($"{expression} = {result}");
                    }
                    isNewCalculation = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private string FormatNumber(decimal number)
        {
            return number.ToString("#,0.##");
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string buttonText = button.Text;

            if (txtDisplay.Text.Length > 0)
            {
                char lastChar = txtDisplay.Text[txtDisplay.Text.Length - 1];

                if ("*/+%".Contains(lastChar) && "*/+%".Contains(buttonText))
                {
                    return;
                }
            }

            txtDisplay.Text += buttonText;
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

                if (!lastPart.Contains("."))
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

            listBoxHistory.Visible = !listBoxHistory.Visible;

            btnClearHistory.Visible = listBoxHistory.Visible;

            if (listBoxHistory.Visible)
            {
                listBoxHistory.Items.Clear();
                foreach (var historyItem in calculationHistory)
                {
                    listBoxHistory.Items.Add(historyItem);
                }
            }

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
            if (txtDisplay.Text.Length > 1)
            {
                txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.Text.Length - 1);
            }
            else
            {
                txtDisplay.Clear();
                txtDisplay1.Clear();
            }
        }

        private void buttonpercent_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtDisplay1.Text))
                {
                    decimal value = Convert.ToDecimal(txtDisplay1.Text);
                    value /= 100;

                    txtDisplay.Text = value.ToString();
                }
            }
            catch
            {
                MessageBox.Show("Invalid operation");
            }
        }


        private void buttonMulti_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtDisplay.Text))
                {
                    if (txtDisplay.Text.StartsWith("-"))
                    {
                        txtDisplay.Text = txtDisplay.Text.Substring(1);
                    }
                    else
                    {
                        txtDisplay.Text = "-" + txtDisplay.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
            }
        }

        private void btnClearHistory_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa lịch sử không?",
                                           "Xác nhận",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                calculationHistory.Clear();
                listBoxHistory.Items.Clear();
            }
        }

        private void listBoxHistory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pnlHistory_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
