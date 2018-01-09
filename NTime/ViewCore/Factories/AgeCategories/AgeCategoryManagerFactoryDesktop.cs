﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewCore.ManagersHttp;
using ViewCore.ManagersInterfaces;
using ViewCore.Entities;
using System.Collections.ObjectModel;
using ViewCore.ManagersDesktop;

namespace ViewCore.Factories.AgeCategories
{
    public class AgeCategoryManagerFactoryDesktop : IAgeCategoryManagerFactory
    {
        public IAgeCategoryManager CreateInstance(IEditableCompetition currentCompetition, AccountInfo accountInfo = null, ConnectionInfo connectionInfo = null)
        {
            return new AgeCategoryManagerDesktop(currentCompetition);
        }
    }
}
