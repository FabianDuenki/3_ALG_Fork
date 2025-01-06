using static System.Net.Mime.MediaTypeNames;

namespace AlgAdv.MB01_03.Exercise.ComplexityTests {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            var tests = new Tests();
            var n = Convert.ToInt32(this.numericUpDown1.Value);
            var res = tests.DoAllTests(n);

            this.textBox1.Text = res.ToString();
        }
    }
}