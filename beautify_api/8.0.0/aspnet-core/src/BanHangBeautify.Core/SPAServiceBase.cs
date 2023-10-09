﻿using Abp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify
{
    /// <summary>
    /// This class can be used as a base class for services in this application.
    /// It has some useful objects property-injected and has some basic methods most of services may need to.
    /// It's suitable for non domain nor application service classes.
    /// For domain services inherit <see cref="BanHangBeautifyDomainServiceBase"/>.
    /// For application services inherit AbpZeroTemplateAppServiceBase.
    /// </summary>
    public abstract class SPAServiceBase: AbpServiceBase
    {
        protected SPAServiceBase()
        {
            LocalizationSourceName = SPAConsts.LocalizationSourceName;
        }
    }
}