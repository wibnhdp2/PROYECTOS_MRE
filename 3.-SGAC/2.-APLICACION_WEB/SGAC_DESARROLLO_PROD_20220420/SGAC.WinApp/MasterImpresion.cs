using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.IO;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Reflection;


namespace SGAC.WinApp
{
    public class MasterImpresion
    {
        #region propiedades

        private int m_intErrorNumero = 0;

        public int _pintErrorNumero
        {
            get { return m_intErrorNumero; }
            set
            {
                m_intErrorNumero = value;
            }
        }

        private string m_strErrorMensaje = "";

        public string _pstrErrorMensaje
        {
            get { return m_strErrorMensaje; }
            set
            {
                m_strErrorMensaje = value;
            }
        }

        private PrintDialog m_PrintDialog;
        /// <summary>
        /// _pPrintDialog: es el objeto cuadro de dialogo para setear la configuracion de impresion
        /// </summary>
        public PrintDialog _pPrintDialog
        {
            get { return m_PrintDialog; }
            set
            {
                m_PrintDialog = value;
            }
        }

        private LocalReport m_LocalReport;
        /// <summary>
        /// _pLocalReport: propiedad para asignar el reporte local Report.rdlc a imprimir
        /// en este objeto se le setean el nombre del reporte, el origen de datos
        /// </summary>
        public LocalReport _pLocalReport
        {
            get { return m_LocalReport; }
            set
            {
                m_LocalReport = value;
            }
        }

        private FrmReporteVistaPrevia m_ReportViewer;
        /// <summary>
        /// _pReportViewer: es el formulario que se usa para la vista previa,
        /// este form contiene un reportviewer
        /// </summary>
        public FrmReporteVistaPrevia _pReportViewer
        {
            get { return m_ReportViewer; }
            set
            {
                m_ReportViewer = value;
            }
        }

        private List<object> m_OrigenesDatos = new List<object>();
        /// <summary>
        /// _pOrigenesDatosReporte: en esta propiedad podemos asignar los origenes de datos
        /// del reporte a mostrar
        /// </summary>
        public List<object> _pOrigenesDatosReporte
        {
            get { return m_OrigenesDatos; }
            set
            {
                m_OrigenesDatos = value;
            }
        }
        private ReportPrintDocument m_ReportPrintDocument;
        /// <summary>
        ///objeto printdocuemnt heredado para imprimir sin vista previa
        /// </summary>
        public ReportPrintDocument _pReportPrintDocument
        {
            get { return m_ReportPrintDocument; }
            set
            {
                m_ReportPrintDocument = value;
            }
        }

    
        private string[,] m_arrparametros;
        /// <summary>
        /// _parrparametros: en esta propiedad podemos asignar los parametros en forma de string
        /// </summary>

        public string[,] _parrparametros
        {
            get { return m_arrparametros; }
            set
            {
                m_arrparametros = value;
            }
        }


        #endregion

        #region Metodos

        //constructor
        public MasterImpresion()
        {
            //inicializo las propiedades
            this._pLocalReport = new LocalReport();
            this._pOrigenesDatosReporte = new List<object>();
            this._pPrintDialog = new PrintDialog();
            this._pReportViewer = new FrmReporteVistaPrevia();
            //seteo el reportviewer del form al modo local
            //this._pReportViewer.reportViewer1.Reset();
            this._pReportViewer.reportViewer1.ProcessingMode = ProcessingMode.Local;

        }

