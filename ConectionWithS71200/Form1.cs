using ConectionWithS71200.Contexto;
using S7.Net;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Diagnostics;
using iText.Kernel.Geom;
using System.Configuration;
namespace ConectionWithS71200
{
    public partial class Form1 : Form
    {
        PlcReportsContext Contexto = new PlcReportsContext();
        public Form1()
        {
            InitializeComponent();
        }
        Plc PLC = new Plc(CpuType.S71200, "192.168.0.1", 0, 0);
        Var_Plc_List Tag_List = new Var_Plc_List();
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            var PLC_DB = 1;
            PLC.ReadClass(Tag_List, PLC_DB);
            textBox1.Text = Var_Plc_List.M101.ToString("0.00");
            textBox2.Text = Var_Plc_List.OUT_1.ToString();
            textBox3.Text = Var_Plc_List.OUT_2.ToString();
            textBox4.Text = Var_Plc_List.OUT_3.ToString();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (PLC.Open() == ErrorCode.NoError)
            {
                timer1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Fecha = DateTime.Today.ToString();
            Lectura lectura = new Lectura();
            lectura.Out1 = textBox2.Text;
            lectura.Out2 = textBox3.Text;
            lectura.Out3 = textBox4.Text;
            lectura.Ia1 = textBox1.Text;
            lectura.Fecha = Fecha;
            Contexto.Lecturas.Add(lectura);
            Contexto.SaveChanges();
            MessageBox.Show("datos guardados con existo en la base de datos");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var Fecha_hora = DateTime.Today;
                PlcReportsContext contexto = new PlcReportsContext();

                string rutaPDF = "Reporte_Lecturas.pdf";
                PdfWriter writer = new PdfWriter(rutaPDF);
                PdfDocument pdfDoc = new PdfDocument(writer);
                Document doc = new Document(pdfDoc);

                //generar una tabla con datos de la BD
                Table tabla = new Table(5);
                tabla.AddCell(new Cell().Add(new Paragraph("Fecha de Lectura")).SetBackgroundColor(ColorConstants.CYAN));
                tabla.AddCell(new Cell().Add(new Paragraph("Salida 1")).SetBackgroundColor(ColorConstants.CYAN));
                tabla.AddCell(new Cell().Add(new Paragraph("Salida 2")).SetBackgroundColor(ColorConstants.CYAN));
                tabla.AddCell(new Cell().Add(new Paragraph("Salida 3")).SetBackgroundColor(ColorConstants.CYAN));
                tabla.AddCell(new Cell().Add(new Paragraph("Lectura Analog 1")).SetBackgroundColor(ColorConstants.CYAN));

                foreach (var item in contexto.Lecturas)
                {
                    tabla.AddCell(new Cell().Add(new Paragraph(item.Fecha)).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                    tabla.AddCell(new Cell().Add(new Paragraph(item.Out1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                    tabla.AddCell(new Cell().Add(new Paragraph(item.Out2)).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                    tabla.AddCell(new Cell().Add(new Paragraph(item.Out3)).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                    tabla.AddCell(new Cell().Add(new Paragraph(item.Ia1)).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                }

                //Generar tittulo
                var titulo = new Paragraph("Reportes De Simatic S7 1200");
                titulo.SetTextAlignment(TextAlignment.CENTER);
                titulo.SetFontSize(17);
                titulo.SetBold();
                doc.Add(titulo);

                //Generar un logo
                string ruta_imagen = "../../../img/PLC.jpg";
                var img = new iText.Layout.Element.Image(ImageDataFactory.Create(ruta_imagen));
                img.ScaleToFit(200, 100);
                img.SetFixedPosition(35, 730);
                doc.Add(img); // Agregar el logo fuera del bucle

                var salto_doble = new Paragraph("\n\n");
                var salto_simple = new Paragraph("\n");
                doc.Add(salto_doble);
                var info_reporte = new Paragraph("Reporte de lecturas de plc hasta la decha de: " + Fecha_hora);
                doc.Add(info_reporte);
                doc.Add(salto_simple);
                doc.Add(tabla);
                // Cerrar el documento después de agregar todo
                doc.Close();
                pdfDoc.Close();

                // Mostrar el documento mediante PowerShell para que ejecute la apertura del PDF
                try
                {
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        UseShellExecute = true,
                        FileName = rutaPDF,
                        Verb = "open"
                    };
                    System.Diagnostics.Process.Start(startInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error al intentar abrir el archivo: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
            }
        }
    }
}