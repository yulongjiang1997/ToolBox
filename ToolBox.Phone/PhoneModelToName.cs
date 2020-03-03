using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ToolBox.PhoneModelParse
{
    /// <summary>
    /// 手机型号工具
    /// </summary>
    public class PhoneModelNumberTool
    {
        public string PhomeModels { get; set; }
        private List<PhoneModel> PhoneMoedls { get; set; }
        public PhoneModelNumberTool()
        {
            PhoneMoedls= new PhoneModelList().Init();
        }

        public string ModelNumberToName(string modelNumber)
        {
            return PhoneMoedls.FirstOrDefault(i => i.ModelNumber == modelNumber)?.Name ?? modelNumber;
        }

    }


    public class PhoneModel
    {
        public string ModelNumber { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
    }

}
