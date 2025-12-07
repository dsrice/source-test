using System;

namespace SourceTest.Generator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class GenerateHelloAttribute : Attribute
    {
    }
}
