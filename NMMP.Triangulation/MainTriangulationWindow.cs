using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TriangleNet.Geometry;
using TriangleNet.Meshing;

namespace NMMP.Triangulation
{
    public partial class MainTriangulationWindow : Form
    {
        private IMesh mesh;
        private int figure = 5;
        private List<ISegment> segments;
        private int size = 75;

        public MainTriangulationWindow()
        {
            InitializeComponent();
        }

        private void DrawLines(Vertex[] vertices)
        {
            var graphics = panel1.CreateGraphics();
            graphics.Clear(Color.FromArgb(240, 240, 240));
            foreach (var e in this.mesh.Edges)
            {
                var vertex0 = vertices[e.P0];
                var vertex1 = vertices[e.P1];
                graphics.DrawLine(
                        Pens.Blue,
                        (float)vertex0.X * size + 50,
                        -1 * (float)vertex0.Y * size + 300,
                        (float)vertex1.X * size + 50,
                        -1 * (float)vertex1.Y * size + 300
                    );
            }
        }

        private void DrawVertexs()
        {
            var graphics = panel1.CreateGraphics();
            foreach (var element in this.mesh.Vertices)
            {
                graphics.FillEllipse(
                        Brushes.Red,
                        (float)element.X * size + 50,
                        -1 * (float)element.Y * size + 300,
                        5,
                        5
                    );
                graphics.DrawString(
                        $"{element.ID}",
                        new Font("Arial", 12),
                        Brushes.Black,
                        (float)element.X * size + 50,
                        -1 * (float)element.Y * size + 300
                    );
            }
            foreach (var e in this.mesh.Triangles)
            {
                var t1 = e.GetVertex(0);
                var t2 = e.GetVertex(1);
                var t3 = e.GetVertex(2);
                var avgX = (t1.X * size + t2.X * size + t3.X * size) / 3;
                var avgY = (t1.Y * size + t2.Y * size + t3.Y * size) / 3;
                graphics.FillEllipse(
                        Brushes.Red,
                        (float)avgX + 50,
                        -1 * (float)avgY + 300,
                        5,
                        5
                    );
                graphics.DrawString(
                        $"{e.ID}",
                        new Font("Arial", 12),
                        Brushes.Brown,
                        (float)avgX + 50,
                        -1 * (float)avgY + 300
                    );
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                var polygon = GetPolygone();
                textBox1.Text = textBox1.Text != String.Empty ? textBox1.Text.Replace('.', ',') : "1";
                if (Math.Abs(Convert.ToDouble(textBox1.Text)) > 0)
                {
                    var area = Math.Abs(Convert.ToDouble(textBox1.Text));
                    var options = new ConstraintOptions() { ConformingDelaunay = true };
                    var quality = new QualityOptions() { MinimumAngle = 30, MaximumArea = area };

                    this.mesh = polygon.Triangulate(options, quality);

                    DrawLines(this.mesh.Vertices.ToArray());
                    DrawVertexs();

                    var nt = this.mesh.Triangles.Select(el => new List<Vertex>()
                {
                    el.GetVertex(0),
                    el.GetVertex(1),
                    el.GetVertex(2)
                });

                    var ct = this.mesh.Vertices;
                    var ntg = GetSegments();

                    JsonParser.Write(nt.ToList(), "NT.json");//трикутники
                    JsonParser.Write(ct.ToList(), "CT.json");//точки
                    JsonParser.Write(ntg.ToList(), "NTG.json");//сегменти

                }
                else
                {
                    throw new Exception();
                }

            }
            catch
            {
                textBox1.Text = "Wrong input data";
            }

        }

        public IEnumerable<List<Vertex>> NT { get; set; }

        public ICollection<Vertex> CT { get; set; }

        public List<List<SerializableLine>> NTG { get; set; }


        #region RadioBtnChange

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            figure = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            figure = 2;

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            figure = 3;

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            figure = 4;

        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            figure = 5;

        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            figure = 6;

        }
        #endregion

        private List<List<SerializableLine>> GetSegments()
        {
            var result =
                segments.Select(segment => (from el in mesh.Segments
                                            let frstVertex = el.GetVertex(0)
                                            let secondVertex = el.GetVertex(1)
                                            let firstFlag =
                                                // ReSharper disable once CompareOfFloatsByEqualityOperator
                                                ((frstVertex.X - segment.GetVertex(0).X) * (segment.GetVertex(1).Y - segment.GetVertex(0).Y) ==
                                                 (segment.GetVertex(1).X - segment.GetVertex(0).X) * (frstVertex.Y - segment.GetVertex(0).Y))
                                            let secondFlag =
                                                // ReSharper disable once CompareOfFloatsByEqualityOperator
                                                ((secondVertex.X - segment.GetVertex(0).X) * (segment.GetVertex(1).Y - segment.GetVertex(0).Y) ==
                                                 (segment.GetVertex(1).X - segment.GetVertex(0).X) * (secondVertex.Y - segment.GetVertex(0).Y))
                                            where firstFlag && secondFlag
                                            select el).ToList())
                .Select(segmentsToAdd => segmentsToAdd.Select(el => new SerializableLine(el.GetVertex(0), el.GetVertex(1))).ToList())
                .ToList();

            return result;
        }

        private Polygon GetPolygone()
        {
            var polygon = new Polygon();

            #region switch
            switch (this.figure)
            {
                case 1:
                    {
                        polygon.Add(new Contour(new[]
                        {
                        new Vertex(0.0, 0.0),
                        new Vertex(1.0, 0.0),
                        new Vertex(0.0, 1.0),
                    }));
                    }
                    break;
                case 2:
                    {
                        polygon.Add(new Contour(new[]
                        {
                        new Vertex(0.0, 0.0),
                        new Vertex(0.0, 2.0),
                        new Vertex(1.0, 2.0),
                        new Vertex(1.0, 0.0),
                    }));
                    }
                    break;
                case 3:
                    {
                        polygon.Add(new Contour(new[]
                        {
                        new Vertex(1.0, 0.0),
                        new Vertex(2.0, 0.0),
                        new Vertex(0.0, 2.0),
                        new Vertex(0.0, 1.0),
                    }));
                    }
                    break;
                case 4:
                    {
                        polygon.Add(new Contour(new[]
                        {
                        new Vertex(0.0, 0.0),
                        new Vertex(3.0, 3.0),
                        new Vertex(6.0, 0.0),
                    }));
                    }
                    break;
                case 5:
                    {
                        polygon.Add(new Contour(new[]
                        {
                        new Vertex(0.0, 0.0),
                        new Vertex(0.0, 1.0),
                        new Vertex(4.0, 1.0),
                        new Vertex(4.0, 0.0),
                    }));
                    }
                    break;
                case 6:
                    {
                        polygon.Add(new Contour(new[]
                        {
                        new Vertex(0.0, 0.0),
                        new Vertex(3.0, 3.0),
                        new Vertex(6.0, 0.0),
                        new Vertex(4.0, 0.0),
                        new Vertex(3.0, 1.0),
                        new Vertex(2.0, 0.0),
                    }));
                    }
                    break;
            }
            #endregion
            segments = polygon.Segments;
            return polygon;
        }
    }
}
