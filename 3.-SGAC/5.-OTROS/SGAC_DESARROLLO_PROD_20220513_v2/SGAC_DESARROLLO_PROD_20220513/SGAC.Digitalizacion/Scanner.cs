using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using WIA;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SGAC.Digitalizacion
{
    public class Scanner
    {
        private readonly DeviceInfo _deviceInfo;
        public Scanner(DeviceInfo deviceInfo)
        {
            this._deviceInfo = deviceInfo;
        }
        public ImageFile Scan()
        {
            ImageFile imageFile = null;
            var device = this._deviceInfo.Connect();
            var Item = device.Items[1];
            //string strwiaFormatBMP = "{B96B3CAB-0728-11D3-9D7B-0000F81EF32E}";
            //string strwiaFormatPNG = "{B96B3CAF-0728-11D3-9D7B-0000F81EF32E}";
            //string strwiaFormatGIF = "{B96B3CB0-0728-11D3-9D7B-0000F81EF32E}";
            //string strwiaFormatJPEG = "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}";
            //string strwiaFormatTIFF = "{B96B3CB1-0728-11D3-9D7B-0000F81EF32E}";

            foreach (WIA.Item item in device.Items)
            {
                StringBuilder propsbuilder = new StringBuilder();

                foreach (WIA.Property itemProperty in item.Properties)
                {
                    //IProperty tempProperty;
                    Object tempNewProperty;

                    if (itemProperty.Name.Equals("Horizontal Resolution"))
                    {
                        tempNewProperty = ConfigurationManager.AppSettings["HorizontalResolution"];
                        ((IProperty)itemProperty).set_Value(ref tempNewProperty);
                    }
                    else if (itemProperty.Name.Equals("Vertical Resolution"))
                    {
                        tempNewProperty = ConfigurationManager.AppSettings["VerticalResolution"]; 
                        ((IProperty)itemProperty).set_Value(ref tempNewProperty);
                    }
                    else if (itemProperty.Name.Equals("Horizontal Extent"))
                    {
                        tempNewProperty = ConfigurationManager.AppSettings["HorizontalExtent"];
                        ((IProperty)itemProperty).set_Value(ref tempNewProperty);
                    }
                    else if (itemProperty.Name.Equals("Vertical Extent"))
                    {
                        tempNewProperty = ConfigurationManager.AppSettings["VerticalExtent"];
                        ((IProperty)itemProperty).set_Value(ref tempNewProperty);
                    }
                }
                //image = (ImageFile)item.Transfer(WIA.FormatID.wiaFormatPNG);
                imageFile = (ImageFile)Item.Transfer();
            }
            //var imageFile = (ImageFile)item.Transfer(strwiaFormatTIFF);
            return imageFile;
        }


        public override string ToString()
        {
            return this._deviceInfo.Properties["Name"].get_Value();
        }

        public void CreatePDF(List<System.Drawing.Image> images, String sPath)
        {

            if (images.Count >= 1)
            {
                Document document = new Document(PageSize.A4, 0, 0, 0, 0);

                try
                {
                    // step 2:
                    // we create a writer that listens to the document
                    // and directs a PDF-stream to a file

                    PdfWriter.GetInstance(document, new FileStream(sPath, FileMode.Create));

                    // step 3: we open the document
                    document.Open();

                    foreach (var image in images)
                    {
                        iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(image, System.Drawing.Imaging.ImageFormat.Jpeg);
                        document.Add(pic);
                        document.NewPage();
                    }
                }
                catch (DocumentException de)
                {
                    Console.Error.WriteLine(de.Message);
                }
                catch (IOException ioe)
                {
                    Console.Error.WriteLine(ioe.Message);
                }

                // step 5: we close the document
                document.Close();
            }
        }

    }
}
