using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WishList.DataTransferObjects;

namespace WishList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("getinfo")]
        public string GetInfo()
        {
            return $"Backend is started {DateTime.Now}";
        }

        [HttpPost]
        [Route("hello")]
        public string Hello([FromQuery] string name)
        {
            return $"Hello {name}";
        }

        [HttpGet]
        [Route("gettestobject")]
        public TestObject GetTestObject()
        {
            TestObject testObject = new TestObject { Name = "Valera", Descriptions = "Poison King", DateCreate = DateTime.Now };
            return testObject;
        }

        [HttpGet]
        [Route("getlistobject")]
        public List<TestObject> GetTestObjects()
        {
            TestObject testObject1 = new TestObject { Name = "Valera", Descriptions = "Poison King", DateCreate = DateTime.Now };
            TestObject testObject2 = new TestObject { Name = "Artem", Descriptions = "CBETLbIU", DateCreate = DateTime.Now };
            TestObject testObject3 = new TestObject { Name = "Yana", Descriptions = "Prosto Yana", DateCreate = DateTime.Now };
            List<TestObject> listObject = new List<TestObject>();
            listObject.Add(testObject3);
            listObject.Add(testObject2);
            listObject.Add(testObject1);
            return listObject;
        }

        [HttpPost]
        [Route("testobject")]
        public bool PostTestObject([FromBody]TestObject test)
        {
            return test != null;
        }
    }
}
