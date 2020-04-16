using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Text;

namespace AXIntegracao.Commom
{
    public class SwaggerHeaders : IOperationFilter
    {

        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "ChaveAPI",
                In = "header",
                Type = "string",
                Required = false
            });
        }
    }
}

