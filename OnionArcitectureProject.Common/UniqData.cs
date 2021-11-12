using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArcitectureProject.Common
{
    public class UniqData
    {
        public string GetUniqueName(string fileName)
            => Guid.NewGuid().ToString() + Path.GetExtension(fileName);
    }
}
