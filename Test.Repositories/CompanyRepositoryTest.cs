using BudgetPlanner.Data;
using BudgetPlanner.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using BudgetPlanner.Persistence.Repositories;
using BudgetPlanner.Persistence.Interfaces;
using BudgetPlanner.Persistence;
using Autofac;

namespace Test.Repositories
{
    [TestClass]
    public class CompanyRepositoryTest
    {
        private ICompanyRepository _companyRepository;

        protected IContainer container;

        public CompanyRepositoryTest()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<CompanyRepository>().As<ICompanyRepository>();
            container = builder.Build();
        } 

        //NONASYNC
        [TestMethod] //dependant on the database being populated
        public void FirstInclTransSub_IncludesTransAndSub()
        {
            //Arrange
            var companyRepos = container.Resolve<ICompanyRepository>();

            //Act
            var actual = _companyRepository.FirstInclTransSub(c => c.CompanyId > 0);
            bool sub = (actual.Subscriptions != null);
            bool trans = (actual.Transactions != null);
            //Assert
            Assert.IsTrue(sub && trans);
        }

        [TestMethod]
        public void FirstInclTrans_IncludesTrans()
        {
            //Arrange
            //Act
            var actual = _companyRepository.FirstInclTrans(c => c.CompanyId > 0);
            bool trans = (actual.Transactions != null);
            //Assert
            Assert.IsTrue(trans);
        }
    }
}
