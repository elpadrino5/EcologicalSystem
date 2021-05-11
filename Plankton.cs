using System;
using System.Collections.Generic;
using System.Text;

namespace EcologicalSystemConsole
{
    public class Plankton: MarineAnimal
    {
        public Plankton(string type, int rn, int cn, int rcl) : base(rn,cn)
        {
            this.Type = type;
            this.ReproClockLimit = rcl;
        }
    }

}
