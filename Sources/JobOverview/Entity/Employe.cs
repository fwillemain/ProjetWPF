﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOverview.Entity
{
    public class Employe
    {
        public int Id { get; set; }
        public string LastName { get; set; }

        public override string ToString()
        {
            return LastName;
        }
    }
}
