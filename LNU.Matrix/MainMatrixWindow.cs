using LNU.Matrix.UtilityClasses;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using NMMP.Triangulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TriangleNet.Geometry;

namespace LNU.Matrix
{
    public partial class MainMatrixWindow : Form
    {
        List<double[,]> Ke;
        List<double[,]> Me;
        List<BasisStorage> Ne;
        List<Matrix<double>> ReMatrix;
        List<Vector<double>> ReVector;
        List<Vector<double>> Qe;
        DataStorage storage;

        Dictionary<int, List<Constants>> conditions = new Dictionary<int, List<Constants>>();

        public MainMatrixWindow()
        {
            InitializeComponent();
            storage = new DataStorage();
            Ke = new List<double[,]>();
            Me = new List<double[,]>();
            Ne = new List<BasisStorage>();
            ReMatrix = new List<Matrix<double>>();
            ReVector = new List<Vector<double>>();
            Qe = new List<Vector<double>>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //DataStorage storage = new DataStorage();
            List<double> squerArea = new List<double>();
            List<EquationStorage> equationStorage = new List<EquationStorage>();

            double a11 = double.Parse(tbA11.Text.Replace('.', ','));
            double a22 = double.Parse(tbA22.Text.Replace('.', ','));
            double a12 = double.Parse(tbA12.Text.Replace('.', ','));
            double sigma = double.Parse(tbSigma.Text.Replace('.', ','));
            double beta = double.Parse(tbBeta.Text.Replace('.', ','));

            GenerateConditions(sigma, beta);

            double squareA11 = Math.Pow(double.Parse(tbA11.Text.Replace('.', ',')), 2);
            double squareA22 = Math.Pow(double.Parse(tbA22.Text.Replace('.', ',')), 2);
            double squareA12 = Math.Pow(double.Parse(tbA12.Text.Replace('.', ',')), 2);

            foreach (var item in storage.NT)
            {
                squerArea.Add(2 * HeronArea.getArea(item[0], item[1], item[2]));
                equationStorage.Add(MatrixKeConstants.getConstants(item[0], item[1], item[2]));
                Ne.Add(Basis.getBase(item[0], item[1], item[2]));
            }

            #region GetMAtrixKe
            double constant = 0;
            for (int i = 0; i < equationStorage.Count; i++)
            {
                constant = 1d / (2d * squerArea[i]);
                Ke.Add(new double[3, 3]
                {
                    {
                        constant * (squareA11 * Math.Pow(equationStorage[i].X.B,2) + squareA22 * Math.Pow(equationStorage[i].X.C,2)),
                        constant * (squareA11 * equationStorage[i].X.B*equationStorage[i].Y.B + squareA22 * equationStorage[i].X.C*equationStorage[i].Y.C),
                        constant * (squareA11 * equationStorage[i].X.B*equationStorage[i].Z.B + squareA22 * equationStorage[i].X.C*equationStorage[i].Z.C)

                    },
                    {
                         constant * (squareA11 * equationStorage[i].X.B*equationStorage[i].Y.B + squareA22 * equationStorage[i].X.C*equationStorage[i].Y.C),
                         constant * (squareA11 * Math.Pow(equationStorage[i].Y.B,2)+ squareA22 * Math.Pow(equationStorage[i].Y.C,2)),
                         constant * (squareA11 * equationStorage[i].Y.B*equationStorage[i].Z.B + squareA22 * equationStorage[i].Y.C*equationStorage[i].Z.C)
                    },
                    {
                        constant * (squareA11 * equationStorage[i].X.B*equationStorage[i].Z.B + squareA22 * equationStorage[i].X.C*equationStorage[i].Z.C),
                        constant * (squareA11 * equationStorage[i].Y.B*equationStorage[i].Z.B + squareA22 * equationStorage[i].Y.C*equationStorage[i].Z.C) ,
                        constant * (squareA11 * Math.Pow(equationStorage[i].Z.B,2) + squareA22 * Math.Pow(equationStorage[i].Z.C,2))
                    }
                });
            }
            #endregion

            #region getMatrixMe
            constant = 0;
            for (int i = 0; i < squerArea.Count; i++)
            {
                constant = squerArea[i] / 24;
                Me.Add(new double[3, 3] {
                    {
                        2 * constant, 1 * constant, 1 * constant
                    },
                    {
                        1 * constant, 2 * constant, 1 * constant
                    },
                    {
                        1 * constant, 1 * constant, 2 * constant
                    }
                });
                
            }
            #endregion

            #region getMatrixRe
            var Uconst = 1d;
            var matrix = DenseMatrix.OfArray(new double[,] { { 1, 2 }, { 2, 1 } });
            foreach (var side in GetSegments())
            {
                var leftSide = sigma / beta * matrix;
                var rigthSide = sigma / beta * matrix;
                var vector = new[] { Uconst * side.UcCof, Uconst * side.UcCof };
                foreach (var segment in side.Segments)
                {
                    var length = Math.Sqrt(Math.Pow(segment.Vertex1.X - segment.Vertex2.X, 2) +
                                            Math.Pow(segment.Vertex1.Y - segment.Vertex2.Y, 2));

                    var reLeft = (leftSide * length / 6);
                    var reRight = (rigthSide * length / 6) * (new DenseVector(vector));
                    ReMatrix.Add(reLeft);
                    ReVector.Add(reRight);
                }
            }
            #endregion

            #region getMatrixQe

            foreach (var item in storage.NT)
            {
                foreach (var me in Me)
                {
                    
                    var vector = GenerateQeMatrixes(item[0], item[1], item[2]);
                    var result = DenseMatrix.OfArray(me).Multiply(vector);
                    Qe.Add(result);
                }

            }

            #endregion

            PrintMatrix();
            WriteMatrix();
        }


