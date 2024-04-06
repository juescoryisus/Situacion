using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace situacion
{
    public partial class Form1 : Form
    {
        private Dictionary<string, int> inventario = new Dictionary<string, int>();
        private DataTable tabla_de_inventario = new DataTable();
        private string fileName;
        public Form1()
        {
            InitializeComponent();
            InitializeTabla_de_inventario();
        }

        private void InitializeTabla_de_inventario()
        {
            tabla_de_inventario.Columns.Add("Producto", typeof(string));
            tabla_de_inventario.Columns.Add("Cantidad", typeof(int));

            dataGridView1.DataSource = tabla_de_inventario;
        }

        private void Actualizar_tabla_de_indevntario()
        {
            tabla_de_inventario.Rows.Clear();
            foreach (var item in inventario)
            {
                tabla_de_inventario.Rows.Add(item.Key, item.Value);
            }
        }

        private void tabla()
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.Interval = 1;

            Series series = new Series
            {
                Name = "Productos",
                IsVisibleInLegend = true,
                ChartType = SeriesChartType.Column
            };

            foreach (var item in inventario)
            {
                series.Points.AddXY(item.Key, item.Value);
            }

            chart1.Series.Add(series);
        }
        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {


            string producto = txtProducto.Text.Trim();
            if (producto == "")
            {
                MessageBox.Show("Por favor ingrese un nombre de producto.");
                return;
            }

            int cantidad;
            if (!int.TryParse(txtCantidad.Text, out cantidad))
            {
                MessageBox.Show("Por favor ingrese una cantidad válida.");
                return;
            }

            if (inventario.ContainsKey(producto))
            {
                inventario[producto] += cantidad;
            }
            else
            {
                inventario.Add(producto, cantidad);
            }

            Actualizar_tabla_de_indevntario();
            tabla();

            // Guardar el archivo al agregar un producto
            string formatoSeleccionado = comboBox1.SelectedItem.ToString();
            switch (formatoSeleccionado)
            {
                case "CSV":
                    GuardarComoCSV();
                    break;
                case "XML":
                    GuardarComoXML();
                    break;
                default:
                    MessageBox.Show("Formato de archivo no válido.");
                    break;
            }
        }

        private void GuardarComoCSV()
        {
            fileName = "inventario.csv";
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Producto,Cantidad");
                foreach (var item in inventario)
                {
                    csvContent.AppendLine($"{item.Key},{item.Value}");
                }
                writer.Write(csvContent.ToString());
            }
            lblNombre_Archivo.Text = $"Datos guardados en: {fileName}";
        }

        private void GuardarComoXML()
        {
            fileName = "inventario.xml";
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Producto");
            dataTable.Columns.Add("Cantidad");

            foreach (var item in inventario)
            {
                dataTable.Rows.Add(item.Key, item.Value);
            }

            dataSet.Tables.Add(dataTable);
            dataSet.WriteXml(fileName);
            lblNombre_Archivo.Text = $"Datos guardados en: {fileName}";
        }
    }

}

    