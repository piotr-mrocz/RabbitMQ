using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producent;
public enum RoutingKeyEnum
{
    [Description("Email")]
    Email = 1, 
    
    [Description("SMS")]
    SMS = 2,

    [Description("Wszystkie")]
    Wszystkie = 3
}
