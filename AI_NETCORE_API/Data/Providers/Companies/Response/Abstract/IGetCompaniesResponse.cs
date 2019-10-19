﻿using Data.BuisnessObject;
using Data.Providers.Common.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Providers.Companies.Response.Abstract
{
    public interface IGetCompaniesResponse : IProvideResult
    {
        IList<Company> Companies { get; }
    }
}
