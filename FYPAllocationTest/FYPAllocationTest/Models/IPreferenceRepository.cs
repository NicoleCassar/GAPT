﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPAllocationTest.Models
{
    public interface IPreferenceRepository
    {
        void Submit(Preference preference);

        Area GetAreaById(string id);


    }
}