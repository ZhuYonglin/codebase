﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using WTMFirst.Controllers;
using WTMFirst.ViewModel.VirusVMs;
using WTMFirst.Model;
using WTMFirst.DataAccess;

namespace WTMFirst.Test
{
    [TestClass]
    public class VirusControllerTest
    {
        private VirusController _controller;
        private string _seed;

        public VirusControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<VirusController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as VirusListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(VirusVM));

            VirusVM vm = rv.Model as VirusVM;
            Virus v = new Virus();
			
            v.VirusName = "pBgxl5S";
            v.VirusCode = "xZwXe";
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Virus>().FirstOrDefault();
				
                Assert.AreEqual(data.VirusName, "pBgxl5S");
                Assert.AreEqual(data.VirusCode, "xZwXe");
            }

        }

        [TestMethod]
        public void EditTest()
        {
            Virus v = new Virus();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.VirusName = "pBgxl5S";
                v.VirusCode = "xZwXe";
                context.Set<Virus>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VirusVM));

            VirusVM vm = rv.Model as VirusVM;
            v = new Virus();
            v.ID = vm.Entity.ID;
       		
            v.VirusName = "dC1lp1mIB";
            v.VirusCode = "errB";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.VirusName", "");
            vm.FC.Add("Entity.VirusCode", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Virus>().FirstOrDefault();
 				
                Assert.AreEqual(data.VirusName, "dC1lp1mIB");
                Assert.AreEqual(data.VirusCode, "errB");
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            Virus v = new Virus();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.VirusName = "pBgxl5S";
                v.VirusCode = "xZwXe";
                context.Set<Virus>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VirusVM));

            VirusVM vm = rv.Model as VirusVM;
            v = new Virus();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<Virus>().Count(), 0);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            Virus v = new Virus();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.VirusName = "pBgxl5S";
                v.VirusCode = "xZwXe";
                context.Set<Virus>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            Virus v1 = new Virus();
            Virus v2 = new Virus();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.VirusName = "pBgxl5S";
                v1.VirusCode = "xZwXe";
                v2.VirusName = "dC1lp1mIB";
                v2.VirusCode = "errB";
                context.Set<Virus>().Add(v1);
                context.Set<Virus>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(VirusBatchVM));

            VirusBatchVM vm = rv.Model as VirusBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<Virus>().Count(), 0);
            }
        }


    }
}
