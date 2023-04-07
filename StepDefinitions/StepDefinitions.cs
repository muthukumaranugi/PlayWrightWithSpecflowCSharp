﻿using PlayWrightWithSpecflowCSharp.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayWrightWithSpecflowCSharp.StepDefinitions
{
    [Binding]
    public partial class StepDefinitions
    {
        IPage page;
        HeroKuappHomePage herokuHomePage;
        HeroKuappCheckboxPage herokuCheckboxPage;
        HeroKuappIFramePage herokuIFramePage;
        public StepDefinitions(IPage webpage)
        {
            page = webpage;
            herokuHomePage = new HeroKuappHomePage(page);
            herokuCheckboxPage = new HeroKuappCheckboxPage(page);
            herokuIFramePage = new HeroKuappIFramePage(page);
        }
    }
}
