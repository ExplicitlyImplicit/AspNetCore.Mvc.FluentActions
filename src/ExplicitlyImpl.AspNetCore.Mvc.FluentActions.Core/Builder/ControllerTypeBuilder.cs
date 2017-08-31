// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions.Core.Builder
{
    public class ControllerTypeBuilder
    {
        public ModuleBuilder ModuleBuilder { get; internal set; }
        public TypeBuilder TypeBuilder { get; internal set; }
        public List<TypeBuilder> NestedTypes { get; internal set; }

        private ControllerTypeBuilder() { }

        public void BuildMethod(string name, ControllerMethodBuilder controllerMethodBuilder)
        {
            var methodBuilder = TypeBuilder.DefineMethod(name, MethodAttributes.Public | MethodAttributes.HideBySig);

            controllerMethodBuilder.TypeBuilder = TypeBuilder;
            controllerMethodBuilder.NestedTypes = new List<TypeBuilder>();
            controllerMethodBuilder.MethodBuilder = methodBuilder;

            controllerMethodBuilder.Build();

            NestedTypes = controllerMethodBuilder.NestedTypes;
        }

        public void SetCustomAttribute(
            ConstructorInfo constructor,
            object[] constructorArgs,
            PropertyInfo[] namedProperties,
            object[] propertyValues,
            FieldInfo[] namedFields,
            object[] fieldValues)
        {
            var attributeBuilder = new CustomAttributeBuilder(
                constructor,
                constructorArgs,
                namedProperties,
                propertyValues,
                namedFields,
                fieldValues
            );

            TypeBuilder.SetCustomAttribute(attributeBuilder);
        }

        public TypeInfo CreateTypeInfo()
        {
            var typeInfo = TypeBuilder.CreateTypeInfo();

            foreach(var nestedType in NestedTypes)
            {
                nestedType.CreateTypeInfo();
            }

            return typeInfo;
        }

        public static ControllerTypeBuilder Create(ModuleBuilder moduleBuilder, string typeName, Type parentType = null)
        {
            var typeBuilder = moduleBuilder.DefineType(
                    typeName,
                    TypeAttributes.Class | 
                    TypeAttributes.Public | 
                    TypeAttributes.AutoClass | 
                    TypeAttributes.AnsiClass | 
                    TypeAttributes.BeforeFieldInit,
                    parentType ?? typeof(Controller));

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

            return new ControllerTypeBuilder
            {
                ModuleBuilder = moduleBuilder,
                TypeBuilder = typeBuilder
            };
        }

        public static ControllerTypeBuilder Create(string assemblyName, string moduleName, string typeName, Type parentType = null)
        {
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName(assemblyName),
                AssemblyBuilderAccess.Run);

            var moduleBuilder = assemblyBuilder.DefineDynamicModule(moduleName);

            return Create(moduleBuilder, typeName, parentType);
        }
    }
}
