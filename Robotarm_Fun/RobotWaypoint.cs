using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robotarm_Fun
{
    public class RobotWaypoint
    {
        public Int16 xCord { get; set; }

        public Int16 yCord { get; set; }

        public Int16 zCord { get; set; }

        public Int16 armRotation { get; set; }

        public string Name { get; set; } = string.Empty;


    }
}