   public void Imprimir()
    {
        this._pintErrorNumero = 0;
        try
        {    
              //asigno el origen de datos
              this._pLocalReport.DataSources.Clear();
              if (this._pOrigenesDatosReporte != null)
              {

                //en un for agrego todos los origenes de datos al reporte
                foreach (object porigen in this._pOrigenesDatosReporte)
                {
                  ReportDataSource rds = new ReportDataSource();
                  //porigen: tiene la propiedad name y value, sacar por reflexion y setarselo a rds
          
                  Type pTipoObjetoOrigen = porigen.GetType();
                  PropertyInfo stringPropertyInfo = pTipoObjetoOrigen.GetProperty("Name");
                  string valueNombreOrigen = (string)stringPropertyInfo.GetValue(porigen, null);
                  rds.Name = valueNombreOrigen;

                  PropertyInfo stringPropertyInfo1 = pTipoObjetoOrigen.GetProperty("Value");
                  object valueOrigen = (object)stringPropertyInfo1.GetValue(porigen, null);
                  rds.Value = valueOrigen;

                  this._pLocalReport.DataSources.Add(rds);
                }
              }
              else
              {
                //si no se asigno un origen de datos al reporte muestro el mensaje y salgo
              }
               //instancio el printdocument, este objeto es del tipo ReportPrintDocument,

       

               int IntNumeroElementos;
               int A;
               IntNumeroElementos = Convert.ToInt16(this._parrparametros.GetLongLength(0));
               ReportParameter[] parameters = new ReportParameter[IntNumeroElementos];


               for (A = 0; A <= IntNumeroElementos - 1; A++)
               {
                   parameters[A] = new ReportParameter(this._parrparametros[A, 0], this._parrparametros[A, 1]);           
               }
               this._pLocalReport.SetParameters(parameters);
               this._pReportPrintDocument = new ReportPrintDocument(this._pLocalReport);

              //mando a imprimir
              this._pReportPrintDocument.Print();
        }
        catch (Exception ex)
        {
            this._pintErrorNumero = -1;
            this._pstrErrorMensaje = ex.Message;
        }        
    }

  /// <summary>
    /// Metodo para Mandar a vista previa la impresion
    /// </summary>
    public void VistaPrevia()
    {
      //asigno el reporte al reportviewer
      this.SetearReporte();

      //refresco y muestro el formulario
      this._pReportViewer.reportViewer1.LocalReport.Refresh();
     
      //muestro el form con el reportviewer 
 	  this._pReportViewer.ShowDialog(); 

    }
/// <summary>
    /// metodo para setear el reporte y el origen de datos 
    /// </summary>
    public void SetearReporte()
    {
      if (this._pReportViewer != null)
      {
        

        //asigno el origen de datos
        this._pReportViewer.reportViewer1.LocalReport.DataSources.Clear();
        if (this._pOrigenesDatosReporte != null)
        {
          
          //en un for agrego todos los origenes de datos al reporte
          foreach (object porigen in this._pOrigenesDatosReporte) 
          {
            ReportDataSource rds = new ReportDataSource();
            //porigen: tiene la propiedad name y value, sacar por reflexion y setarselo a rds
            //rds.Name = "ClientesList";
            //rds.Value = porigen;
            
            Type pTipoObjetoOrigen= porigen.GetType();


            PropertyInfo stringPropertyInfo = pTipoObjetoOrigen.GetProperty("Name");
            string valueNombreOrigen = (string)stringPropertyInfo.GetValue(porigen, null);
            rds.Name = valueNombreOrigen;

            PropertyInfo stringPropertyInfo1 = pTipoObjetoOrigen.GetProperty("Value");
            object valueOrigen = (object)stringPropertyInfo1.GetValue(porigen, null);
            rds.Value = valueOrigen; 

            this._pReportViewer.reportViewer1.LocalReport.DataSources.Add(rds);
          }
         

        }
        else
        {
          //si no se asigno un origen de datos al reporte muestro el mensaje y salgo
        }
        //asigno el reporte asignado a la propiedad this._pLocalReport
        if (this._pLocalReport != null)
        {
          this._pReportViewer.reportViewer1.LocalReport.ReportPath = this._pLocalReport.ReportPath;
        }
        else
        {
          //si no se asigno a la propiedad this._pLocalReport un reporte
          //muestro el mensaje y salgo

        }
        this._pReportViewer.reportViewer1.LocalReport.Refresh(); 
        
      }
    }

     #endregion
  }
}