        private void WriteMatrix()
        {
            JsonParser.Write(Me, "ME.json");
            JsonParser.Write(Qe, "QE.json");
            JsonParser.Write(Ke, "KE.json");
            JsonParser.Write(ReMatrix.Select(el => el.Storage).ToList(), "ReMatrix.json");
            JsonParser.Write(ReVector.Select(el => el.Storage).ToList(), "ReVector.json");
        }

        private void PrintMatrix()
        {
            textBox1.Text = String.Empty;

            if (radioButton1.Checked)
            {
                textBox1.Text = "Matrix Ke" + System.Environment.NewLine;
                List<DenseMatrix> matrixKe = Ke.Select(item => DenseMatrix.OfArray(item)).ToList();

                radioButton4.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                radioButton5.Checked = false;

                foreach (var item in matrixKe)
                {
                    textBox1.Text += item.ToString();
                }
            }

            if (radioButton2.Checked)
            {
                textBox1.Text = "Matrix Me " + System.Environment.NewLine;

                List<DenseMatrix> matrixMe = Me.Select(item => DenseMatrix.OfArray(item)).ToList();

                radioButton1.Checked = false;
                radioButton4.Checked = false;
                radioButton3.Checked = false;
                radioButton5.Checked = false;

                foreach (var item in matrixMe)
                {
                    textBox1.Text += item.ToString();
                }
            }

            if (radioButton3.Checked)
            {
                textBox1.Text = "Matrix Qe " + System.Environment.NewLine;

                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton4.Checked = false;
                radioButton5.Checked = false;

                foreach (var item in Qe)
                {
                    textBox1.Text += item.ToString();
                }
            }

            if (radioButton4.Checked)
            {
                textBox1.Text = "Matrix Re " + System.Environment.NewLine;

                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                radioButton5.Checked = false;
                foreach (var item in ReMatrix)
                {
                    textBox1.Text += item.ToString();
                }
            }

            if (radioButton5.Checked)
            {
                textBox1.Text = "Vector Re " + System.Environment.NewLine;

                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                radioButton4.Checked = false;
                foreach (var item in ReVector)
                {
                    textBox1.Text += item.ToString();
                }
            }


        }
        private static double Func(double x, double y) => 1d;

        private Vector<double> GenerateQeMatrixes(Vertex x, Vertex y, Vertex z)
        {
            var f = new double[3] {
                    Func(x.X, x.Y),
                    Func(y.X, y.Y),
                    Func(z.X, z.Y)
                };
            return Vector.Build.DenseOfArray(f);
        }

        private IEnumerable<Constants> GetSegments()
        {
            List<Constants> conds;
            if (!conditions.TryGetValue(storage.Figure[0], out conds)) return null;
            if (conds.Count != storage.NTG.Count) return conds;
            for (var i = 0; i < conds.Count; i++)
            {
                conds[i].Segments = storage.NTG[i];
            }
            return conds;
        }


        private void GenerateConditions(double sigmaValue, double bethaValue)
        {
            var firstCondition = new Constants(Math.Pow(0.1, 6), sigmaValue, 1);
            var secondCondition = new Constants(Math.Pow(0.1, 6), sigmaValue, Math.Pow(0.1, 6));
            var thirdCondition = new Constants(bethaValue, sigmaValue, 1);
           
            conditions = new Dictionary<int, List<Constants>>
            {
                {1, new List<Constants>()
                    { firstCondition, firstCondition, secondCondition }},
                {2, new List<Constants>()
                    { secondCondition, thirdCondition, firstCondition, thirdCondition }},
                {3, new List<Constants>()
                    { secondCondition, firstCondition, secondCondition, thirdCondition }},
                {4, new List<Constants>()
                    { firstCondition, firstCondition, secondCondition }},
                {5, new List<Constants>()
                    { thirdCondition, firstCondition, thirdCondition, secondCondition }},
                {6, new List<Constants>()
                    { firstCondition, firstCondition, thirdCondition, secondCondition, thirdCondition }}
            };
        }

    }

}
