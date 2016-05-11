using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Addovation.Cloud.Apps.AddoResources.Client.Portable;

namespace TestInterface.Models
{
    class TemplateModel
    {
        public Template Model { get; set; }

        public TemplateModel(Template t)
        {
            Model = t;
        }

        public override string ToString()
        {
            return Model.ClassValue + " | " + Model.ClassDescription + " | " + Model.FileName;
        }
    }
}
