using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using System.Text;
using WIA;

namespace SGAC.WebApp.Accesorios
{
    public class Scanner
    {
        private readonly DeviceInfo loDeviceInfo;

        public Scanner() { }

        public Scanner(DeviceInfo deviceInfo) {
            this.loDeviceInfo = deviceInfo;
        }

        public ImageFile Scan()
        {
            ImageFile imageFile = null;
            var device = this.loDeviceInfo.Connect();
            var Item = device.Items[1];

            foreach (WIA.Item item in device.Items)
            {
                StringBuilder propsbuilder = new StringBuilder();

                foreach (WIA.Property itemProperty in item.Properties)
                {

                    Object tempNewProperty;

                    if (itemProperty.Name.Equals("Horizontal Resolution"))
                    {
                        tempNewProperty = 75;
                        ((IProperty)itemProperty).set_Value(ref tempNewProperty);
                    }
                    else if (itemProperty.Name.Equals("Vertical Resolution"))
                    {
                        tempNewProperty = 75;
                        ((IProperty)itemProperty).set_Value(ref tempNewProperty);
                    }
                    else if (itemProperty.Name.Equals("Horizontal Extent"))
                    {
                        tempNewProperty = 619;
                        ((IProperty)itemProperty).set_Value(ref tempNewProperty);
                    }
                    else if (itemProperty.Name.Equals("Vertical Extent"))
                    {
                        tempNewProperty = 876;
                        ((IProperty)itemProperty).set_Value(ref tempNewProperty);
                    }
                }

                imageFile = (ImageFile)Item.Transfer();
            }
            return imageFile;
        }
    }
}