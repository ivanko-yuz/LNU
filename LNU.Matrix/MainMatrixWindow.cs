using LNU.Matrix.UtilityClasses;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TriangleNet.Geometry;

namespace LNU.Matrix
{
    public partial class MainMatrixWindow : Form
    {
        List<double[,]> Ke;
        List<double[,]> Me;
        List<BasisStorage> Ne;
        List<DenseVector> Re;
        List<Vector<double>> Qe;

        public MainMatrixWindow()
        {
            InitializeComponent();
            Ke = new List<double[,]>();
            Me = new List<double[,]>();
            Ne = new List<BasisStorage>();
            Re = new List<DenseVector>();
            Qe = new List<Vector<double>>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataStorage storage = new DataStorage();
            List<double> squerArea = new List<double>();
            List<EquationStorage> equationStorage = new List<EquationStorage>();

            double a11 = double.Parse(tbA11.Text.Replace('.', ','));
            double a22 = double.Parse(tbA22.Text.Replace('.', ','));
            double a12 = double.Parse(tbA12.Text.Replace('.', ','));
            double sigma = double.Parse(tbSigma.Text.Replace('.', ','));
            double beta = double.Parse(tbBeta.Text.Replace('.', ','));



            double squareA11 = Math.Pow(double.Parse(tbA11.Text.Replace('.', ',')), 2);
            double squareA22 = Math.Pow(double.Parse(tbA22.Text.Replace('.', ',')), 2);
            double squareA12 = Math.Pow(double.Parse(tbA12.Text.Replace('.', ',')), 2);

            foreach (var item in storage.NT)
            {
                squerArea.Add(HeronArea.getArea(item[0], item[1], item[2]));
                equationStorage.Add(MatrixKeConstants.getConstants(item[0], item[1], item[2]));
                Ne.Add(Basis.getBase(item[0], item[1], item[2]));
            }

            #region GetMAtrixKe
            double constant = 0;
            for (int i = 0; i < equationStorage.Count; i++)
            {
                constant = 1 / (2 * squerArea[i]);
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
                PrintMatrix();
            }
            #endregion

            #region getMatrixRe

            var matrix = DenseMatrix.OfArray(new double[,] { { 1, 2 }, { 2, 1 } });
            var leftSide = sigma / beta * matrix;
            var rigthSide = sigma / beta * matrix;
            foreach (var side in storage.NTG)
            {
                foreach (var segment in side)
                {
                    var vector = new[] { func(segment.Vertex1.X, segment.Vertex1.Y), func(segment.Vertex2.X, segment.Vertex2.Y) };
                    var re = leftSide * (new DenseVector(vector)) - rigthSide * (new DenseVector(vector));
                    Re.Add(re);
                }
            }
            #endregion

            #region getMatrixQe
            foreach (var item in storage.NT)
            {
                var j = 0;
                foreach (var me in Me)
                {
                    
                    var vector = GenerateQeMatrixes(item[0], item[1], item[2]);
                    var result = DenseMatrix.OfArray(me).Multiply(vector);
                    Qe.Add(result);
                }

            }

            #endregion
        }




        private void PrintMatrix()
        {
            textBox1.Text = String.Empty;

            if (radioButton1.Checked)
            {
                radioButton4.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                foreach (var item in Ke)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            textBox1.Text += item[i, j].ToString("0.000") + " ";
                        }
                        textBox1.Text += System.Environment.NewLine;
                    }
                    textBox1.Text += System.Environment.NewLine;

                }
            }

            if (radioButton2.Checked)
            {
                radioButton1.Checked = false;
                radioButton4.Checked = false;
                radioButton3.Checked = false;
                foreach (var item in Me)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            textBox1.Text += item[i, j].ToString("0.000") + " ";
                        }
                        textBox1.Text += System.Environment.NewLine;
                    }
                    textBox1.Text += System.Environment.NewLine;

                }
            }

            if (radioButton3.Checked)
            {
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton4.Checked = false;

                foreach (var item in Qe)
                {
                    textBox1.Text += item.ToString();
                }
            }

            if (radioButton4.Checked)
            {
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                foreach (var item in Re)
                {
                    textBox1.Text += item.ToString();
                }

            }
        }
        private double func(double x, double y) => 1d;

        private Vector<double> GenerateQeMatrixes(Vertex x, Vertex y, Vertex z)
        {
            var f = new double[3] {
                    func(x.X, x.Y),
                    func(y.X, y.Y),
                    func(z.X, z.Y)
                };
            return Vector.Build.DenseOfArray(f);
        }

    }

}
