using Microsoft.AspNetCore.Mvc;
using System;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ParameterlessControllerReturnsString : Controller
    {
        [HttpGet]
        public string HandlerAction()
        {
            return "Hello";
        }
    }

    public class ParameterlessControllerReturnsInt : Controller
    {
        [HttpGet]
        public int HandlerAction()
        {
            return 13;
        }
    }

    public class ParameterlessControllerReturnsGuid : Controller
    {
        [HttpGet]
        public Guid HandlerAction()
        {
            return new Guid("2a6d4959-817c-4514-90f3-52b518e9ddb0");
        }
    }

    public enum ExampleEnumWithoutUsings
    {
        ExampleEnumValue1,
        ExampleEnumValue2,
        ExampleEnumValue3
    }

    public class ParameterlessControllerReturnsEnum : Controller
    {
        [HttpGet]
        public ExampleEnumWithoutUsings HandlerAction()
        {
            return ExampleEnumWithoutUsings.ExampleEnumValue2;
        }
    }

    public class ExampleClassWithoutUsings
    {
        public string StringField;

        public int IntField;

        public string StringProperty { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ExampleClassWithoutUsings &&
                ((ExampleClassWithoutUsings)obj).StringField == StringField &&
                ((ExampleClassWithoutUsings)obj).IntField == IntField &&
                ((ExampleClassWithoutUsings)obj).StringProperty == StringProperty;
        }
    }

    public class ParameterlessControllerReturnsObject : Controller
    {
        [HttpGet]
        public ExampleClassWithoutUsings HandlerAction()
        {
            return new ExampleClassWithoutUsings
            {
                StringField = "Hello",
                IntField = 14,
                StringProperty = "World!"
            };
        }
    }
}
