﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Creators.Configurations.Request.Abstract
{
    public interface IUpdateConfigurationRequest
    {
        string Name { get; }
        int Value { get; }
    }
}
